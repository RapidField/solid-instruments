// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Aes
{
    /// <summary>
    /// Represents the AES symmetric-key encryption cipher using a 128-bit key in Cipher Block Chaining (CBC) mode.
    /// </summary>
    internal sealed class Aes128CbcCipher : AesCipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Aes128CbcCipher" /> class.
        /// </summary>
        /// <param name="randomnessProvider">
        /// A random number generator used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public Aes128CbcCipher(RandomNumberGenerator randomnessProvider)
            : base(KeySizeSetting, ModeSetting, randomnessProvider)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Aes128CbcCipher" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the bit-length of the key for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 KeySizeSetting = 128;

        /// <summary>
        /// Represents the encryption mode for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const CipherMode ModeSetting = CipherMode.CBC;
    }
}