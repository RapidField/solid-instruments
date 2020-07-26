// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Konscious.Security.Cryptography;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing.Argon2
{
    /// <summary>
    /// Represents a <see cref="HashAlgorithm" /> implementation of the PBKDF2 key derivation function.
    /// </summary>
    internal sealed class Argon2HashAlgorithm : HashAlgorithmBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </summary>
        /// <param name="degreeOfParallelism">
        /// The number of threads to use concurrently while processing the hash value.
        /// </param>
        /// <param name="iterationCount">
        /// The number of Argon2 iterations to perform.
        /// </param>
        /// <param name="memorySizeInKilobytes">
        /// The number of 1KB memory blocks to use while processing the hash value.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="degreeOfParallelism" /> is less than one -or- <paramref name="iterationCount" /> is less than one -or-
        /// <paramref name="memorySizeInKilobytes" /> is less than one.
        /// </exception>
        [DebuggerHidden]
        private Argon2HashAlgorithm(Int32 degreeOfParallelism, Int32 iterationCount, Int32 memorySizeInKilobytes)
            : base(DigestLengthInBitsForArgon2)
        {
            DegreeOfParallelism = degreeOfParallelism.RejectIf().IsLessThan(1, nameof(degreeOfParallelism));
            IterationCount = iterationCount.RejectIf().IsLessThan(1, nameof(iterationCount));
            MemorySizeInKilobytes = memorySizeInKilobytes.RejectIf().IsLessThan(1, nameof(memorySizeInKilobytes));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Argon2HashAlgorithm" /> class that performs eight (8) iterations using 8MB of
        /// memory and eight (8) threads.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static new HashAlgorithm Create() => WithBalancedConfiguration();

        /// <summary>
        /// Creates a new instance of the <see cref="Argon2HashAlgorithm" /> class that performs eight (8) iterations using 8MB of
        /// memory and eight (8) threads.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithm WithBalancedConfiguration() => new Argon2HashAlgorithm(IntermediateDegreeOfParallelism, IntermediateIterationCount, IntermediateMemorySizeInKilobytes);

        /// <summary>
        /// Creates a new instance of the <see cref="Argon2HashAlgorithm" /> class that performs twenty-four (24) iterations using
        /// 4MB of memory and eight (8) threads.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithm WithIterativelyExpensiveConfiguration() => new Argon2HashAlgorithm(IntermediateDegreeOfParallelism, HighIterationCount, LowMemorySizeInKilobytes);

        /// <summary>
        /// Creates a new instance of the <see cref="Argon2HashAlgorithm" /> class that performs eight (8) iterations using 16MB of
        /// memory and four (4) threads.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithm WithMemoryExpensiveConfiguration() => new Argon2HashAlgorithm(LowDegreeOfParallelism, IntermediateIterationCount, HighMemorySizeInKilobytes);

        /// <summary>
        /// Creates a new instance of the <see cref="Argon2HashAlgorithm" /> class that performs four (4) iterations using 8MB of
        /// memory and sixteen (16) threads.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Argon2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static HashAlgorithm WithThreadExpensiveConfiguration() => new Argon2HashAlgorithm(HighDegreeOfParallelism, LowIterationCount, IntermediateMemorySizeInKilobytes);

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
            var keyDerivationFunction = new Argon2id(plaintext)
            {
                AssociatedData = DefaultAssociatedData,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = IterationCount,
                MemorySize = MemorySizeInKilobytes,
                Salt = salt
            };
            return keyDerivationFunction.GetBytes(digestLengthInBytes);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Argon2HashAlgorithm" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the number of threads to use concurrently while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 DegreeOfParallelism;

        /// <summary>
        /// Represents the number of Argon2 iterations to perform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 IterationCount;

        /// <summary>
        /// Represents the number of 1KB memory blocks to use while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 MemorySizeInKilobytes;

        /// <summary>
        /// Represents the high default number of threads to use concurrently while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 HighDegreeOfParallelism = 16;

        /// <summary>
        /// Represents the high default number of Argon2 iterations to perform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 HighIterationCount = 24;

        /// <summary>
        /// Represents the default number of 1KB memory blocks to use while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 HighMemorySizeInKilobytes = 16384;

        /// <summary>
        /// Represents the intermediate default number of threads to use concurrently while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IntermediateDegreeOfParallelism = 8;

        /// <summary>
        /// Represents the intermediate default number of Argon2 iterations to perform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IntermediateIterationCount = 8;

        /// <summary>
        /// Represents the default number of 1KB memory blocks to use while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 IntermediateMemorySizeInKilobytes = 8192;

        /// <summary>
        /// Represents the low default number of threads to use concurrently while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 LowDegreeOfParallelism = 4;

        /// <summary>
        /// Represents the low default number of Argon2 iterations to perform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 LowIterationCount = 4;

        /// <summary>
        /// Represents the default number of 1KB memory blocks to use while processing the hash value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 LowMemorySizeInKilobytes = 4096;

        /// <summary>
        /// Represents an arbitrary byte array that is used for every <see cref="Argon2HashAlgorithm" /> instance to thwart
        /// dictionary attacks.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] DefaultAssociatedData = new Byte[] { 0xcc, 0xf0, 0x3c, 0x03, 0xaf, 0xa5, 0x0f, 0xca, 0xac, 0xf0, 0x5a, 0xfa, 0x30, 0xc3, 0x0f, 0xcc };
    }
}