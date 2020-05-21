// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Cryptography.Symmetric.Aes;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in bit field and encrypted at rest.
    /// </summary>
    public class SecureMemory : Instrument, ISecureMemory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemory" /> class.
        /// </summary>
        /// <param name="lengthInBytes">
        /// The length of the bit field, in bytes.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="lengthInBytes" /> is less than or equal to zero.
        /// </exception>
        public SecureMemory(Int32 lengthInBytes)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Cipher = new Aes128CbcCipher(RandomnessProvider);
            LengthInBytes = lengthInBytes.RejectIf().IsLessThanOrEqualTo(0, nameof(lengthInBytes));
            PrivateKey = new PinnedMemory(Cipher.KeySizeInBytes, true);
            RandomnessProvider.GetBytes(PrivateKey);

            using (var initializationVector = new PinnedMemory(Cipher.BlockSizeInBytes, true))
            {
                RandomnessProvider.GetBytes(initializationVector);

                using (var plaintext = new PinnedMemory(lengthInBytes))
                {
                    Ciphertext = Cipher.Encrypt(plaintext, PrivateKey, initializationVector);
                }
            }
        }

        /// <summary>
        /// Generates cryptographically secure pseudo-random bytes that are pinned in bit field and encrypted at rest.
        /// </summary>
        /// <remarks>
        /// This method is useful for generating keys and other sensitive random values.
        /// </remarks>
        /// <param name="lengthInBytes">
        /// The length of the random bit field, in bytes.
        /// </param>
        /// <returns>
        /// A cryptographically secure pseudo-random byte array of specified length.
        /// </returns>
        public static ISecureMemory GenerateHardenedRandomBytes(Int32 lengthInBytes)
        {
            var hardenedRandomBytes = new SecureMemory(lengthInBytes);
            hardenedRandomBytes.Access((memory) =>
            {
                RandomnessProvider.GetBytes(memory);
            });

            return hardenedRandomBytes;
        }

        /// <summary>
        /// Decrypts the bit field, performs the specified operation against the pinned plaintext and encrypts the bit field as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="action">
        /// The operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Access(Action<PinnedMemory> action)
        {
            action = action.RejectIf().IsNull(nameof(action));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                using (var plaintext = new PinnedMemory(LengthInBytes, true))
                {
                    DecryptField(plaintext);

                    try
                    {
                        action(plaintext);
                    }
                    finally
                    {
                        EncryptField(plaintext);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => Ciphertext.ComputeThirtyTwoBitHash() ^ 0x3a566a5c;

        /// <summary>
        /// Converts the value of the current <see cref="SecureMemory" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="SecureMemory" />.
        /// </returns>
        public override String ToString() => $"{{ {nameof(LengthInBytes)}: {LengthInBytes} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecureMemory" />.
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
                    Cipher.Dispose();
                    Ciphertext.Dispose();
                    PrivateKey.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Decrypts the current bit field and writes the result to the specified bit field.
        /// </summary>
        /// <param name="plaintext">
        /// The bit field to which the plaintext result is written.
        /// </param>
        [DebuggerHidden]
        private void DecryptField(PinnedMemory plaintext)
        {
            using (var memory = Cipher.Decrypt(Ciphertext, PrivateKey))
            {
                memory.Span.CopyTo(plaintext);
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext and writes the result to the current bit field.
        /// </summary>
        /// <param name="plaintext">
        /// The bit field containing the plaintext to encrypt.
        /// </param>
        [DebuggerHidden]
        private void EncryptField(PinnedMemory plaintext)
        {
            using (var initializationVector = new PinnedMemory(Cipher.BlockSizeInBytes, true))
            {
                RandomnessProvider.GetBytes(initializationVector);

                using (var ciphertext = Cipher.Encrypt(plaintext, PrivateKey, initializationVector))
                {
                    ciphertext.Span.CopyTo(Ciphertext);
                }
            }
        }

        /// <summary>
        /// Gets a static, cryptographically secure pseudo-random number generator that hardens platform-generated bytes using
        /// 256-bit AES encryption.
        /// </summary>
        public static RandomNumberGenerator RandomnessProvider => HardenedRandomNumberGenerator.Instance;

        /// <summary>
        /// Gets the length of the bit field, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }

        /// <summary>
        /// Represents a cipher that is used to encrypt and decrypt the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SymmetricKeyCipher Cipher;

        /// <summary>
        /// Represents the ciphertext bits for the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedMemory Ciphertext;

        /// <summary>
        /// Represents a private key that is used to encrypt and decrypt the bit field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedMemory PrivateKey;
    }
}