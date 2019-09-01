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
        internal static SymmetricKeyCipher ToCipher(this SymmetricAlgorithmSpecification target, RandomNumberGenerator randomnessProvider)
        {
            switch (target)
            {
                case SymmetricAlgorithmSpecification.Unspecified:

                    return null;

                case SymmetricAlgorithmSpecification.Aes128Cbc:

                    return new Aes128CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Aes128Ecb:

                    return new Aes128EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Aes256Cbc:

                    return new Aes256CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Aes256Ecb:

                    return new Aes256EcbCipher(randomnessProvider);

                default:

                    throw new ArgumentException($"{target} is not a supported {nameof(SymmetricAlgorithmSpecification)}.", nameof(target));
            }
        }

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
        internal static Int32 ToKeyBitLength(this SymmetricAlgorithmSpecification target)
        {
            switch (target)
            {
                case SymmetricAlgorithmSpecification.Unspecified:

                    return default;

                case SymmetricAlgorithmSpecification.Aes128Cbc:

                    return 128;

                case SymmetricAlgorithmSpecification.Aes128Ecb:

                    return 128;

                case SymmetricAlgorithmSpecification.Aes256Cbc:

                    return 256;

                case SymmetricAlgorithmSpecification.Aes256Ecb:

                    return 256;

                default:

                    throw new ArgumentException($"{target} is not a supported {nameof(SymmetricAlgorithmSpecification)}.", nameof(target));
            }
        }
    }
}