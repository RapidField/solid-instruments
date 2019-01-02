// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Aes
{
    /// <summary>
    /// Represents the AES symmetric-key encryption cipher.
    /// </summary>
    internal abstract class AesCipher : SymmetricKeyCipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AesCipher" /> class.
        /// </summary>
        /// <param name="keySize">
        /// The bit-length of the key for the cipher.
        /// </param>
        /// <param name="mode">
        /// The encryption mode for the cipher.
        /// </param>
        /// <param name="randomnessProvider">
        /// A random number generator used to generate initialization vectors.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        protected AesCipher(Int32 keySize, CipherMode mode, RandomNumberGenerator randomnessProvider)
            : base(BlockSizeSetting, keySize, mode, PaddingModeSetting, randomnessProvider)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AesCipher" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Initializes the underlying symmetric-key algorithm provider for this instance.
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="System.Security.Cryptography.Aes" />.
        /// </returns>
        [DebuggerHidden]
        protected sealed override SymmetricAlgorithm InitializeProvider() => System.Security.Cryptography.Aes.Create();

        /// <summary>
        /// Represents the bit-length of a single block for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 BlockSizeSetting = 128;

        /// <summary>
        /// Represents the padding setting for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const PaddingMode PaddingModeSetting = PaddingMode.PKCS7;
    }
}