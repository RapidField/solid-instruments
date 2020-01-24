// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Cryptography.Symmetric.Aes;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory and encrypted at rest.
    /// </summary>
    public class SecureBuffer : Instrument, ISecureBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureBuffer" /> class.
        /// </summary>
        /// <param name="lengthInBytes">
        /// The length of the buffer, in bytes.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="lengthInBytes" /> is less than or equal to zero.
        /// </exception>
        public SecureBuffer(Int32 lengthInBytes)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Cipher = new Aes128EcbCipher(RandomnessProvider);
            LengthInBytes = lengthInBytes.RejectIf().IsLessThanOrEqualTo(0, nameof(lengthInBytes));
            PrivateKey = new PinnedBuffer(Cipher.KeySizeInBytes, true);
            RandomnessProvider.GetBytes(PrivateKey);

            using (var initializationVector = new PinnedBuffer(Cipher.BlockSizeInBytes, true))
            {
                RandomnessProvider.GetBytes(initializationVector);

                using (var plaintext = new PinnedBuffer(lengthInBytes))
                {
                    Ciphertext = Cipher.Encrypt(plaintext, PrivateKey, initializationVector);
                }
            }
        }

        /// <summary>
        /// Generates cryptographically secure pseudo-random bytes that are pinned in memory and encrypted at rest.
        /// </summary>
        /// <remarks>
        /// This method is useful for generating keys and other sensitive random values.
        /// </remarks>
        /// <param name="lengthInBytes">
        /// The length of the random buffer, in bytes.
        /// </param>
        /// <returns>
        /// A cryptographically secure pseudo-random byte array of specified length.
        /// </returns>
        public static ISecureBuffer GenerateHardenedRandomBytes(Int32 lengthInBytes)
        {
            var hardenedRandomBytes = new SecureBuffer(lengthInBytes);
            hardenedRandomBytes.Access((pinnedBuffer) =>
            {
                RandomnessProvider.GetBytes(pinnedBuffer);
            });

            return hardenedRandomBytes;
        }

        /// <summary>
        /// Decrypts the buffer, performs the specified operation against the pinned plaintext and encrypts the buffer as a
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
        public void Access(Action<PinnedBuffer> action)
        {
            action = action.RejectIf().IsNull(nameof(action));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                using (var plaintext = new PinnedBuffer(LengthInBytes, true))
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
        /// Releases all resources consumed by the current <see cref="SecureBuffer" />.
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
        /// Decrypts the current buffer and writes the result to the specified buffer.
        /// </summary>
        /// <param name="plaintext">
        /// The buffer to which the plaintext result is written.
        /// </param>
        [DebuggerHidden]
        private void DecryptField(PinnedBuffer plaintext)
        {
            using (var buffer = Cipher.Decrypt(Ciphertext, PrivateKey))
            {
                buffer.Span.CopyTo(plaintext);
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext and writes the result to the current buffer.
        /// </summary>
        /// <param name="plaintext">
        /// The buffer containing the plaintext to encrypt.
        /// </param>
        [DebuggerHidden]
        private void EncryptField(PinnedBuffer plaintext)
        {
            using (var initializationVector = new PinnedBuffer(Cipher.BlockSizeInBytes, true))
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
        /// Gets the length of the buffer, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }

        /// <summary>
        /// Represents a cipher that is used to encrypt and decrypt the buffer field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SymmetricKeyCipher Cipher;

        /// <summary>
        /// Represents the ciphertext bits for the buffer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedBuffer Ciphertext;

        /// <summary>
        /// Represents a private key that is used to encrypt and decrypt the buffer field.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedBuffer PrivateKey;
    }
}