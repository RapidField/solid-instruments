// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetric-key algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    public sealed class SecureSymmetricKey : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureSymmetricKey" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm for which a key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A buffer that is used to derive key bits.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> or
        /// <paramref name="derivationMode" /> is equal to <see cref="SecureSymmetricKeyDerivationMode.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private SecureSymmetricKey(SymmetricAlgorithmSpecification algorithm, SecureSymmetricKeyDerivationMode derivationMode, PinnedBuffer keySource)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(SymmetricAlgorithmSpecification.Unspecified, nameof(algorithm));
            DerivationMode = derivationMode.RejectIf().IsEqualToValue(SecureSymmetricKeyDerivationMode.Unspecified, nameof(derivationMode));
            KeySource = new SecureBuffer(KeySourceLengthInBytes);
            LazyPbkdf2Provider = new Lazy<Rfc2898DeriveBytes>(InitializePbkdf2Algorithm, LazyThreadSafetyMode.ExecutionAndPublication);

            // Gather information about the derived key.
            var keyBitLength = algorithm.ToKeyBitLength();
            DerivedKeyLength = (keyBitLength / 8);
            BlockWordCount = (keyBitLength / 32);
            BlockCount = (KeySourceWordCount / BlockWordCount);

            // Copy in the key source bits.
            KeySource.Access(buffer => Array.Copy(keySource, buffer, buffer.Length));
        }

        /// <summary>
        /// Creates a new instance of a <see cref="SecureSymmetricKey" /> using the specified buffer.
        /// </summary>
        /// <param name="buffer">
        /// A binary representation of a <see cref="SecureSymmetricKey" />.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="SecureSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="buffer" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.
        /// </exception>
        public static SecureSymmetricKey FromBuffer(SecureBuffer buffer)
        {
            buffer.RejectIf().IsNull(nameof(buffer)).OrIf(argument => argument.Length != SerializedLength, nameof(buffer), "The specified buffer is invalid.");

            try
            {
                var result = (SecureSymmetricKey)null;

                buffer.Access(pinnedCiphertext =>
                {
                    using (var plaintextBuffer = new PinnedBuffer(SerializedPlaintextLength, true))
                    {
                        using (var cipher = BufferEncryptionAlgorithm.ToCipher(RandomnessProvider))
                        {
                            using (var plaintext = cipher.Decrypt(pinnedCiphertext, BufferEncryptionKey))
                            {
                                Array.Copy(plaintext, 0, plaintextBuffer, 0, SerializedPlaintextLength);
                            }
                        }

                        using (var keySource = new PinnedBuffer(KeySourceLengthInBytes, true))
                        {
                            Array.Copy(plaintextBuffer, KeySourceBufferIndex, keySource, 0, KeySourceLengthInBytes);
                            var algorithm = (SymmetricAlgorithmSpecification)plaintextBuffer[AlgorithmBufferIndex];
                            var derivationMode = (SecureSymmetricKeyDerivationMode)plaintextBuffer[DerivationModeBufferIndex];
                            result = new SecureSymmetricKey(algorithm, derivationMode, keySource);
                        }
                    }
                });

                return result;
            }
            catch
            {
                throw new ArgumentException("The specified buffer is invalid.", nameof(buffer));
            }
        }

        /// <summary>
        /// Generates a new <see cref="SecureSymmetricKey" />.
        /// </summary>
        /// <remarks>
        /// Keys generated by this method employ <see cref="SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution" /> for key
        /// derivation and <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" /> for transformation.
        /// </remarks>
        /// <returns>
        /// A new <see cref="SecureSymmetricKey" />.
        /// </returns>
        public static SecureSymmetricKey New() => New(DefaultAlgorithm);

        /// <summary>
        /// Generates a new <see cref="SecureSymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SecureSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        public static SecureSymmetricKey New(SymmetricAlgorithmSpecification algorithm) => New(algorithm, DefaultDerivationMode);

        /// <summary>
        /// Generates a new <see cref="SecureSymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is
        /// <see cref="SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SecureSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> or
        /// <paramref name="derivationMode" /> is equal to <see cref="SecureSymmetricKeyDerivationMode.Unspecified" />.
        /// </exception>
        public static SecureSymmetricKey New(SymmetricAlgorithmSpecification algorithm, SecureSymmetricKeyDerivationMode derivationMode)
        {
            using (var keySource = new PinnedBuffer(KeySourceLengthInBytes, true))
            {
                RandomnessProvider.GetBytes(keySource);
                return New(algorithm, derivationMode, keySource);
            }
        }

        /// <summary>
        /// Generates a new <see cref="SecureSymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is
        /// <see cref="SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution" />.
        /// </param>
        /// <param name="keySource">
        /// A buffer comprising 384 bytes (3,072 bits) from which the private key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="SecureSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> is not 384 bytes in length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> -or-
        /// <paramref name="derivationMode" /> is equal to <see cref="SecureSymmetricKeyDerivationMode.Unspecified" />.
        /// </exception>
        public static SecureSymmetricKey New(SymmetricAlgorithmSpecification algorithm, SecureSymmetricKeyDerivationMode derivationMode, PinnedBuffer keySource)
        {
            keySource.RejectIf(argument => argument.Length != KeySourceLengthInBytes, nameof(keySource), $"The key source is not {KeySourceLengthInBytes} bytes in length.");
            return new SecureSymmetricKey(algorithm, derivationMode, keySource);
        }

        /// <summary>
        /// Converts the value of the current <see cref="SecureSymmetricKey" /> to its equivalent binary representation.
        /// </summary>
        /// <returns>
        /// A binary representation of the current <see cref="SecureSymmetricKey" />.
        /// </returns>
        public SecureBuffer ToBuffer()
        {
            var resultBuffer = new SecureBuffer(SerializedLength);

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    using (var plaintextBuffer = new PinnedBuffer(SerializedPlaintextLength, true))
                    {
                        KeySource.Access(pinnedKeySourceBuffer =>
                        {
                            Array.Copy(pinnedKeySourceBuffer, 0, plaintextBuffer, KeySourceBufferIndex, KeySourceLengthInBytes);
                        });

                        plaintextBuffer[AlgorithmBufferIndex] = (Byte)Algorithm;
                        plaintextBuffer[DerivationModeBufferIndex] = (Byte)DerivationMode;

                        using (var cipher = BufferEncryptionAlgorithm.ToCipher(RandomnessProvider))
                        {
                            using (var initializationVector = new PinnedBuffer(cipher.BlockSizeInBytes, true))
                            {
                                RandomnessProvider.GetBytes(initializationVector);

                                resultBuffer.Access(pinnedResultBuffer =>
                                {
                                    using (var ciphertext = cipher.Encrypt(plaintextBuffer, BufferEncryptionKey, initializationVector))
                                    {
                                        Array.Copy(ciphertext, 0, pinnedResultBuffer, 0, SerializedLength);
                                    }
                                });
                            }
                        }
                    }
                }

                return resultBuffer;
            }
            catch
            {
                resultBuffer.Dispose();
                throw new SecurityException("Key serialization failed.");
            }
        }

        /// <summary>
        /// Converts the current <see cref="SecureSymmetricKey" /> to symmetric-key plaintext with correct bit-length for the
        /// encryption mode specified by <see cref="Algorithm" />.
        /// </summary>
        /// <returns>
        /// The derived key.
        /// </returns>
        [DebuggerHidden]
        internal PinnedBuffer DeriveKey()
        {
            var result = (PinnedBuffer)null;

            try
            {
                using (var controlToken = StateControl.Enter())
                {
                    if (DerivationMode == SecureSymmetricKeyDerivationMode.Pbkdf2)
                    {
                        try
                        {
                            // Perform PBKDF2 key-derivation.
                            result = new PinnedBuffer(Pbkdf2Provider.GetBytes(DerivedKeyLength), true);
                        }
                        finally
                        {
                            Pbkdf2Provider.Reset();
                        }

                        return result;
                    }

                    result = new PinnedBuffer(DerivedKeyLength, true);

                    using (var sourceWords = new PinnedStructureArray<UInt32>(KeySourceWordCount, true))
                    {
                        KeySource.Access((PinnedBuffer buffer) =>
                        {
                            // Convert the source buffer to an array of 32-bit words.
                            Buffer.BlockCopy(buffer, 0, sourceWords, 0, KeySourceLengthInBytes);
                        });

                        using (var transformedWords = new PinnedStructureArray<UInt32>(BlockWordCount, true))
                        {
                            // Copy out the first block. If nothing further is done, this satisfies truncation mode.
                            Array.Copy(sourceWords, transformedWords, BlockWordCount);

                            switch (DerivationMode)
                            {
                                case SecureSymmetricKeyDerivationMode.Truncation:

                                    break;

                                case SecureSymmetricKeyDerivationMode.XorLayering:

                                    for (var i = 1; i < BlockCount; i++)
                                    {
                                        for (var j = 0; j < BlockWordCount; j++)
                                        {
                                            // Perform the XOR layering operation.
                                            transformedWords[j] = (transformedWords[j] ^ sourceWords[(i * BlockWordCount) + j]);
                                        }
                                    }

                                    break;

                                case SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution:

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

                            // Copy out the key bits.
                            Buffer.BlockCopy(transformedWords, 0, result, 0, DerivedKeyLength);
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
        /// Releases all resources consumed by the current <see cref="SecureSymmetricKey" />.
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
                    BufferEncryptionKey.Dispose();
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
            Rfc2898DeriveBytes result = null;

            KeySource.Access(buffer =>
            {
                var iterationSumBuffer = buffer.Take(Pbkdf2IterationSumLengthInBytes);
                var saltBuffer = buffer.Skip(Pbkdf2IterationSumLengthInBytes).Take(Pbkdf2SaltLengthInBytes);
                var passwordBuffer = buffer.Skip(Pbkdf2IterationSumLengthInBytes + Pbkdf2SaltLengthInBytes).Take(Pbkdf2PasswordLengthInBytes);
                var iterationCount = Pbkdf2MinimumIterationCount;

                foreach (var iterationSumValue in iterationSumBuffer)
                {
                    iterationCount += iterationSumValue;
                }

                result = new Rfc2898DeriveBytes(passwordBuffer.ToArray(), saltBuffer.ToArray(), iterationCount);
            });

            return result;
        }

        /// <summary>
        /// Gets the PBKDF2 algorithm provider for the current <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Rfc2898DeriveBytes Pbkdf2Provider => LazyPbkdf2Provider.Value;

        /// <summary>
        /// Represents the number of bytes comprising a post-encrypted, serialized representation of a
        /// <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SerializedLength = (SerializedPlaintextLength + (BufferEncryptionAlgorithmBlockSizeInBytes * 2) - AlgorithmLength - DerivationModeLength);

        /// <summary>
        /// Represents the symmetric-key algorithm for which a key is derived.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly SymmetricAlgorithmSpecification Algorithm;

        /// <summary>
        /// Represents the byte index of the algorithm byte within an unencrypted, serialized buffer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AlgorithmBufferIndex = (KeySourceBufferIndex + KeySourceLengthInBytes);

        /// <summary>
        /// Represents the byte length of <see cref="Algorithm" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AlgorithmLength = sizeof(SymmetricAlgorithmSpecification);

        /// <summary>
        /// Represents the symmetric-key algorithm that is used to obscure serialized buffers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification BufferEncryptionAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

        /// <summary>
        /// Represents the block size, in bytes, for the symmetric-key algorithm that is used to obscure serialized buffers.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 BufferEncryptionAlgorithmBlockSizeInBytes = 16;

        /// <summary>
        /// Represents the default symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

        /// <summary>
        /// Represents the default derivation mode for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SecureSymmetricKeyDerivationMode DefaultDerivationMode = SecureSymmetricKeyDerivationMode.Pbkdf2;

        /// <summary>
        /// Represents the byte index of the derivation mode byte within an unencrypted, serialized buffer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DerivationModeBufferIndex = (AlgorithmBufferIndex + AlgorithmLength);

        /// <summary>
        /// Represents the byte length of <see cref="DerivationMode" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DerivationModeLength = sizeof(SecureSymmetricKeyDerivationMode);

        /// <summary>
        /// Represents the byte index of the key source within an unencrypted, serialized buffer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 KeySourceBufferIndex = 0;

        /// <summary>
        /// Represents the number of bytes comprising <see cref="KeySource" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 KeySourceLengthInBytes = (KeySourceWordCount * sizeof(UInt32));

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
        /// Represents the minimum number of iterations for PBKDF2-derived keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Pbkdf2MinimumIterationCount = 16384;

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
        /// Represents the number of bytes comprising a pre-encrypted, serialized representation of a
        /// <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SerializedPlaintextLength = (KeySourceLengthInBytes + AlgorithmLength + DerivationModeLength);

        /// <summary>
        /// Represents the key for the symmetric-key algorithm that is used to obscure serialized buffers.
        /// </summary>
        /// <remarks>
        /// The author acknowledges that obscurity does not ensure security. Encrypting sensitive information with a known key does
        /// not secure it. This is intended to stand up a barrier against unsophisticated attacks targeting users who have mistakenly
        /// exposed their key source.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly PinnedBuffer BufferEncryptionKey = new PinnedBuffer(new Byte[]
        {
            0xaa, 0xf0, 0xcc, 0xff, 0x00, 0x33, 0x0f, 0x55, 0xff, 0xcc, 0xf0, 0xaa, 0x55, 0x0f, 0x33, 0x00
        });

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="SecureSymmetricKey" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer Finalizer = new StaticMemberFinalizer(BufferEncryptionKey.Dispose);

        /// <summary>
        /// Represents a static random number generator instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly RandomNumberGenerator RandomnessProvider = HardenedRandomNumberGenerator.Instance;

        /// <summary>
        /// Represents substitution bytes that are used to fulfill key derivation operations when <see cref="DerivationMode" /> is
        /// equal to <see cref="SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] SubstitutionBox =
        {
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
        /// Represents the mode used to derive key bits from the current <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SecureSymmetricKeyDerivationMode DerivationMode;

        /// <summary>
        /// Represents the number of bytes contained within the derived key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 DerivedKeyLength;

        /// <summary>
        /// Represents a buffer that is used to derive key bits from the current <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SecureBuffer KeySource;

        /// <summary>
        /// Represents the lazily-initialized PBKDF2 algorithm provider for the current <see cref="SecureSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Rfc2898DeriveBytes> LazyPbkdf2Provider;
    }
}