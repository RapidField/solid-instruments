// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Hashing.Pbkdf2
{
    /// <summary>
    /// Represents a <see cref="HashAlgorithm" /> implementation of the PBKDF2 key derivation function.
    /// </summary>
    internal sealed class Pbkdf2HashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </summary>
        [DebuggerHidden]
        private Pbkdf2HashAlgorithm()
            : base()
        {
            return;
        }

        /// <summary>
        /// Resets the hash algorithm to its initial state.
        /// </summary>
        public override void Initialize() => HashValue = Array.Empty<Byte>();

        /// <summary>
        /// Creates a new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="Pbkdf2HashAlgorithm" /> class.
        /// </returns>
        [DebuggerHidden]
        internal static new Pbkdf2HashAlgorithm Create() => new Pbkdf2HashAlgorithm();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="Pbkdf2HashAlgorithm" />.
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
        protected override void HashCore(Byte[] array, Int32 ibStart, Int32 cbSize)
        {
            using var plaintext = new PinnedMemory(array.Skip(ibStart).Take(cbSize).ToArray(), true);
            using var salt = DeriveSaltValue(plaintext);
            using var keyDerivationFunction = new Rfc2898DeriveBytes(plaintext, salt, IterationCount, KeyDerivationHashAlgorithm.ToHashAlgorithmName());
            HashValue = keyDerivationFunction.GetBytes(DigestLengthInBytes);
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
        /// application. Therefore, <paramref name="plaintext" /> contains both plaintext and salt bytes and the PBKDF2 salt input
        /// can safely be derived from them.
        /// </remarks>
        /// <param name="plaintext">
        /// Plaintext bytes from which to derive a salt value.
        /// </param>
        /// <returns>
        /// The derived salt bytes.
        /// </returns>
        [DebuggerHidden]
        private static PinnedMemory DeriveSaltValue(Byte[] plaintext)
        {
            using var saltDerivationAlgorithm = SaltDerivationHashAlgorithm.ToHashAlgorithm();
            var saltBytes = saltDerivationAlgorithm.ComputeHash(plaintext);
            saltBytes.ReverseStaggerSort();
            saltBytes = saltBytes.PerformCircularBitShift(SaltDerivationBitShiftDirection, SaltDerivationBitShiftCount);
            return new PinnedMemory(saltBytes, true);
        }

        /// <summary>
        /// Gets the length, in bits, of digests produced by <see cref="Pbkdf2HashAlgorithm" /> instances.
        /// </summary>
        public override Int32 HashSize => DigestLengthInBits;

        /// <summary>
        /// Represents the length, in bits, of digests produced by <see cref="Pbkdf2HashAlgorithm" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 DigestLengthInBits = 256;

        /// <summary>
        /// Represents the length, in bytes, of digests produced by <see cref="Pbkdf2HashAlgorithm" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 DigestLengthInBytes = DigestLengthInBits / 8;

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

        /// <summary>
        /// Represents the bit shift count that is used to derive PBKDF2 salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 SaltDerivationBitShiftCount = 5;

        /// <summary>
        /// Represents the bit shift direction that is used to derive PBKDF2 salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const BitShiftDirection SaltDerivationBitShiftDirection = BitShiftDirection.Right;

        /// <summary>
        /// Represents the hashing algorithm that is used to derive PBKDF2 salt bytes from the plaintext.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const HashingAlgorithmSpecification SaltDerivationHashAlgorithm = HashingAlgorithmSpecification.ShaTwo256;
    }
}