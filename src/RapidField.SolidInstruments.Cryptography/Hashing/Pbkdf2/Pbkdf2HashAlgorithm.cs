// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing.Pbkdf2
{
    /// <summary>
    /// Represents a <see cref="HashAlgorithm" /> implementation of the PBKDF2 key derivation function.
    /// </summary>
    internal sealed class Pbkdf2HashAlgorithm : HashAlgorithmBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </summary>
        [DebuggerHidden]
        private Pbkdf2HashAlgorithm()
            : base(DigestLengthInBitsForPbkdf2)
        {
            return;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static new HashAlgorithm Create() => new Pbkdf2HashAlgorithm();

        /// <summary>
        /// Produces and returns a digest for the specified plaintext.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext from which to produce a digest.
        /// </param>
        /// <param name="salt">
        /// The deterministic salt bytes for <paramref name="plaintext" />.
        /// </param>
        /// <param name="digestLengthInBytes">
        /// The length, in bytes, of digests produced by the algorithm.
        /// </param>
        /// <returns>
        /// The resulting digest bytes.
        /// </returns>
        protected override sealed Byte[] ComputeHash(PinnedMemory plaintext, PinnedMemory salt, Int32 digestLengthInBytes)
        {
            using var keyDerivationFunction = new Rfc2898DeriveBytes(plaintext, salt, IterationCount, KeyDerivationHashAlgorithm.ToHashAlgorithmName());
            return keyDerivationFunction.GetBytes(digestLengthInBytes);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Pbkdf2HashAlgorithm" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the number of PBKDF2 iterations to perform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IterationCount = 17711;

        /// <summary>
        /// Represents the hashing algorithm that is used to derive PBKDF2 key bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification KeyDerivationHashAlgorithm = HashingAlgorithmSpecification.ShaTwo512;
    }
}