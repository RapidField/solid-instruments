// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="IEnumerable{Byte}" /> interface with general purpose features.
    /// </summary>
    public static class ByteCollectionExtensions
    {
        /// <summary>
        /// Compresses the current uncompressed <see cref="Byte" /> array.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Byte" /> array.
        /// </param>
        /// <returns>
        /// The resulting compressed bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        public static Byte[] Compress(this Byte[] target)
        {
            if (target.RejectIf().IsNull(nameof(target)).TargetArgument.Length == 0)
            {
                return Array.Empty<Byte>();
            }

            using (var compressedStream = new MemoryStream())
            {
                using (var compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
                {
                    compressionStream.Write(target, 0, target.Length);
                }

                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Computes a fast 32-bit hash using the bytes in the current <see cref="IEnumerable{Byte}" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{Byte}" />.
        /// </param>
        public static Int32 ComputeThirtyTwoBitHash(this IEnumerable<Byte> target)
        {
            var hash = target.ComputeOneHundredTwentyEightBitHash();
            return ((BitConverter.ToInt32(hash, 0) ^ BitConverter.ToInt32(hash, 8)) ^ (BitConverter.ToInt32(hash, 4) ^ BitConverter.ToInt32(hash, 12)));
        }

        /// <summary>
        /// Decompresses the current compressed <see cref="Byte" /> array.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Byte" /> array.
        /// </param>
        /// <returns>
        /// The resulting decompressed bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target" /> is <see langword="null" />.
        /// </exception>
        public static Byte[] Decompress(this Byte[] target)
        {
            if (target.RejectIf().IsNull(nameof(target)).TargetArgument.Length == 0)
            {
                return Array.Empty<Byte>();
            }

            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(target))
                {
                    using (var decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedStream);
                    }
                }

                return decompressedStream.ToArray();
            }
        }

        /// <summary>
        /// Generates a new <see cref="Guid" /> that uniquely identifies the contents of the current
        /// <see cref="IEnumerable{Byte}" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{Byte}" />.
        /// </param>
        /// <returns>
        /// A new <see cref="Guid" /> that uniquely identifies the contents of the current <see cref="IEnumerable{Byte}" />.
        /// </returns>
        public static Guid GenerateChecksumIdentity(this IEnumerable<Byte> target) => new Guid(target.ComputeOneHundredTwentyEightBitHash());

        /// <summary>
        /// Performs a circular shift on the bits in the current <see cref="Byte" /> array.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="Byte" /> array.
        /// </param>
        /// <param name="direction">
        /// A direction to shift the bits in the current <see cref="Byte" /> array. The default value is
        /// <see cref="BitShiftDirection.Right" />.
        /// </param>
        /// <param name="bitShiftCount">
        /// The number of bits to shift by.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value of <paramref name="direction" /> is equal to <see cref="BitShiftDirection.Unspecified" /> and/or the value of
        /// <paramref name="bitShiftCount" /> is less than zero.
        /// </exception>
        public static Byte[] PerformCircularBitShift(this Byte[] target, BitShiftDirection direction, Int32 bitShiftCount)
        {
            direction.RejectIf().IsEqualToValue(BitShiftDirection.Unspecified, nameof(direction));

            if (target.Length == 0)
            {
                return target;
            }

            var collectionByteLength = target.Length;
            var collectionBitLength = (collectionByteLength * 8);
            var simplifiedBitShiftCount = (bitShiftCount.RejectIf().IsLessThan(0, nameof(bitShiftCount)) % collectionBitLength);

            if (simplifiedBitShiftCount == 0)
            {
                return target;
            }

            var simplifiedByteShiftCount = (simplifiedBitShiftCount / 8);
            var bitShiftRemainder = (simplifiedBitShiftCount % 8);
            var outputBuffer = new Byte[collectionByteLength];
            Int32 headIndex;
            Int32 readIndex;
            Int32 writeIndex;

            switch (direction)
            {
                case BitShiftDirection.Left:

                    headIndex = simplifiedByteShiftCount;

                    if (bitShiftRemainder == 0)
                    {
                        for (readIndex = headIndex, writeIndex = 0; readIndex < collectionByteLength; readIndex++, writeIndex++)
                        {
                            outputBuffer[writeIndex] = target[readIndex];
                        }

                        for (readIndex = 0; readIndex < headIndex; readIndex++, writeIndex++)
                        {
                            outputBuffer[writeIndex] = target[readIndex];
                        }
                    }
                    else
                    {
                        Byte leftElement;
                        Byte rightElement;

                        for (readIndex = headIndex, writeIndex = 0; readIndex < collectionByteLength; readIndex++, writeIndex++)
                        {
                            leftElement = target[readIndex];
                            rightElement = target[readIndex == (collectionByteLength - 1) ? 0 : (readIndex + 1)];
                            outputBuffer[writeIndex] = unchecked((Byte)(leftElement >> bitShiftRemainder | rightElement << (8 - bitShiftRemainder)));
                        }

                        for (readIndex = 0; readIndex < headIndex; readIndex++, writeIndex++)
                        {
                            leftElement = target[readIndex];
                            rightElement = target[readIndex + 1];
                            outputBuffer[writeIndex] = unchecked((Byte)(leftElement >> bitShiftRemainder | rightElement << (8 - bitShiftRemainder)));
                        }
                    }

                    break;

                case BitShiftDirection.Right:

                    headIndex = (collectionByteLength - simplifiedByteShiftCount);

                    if (bitShiftRemainder == 0)
                    {
                        for (readIndex = headIndex, writeIndex = 0; readIndex < collectionByteLength; readIndex++, writeIndex++)
                        {
                            outputBuffer[writeIndex] = target[readIndex];
                        }

                        for (readIndex = 0; readIndex < headIndex; readIndex++, writeIndex++)
                        {
                            outputBuffer[writeIndex] = target[readIndex];
                        }
                    }
                    else
                    {
                        Byte leftElement;
                        Byte rightElement;

                        for (readIndex = headIndex, writeIndex = 0; readIndex < collectionByteLength; readIndex++, writeIndex++)
                        {
                            leftElement = target[readIndex - 1];
                            rightElement = target[readIndex];
                            outputBuffer[writeIndex] = unchecked((Byte)(leftElement >> (8 - bitShiftRemainder) | rightElement << bitShiftRemainder));
                        }

                        for (readIndex = 0; readIndex < headIndex; readIndex++, writeIndex++)
                        {
                            leftElement = target[readIndex == 0 ? (collectionByteLength - 1) : (readIndex - 1)];
                            rightElement = target[readIndex];
                            outputBuffer[writeIndex] = unchecked((Byte)(leftElement >> (8 - bitShiftRemainder) | rightElement << bitShiftRemainder));
                        }
                    }

                    break;

                default:

                    throw new ArgumentException($"{direction} is not a supported {nameof(BitShiftDirection)}.", nameof(direction));
            }

            return outputBuffer;
        }

        /// <summary>
        /// Computes a fast 128-bit hash using the bytes in the current <see cref="IEnumerable{Byte}" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="IEnumerable{Byte}" />.
        /// </param>
        [DebuggerHidden]
        private static Byte[] ComputeOneHundredTwentyEightBitHash(this IEnumerable<Byte> target) => target.Any() ? ChecksumAlgorithm.ComputeHash(target.ToArray()) : EmptyCollectionOneHundredTwentyEightBitHash;

        /// <summary>
        /// Gets a hash algorithm that is used to calculate 128-bit checksum hashes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [ThreadStatic]
        private static readonly HashAlgorithm ChecksumAlgorithm = MD5.Create();

        /// <summary>
        /// Represents the binary 128-bit hash for an empty collection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[] EmptyCollectionOneHundredTwentyEightBitHash = { 0x9a, 0x69, 0x39, 0xc6, 0xac, 0x93, 0x56, 0x96, 0x65, 0xa5, 0x35, 0x5a, 0x6c, 0x53, 0xa9, 0xca };

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="ByteCollectionExtensions" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [ThreadStatic]
        private static readonly StaticMemberFinalizer Finalizer = new StaticMemberFinalizer(ChecksumAlgorithm.Dispose);
    }
}