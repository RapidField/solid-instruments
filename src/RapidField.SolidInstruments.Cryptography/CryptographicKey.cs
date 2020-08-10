// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using RapidField.SolidInstruments.Cryptography.Secrets;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a cryptographic algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="CryptographicKey{TAlgorithm}" /> is the default implementation of <see cref="ICryptographicKey{TAlgorithm}" />.
    /// </remarks>
    /// <typeparam name="TAlgorithm">
    /// The type of the cryptographic algorithm for which a key is derived.
    /// </typeparam>
    public abstract class CryptographicKey<TAlgorithm> : CryptographicKey, ICryptographicKey<TAlgorithm>
        where TAlgorithm : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKey{TAlgorithm}" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The cryptographic algorithm for which a key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <param name="derivedKeyLengthInBits">
        /// The length of the derived key, in bits.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> contains too many elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to the default/unspecified value -or- <paramref name="derivedKeyLengthInBits" />
        /// is less than or equal to zero.
        /// </exception>
        [DebuggerHidden]
        protected CryptographicKey(TAlgorithm algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource, Int32 derivedKeyLengthInBits)
            : base()
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(default, nameof(algorithm));
            DerivationMode = derivationMode.RejectIf().IsEqualToValue(CryptographicKeyDerivationMode.Unspecified, nameof(derivationMode));
            KeySource = new SecureMemory(KeySourceLengthInBytes);
            LazyPbkdf2Provider = new Lazy<Rfc2898DeriveBytes>(InitializePbkdf2Algorithm, LazyThreadSafetyMode.ExecutionAndPublication);

            // Gather information about the derived key.
            DerivedKeyLength = (derivedKeyLengthInBits.RejectIf().IsLessThanOrEqualTo(0, nameof(derivedKeyLengthInBits)).TargetArgument / 8);
            BlockWordCount = (derivedKeyLengthInBits / 32);
            BlockCount = (KeySourceWordCount / BlockWordCount);

            // Copy in the key source bits.
            KeySource.Access(memory => Array.Copy(keySource.RejectIf().IsNull(nameof(keySource)).OrIf(argument => argument.Length > KeySourceLengthInBytes, nameof(keySource), "The specified key source contains too many elements.").TargetArgument, memory, keySource.Length));
        }

        /// <summary>
        /// Converts the current <see cref="CryptographicKey{TAlgorithm}" /> to cryptographic key plaintext with correct bit-length
        /// for the encryption mode specified by <see cref="Algorithm" />.
        /// </summary>
        /// <returns>
        /// The derived key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        public ISecureMemory ToDerivedKeyBytes()
        {
            var result = new SecureMemory(DerivedKeyLength);

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();

                    if (DerivationMode == CryptographicKeyDerivationMode.Pbkdf2)
                    {
                        try
                        {
                            result.Access(memory =>
                            {
                                // Perform PBKDF2 key derivation.
                                var keyBytes = new Span<Byte>(Pbkdf2Provider.GetBytes(DerivedKeyLength));
                                keyBytes.CopyTo(memory);
                                keyBytes.Clear();
                            });
                        }
                        finally
                        {
                            Pbkdf2Provider.Reset();
                        }

                        return result;
                    }

                    using (var sourceWords = new PinnedMemory<UInt32>(KeySourceWordCount, true))
                    {
                        KeySource.Access(memory =>
                        {
                            // Convert the source bit field to an array of 32-bit words.
                            Buffer.BlockCopy(memory, 0, sourceWords, 0, KeySourceLengthInBytes);
                        });

                        using (var transformedWords = new PinnedMemory<UInt32>(BlockWordCount, true))
                        {
                            // Copy out the first block. If nothing further is done, this satisfies truncation mode.
                            Array.Copy(sourceWords, transformedWords, BlockWordCount);

                            switch (DerivationMode)
                            {
                                case CryptographicKeyDerivationMode.Truncation:

                                    break;

                                case CryptographicKeyDerivationMode.XorLayering:

                                    for (var i = 1; i < BlockCount; i++)
                                    {
                                        for (var j = 0; j < BlockWordCount; j++)
                                        {
                                            // Perform the XOR layering operation.
                                            transformedWords[j] = (transformedWords[j] ^ sourceWords[(i * BlockWordCount) + j]);
                                        }
                                    }

                                    break;

                                case CryptographicKeyDerivationMode.XorLayeringWithSubstitution:

                                    for (var i = 1; i < BlockCount; i++)
                                    {
                                        for (var j = 0; j < BlockWordCount; j++)
                                        {
                                            // Perform the XOR layering operation with substitution.
                                            transformedWords[j] = (SubstituteWord(transformedWords[j]) ^ sourceWords[(i * BlockWordCount) + j]);
                                        }
                                    }

                                    break;

                                default:

                                    throw new UnsupportedSpecificationException($"The specified key derivation mode, {DerivationMode}, is not supported.");
                            }

                            result.Access(memory =>
                            {
                                // Copy out the key bits.
                                var keyBytes = new Byte[DerivedKeyLength];
                                Buffer.BlockCopy(transformedWords, 0, keyBytes, 0, DerivedKeyLength);
                                keyBytes.CopyTo(memory);

                                for (var i = 0; i < DerivedKeyLength; i++)
                                {
                                    keyBytes[i] = 0x00;
                                }
                            });
                        }
                    }

                    return result;
                }
            }
            catch
            {
                result?.Dispose();
                throw new SecurityException("Key derivation failed.");
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="CryptographicKey{TAlgorithm}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Algorithm)}\": {Algorithm} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    SecureMemoryEncryptionKey.Dispose();
                    KeySource.Dispose();
                    LazyPbkdf2Provider.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="CryptographicKey{TAlgorithm}" /> to a secure bit field.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </returns>
        protected sealed override ISecureMemory ToSecureMemory(IConcurrencyControlToken controlToken)
        {
            var secureMemory = new SecureMemory(SerializedLength);

            try
            {
                using (var plaintextMemory = new PinnedMemory(SerializedPlaintextLength, true))
                {
                    KeySource.Access(pinnedKeySourceMemory =>
                    {
                        Array.Copy(pinnedKeySourceMemory, 0, plaintextMemory, KeySourceSecureMemoryIndex, KeySourceLengthInBytes);
                    });

                    plaintextMemory[AlgorithmSecureMemoryIndex] = Convert.ToByte(Algorithm);
                    plaintextMemory[DerivationModeSecureMemoryIndex] = Convert.ToByte(DerivationMode);

                    using (var cipher = SecureMemoryEncryptionAlgorithm.ToCipher(RandomnessProvider))
                    {
                        using (var initializationVector = new PinnedMemory(cipher.BlockSizeInBytes, true))
                        {
                            RandomnessProvider.GetBytes(initializationVector);

                            secureMemory.Access(pinnedMemory =>
                            {
                                using (var ciphertext = cipher.Encrypt(plaintextMemory, SecureMemoryEncryptionKey, initializationVector))
                                {
                                    Array.Copy(ciphertext, 0, pinnedMemory, 0, SerializedLength);
                                }
                            });
                        }
                    }
                }

                return secureMemory;
            }
            catch
            {
                secureMemory.Dispose();
                throw new SecurityException("Key serialization failed.");
            }
        }

        /// <summary>
        /// Finalizes static members of the <see cref="CryptographicKey{TAlgorithm}" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers() => SecureMemoryEncryptionKey.Dispose();

        /// <summary>
        /// Substitutes the specified input word for a word derived from <see cref="SubstitutionBox" />.
        /// </summary>
        /// <param name="inputWord">
        /// A word to be substituted.
        /// </param>
        /// <returns>
        /// The substitution word.
        /// </returns>
        [DebuggerHidden]
        private static UInt32 SubstituteWord(UInt32 inputWord)
        {
            var inputBytes = BitConverter.GetBytes(inputWord);
            var substitutionBytes = new Byte[4] { SubstitutionBox[inputBytes[2]], SubstitutionBox[inputBytes[1]], SubstitutionBox[inputBytes[3]], SubstitutionBox[inputBytes[0]] };
            return BitConverter.ToUInt32(substitutionBytes, 0);
        }

        /// <summary>
        /// Initializes an instance of an <see cref="Rfc2898DeriveBytes" /> using the leading bytes of the key source.
        /// </summary>
        /// <returns>
        /// An instance of an <see cref="Rfc2898DeriveBytes" />.
        /// </returns>
        [DebuggerHidden]
        private Rfc2898DeriveBytes InitializePbkdf2Algorithm()
        {
            var algorithm = (Rfc2898DeriveBytes)null;

            KeySource.Access(memory =>
            {
                var memorySpan = memory.ReadOnlySpan;
                var iterationSumBytes = memorySpan.Slice(0, Pbkdf2IterationSumLengthInBytes);
                var saltBytes = memorySpan.Slice(Pbkdf2IterationSumLengthInBytes, Pbkdf2SaltLengthInBytes);
                var passwordBytes = memorySpan.Slice(Pbkdf2IterationSumLengthInBytes + Pbkdf2SaltLengthInBytes, Pbkdf2PasswordLengthInBytes);
                var iterationCount = Pbkdf2MinimumIterationCount;

                foreach (var iterationSumValue in iterationSumBytes)
                {
                    iterationCount += iterationSumValue;
                }

                algorithm = new Rfc2898DeriveBytes(passwordBytes.ToArray(), saltBytes.ToArray(), iterationCount);
            });

            return algorithm;
        }

        /// <summary>
        /// Gets the cryptographic algorithm for which a key is derived.
        /// </summary>
        public TAlgorithm Algorithm
        {
            get;
        }

        /// <summary>
        /// Gets the substitution bytes that are used to fulfill key derivation operations when <see cref="DerivationMode" /> is
        /// equal to <see cref="CryptographicKeyDerivationMode.XorLayeringWithSubstitution" />.
        /// </summary>
        /// <remarks>
        /// This property is exposed for testing.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static IEnumerable<Byte> SubstitutionBoxBytes => SubstitutionBox;

        /// <summary>
        /// Gets the PBKDF2 algorithm provider for the current <see cref="CryptographicKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Rfc2898DeriveBytes Pbkdf2Provider => LazyPbkdf2Provider.Value;

        /// <summary>
        /// Represents the byte index of the algorithm byte within an unencrypted, serialized bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 AlgorithmSecureMemoryIndex = (KeySourceSecureMemoryIndex + KeySourceLengthInBytes);

        /// <summary>
        /// Represents the default derivation mode for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const CryptographicKeyDerivationMode DefaultDerivationMode = CryptographicKeyDerivationMode.Pbkdf2;

        /// <summary>
        /// Represents the byte index of the derivation mode byte within an unencrypted, serialized bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 DerivationModeSecureMemoryIndex = (AlgorithmSecureMemoryIndex + AlgorithmLength);

        /// <summary>
        /// Represents the number of bytes comprising <see cref="KeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 KeySourceLengthInBytes = (KeySourceWordCount * sizeof(UInt32));

        /// <summary>
        /// Represents the byte index of the key source within an unencrypted, serialized bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 KeySourceSecureMemoryIndex = 0;

        /// <summary>
        /// Represents the number of bytes comprising a post-encrypted, serialized representation of a
        /// <see cref="CryptographicKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SerializedLength = (SerializedPlaintextLength + (SecureMemoryEncryptionAlgorithmBlockSizeInBytes * 2) - AlgorithmLength - DerivationModeLength);

        /// <summary>
        /// Represents the number of bytes comprising a pre-encrypted, serialized representation of a
        /// <see cref="CryptographicKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SerializedPlaintextLength = (KeySourceLengthInBytes + AlgorithmLength + DerivationModeLength);

        /// <summary>
        /// Represents the key for the cryptographic key algorithm that is used to obscure serialized bit fields.
        /// </summary>
        /// <remarks>
        /// The author acknowledges that obscurity does not ensure security. Encrypting sensitive information with a known key does
        /// not secure it. This is intended to stand up a barrier against unsophisticated attacks targeting users who have
        /// mistakenly exposed their key source. This sequence is fairly arbitrary and can be modified if needed, but there are
        /// probably few good reasons to.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly PinnedMemory SecureMemoryEncryptionKey = new PinnedMemory(new Byte[]
        {
            0xaa, 0xf0, 0xcc, 0xff, 0x00, 0x33, 0x0f, 0x55, 0xff, 0xcc, 0xf0, 0xaa, 0x55, 0x0f, 0x33, 0x00
        });

        /// <summary>
        /// Represents the byte length of <see cref="Algorithm" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AlgorithmLength = sizeof(SymmetricAlgorithmSpecification);

        /// <summary>
        /// Represents the byte length of <see cref="DerivationMode" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DerivationModeLength = sizeof(CryptographicKeyDerivationMode);

        /// <summary>
        /// Represents the exact number of 32-bit key words that are derived from <see cref="KeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 KeySourceWordCount = 96;

        /// <summary>
        /// Represents the number of key source bytes constituting the iteration sum for PBKDF2-derived keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Pbkdf2IterationSumLengthInBytes = (KeySourceLengthInBytes / 3);

        /// <summary>
        /// Represents the number of key source bytes constituting the password for PBKDF2-derived keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Pbkdf2PasswordLengthInBytes = (KeySourceLengthInBytes / 3);

        /// <summary>
        /// Represents the number of key source bytes constituting the salt for PBKDF2-derived keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Pbkdf2SaltLengthInBytes = (KeySourceLengthInBytes / 3);

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="CryptographicKey{TAlgorithm}" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new StaticMemberFinalizer(FinalizeStaticMembers);

        /// <summary>
        /// Represents the substitution bytes that are used to fulfill key derivation operations when <see cref="DerivationMode" />
        /// is equal to <see cref="CryptographicKeyDerivationMode.XorLayeringWithSubstitution" />.
        /// </summary>
        /// <remarks>
        /// This sequence is deliberately and carefully balanced. Modifications can introduce severe security flaws and break the
        /// class functionally. Do not modify without very careful consideration and extremely good reason.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] SubstitutionBox =
        {
            // IMPORTANT Read the remarks above. Inform senior members of the team if a pull request introduces changes here.
            0xff, 0xf0, 0xfd, 0xf2, 0xfb, 0xf4, 0xf9, 0xf6, 0xf7, 0xf8, 0xf5, 0xfa, 0xf3, 0xfc, 0xf1, 0xfe,
            0x0f, 0x00, 0x0d, 0x02, 0x0b, 0x04, 0x09, 0x06, 0x07, 0x08, 0x05, 0x0a, 0x03, 0x0c, 0x01, 0x0e,
            0x4f, 0x40, 0x4d, 0x42, 0x4b, 0x44, 0x49, 0x46, 0x47, 0x48, 0x45, 0x4a, 0x43, 0x4c, 0x41, 0x4e,
            0x2f, 0x20, 0x2d, 0x22, 0x2b, 0x24, 0x29, 0x26, 0x27, 0x28, 0x25, 0x2a, 0x23, 0x2c, 0x21, 0x2e,
            0xbf, 0xb0, 0xbd, 0xb2, 0xbb, 0xb4, 0xb9, 0xb6, 0xb7, 0xb8, 0xb5, 0xba, 0xb3, 0xbc, 0xb1, 0xbe,
            0x9f, 0x90, 0x9d, 0x92, 0x9b, 0x94, 0x99, 0x96, 0x97, 0x98, 0x95, 0x9a, 0x93, 0x9c, 0x91, 0x9e,
            0xdf, 0xd0, 0xdd, 0xd2, 0xdb, 0xd4, 0xd9, 0xd6, 0xd7, 0xd8, 0xd5, 0xda, 0xd3, 0xdc, 0xd1, 0xde,
            0x6f, 0x60, 0x6d, 0x62, 0x6b, 0x64, 0x69, 0x66, 0x67, 0x68, 0x65, 0x6a, 0x63, 0x6c, 0x61, 0x6e,
            0x7f, 0x70, 0x7d, 0x72, 0x7b, 0x74, 0x79, 0x76, 0x77, 0x78, 0x75, 0x7a, 0x73, 0x7c, 0x71, 0x7e,
            0xcf, 0xc0, 0xcd, 0xc2, 0xcb, 0xc4, 0xc9, 0xc6, 0xc7, 0xc8, 0xc5, 0xca, 0xc3, 0xcc, 0xc1, 0xce,
            0x8f, 0x80, 0x8d, 0x82, 0x8b, 0x84, 0x89, 0x86, 0x87, 0x88, 0x85, 0x8a, 0x83, 0x8c, 0x81, 0x8e,
            0xaf, 0xa0, 0xad, 0xa2, 0xab, 0xa4, 0xa9, 0xa6, 0xa7, 0xa8, 0xa5, 0xaa, 0xa3, 0xac, 0xa1, 0xae,
            0x3f, 0x30, 0x3d, 0x32, 0x3b, 0x34, 0x39, 0x36, 0x37, 0x38, 0x35, 0x3a, 0x33, 0x3c, 0x31, 0x3e,
            0x5f, 0x50, 0x5d, 0x52, 0x5b, 0x54, 0x59, 0x56, 0x57, 0x58, 0x55, 0x5a, 0x53, 0x5c, 0x51, 0x5e,
            0x1f, 0x10, 0x1d, 0x12, 0x1b, 0x14, 0x19, 0x16, 0x17, 0x18, 0x15, 0x1a, 0x13, 0x1c, 0x11, 0x1e,
            0xef, 0xe0, 0xed, 0xe2, 0xeb, 0xe4, 0xe9, 0xe6, 0xe7, 0xe8, 0xe5, 0xea, 0xe3, 0xec, 0xe1, 0xee
        };

        /// <summary>
        /// Represents the number of key blocks for the specified algorithm.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 BlockCount;

        /// <summary>
        /// Represents the number of 32-bit words contained within a key block.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 BlockWordCount;

        /// <summary>
        /// Represents the mode used to derive key bits from the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CryptographicKeyDerivationMode DerivationMode;

        /// <summary>
        /// Represents the number of bytes contained within the derived key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 DerivedKeyLength;

        /// <summary>
        /// Represents a bit field that is used to derive key bits from the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ISecureMemory KeySource;

        /// <summary>
        /// Represents the lazily-initialized PBKDF2 algorithm provider for the current <see cref="CryptographicKey{TAlgorithm}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Rfc2898DeriveBytes> LazyPbkdf2Provider;
    }

    /// <summary>
    /// Represents a cryptographic algorithm and key bits.
    /// </summary>
    /// <remarks>
    /// <see cref="CryptographicKey" /> is the default implementation of <see cref="ICryptographicKey" />.
    /// </remarks>
    public abstract class CryptographicKey : Instrument, ICryptographicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKey" /> class.
        /// </summary>
        [DebuggerHidden]
        protected CryptographicKey()
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            return;
        }

        /// <summary>
        /// Converts the value of the current <see cref="CryptographicKey" /> to a secure bit field.
        /// </summary>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="CryptographicKey" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        public ISecureMemory ToSecureMemory()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return ToSecureMemory(controlToken);
            }
        }

        /// <summary>
        /// Generates private key source bytes using the specified, arbitrary length bit field.
        /// </summary>
        /// <param name="keyMaterial">
        /// A non-empty byte array from which the cryptographic key is derived.
        /// </param>
        /// <param name="keySourceLengthInBytes">
        /// The desired number of key source bytes.
        /// </param>
        /// <returns>
        /// The key source bytes.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMaterial" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMaterial" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keySourceLengthInBytes" /> is less than or equal to zero.
        /// </exception>
        [DebuggerHidden]
        internal static PinnedMemory DeriveKeySourceBytesFromKeyMaterial(Byte[] keyMaterial, Int32 keySourceLengthInBytes)
        {
            var hashingProcessor = new HashingProcessor(RandomnessProvider);

            using (var saltBytes = new ReadOnlyPinnedMemory(hashingProcessor.CalculateHash(keyMaterial, PasswordSaltHashingAlgorithm)))
            {
                var pbkdf2Provider = new Rfc2898DeriveBytes(keyMaterial, saltBytes, Pbkdf2MinimumIterationCount);
                return new PinnedMemory(pbkdf2Provider.GetBytes(keySourceLengthInBytes.RejectIf().IsLessThanOrEqualTo(0, nameof(keySourceLengthInBytes))));
            }
        }

        /// <summary>
        /// Generates private key source bytes using the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the cryptographic key is derived.
        /// </param>
        /// <param name="keySourceLengthInBytes">
        /// The desired number of key source bytes.
        /// </param>
        /// <returns>
        /// The key source bytes.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keySourceLengthInBytes" /> is less than or equal to zero.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        internal static PinnedMemory DeriveKeySourceBytesFromPassword(IPassword password, Int32 keySourceLengthInBytes)
        {
            _ = password.RejectIf().IsNull(nameof(password)).OrIf(argument => argument.GetCharacterLength() < MinimumPasswordLength, nameof(password), $"The specified password is shorter than {MinimumPasswordLength} characters.");

            using (var passwordBytes = new ReadOnlyPinnedMemory(Password.GetPasswordPlaintextBytes(password)))
            {
                return DeriveKeySourceBytesFromKeyMaterial(passwordBytes, keySourceLengthInBytes);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CryptographicKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Converts the value of the current <see cref="CryptographicKey" /> to a secure bit field.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="CryptographicKey" />.
        /// </returns>
        protected abstract ISecureMemory ToSecureMemory(IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicKey" /> can be used to digitally sign
        /// information using asymmetric key cryptography.
        /// </summary>
        public Boolean SupportsDigitalSignature => Usage.HasFlag(CryptographicComponentUsage.DigitalSignature);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicKey" /> can be used to securely exchange
        /// symmetric keys with remote parties.
        /// </summary>
        public Boolean SupportsKeyExchange => Usage.HasFlag(CryptographicComponentUsage.KeyExchange);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="CryptographicKey" /> can be used to encrypt or decrypt
        /// information using symmetric key cryptography.
        /// </summary>
        public Boolean SupportsSymmetricKeyEncryption => Usage.HasFlag(CryptographicComponentUsage.SymmetricKeyEncryption);

        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="CryptographicKey" />.
        /// </summary>
        public abstract CryptographicComponentUsage Usage
        {
            get;
        }

        /// <summary>
        /// Represents the minimum allowable length, in characters, of a password.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 MinimumPasswordLength = 13;

        /// <summary>
        /// Represents the hashing algorithm that is used to produce salt bytes from a password.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const HashingAlgorithmSpecification PasswordSaltHashingAlgorithm = HashingAlgorithmSpecification.ShaTwo512;

        /// <summary>
        /// Represents the minimum number of iterations for PBKDF2-derived keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 Pbkdf2MinimumIterationCount = 16384;

        /// <summary>
        /// Represents the cryptographic key algorithm that is used to obscure serialized bit fields.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const SymmetricAlgorithmSpecification SecureMemoryEncryptionAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

        /// <summary>
        /// Represents the block size, in bytes, for the cryptographic key algorithm that is used to obscure serialized bit fields.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SecureMemoryEncryptionAlgorithmBlockSizeInBytes = 16;

        /// <summary>
        /// Represents the default lifespan for a cryptographic key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan DefaultLifespanDuration = TimeSpan.FromDays(90);

        /// <summary>
        /// Represents the minimum allowable lifespan for a cryptographic key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan MinimumLifespanDuration = TimeSpan.FromSeconds(8);

        /// <summary>
        /// Represents the encoding that is used when evaluating passwords.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Encoding PasswordEncoding = Encoding.Unicode;

        /// <summary>
        /// Represents a static random number generator instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly RandomNumberGenerator RandomnessProvider = HardenedRandomNumberGenerator.Instance;
    }
}