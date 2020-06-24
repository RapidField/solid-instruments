// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetric-key algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="SymmetricKey" /> is the default implementation of <see cref="ISymmetricKey" />.
    /// </remarks>
    public sealed class SymmetricKey : CryptographicKey<SymmetricAlgorithmSpecification>, ISymmetricKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricKey" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm for which a key is derived.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> or
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private SymmetricKey(SymmetricAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource)
            : base(algorithm, derivationMode, keySource, algorithm.ToKeyBitLength())
        {
            return;
        }

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
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
        public static SymmetricKey FromPassword(IPassword password) => FromPassword(password, DefaultAlgorithm);

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static SymmetricKey FromPassword(IPassword password, SymmetricAlgorithmSpecification algorithm) => FromPassword(password, algorithm, DefaultDerivationMode);

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified password.
        /// </summary>
        /// <param name="password">
        /// A Unicode password with length greater than or equal to thirteen characters from which the symmetric key is derived.
        /// </param>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> -or-
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="password" /> is disposed.
        /// </exception>
        [DebuggerHidden]
        public static SymmetricKey FromPassword(IPassword password, SymmetricAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode)
        {
            using (var keySource = DeriveKeySourceBytesFromPassword(password, KeySourceLengthInBytes))
            {
                return New(algorithm.RejectIf().IsEqualToValue(SymmetricAlgorithmSpecification.Unspecified, nameof(algorithm)), derivationMode.RejectIf().IsEqualToValue(CryptographicKeyDerivationMode.Unspecified, nameof(derivationMode)), keySource);
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="SymmetricKey" /> using the specified secure bit field.
        /// </summary>
        /// <param name="secureMemory">
        /// A secure bit field containing a <see cref="SymmetricKey" />.
        /// </param>
        /// <returns>
        /// A new instance of a <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="secureMemory" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secureMemory" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public static SymmetricKey FromSecureMemory(ISecureMemory secureMemory)
        {
            secureMemory.RejectIf().IsNull(nameof(secureMemory)).OrIf(argument => argument.LengthInBytes != SerializedLength, nameof(secureMemory), "The specified memory field is invalid.");

            try
            {
                var key = (SymmetricKey)null;

                secureMemory.Access(pinnedCiphertext =>
                {
                    using (var plaintextBuffer = new PinnedMemory(SerializedPlaintextLength, true))
                    {
                        using (var cipher = SecureMemoryEncryptionAlgorithm.ToCipher(RandomnessProvider))
                        {
                            using (var plaintext = cipher.Decrypt(pinnedCiphertext, SecureMemoryEncryptionKey))
                            {
                                Array.Copy(plaintext, 0, plaintextBuffer, 0, SerializedPlaintextLength);
                            }
                        }

                        using (var keySource = new PinnedMemory(KeySourceLengthInBytes, true))
                        {
                            Array.Copy(plaintextBuffer, KeySourceSecureMemoryIndex, keySource, 0, KeySourceLengthInBytes);
                            var algorithm = (SymmetricAlgorithmSpecification)plaintextBuffer[AlgorithmSecureMemoryIndex];
                            var derivationMode = (CryptographicKeyDerivationMode)plaintextBuffer[DerivationModeSecureMemoryIndex];
                            key = new SymmetricKey(algorithm, derivationMode, keySource);
                        }
                    }
                });

                return key;
            }
            catch (Exception exception)
            {
                throw new ArgumentException("The specified memory field is invalid.", nameof(secureMemory), exception);
            }
        }

        /// <summary>
        /// Generates a new <see cref="SymmetricKey" />.
        /// </summary>
        /// <remarks>
        /// Keys generated by this method employ <see cref="CryptographicKeyDerivationMode.XorLayeringWithSubstitution" /> for key
        /// derivation and <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" /> for transformation.
        /// </remarks>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        [DebuggerHidden]
        public static SymmetricKey New() => New(DefaultAlgorithm);

        /// <summary>
        /// Generates a new <see cref="SymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public static SymmetricKey New(SymmetricAlgorithmSpecification algorithm) => New(algorithm, DefaultDerivationMode);

        /// <summary>
        /// Generates a new <see cref="SymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> -or-
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public static SymmetricKey New(SymmetricAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode)
        {
            using (var keySource = new PinnedMemory(KeySourceLengthInBytes, true))
            {
                RandomnessProvider.GetBytes(keySource);
                return New(algorithm, derivationMode, keySource);
            }
        }

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified, arbitrary length bit field.
        /// </summary>
        /// <param name="keyMaterial">
        /// A non-empty byte array from which the symmetric key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMaterial" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMaterial" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static SymmetricKey FromKeyMaterial(Byte[] keyMaterial) => FromKeyMaterial(keyMaterial, DefaultAlgorithm);

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified, arbitrary length bit field.
        /// </summary>
        /// <param name="keyMaterial">
        /// A non-empty byte array from which the symmetric key is derived.
        /// </param>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMaterial" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMaterial" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal static SymmetricKey FromKeyMaterial(Byte[] keyMaterial, SymmetricAlgorithmSpecification algorithm) => FromKeyMaterial(keyMaterial, algorithm, DefaultDerivationMode);

        /// <summary>
        /// Derives a new <see cref="SymmetricKey" /> from the specified, arbitrary length bit field.
        /// </summary>
        /// <param name="keyMaterial">
        /// A non-empty byte array from which the symmetric key is derived.
        /// </param>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMaterial" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMaterial" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> -or-
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal static SymmetricKey FromKeyMaterial(Byte[] keyMaterial, SymmetricAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode)
        {
            using (var keySource = DeriveKeySourceBytesFromKeyMaterial(keyMaterial, KeySourceLengthInBytes))
            {
                return New(algorithm.RejectIf().IsEqualToValue(SymmetricAlgorithmSpecification.Unspecified, nameof(algorithm)), derivationMode.RejectIf().IsEqualToValue(CryptographicKeyDerivationMode.Unspecified, nameof(derivationMode)), keySource);
            }
        }

        /// <summary>
        /// Generates a new <see cref="SymmetricKey" />.
        /// </summary>
        /// <param name="algorithm">
        /// The symmetric-key algorithm that the generated key is derived to interoperate with. The default value is
        /// <see cref="SymmetricAlgorithmSpecification.Aes256Cbc" />.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the generated key. The default value is <see cref="CryptographicKeyDerivationMode.Pbkdf2" />.
        /// </param>
        /// <param name="keySource">
        /// A bit field comprising 384 bytes (3,072 bits) from which the private key is derived.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> is not 384 bytes in length.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="SymmetricAlgorithmSpecification.Unspecified" /> -or-
        /// <paramref name="derivationMode" /> is equal to <see cref="CryptographicKeyDerivationMode.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal static SymmetricKey New(SymmetricAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource) => new SymmetricKey(algorithm, derivationMode, keySource.RejectIf(argument => argument.Length != KeySourceLengthInBytes, nameof(keySource), $"The key source is not {KeySourceLengthInBytes} bytes in length."));

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SymmetricKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the default symmetric-key algorithm specification for new keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SymmetricAlgorithmSpecification DefaultAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
    }
}