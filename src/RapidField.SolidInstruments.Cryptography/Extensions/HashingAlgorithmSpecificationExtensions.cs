// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Hashing;
using RapidField.SolidInstruments.Cryptography.Hashing.Argon2;
using RapidField.SolidInstruments.Cryptography.Hashing.Pbkdf2;
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
            HashingAlgorithmSpecification.Argon2idBalanced => HashAlgorithmBase.DigestLengthInBitsForArgon2,
            HashingAlgorithmSpecification.Argon2idIterativelyExpensive => HashAlgorithmBase.DigestLengthInBitsForArgon2,
            HashingAlgorithmSpecification.Argon2idMemoryExpensive => HashAlgorithmBase.DigestLengthInBitsForArgon2,
            HashingAlgorithmSpecification.Argon2idThreadExpensive => HashAlgorithmBase.DigestLengthInBitsForArgon2,
            HashingAlgorithmSpecification.Md5 => 128,
            HashingAlgorithmSpecification.Pbkdf2 => HashAlgorithmBase.DigestLengthInBitsForPbkdf2,
            HashingAlgorithmSpecification.ShaTwo256 => 256,
            HashingAlgorithmSpecification.ShaTwo384 => 384,
            HashingAlgorithmSpecification.ShaTwo512 => 512,
            _ => throw new UnsupportedSpecificationException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.")
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
            HashingAlgorithmSpecification.Argon2idBalanced => Argon2HashAlgorithm.WithBalancedConfiguration(),
            HashingAlgorithmSpecification.Argon2idIterativelyExpensive => Argon2HashAlgorithm.WithIterativelyExpensiveConfiguration(),
            HashingAlgorithmSpecification.Argon2idMemoryExpensive => Argon2HashAlgorithm.WithMemoryExpensiveConfiguration(),
            HashingAlgorithmSpecification.Argon2idThreadExpensive => Argon2HashAlgorithm.WithThreadExpensiveConfiguration(),
            HashingAlgorithmSpecification.Md5 => MD5.Create(),
            HashingAlgorithmSpecification.Pbkdf2 => Pbkdf2HashAlgorithm.Create(),
            HashingAlgorithmSpecification.ShaTwo256 => SHA256.Create(),
            HashingAlgorithmSpecification.ShaTwo384 => SHA384.Create(),
            HashingAlgorithmSpecification.ShaTwo512 => SHA512.Create(),
            _ => throw new UnsupportedSpecificationException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.")
        };

        /// <summary>
        /// Returns a new <see cref="HashAlgorithmName" /> matching the current <see cref="HashingAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="HashingAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// A new <see cref="HashAlgorithmName" /> matching the current <see cref="HashingAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithmName ToHashAlgorithmName(this HashingAlgorithmSpecification target) => target switch
        {
            HashingAlgorithmSpecification.Unspecified => default,
            HashingAlgorithmSpecification.Md5 => HashAlgorithmName.MD5,
            HashingAlgorithmSpecification.ShaTwo256 => HashAlgorithmName.SHA256,
            HashingAlgorithmSpecification.ShaTwo384 => HashAlgorithmName.SHA384,
            HashingAlgorithmSpecification.ShaTwo512 => HashAlgorithmName.SHA512,
            _ => throw new UnsupportedSpecificationException($"{target} is not a supported {nameof(HashingAlgorithmSpecification)}.")
        };
    }
}