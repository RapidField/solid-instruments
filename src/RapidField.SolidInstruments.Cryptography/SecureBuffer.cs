// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory and encrypted at rest.
    /// </summary>
    public class SecureBuffer : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureBuffer" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the buffer, in bytes.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public SecureBuffer(Int32 length)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Length = length.RejectIf().IsLessThanOrEqualTo(0, nameof(length));
            Entropy = new PinnedBuffer(EntropyLength, true);
            RandomnessProvider.GetBytes(Entropy);
            var ciphertextBytes = ProtectedData.Protect(new Byte[length], Entropy, Scope);
            Ciphertext = new PinnedBuffer(ciphertextBytes.Length, true);
            Array.Copy(ciphertextBytes, Ciphertext, Ciphertext.Length);
        }

        /// <summary>
        /// Generates cryptographically secure pseudo-random bytes that are pinned in memory and encrypted at rest.
        /// </summary>
        /// <remarks>
        /// This method is useful for generating keys and other sensitive random values.
        /// </remarks>
        /// <param name="length">
        /// The length of the random buffer, in bytes.
        /// </param>
        /// <returns>
        /// A cryptographically secure pseudo-random byte array of specified length.
        /// </returns>
        public static SecureBuffer GenerateHardenedRandomBytes(Int32 length)
        {
            var hardenedRandomBytes = new SecureBuffer(length);
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Access(Action<PinnedBuffer> action)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                using (var plaintext = new PinnedBuffer(Length, true))
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
                    Ciphertext.Dispose();
                    Entropy.Dispose();
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
        private void DecryptField(PinnedBuffer plaintext) => Array.Copy(ProtectedData.Unprotect(Ciphertext, Entropy, Scope), plaintext, Length);

        /// <summary>
        /// Encrypts the specified plaintext and writes the result to the current buffer.
        /// </summary>
        /// <param name="plaintext">
        /// The buffer containing the plaintext to encrypt.
        /// </param>
        [DebuggerHidden]
        private void EncryptField(PinnedBuffer plaintext) => Array.Copy(ProtectedData.Protect(plaintext, Entropy, Scope), Ciphertext, Ciphertext.Length);

        /// <summary>
        /// Gets a static, cryptographically secure pseudo-random number generator that hardens platform-generated bytes using
        /// 256-bit AES encryption.
        /// </summary>
        public static RandomNumberGenerator RandomnessProvider => HardenedRandomNumberGenerator.Instance;

        /// <summary>
        /// Represents the length of the buffer, in bytes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 Length;

        /// <summary>
        /// Represents the length, in bytes, of <see cref="Entropy" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 EntropyLength = 16;

        /// <summary>
        /// Represents the data protection scope used by <see cref="DecryptField(PinnedBuffer)" /> and
        /// <see cref="EncryptField(PinnedBuffer)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const DataProtectionScope Scope = DataProtectionScope.CurrentUser;

        /// <summary>
        /// Represents the ciphertext bits for the buffer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedBuffer Ciphertext;

        /// <summary>
        /// Represents the entropy bits used by <see cref="DecryptField(PinnedBuffer)" /> and
        /// <see cref="EncryptField(PinnedBuffer)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedBuffer Entropy;
    }
}