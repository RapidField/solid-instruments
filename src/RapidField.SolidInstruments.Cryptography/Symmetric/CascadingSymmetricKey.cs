// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a series of <see cref="ISymmetricKey" /> instances that constitute instructions for applying cascading encryption
    /// and decryption.
    /// </summary>
    /// <remarks>
    /// <see cref="CascadingSymmetricKey" /> is the default implementation of <see cref="ICascadingSymmetricKey" />.
    /// </remarks>
    public sealed class CascadingSymmetricKey : CryptographicKey, ICascadingSymmetricKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSymmetricKey" /> class.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys.
        /// </param>
        /// <param name="algorithms">
        /// The layered algorithm specifications.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private CascadingSymmetricKey(CryptographicKeyDerivationMode derivationMode, params SymmetricAlgorithmSpecification[] algorithms)
            : base()
        {
            var keys = new SymmetricKey[algorithms.Length];

            for (var i = 0; i < algorithms.Length; i++)
            {
                keys[i] = SymmetricKey.New(algorithms[i], derivationMode);
            }

            Keys = keys;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSymmetricKey" /> class.
        /// </summary>
        /// <param name="keys">
        /// An ordered array of keys comprising the cascading key.
        /// </param>
        [DebuggerHidden]
        private CascadingSymmetricKey(ISymmetricKey[] keys)
            : base()
        {
            Keys = keys;
        }

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey FromPassword(IPassword password) => FromPassword(password, DefaultDerivationMode, DefaultFirstLayerAlgorithm, DefaultSecondLayerAlgorithm);

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second (outer) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" />.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey FromPassword(IPassword password, CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm });

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey FromPassword(IPassword password, CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm });

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third layer of encryption.
        /// </param>
        /// <param name="fourthLayerAlgorithm">
        /// The algorithm for the fourth (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey FromPassword(IPassword password, CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm, SymmetricAlgorithmSpecification fourthLayerAlgorithm) => FromPassword(password, derivationMode, new SymmetricAlgorithmSpecification[] { firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm });

        /// <summary>
        /// Creates a new instance of a <see cref="CascadingSymmetricKey" /> using the specified secure bit field.
        /// </summary>
        /// <param name="secureMemory">
        /// A secure bit field containing a <see cref="CascadingSymmetricKey" />.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="secureMemory" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secureMemory" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey FromSecureMemory(ISecureMemory secureMemory)
        {
            secureMemory.RejectIf().IsNull(nameof(secureMemory)).OrIf(argument => argument.LengthInBytes != SerializedLength, nameof(secureMemory), "The specified memory field is invalid.");

            try
            {
                var keys = (ISymmetricKey[])null;

                secureMemory.Access(memory =>
                {
                    // Interrogate the final 16 bits to determine the depth.
                    var keyLength = SymmetricKey.SerializedLength;
                    var depth = BitConverter.ToUInt16(memory, SerializedLength - sizeof(UInt16));
                    keys = new ISymmetricKey[depth];

                    for (var i = 0; i < depth; i++)
                    {
                        using (var secureMemory = new SecureMemory(keyLength))
                        {
                            secureMemory.Access(key =>
                            {
                                // Copy out the key buffers.
                                Array.Copy(memory, keyLength * i, key, 0, keyLength);
                            });

                            keys[i] = SymmetricKey.FromSecureMemory(secureMemory);
                        }
                    }
                });

                return new CascadingSymmetricKey(keys);
            }
            catch
            {
                throw new ArgumentException("The specified memory field is invalid.", nameof(secureMemory));
            }
        }

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <remarks>
        /// Keys generated by this method employ <see cref="CryptographicKeyDerivationMode.XorLayeringWithSubstitution" /> for key
        /// derivation, <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" /> for first (inner) layer transformation and
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" /> for second (outer) layer transformation.
        /// </remarks>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        [DebuggerHidden]
        public static CascadingSymmetricKey New() => new CascadingSymmetricKey(DefaultDerivationMode, DefaultFirstLayerAlgorithm, DefaultSecondLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second (outer) layer of encryption. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes128Ecb" />.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey New(CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey New(CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="firstLayerAlgorithm">
        /// The algorithm for the first (inner-most) layer of encryption.
        /// </param>
        /// <param name="secondLayerAlgorithm">
        /// The algorithm for the second layer of encryption.
        /// </param>
        /// <param name="thirdLayerAlgorithm">
        /// The algorithm for the third layer of encryption.
        /// </param>
        /// <param name="fourthLayerAlgorithm">
        /// The algorithm for the fourth (outer-most) layer of encryption.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public static CascadingSymmetricKey New(CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm, SymmetricAlgorithmSpecification fourthLayerAlgorithm) => new CascadingSymmetricKey(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CascadingSymmetricKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (Keys?.Any() ?? false)
                {
                    foreach (var key in Keys)
                    {
                        key.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="CascadingSymmetricKey" /> to a secure bit field.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="CryptographicKey" />.
        /// </returns>
        protected sealed override ISecureMemory ToSecureMemory(IConcurrencyControlToken controlToken)
        {
            var secureMemory = new SecureMemory(SerializedLength);

            try
            {
                secureMemory.Access(pinnedMemory =>
                {
                    var keyLength = SymmetricKey.SerializedLength;

                    for (var i = 0; i < MaximumDepth; i++)
                    {
                        if (i < Depth)
                        {
                            using (var keyMemory = Keys.ElementAt(i).ToSecureMemory())
                            {
                                keyMemory.Access(key =>
                                {
                                    // Copy the key buffers out to the result buffer.
                                    Array.Copy(key, 0, pinnedMemory, keyLength * i, keyLength);
                                });
                            }

                            continue;
                        }

                        // Fill the unused segments with random bytes.
                        var randomBytes = new Byte[keyLength];
                        HardenedRandomNumberGenerator.Instance.GetBytes(randomBytes);
                        Array.Copy(randomBytes, 0, pinnedMemory, keyLength * i, keyLength);
                    }

                    // Append the depth as a 16-bit integer.
                    Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(Depth)), 0, pinnedMemory, SerializedLength - sizeof(UInt16), sizeof(UInt16));
                });

                return secureMemory;
            }
            catch
            {
                secureMemory.Dispose();
                throw new SecurityException("Key serialization failed.");
            }
        }

        /// <summary>
        /// Derives a new <see cref="CascadingSymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated keys. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="algorithms">
        /// An ordered array of algorithms.
        /// </param>
        /// <returns>
        /// A new <see cref="CascadingSymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" /> -or- <paramref name="algorithms" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" /> -or- one or
        /// more algorithm layers are equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        private static CascadingSymmetricKey FromPassword(IPassword password, CryptographicKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification[] algorithms)
        {
            var keyDepth = algorithms.RejectIf().IsNullOrEmpty(nameof(algorithms)).TargetArgument.Length;
            var singleKeySourceLengthInBytes = SymmetricKey.KeySourceLengthInBytes;
            var totalSourceLengthInBytes = singleKeySourceLengthInBytes * keyDepth;
            var keys = new ISymmetricKey[keyDepth];

            using (var keySourceBytes = DeriveKeySourceBytesFromPassword(password, totalSourceLengthInBytes))
            {
                for (var i = 0; i < keyDepth; i++)
                {
                    var algorithm = algorithms[i];

                    using (var keySource = new PinnedMemory(keySourceBytes.Span.Slice(i * singleKeySourceLengthInBytes, singleKeySourceLengthInBytes).ToArray()))
                    {
                        keys[i] = SymmetricKey.New(algorithm, derivationMode, keySource);
                    }
                }
            }

            return new CascadingSymmetricKey(keys);
        }

        /// <summary>
        /// Gets the number of layers of encryption that a resulting transform will apply.
        /// </summary>
        public Int32 Depth => Keys.Count();

        /// <summary>
        /// Gets the ordered array of keys comprising the cascading key.
        /// </summary>
        public IEnumerable<ISymmetricKey> Keys
        {
            get;
        }

        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="CascadingSymmetricKey" />.
        /// </summary>
        public override sealed CryptographicComponentUsage Usage => CryptographicComponentUsage.SymmetricKeyEncryption;

        /// <summary>
        /// Represents the number of bytes comprising a serialized representation of a <see cref="CascadingSymmetricKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SerializedLength = (SymmetricKey.SerializedLength * MaximumDepth) + sizeof(UInt16);

        /// <summary>
        /// Represents the default derivation mode for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const CryptographicKeyDerivationMode DefaultDerivationMode = CryptographicKeyDerivationMode.Pbkdf2;

        /// <summary>
        /// Represents the default inner-layer symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultFirstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

        /// <summary>
        /// Represents the default outer-layer symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultSecondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

        /// <summary>
        /// Represents the maximum number of layers of encryption that a resulting transform can apply.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumDepth = 4;
    }
}