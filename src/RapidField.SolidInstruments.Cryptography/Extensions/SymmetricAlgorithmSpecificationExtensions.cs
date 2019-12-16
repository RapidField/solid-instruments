// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Cryptography.Symmetric.Aes;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="SymmetricAlgorithmSpecification" /> enumeration with cryptographic features.
    /// </summary>
    internal static class SymmetricAlgorithmSpecificationExtensions
    {
        /// <summary>
        /// Returns a new <see cref="SymmetricKeyCipher" /> matching the current <see cref="SymmetricAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="SymmetricAlgorithmSpecification" />.
        /// </param>
        /// <param name="randomnessProvider">
        /// A random number generator used to generate initialization vectors.
        /// </param>
        /// <returns>
        /// A new <see cref="SymmetricKeyCipher" /> matching the current <see cref="SymmetricAlgorithmSpecification" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="randomnessProvider" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal static SymmetricKeyCipher ToCipher(this SymmetricAlgorithmSpecification target, RandomNumberGenerator randomnessProvider) => target switch
        {
            SymmetricAlgorithmSpecification.Unspecified => null,
            SymmetricAlgorithmSpecification.Aes128Cbc => new Aes128CbcCipher(randomnessProvider),
            SymmetricAlgorithmSpecification.Aes128Ecb => new Aes128EcbCipher(randomnessProvider),
            SymmetricAlgorithmSpecification.Aes256Cbc => new Aes256CbcCipher(randomnessProvider),
            SymmetricAlgorithmSpecification.Aes256Ecb => new Aes256EcbCipher(randomnessProvider),
            _ => throw new ArgumentException($"{target} is not a supported {nameof(SymmetricAlgorithmSpecification)}.", nameof(target))
        };

        /// <summary>
        /// Returns the key bit-length component of the current <see cref="SymmetricAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="SymmetricAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the key for the current <see cref="SymmetricAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToKeyBitLength(this SymmetricAlgorithmSpecification target) => target switch
        {
            SymmetricAlgorithmSpecification.Unspecified => default,
            SymmetricAlgorithmSpecification.Aes128Cbc => 128,
            SymmetricAlgorithmSpecification.Aes128Ecb => 128,
            SymmetricAlgorithmSpecification.Aes256Cbc => 256,
            SymmetricAlgorithmSpecification.Aes256Ecb => 256,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(SymmetricAlgorithmSpecification)}.", nameof(target))
        };
    }
}