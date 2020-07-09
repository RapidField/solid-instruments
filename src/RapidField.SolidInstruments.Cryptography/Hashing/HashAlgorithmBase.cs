// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing.Argon2;
using RapidField.SolidInstruments.Cryptography.Hashing.Pbkdf2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Represents a <see cref="HashAlgorithm" /> implementation.
    /// </summary>
    internal abstract class HashAlgorithmBase : HashAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashAlgorithmBase" /> class.
        /// </summary>
        /// <param name="digestLengthInBits">
        /// The length, in bits, of digests produced by the algorithm.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digestLengthInBits" /> is less than or equal to zero.
        /// </exception>
        protected HashAlgorithmBase(Int32 digestLengthInBits)
            : base()
        {
            DigestLengthInBits = digestLengthInBits.RejectIf().IsLessThanOrEqualTo(0, nameof(digestLengthInBits));
        }

        /// <summary>
        /// Resets the hash algorithm to its initial state.
        /// </summary>
        public override void Initialize() => HashValue = Array.Empty<Byte>();

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
        protected abstract Byte[] ComputeHash(PinnedMemory plaintext, PinnedMemory salt, Int32 digestLengthInBytes);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="HashAlgorithm" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Routes data written to the object into the hash algorithm.
        /// </summary>
        /// <param name="array">
        /// The input to compute the hash code for.
        /// </param>
        /// <param name="ibStart">
        /// The offset into the byte array from which to begin using data.
        /// </param>
        /// <param name="cbSize">
        /// The number of bytes in the byte array to use as data.
        /// </param>
        /// <exception cref="SecurityException">
        /// An exception was raised while computing the hash value.
        /// </exception>
        protected override void HashCore(Byte[] array, Int32 ibStart, Int32 cbSize)
        {
            try
            {
                using var plaintext = new PinnedMemory(array.Skip(ibStart).Take(cbSize).ToArray(), true);
                using var salt = DeriveDeterministicSaltValue(plaintext);
                HashValue = ComputeHash(plaintext, salt, DigestLengthInBytes);
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while computing a hash value.", exception);
            }
        }

        /// <summary>
        /// Finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
        protected override Byte[] HashFinal() => HashValue;

        /// <summary>
        /// Derives a deterministic 256-bit salt value from the specified plaintext bytes.
        /// </summary>
        /// <remarks>
        /// Salt bytes are derived deterministically because <see cref="HashingProcessor" /> handles salt generation and
        /// application. Therefore, <paramref name="plaintext" /> contains both plaintext and salt bytes and the deterministic salt
        /// input can safely be derived from them.
        /// </remarks>
        /// <param name="plaintext">
        /// Plaintext bytes from which to derive a salt value.
        /// </param>
        /// <returns>
        /// The derived salt bytes.
        /// </returns>
        [DebuggerHidden]
        private static PinnedMemory DeriveDeterministicSaltValue(IReadOnlyPinnedMemory<Byte> plaintext)
        {
            var processedPlaintext = new List<Byte>(plaintext);

            try
            {
                processedPlaintext.ReverseStaggerSort();
                using var saltDerivationAlgorithm = SaltDerivationHashAlgorithm.ToHashAlgorithm();
                var saltBytes = saltDerivationAlgorithm.ComputeHash(processedPlaintext.ToArray().PerformCircularBitShift(SaltDerivationBitShiftDirection, SaltDerivationBitShiftCount));
                return new PinnedMemory(saltBytes, true);
            }
            finally
            {
                processedPlaintext.Clear();
            }
        }

        /// <summary>
        /// Gets the length, in bits, of digests produced by <see cref="HashAlgorithmBase" /> instances.
        /// </summary>
        public override Int32 HashSize => DigestLengthInBits;

        /// <summary>
        /// Gets the length, in bytes, of digests produced by <see cref="HashAlgorithmBase" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 DigestLengthInBytes => DigestLengthInBits / 8;

        /// <summary>
        /// Represents the length, in bits, of digests produced by <see cref="Argon2HashAlgorithm" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 DigestLengthInBitsForArgon2 = 256;

        /// <summary>
        /// Represents the length, in bits, of digests produced by <see cref="Pbkdf2HashAlgorithm" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 DigestLengthInBitsForPbkdf2 = 256;

        /// <summary>
        /// Represents the bit shift count that is used to derive deterministic salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SaltDerivationBitShiftCount = 5;

        /// <summary>
        /// Represents the bit shift direction that is used to derive deterministic salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const BitShiftDirection SaltDerivationBitShiftDirection = BitShiftDirection.Right;

        /// <summary>
        /// Represents the hashing algorithm that is used to derive deterministic salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification SaltDerivationHashAlgorithm = HashingAlgorithmSpecification.ShaTwo256;

        /// <summary>
        /// Represents the length, in bits, of digests produced by <see cref="HashAlgorithmBase" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 DigestLengthInBits;
    }
}