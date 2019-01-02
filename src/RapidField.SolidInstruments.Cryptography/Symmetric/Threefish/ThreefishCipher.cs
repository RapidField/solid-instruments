// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Threefish
{
    /// <summary>
    /// Represents the Threefish symmetric-key encryption cipher.
    /// </summary>
    internal abstract class ThreefishCipher : SymmetricKeyCipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreefishCipher" /> class.
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
        protected ThreefishCipher(Int32 keySize, CipherMode mode, RandomNumberGenerator randomnessProvider)
            : base(keySize, keySize, mode, PaddingModeSetting, randomnessProvider)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ThreefishCipher" />.
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
        /// A new instance of a <see cref="ThreefishCryptoServiceProvider" />.
        /// </returns>
        [DebuggerHidden]
        protected sealed override SymmetricAlgorithm InitializeProvider() => new ThreefishCryptoServiceProvider(RandomnessProvider, TweakWordOne, TweakWordTwo);

        /// <summary>
        /// Represents the padding setting for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const PaddingMode PaddingModeSetting = PaddingMode.PKCS7;

        /// <summary>
        /// Represents the first 64-bit tweak word.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const UInt64 TweakWordOne = 0xf0c31db833caac55;

        /// <summary>
        /// Represents the second 64-bit tweak word.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const UInt64 TweakWordTwo = 0xaa5335cc47e23c0f;
    }
}