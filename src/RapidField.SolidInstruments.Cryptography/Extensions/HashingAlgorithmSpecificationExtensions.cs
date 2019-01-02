// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Hashing;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="HashingAlgorithmSpecification" /> enumeration with cryptographic features.
    /// </summary>
    internal static class HashingAlgorithmSpecificationExtensions
    {
        /// <summary>
        /// Returns the digest bit-length component of the current <see cref="HashingAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="HashingAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the digest for the current <see cref="HashingAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToDigestBitLength(this HashingAlgorithmSpecification target)
        {
            switch (target)
            {
                case HashingAlgorithmSpecification.Unspecified:

                    return default(Int32);

                case HashingAlgorithmSpecification.Md5:

                    return 128;

                case HashingAlgorithmSpecification.ShaTwo256:

                    return 256;

                case HashingAlgorithmSpecification.ShaTwo384:

                    return 384;

                case HashingAlgorithmSpecification.ShaTwo512:

                    return 512;

                default:

                    throw new ArgumentException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.", nameof(target));
            }
        }

        /// <summary>
        /// Returns a new <see cref="HashAlgorithm" /> matching the current <see cref="HashingAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="HashingAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// A new <see cref="HashAlgorithm" /> matching the current <see cref="HashingAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithm ToHashAlgorithm(this HashingAlgorithmSpecification target)
        {
            switch (target)
            {
                case HashingAlgorithmSpecification.Unspecified:

                    return null;

                case HashingAlgorithmSpecification.Md5:

                    return MD5.Create();

                case HashingAlgorithmSpecification.ShaTwo256:

                    return SHA256.Create();

                case HashingAlgorithmSpecification.ShaTwo384:

                    return SHA384.Create();

                case HashingAlgorithmSpecification.ShaTwo512:

                    return SHA512.Create();

                default:

                    throw new ArgumentException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.", nameof(target));
            }
        }
    }
}