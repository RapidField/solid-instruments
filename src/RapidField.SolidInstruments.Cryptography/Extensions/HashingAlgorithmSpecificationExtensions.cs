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
        internal static Int32 ToDigestBitLength(this HashingAlgorithmSpecification target) => target switch
        {
            HashingAlgorithmSpecification.Unspecified => default,
            HashingAlgorithmSpecification.Md5 => 128,
            HashingAlgorithmSpecification.ShaTwo256 => 256,
            HashingAlgorithmSpecification.ShaTwo384 => 384,
            HashingAlgorithmSpecification.ShaTwo512 => 512,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.", nameof(target))
        };

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
        internal static HashAlgorithm ToHashAlgorithm(this HashingAlgorithmSpecification target) => target switch
        {
            HashingAlgorithmSpecification.Unspecified => null,
            HashingAlgorithmSpecification.Md5 => MD5.Create(),
            HashingAlgorithmSpecification.ShaTwo256 => SHA256.Create(),
            HashingAlgorithmSpecification.ShaTwo384 => SHA384.Create(),
            HashingAlgorithmSpecification.ShaTwo512 => SHA512.Create(),
            _ => throw new ArgumentException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.", nameof(target))
        };
    }
}