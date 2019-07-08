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

                /* =================================================================================================================
                 * Twofish and Threefish are out-of-scope for MVP-01.
                 * =================================================================================================================
                case SymmetricAlgorithmSpecification.Twofish128Cbc:

                    return new Twofish128CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Twofish128Ecb:

                    return new Twofish128EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Twofish192Cbc:

                    return new Twofish192CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Twofish192Ecb:

                    return new Twofish192EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Twofish256Cbc:

                    return new Twofish256CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Twofish256Ecb:

                    return new Twofish256EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish256Cbc:

                    return new Threefish256CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish256Ecb:

                    return new Threefish256EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish512Cbc:

                    return new Threefish512CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish512Ecb:

                    return new Threefish512EcbCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish1024Cbc:

                    return new Threefish1024CbcCipher(randomnessProvider);

                case SymmetricAlgorithmSpecification.Threefish1024Ecb:

                    return new Threefish1024EcbCipher(randomnessProvider);*/

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

                /* =================================================================================================================
                 * Twofish and Threefish are out-of-scope for MVP-01.
                 * =================================================================================================================
                case SymmetricAlgorithmSpecification.Twofish128Cbc:

                    return 128;

                case SymmetricAlgorithmSpecification.Twofish128Ecb:

                    return 128;

                case SymmetricAlgorithmSpecification.Twofish192Cbc:

                    return 192;

                case SymmetricAlgorithmSpecification.Twofish192Ecb:

                    return 192;

                case SymmetricAlgorithmSpecification.Twofish256Cbc:

                    return 256;

                case SymmetricAlgorithmSpecification.Twofish256Ecb:

                    return 256;

                case SymmetricAlgorithmSpecification.Threefish256Cbc:

                    return 256;

                case SymmetricAlgorithmSpecification.Threefish256Ecb:

                    return 256;

                case SymmetricAlgorithmSpecification.Threefish512Cbc:

                    return 512;

                case SymmetricAlgorithmSpecification.Threefish512Ecb:

                    return 512;

                case SymmetricAlgorithmSpecification.Threefish1024Cbc:

                    return 1024;

                case SymmetricAlgorithmSpecification.Threefish1024Ecb:

                    return 1024;*/

                default:

                    throw new ArgumentException($"{target} is not a supported {nameof(SymmetricAlgorithmSpecification)}.", nameof(target));
            }
        }
    }
}