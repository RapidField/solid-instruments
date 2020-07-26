// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Provides operational scaffolding for classes that perform cryptographic transformations.
    /// </summary>
    internal abstract class CryptographicTransform : Instrument, ICryptoTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicTransform" /> class.
        /// </summary>
        /// <param name="blockSize">
        /// The block size for the cipher.
        /// </param>
        /// <param name="mode">
        /// The encryption mode for the cipher.
        /// </param>
        /// <param name="paddingMode">
        /// The padding setting for the cipher.
        /// </param>
        /// <param name="encryptionDirection">
        /// The direction for the cryptographic operation to be performed.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="blockSize" /> is less than or equal to zero -or- <paramref name="encryptionDirection" /> is equal to
        /// <see cref="EncryptionDirection.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        protected CryptographicTransform(Int32 blockSize, CipherMode mode, PaddingMode paddingMode, EncryptionDirection encryptionDirection)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            BlockSizeInBytes = Convert.ToByte((blockSize.RejectIf().IsLessThanOrEqualTo(0, nameof(blockSize)) / 8));
            EncryptionDirection = encryptionDirection.RejectIf().IsEqualToValue(EncryptionDirection.Unspecified, nameof(encryptionDirection));
            Mode = mode;
            PaddingMode = paddingMode;
        }

        /// <summary>
        /// Transforms the specified region of the input byte array and copy the resulting transform to the specified region of the
        /// output byte array.
        /// </summary>
        /// <param name="inputBuffer">
        /// The input for which to compute the transform.
        /// </param>
        /// <param name="inputOffset">
        /// The offset into the input byte array from which to begin using data.
        /// </param>
        /// <param name="inputCount">
        /// The number of bytes in the input byte array to use as data.
        /// </param>
        /// <param name="outputBuffer">
        /// The output to which to write the transform.
        /// </param>
        /// <param name="outputOffset">
        /// The offset into the output byte array from which to begin writing data.
        /// </param>
        /// <returns>
        /// The number of bytes written.
        /// </returns>
        [DebuggerHidden]
        public Int32 TransformBlock(Byte[] inputBuffer, Int32 inputOffset, Int32 inputCount, Byte[] outputBuffer, Int32 outputOffset)
        {
            var targetRange = inputBuffer.Skip(inputOffset).Take(inputCount).ToList();
            TransformRange(targetRange, this);
            var targetRangeLength = targetRange.Count;

            for (var i = 0; i < targetRangeLength; i++)
            {
                outputBuffer[i + outputOffset] = targetRange[i];
            }

            return targetRangeLength;
        }

        /// <summary>
        /// Transform the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">
        /// The input for which to compute the transform.
        /// </param>
        /// <param name="inputOffset">
        /// The offset into the byte array from which to begin using data.
        /// </param>
        /// <param name="inputCount">
        /// The number of bytes in the byte array to use as data.
        /// </param>
        /// <returns>
        /// The computed transform.
        /// </returns>
        [DebuggerHidden]
        public Byte[] TransformFinalBlock(Byte[] inputBuffer, Int32 inputOffset, Int32 inputCount)
        {
            if (inputCount == 0)
            {
                return Array.Empty<Byte>();
            }

            var targetRange = inputBuffer.Skip(inputOffset).Take(inputCount).ToList();
            TransformRange(targetRange, this);
            return targetRange.ToArray();
        }

        /// <summary>
        /// Perform the decryption operation on the supplied block.
        /// </summary>
        /// <param name="block">
        /// The block to be decrypted.
        /// </param>
        protected abstract void DecryptBlock(Byte[] block);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CryptographicTransform" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Perform the encryption operation on the supplied block.
        /// </summary>
        /// <param name="block">
        /// The block to be encrypted.
        /// </param>
        protected abstract void EncryptBlock(Byte[] block);

        /// <summary>
        /// Evaluate and apply padding to the supplied block as needed.
        /// </summary>
        /// <param name="block">
        /// A partial-length or full-length block to be evaluated.
        /// </param>
        /// <param name="blockSizeInBytes">
        /// The block size for the transformation.
        /// </param>
        /// <param name="paddingMode">
        /// The padding mode for the transformation.
        /// </param>
        /// <returns>
        /// The resulting full-length block.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="paddingMode" /> is not a supported padding mode.
        /// </exception>
        [DebuggerHidden]
        private static Byte[] ConditionallyApplyPadding(Byte[] block, Int32 blockSizeInBytes, PaddingMode paddingMode)
        {
            if (paddingMode == PaddingModeNone)
            {
                return block;
            }

            var paddingLengthInBytes = (blockSizeInBytes - block.Length);

            if (paddingLengthInBytes <= 0)
            {
                return block;
            }

            var blockAsByteList = new List<Byte>(block);

            switch (paddingMode)
            {
                case PaddingModeZeros:

                    while (blockAsByteList.Count < (blockSizeInBytes - 1))
                    {
                        blockAsByteList.Add(0x00);
                    }

                    blockAsByteList.Add(Convert.ToByte(paddingLengthInBytes));
                    break;

                case PaddingModePkcs7:

                    while (blockAsByteList.Count < blockSizeInBytes)
                    {
                        blockAsByteList.Add(Convert.ToByte(paddingLengthInBytes));
                    }

                    break;

                default:

                    throw new ArgumentException($"{paddingMode} is not a supported {nameof(PaddingMode)}.", nameof(paddingMode));
            }

            return blockAsByteList.ToArray();
        }

        /// <summary>
        /// Evaluate and remove padding from the supplied block as needed.
        /// </summary>
        /// <param name="block">
        /// A partial-length or full-length block to be evaluated.
        /// </param>
        /// <param name="blockSizeInBytes">
        /// The block size for the transformation.
        /// </param>
        /// <param name="paddingMode">
        /// The padding mode for the transformation.
        /// </param>
        /// <returns>
        /// The resulting full-length or truncated block.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="paddingMode" /> is not a supported padding mode.
        /// </exception>
        /// <exception cref="CryptographicException">
        /// An unexpected padding pattern was encountered.
        /// </exception>
        [DebuggerHidden]
        private static Byte[] ConditionallyRemovePadding(Byte[] block, Int32 blockSizeInBytes, PaddingMode paddingMode)
        {
            if (paddingMode == PaddingModeNone || block.Length != blockSizeInBytes)
            {
                return block;
            }

            var paddingLengthInBytes = block[^1];

            if (paddingLengthInBytes > blockSizeInBytes)
            {
                return block;
            }

            var ciphertextByteCount = Convert.ToByte(blockSizeInBytes - paddingLengthInBytes);
            var blockWithPaddingRemoved = new Byte[ciphertextByteCount];
            var startPosition = (blockSizeInBytes - 2);

            switch (paddingMode)
            {
                case PaddingModeZeros:

                    for (var i = startPosition; i >= 0; i--)
                    {
                        if (i >= ciphertextByteCount)
                        {
                            if (block[i] != 0x00)
                            {
                                throw new CryptographicException($"A decryption operation failed to yield a valid plaintext result. Padding mode: {paddingMode}.");
                            }
                        }
                        else
                        {
                            blockWithPaddingRemoved[i] = block[i];
                        }
                    }

                    break;

                case PaddingModePkcs7:

                    for (var i = startPosition; i >= 0; i--)
                    {
                        if (i >= ciphertextByteCount)
                        {
                            if (block[i] != paddingLengthInBytes)
                            {
                                throw new CryptographicException($"A decryption operation failed to yield a valid plaintext result. Padding mode: {paddingMode}.");
                            }
                        }
                        else
                        {
                            blockWithPaddingRemoved[i] = block[i];
                        }
                    }

                    break;

                default:

                    throw new ArgumentException($"{paddingMode} is not a supported {nameof(PaddingMode)}.", nameof(paddingMode));
            }

            return blockWithPaddingRemoved;
        }

        /// <summary>
        /// Transform the supplied byte range.
        /// </summary>
        /// <param name="range">
        /// The data to be transformed.
        /// </param>
        /// <param name="transform">
        /// The cryptographic transform that performs the operation.
        /// </param>
        [DebuggerHidden]
        private static void TransformRange(List<Byte> range, CryptographicTransform transform)
        {
            var blockSizeInBytes = transform.BlockSizeInBytes;
            var encryptionDirection = transform.EncryptionDirection;
            var paddingMode = transform.PaddingMode;
            var currentLeadingRangeIndex = 0;
            var nextLeadingRangeIndex = (currentLeadingRangeIndex + blockSizeInBytes);
            var currentBlock = (Byte[])null;

            while (currentLeadingRangeIndex < range.Count)
            {
                var currentBlockIsFinalBlock = (nextLeadingRangeIndex >= range.Count);

                switch (encryptionDirection)
                {
                    case EncryptionDirection.Encryption:

                        if (currentBlockIsFinalBlock)
                        {
                            currentBlock = ConditionallyApplyPadding(range.Skip(currentLeadingRangeIndex).ToArray(), blockSizeInBytes, paddingMode);

                            while (range.Count < nextLeadingRangeIndex)
                            {
                                // Expand the range to accommodate the full size of the padded blocks.
                                range.Add(0x00);
                            }
                        }
                        else
                        {
                            currentBlock = range.Skip(currentLeadingRangeIndex).Take(blockSizeInBytes).ToArray();
                        }

                        transform.EncryptBlock(currentBlock);
                        break;

                    case EncryptionDirection.Decryption:

                        currentBlock = range.Skip(currentLeadingRangeIndex).Take(blockSizeInBytes).ToArray();
                        transform.DecryptBlock(currentBlock);

                        if (currentBlockIsFinalBlock)
                        {
                            currentBlock = ConditionallyRemovePadding(currentBlock, blockSizeInBytes, paddingMode);
                            var countOfPaddingBytesRemoved = (blockSizeInBytes - currentBlock.Length);

                            if (countOfPaddingBytesRemoved > 0)
                            {
                                // Contract the range to account for cleared padding bytes.
                                range.RemoveRange((currentLeadingRangeIndex + currentBlock.Length), countOfPaddingBytesRemoved);
                            }
                        }

                        break;

                    default:

                        throw new UnsupportedSpecificationException($"The specified encryption direction, {encryptionDirection}, is not supported.");
                }

                var currentBlockLength = currentBlock.Length;

                for (var i = 0; i < currentBlockLength; i++)
                {
                    range[i + currentLeadingRangeIndex] = currentBlock[i];
                }

                currentLeadingRangeIndex += blockSizeInBytes;
                nextLeadingRangeIndex += blockSizeInBytes;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        public Boolean CanReuseTransform => true;

        /// <summary>
        /// Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        public Boolean CanTransformMultipleBlocks => true;

        /// <summary>
        /// Gets the input block size, in bytes.
        /// </summary>
        public Int32 InputBlockSize => BlockSizeInBytes;

        /// <summary>
        /// Gets the output block size, in bytes.
        /// </summary>
        public Int32 OutputBlockSize => BlockSizeInBytes;

        /// <summary>
        /// Gets the block size, in bits.
        /// </summary>
        protected Int32 BlockSizeInBits => (BlockSizeInBytes * 8);

        /// <summary>
        /// Gets the block size, in bytes.
        /// </summary>
        protected Byte BlockSizeInBytes
        {
            get;
        }

        /// <summary>
        /// Gets the direction for the cryptographic operation being performed.
        /// </summary>
        protected EncryptionDirection EncryptionDirection
        {
            get;
        }

        /// <summary>
        /// Gets the encryption mode for the cipher.
        /// </summary>
        protected CipherMode Mode
        {
            get;
        }

        /// <summary>
        /// Gets the padding setting for the cipher.
        /// </summary>
        protected PaddingMode PaddingMode
        {
            get;
        }

#pragma warning disable SecurityIntelliSenseCS

        /// <summary>
        /// Represents the <see cref="CipherMode.CBC" /> cipher mode.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const CipherMode CipherModeCbc = CipherMode.CBC;

        /// <summary>
        /// Represents the <see cref="CipherMode.ECB" /> cipher mode.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const CipherMode CipherModeEcb = CipherMode.ECB;

        /// <summary>
        /// Represents the <see cref="PaddingMode.None" /> padding mode.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const PaddingMode PaddingModeNone = PaddingMode.None;

        /// <summary>
        /// Represents the <see cref="PaddingMode.PKCS7" /> padding mode.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const PaddingMode PaddingModePkcs7 = PaddingMode.PKCS7;

        /// <summary>
        /// Represents the <see cref="PaddingMode.Zeros" /> padding mode.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const PaddingMode PaddingModeZeros = PaddingMode.Zeros;

#pragma warning restore SecurityIntelliSenseCS
    }
}