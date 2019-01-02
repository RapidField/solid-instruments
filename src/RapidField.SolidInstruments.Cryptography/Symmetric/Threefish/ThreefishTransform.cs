// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Threefish
{
    /// <summary>
    /// Performs a cryptographic transformation of data using the Threefish algorithm.
    /// </summary>
    internal sealed class ThreefishTransform : CryptographicTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreefishTransform" /> class.
        /// </summary>
        /// <param name="keySize">
        /// The bit-length of the key for the cipher.
        /// </param>
        /// <param name="key">
        /// The private key.
        /// </param>
        /// <param name="initializationVector">
        /// An initialization vector for the cipher.
        /// </param>
        /// <param name="tweakWordOne">
        /// The first 64-bit tweak word.
        /// </param>
        /// <param name="tweakWordTwo">
        /// The second 64-bit tweak word.
        /// </param>
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
        /// <paramref name="blockSize" /> is less than or equal to zero, or <paramref name="encryptionDirection" /> is equal to
        /// <see cref="EncryptionDirection.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        public ThreefishTransform(Int32 keySize, Byte[] key, Byte[] initializationVector, UInt64 tweakWordOne, UInt64 tweakWordTwo, Int32 blockSize, CipherMode mode, PaddingMode paddingMode, EncryptionDirection encryptionDirection)
            : base(blockSize, mode, paddingMode, encryptionDirection)
        {
            InitializationVector = new UInt64[initializationVector is null ? 0 : (initializationVector.Length / 8)];
            KeyLength = keySize;
            Key = new UInt64[KeyWordCount];
            Tweak = new UInt64[TweakArrayLength];
            InitializeKey(key);
            InitializeInitializationVector(initializationVector);
            InitializeTweak(tweakWordOne, tweakWordTwo);
        }

        /// <summary>
        /// Performs the decryption operation on the specified block.
        /// </summary>
        /// <param name="block">
        /// The block to be decrypted.
        /// </param>
        [DebuggerHidden]
        protected override void DecryptBlock(Byte[] block)
        {
            var buffer = new UInt64[BlockWordCount];
            var blockWords = new UInt64[BlockWordCount];

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] = BitConverter.ToUInt64(block, (i * 8));
            }

            switch (Mode)
            {
                case CipherMode.CBC:

                    blockWords.CopyTo(buffer, 0);
                    break;

                case CipherMode.ECB:

                    break;

                default:

                    throw new InvalidOperationException(UnsupportedCipherModeExceptionMessageTemplate.ApplyFormat(Mode));
            }

            switch (BlockSizeInBytes)
            {
                case 32:

                    DecryptTwoHundredFiftySixBits(ref blockWords);
                    break;

                case 64:

                    DecryptFiveHundredTwelveBits(ref blockWords);
                    break;

                case 128:

                    DecryptOneThousandTwentyFourBits(ref blockWords);
                    break;

                default:

                    throw new InvalidOperationException(UnsupportedBlockSizeExceptionMessageTemplate.ApplyFormat((BlockSizeInBits)));
            }

            for (var i = 0; i < BlockWordCount; i++)
            {
                switch (Mode)
                {
                    case CipherMode.CBC:

                        blockWords[i] ^= InitializationVector[i];
                        InitializationVector[i] = buffer[i];
                        break;

                    case CipherMode.ECB:

                        break;

                    default:

                        throw new InvalidOperationException(UnsupportedCipherModeExceptionMessageTemplate.ApplyFormat(Mode));
                }

                var currentWord = BitConverter.GetBytes(blockWords[i]);

                for (var j = 0; j < 8; j++)
                {
                    block[((i * 8) + j)] = currentWord[j];
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ThreefishTransform" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        [DebuggerHidden]
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Performs the encryption operation on the specified block.
        /// </summary>
        /// <param name="block">
        /// The block to be encrypted.
        /// </param>
        [DebuggerHidden]
        protected override void EncryptBlock(Byte[] block)
        {
            var blockWords = new UInt64[BlockWordCount];

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] = BitConverter.ToUInt64(block, (i * 8));

                switch (Mode)
                {
                    case CipherMode.CBC:

                        blockWords[i] ^= InitializationVector[i];
                        break;

                    case CipherMode.ECB:

                        break;

                    default:

                        throw new InvalidOperationException(UnsupportedCipherModeExceptionMessageTemplate.ApplyFormat(Mode));
                }
            }

            switch (BlockSizeInBytes)
            {
                case 32:

                    EncryptTwoHundredFiftySixBits(ref blockWords);
                    break;

                case 64:

                    EncryptFiveHundredTwelveBits(ref blockWords);
                    break;

                case 128:

                    EncryptOneThousandTwentyFourBits(ref blockWords);
                    break;

                default:

                    throw new InvalidOperationException(UnsupportedBlockSizeExceptionMessageTemplate.ApplyFormat(BlockSizeInBits));
            }

            for (var i = 0; i < BlockWordCount; i++)
            {
                switch (Mode)
                {
                    case CipherMode.CBC:

                        InitializationVector[i] = blockWords[i];
                        break;

                    case CipherMode.ECB:

                        break;

                    default:

                        throw new InvalidOperationException(UnsupportedCipherModeExceptionMessageTemplate.ApplyFormat(Mode));
                }

                var currentWord = BitConverter.GetBytes(blockWords[i]);

                for (var j = 0; j < 8; j++)
                {
                    block[((i * 8) + j)] = currentWord[j];
                }
            }
        }

        /// <summary>
        /// Perform a forward mix operation on the specified 64-bit words.
        /// </summary>
        /// <param name="firstValue">
        /// The first 64-bit word.
        /// </param>
        /// <param name="secondValue">
        /// The second 64-bit word.
        /// </param>
        /// <param name="rotationBitCount">
        /// A bit rotation value for the operation.
        /// </param>
        [DebuggerHidden]
        private static void MixForward(ref UInt64 firstValue, ref UInt64 secondValue, Int32 rotationBitCount)
        {
            firstValue += secondValue;
            secondValue = RotateLeft(secondValue, rotationBitCount) ^ firstValue;
        }

        /// <summary>
        /// Perform a forward mix operation on the specified 64-bit words.
        /// </summary>
        /// <param name="firstValue">
        /// The first 64-bit word.
        /// </param>
        /// <param name="secondValue">
        /// The second 64-bit word.
        /// </param>
        /// <param name="rotationBitCount">
        /// A bit rotation value for the operation.
        /// </param>
        /// <param name="firstKeySegment">
        /// The first key segment modifier for the operation.
        /// </param>
        /// <param name="secondKeySegment">
        /// The second key segment modifier for the operation.
        /// </param>
        [DebuggerHidden]
        private static void MixForward(ref UInt64 firstValue, ref UInt64 secondValue, Int32 rotationBitCount, UInt64 firstKeySegment, UInt64 secondKeySegment)
        {
            secondValue += secondKeySegment;
            firstValue += secondValue + firstKeySegment;
            secondValue = RotateLeft(secondValue, rotationBitCount) ^ firstValue;
        }

        /// <summary>
        /// Perform a reverse mix operation on the specified 64-bit words.
        /// </summary>
        /// <param name="firstValue">
        /// The first 64-bit word.
        /// </param>
        /// <param name="secondValue">
        /// The second 64-bit word.
        /// </param>
        /// <param name="rotationBitCount">
        /// A bit rotation value for the operation.
        /// </param>
        [DebuggerHidden]
        private static void MixReverse(ref UInt64 firstValue, ref UInt64 secondValue, Int32 rotationBitCount)
        {
            secondValue = RotateRight(secondValue ^ firstValue, rotationBitCount);
            firstValue -= secondValue;
        }

        /// <summary>
        /// Perform a reverse mix operation on the specified 64-bit words.
        /// </summary>
        /// <param name="firstValue">
        /// The first 64-bit word.
        /// </param>
        /// <param name="secondValue">
        /// The second 64-bit word.
        /// </param>
        /// <param name="rotationBitCount">
        /// A bit rotation value for the operation.
        /// </param>
        /// <param name="firstKeySegment">
        /// The first key segment modifier for the operation.
        /// </param>
        /// <param name="secondKeySegment">
        /// The second key segment modifier for the operation.
        /// </param>
        [DebuggerHidden]
        private static void MixReverse(ref UInt64 firstValue, ref UInt64 secondValue, Int32 rotationBitCount, UInt64 firstKeySegment, UInt64 secondKeySegment)
        {
            secondValue = RotateRight(secondValue ^ firstValue, rotationBitCount);
            firstValue -= secondValue + firstKeySegment;
            secondValue -= secondKeySegment;
        }

        /// <summary>
        /// Rotates the specified 64-bit value left by the specified number of bits.
        /// </summary>
        /// <param name="value">
        /// The value on which to perform a bit rotation operation.
        /// </param>
        /// <param name="bitCount">
        /// The number of bits to rotate the value by.
        /// </param>
        /// <returns>
        /// The result of the rotation operation.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt64 RotateLeft(UInt64 value, Int32 bitCount) => (value << bitCount) | (value >> (64 - bitCount));

        /// <summary>
        /// Rotates the specified 64-bit value right by the specified number of bits.
        /// </summary>
        /// <param name="value">
        /// The value on which to perform a bit rotation operation.
        /// </param>
        /// <param name="bitCount">
        /// The number of bits to rotate the value by.
        /// </param>
        /// <returns>
        /// The result of the rotation operation.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt64 RotateRight(UInt64 value, Int32 bitCount) => (value >> bitCount) | (value << (64 - bitCount));

        /// <summary>
        /// Performs the decryption operation on the specified 512-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be decrypted.
        /// </param>
        [DebuggerHidden]
        private void DecryptFiveHundredTwelveBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var blockWordFive = block[4];
            var blockWordSix = block[5];
            var blockWordSeven = block[6];
            var blockWordEight = block[7];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var keyWordSix = Key[5];
            var keyWordSeven = Key[6];
            var keyWordEight = Key[7];
            var keyWordNine = Key[8];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            blockWordOne -= keyWordOne;
            blockWordTwo -= keyWordTwo;
            blockWordThree -= keyWordThree;
            blockWordFour -= keyWordFour;
            blockWordFive -= keyWordFive;
            blockWordSix -= (keyWordSix + tweakWordOne);
            blockWordSeven -= (keyWordSeven + tweakWordTwo);
            blockWordEight -= (keyWordEight + 18);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordSix + tweakWordOne), (keyWordSeven + 17));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordFour, (keyWordFive + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordNine, keyWordOne);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordFive + tweakWordThree), (keyWordSix + 16));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordThree, (keyWordFour + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordEight, keyWordNine);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordFour + tweakWordTwo), (keyWordFive + 15));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordTwo, (keyWordThree + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordNine, keyWordOne);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordThree + tweakWordOne), (keyWordFour + 14));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordOne, (keyWordTwo + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordEight, keyWordNine);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordTwo + tweakWordThree), (keyWordThree + 13));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordNine, (keyWordOne + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordFive, keyWordSix);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordOne + tweakWordTwo), (keyWordTwo + 12));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordEight, (keyWordNine + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordFour, keyWordFive);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordNine + tweakWordOne), (keyWordOne + 11));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordSeven, (keyWordEight + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordFive, keyWordSix);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordThree, keyWordFour);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordEight + tweakWordThree), (keyWordNine + 10));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordFour, keyWordFive);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordSeven + tweakWordTwo), (keyWordEight + 9));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordFive, (keyWordSix + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordThree, keyWordFour);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordSix + tweakWordOne), (keyWordSeven + 8));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordFour, (keyWordFive + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordNine, keyWordOne);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordFive + tweakWordThree), (keyWordSix + 7));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordThree, (keyWordFour + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordEight, keyWordNine);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordTwo, (keyWordThree + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordNine, keyWordOne);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordOne, (keyWordTwo + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordEight, keyWordNine);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordNine, (keyWordOne + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordFive, keyWordSix);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordEight, (keyWordNine + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordFour, keyWordFive);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordNine + tweakWordOne), (keyWordOne + 2));
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordSeven, (keyWordEight + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordFive, keyWordSix);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordThree, keyWordFour);
            MixReverse(ref blockWordFive, ref blockWordFour, 22);
            MixReverse(ref blockWordThree, ref blockWordSix, 56);
            MixReverse(ref blockWordOne, ref blockWordEight, 35);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 8);
            MixReverse(ref blockWordThree, ref blockWordEight, 43);
            MixReverse(ref blockWordOne, ref blockWordSix, 39);
            MixReverse(ref blockWordSeven, ref blockWordFour, 29);
            MixReverse(ref blockWordFive, ref blockWordTwo, 25);
            MixReverse(ref blockWordOne, ref blockWordFour, 17);
            MixReverse(ref blockWordSeven, ref blockWordSix, 10);
            MixReverse(ref blockWordFive, ref blockWordEight, 50);
            MixReverse(ref blockWordThree, ref blockWordTwo, 13);
            MixReverse(ref blockWordSeven, ref blockWordEight, 24, (keyWordEight + tweakWordThree), (keyWordNine + 1));
            MixReverse(ref blockWordFive, ref blockWordSix, 34, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 30, keyWordFour, keyWordFive);
            MixReverse(ref blockWordOne, ref blockWordTwo, 39, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordFive, ref blockWordFour, 56);
            MixReverse(ref blockWordThree, ref blockWordSix, 54);
            MixReverse(ref blockWordOne, ref blockWordEight, 9);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 44);
            MixReverse(ref blockWordThree, ref blockWordEight, 39);
            MixReverse(ref blockWordOne, ref blockWordSix, 36);
            MixReverse(ref blockWordSeven, ref blockWordFour, 49);
            MixReverse(ref blockWordFive, ref blockWordTwo, 17);
            MixReverse(ref blockWordOne, ref blockWordFour, 42);
            MixReverse(ref blockWordSeven, ref blockWordSix, 14);
            MixReverse(ref blockWordFive, ref blockWordEight, 27);
            MixReverse(ref blockWordThree, ref blockWordTwo, 33);
            MixReverse(ref blockWordSeven, ref blockWordEight, 37, (keyWordSeven + tweakWordTwo), keyWordEight);
            MixReverse(ref blockWordFive, ref blockWordSix, 19, keyWordFive, (keyWordSix + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 36, keyWordThree, keyWordFour);
            MixReverse(ref blockWordOne, ref blockWordTwo, 46, keyWordOne, keyWordTwo);
            block[7] = blockWordEight;
            block[6] = blockWordSeven;
            block[5] = blockWordSix;
            block[4] = blockWordFive;
            block[3] = blockWordFour;
            block[2] = blockWordThree;
            block[1] = blockWordTwo;
            block[0] = blockWordOne;
        }

        /// <summary>
        /// Performs the decryption operation on the specified 1024-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be decrypted.
        /// </param>
        [DebuggerHidden]
        private void DecryptOneThousandTwentyFourBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var blockWordFive = block[4];
            var blockWordSix = block[5];
            var blockWordSeven = block[6];
            var blockWordEight = block[7];
            var blockWordNine = block[8];
            var blockWordTen = block[9];
            var blockWordEleven = block[10];
            var blockWordTwelve = block[11];
            var blockWordThirteen = block[12];
            var blockWordFourteen = block[13];
            var blockWordFifteen = block[14];
            var blockWordSixteen = block[15];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var keyWordSix = Key[5];
            var keyWordSeven = Key[6];
            var keyWordEight = Key[7];
            var keyWordNine = Key[8];
            var keyWordTen = Key[9];
            var keyWordEleven = Key[10];
            var keyWordTwelve = Key[11];
            var keyWordThirteen = Key[12];
            var keyWordFourteen = Key[13];
            var keyWordFifteen = Key[14];
            var keyWordSixteen = Key[15];
            var keyWordSeventeen = Key[16];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            blockWordOne -= keyWordFour;
            blockWordTwo -= keyWordFive;
            blockWordThree -= keyWordSix;
            blockWordFour -= keyWordSeven;
            blockWordFive -= keyWordEight;
            blockWordSix -= keyWordNine;
            blockWordSeven -= keyWordTen;
            blockWordEight -= keyWordEleven;
            blockWordNine -= keyWordTwelve;
            blockWordTen -= keyWordThirteen;
            blockWordEleven -= keyWordFourteen;
            blockWordTwelve -= keyWordFifteen;
            blockWordThirteen -= keyWordSixteen;
            blockWordFourteen -= (keyWordSeventeen + tweakWordThree);
            blockWordFifteen -= (keyWordOne + tweakWordOne);
            blockWordSixteen -= (keyWordTwo + 20);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSeventeen + tweakWordThree), (keyWordOne + 19));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFifteen, (keyWordSixteen + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordNine, keyWordTen);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordFive, keyWordSix);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordThree, keyWordFour);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSixteen + tweakWordTwo), (keyWordSeventeen + 18));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFourteen, (keyWordFifteen + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordEight, keyWordNine);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordFour, keyWordFive);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordFifteen + tweakWordOne), (keyWordSixteen + 17));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordThirteen, (keyWordFourteen + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordNine, keyWordTen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordFive, keyWordSix);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordThree, keyWordFour);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFourteen + tweakWordThree), (keyWordFifteen + 16));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTwelve, (keyWordThirteen + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordEight, keyWordNine);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordFour, keyWordFive);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordThirteen + tweakWordTwo), (keyWordFourteen + 15));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordEleven, (keyWordTwelve + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordNine, keyWordTen);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordFive, keyWordSix);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordThree, keyWordFour);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTwelve + tweakWordOne), (keyWordThirteen + 14));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTen, (keyWordEleven + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordEight, keyWordNine);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordFour, keyWordFive);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordEleven + tweakWordThree), (keyWordTwelve + 13));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordNine, (keyWordTen + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordFive, keyWordSix);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordThree, keyWordFour);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTen + tweakWordTwo), (keyWordEleven + 12));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordEight, (keyWordNine + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordFour, keyWordFive);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordNine + tweakWordOne), (keyWordTen + 11));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordSeven, (keyWordEight + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordFive, keyWordSix);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordThree, keyWordFour);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordEight + tweakWordThree), (keyWordNine + 10));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordFour, keyWordFive);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSeven + tweakWordTwo), (keyWordEight + 9));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFive, (keyWordSix + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordThree, keyWordFour);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSix + tweakWordOne), (keyWordSeven + 8));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFour, (keyWordFive + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordNine, keyWordTen);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordFive + tweakWordThree), (keyWordSix + 7));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordThree, (keyWordFour + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordOne, keyWordTwo);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordEight, keyWordNine);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTwo, (keyWordThree + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordSeventeen, keyWordOne);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordNine, keyWordTen);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordOne, (keyWordTwo + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordSixteen, keyWordSeventeen);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordEight, keyWordNine);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordSeventeen, (keyWordOne + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordFifteen, keyWordSixteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordNine, keyWordTen);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordFive, keyWordSix);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordSixteen, (keyWordSeventeen + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordFourteen, keyWordFifteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordEight, keyWordNine);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordFour, keyWordFive);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSeventeen + tweakWordOne), (keyWordOne + 2));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFifteen, (keyWordSixteen + tweakWordThree));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordThirteen, keyWordFourteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordNine, keyWordTen);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordFive, keyWordSix);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordThree, keyWordFour);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 20);
            MixReverse(ref blockWordEleven, ref blockWordFour, 37);
            MixReverse(ref blockWordNine, ref blockWordSix, 31);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 23);
            MixReverse(ref blockWordFive, ref blockWordTen, 52);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 35);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 48);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 9);
            MixReverse(ref blockWordEleven, ref blockWordTen, 25);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 44);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 46);
            MixReverse(ref blockWordFive, ref blockWordFour, 47);
            MixReverse(ref blockWordThree, ref blockWordSix, 44);
            MixReverse(ref blockWordOne, ref blockWordEight, 31);
            MixReverse(ref blockWordNine, ref blockWordTwo, 41);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 42);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 53);
            MixReverse(ref blockWordEleven, ref blockWordEight, 4);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 51);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 56);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 34);
            MixReverse(ref blockWordOne, ref blockWordTen, 16);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSixteen + tweakWordThree), (keyWordSeventeen + 1));
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFourteen, (keyWordFifteen + tweakWordTwo));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 47, keyWordTwelve, keyWordThirteen);
            MixReverse(ref blockWordNine, ref blockWordTen, 12, keyWordTen, keyWordEleven);
            MixReverse(ref blockWordSeven, ref blockWordEight, 31, keyWordEight, keyWordNine);
            MixReverse(ref blockWordFive, ref blockWordSix, 37, keyWordSix, keyWordSeven);
            MixReverse(ref blockWordThree, ref blockWordFour, 9, keyWordFour, keyWordFive);
            MixReverse(ref blockWordOne, ref blockWordTwo, 41, keyWordTwo, keyWordThree);
            MixReverse(ref blockWordThirteen, ref blockWordEight, 25);
            MixReverse(ref blockWordEleven, ref blockWordFour, 16);
            MixReverse(ref blockWordNine, ref blockWordSix, 28);
            MixReverse(ref blockWordFifteen, ref blockWordTwo, 47);
            MixReverse(ref blockWordFive, ref blockWordTen, 41);
            MixReverse(ref blockWordSeven, ref blockWordFourteen, 48);
            MixReverse(ref blockWordThree, ref blockWordTwelve, 20);
            MixReverse(ref blockWordOne, ref blockWordSixteen, 5);
            MixReverse(ref blockWordEleven, ref blockWordTen, 17);
            MixReverse(ref blockWordNine, ref blockWordTwelve, 59);
            MixReverse(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixReverse(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixReverse(ref blockWordSeven, ref blockWordTwo, 13);
            MixReverse(ref blockWordFive, ref blockWordFour, 51);
            MixReverse(ref blockWordThree, ref blockWordSix, 4);
            MixReverse(ref blockWordOne, ref blockWordEight, 33);
            MixReverse(ref blockWordNine, ref blockWordTwo, 52);
            MixReverse(ref blockWordFifteen, ref blockWordSix, 23);
            MixReverse(ref blockWordThirteen, ref blockWordFour, 18);
            MixReverse(ref blockWordEleven, ref blockWordEight, 49);
            MixReverse(ref blockWordFive, ref blockWordSixteen, 55);
            MixReverse(ref blockWordSeven, ref blockWordTwelve, 10);
            MixReverse(ref blockWordThree, ref blockWordFourteen, 19);
            MixReverse(ref blockWordOne, ref blockWordTen, 38);
            MixReverse(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFifteen + tweakWordTwo), keyWordSixteen);
            MixReverse(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordThirteen, (keyWordFourteen + tweakWordOne));
            MixReverse(ref blockWordEleven, ref blockWordTwelve, 17, keyWordEleven, keyWordTwelve);
            MixReverse(ref blockWordNine, ref blockWordTen, 8, keyWordNine, keyWordTen);
            MixReverse(ref blockWordSeven, ref blockWordEight, 47, keyWordSeven, keyWordEight);
            MixReverse(ref blockWordFive, ref blockWordSix, 8, keyWordFive, keyWordSix);
            MixReverse(ref blockWordThree, ref blockWordFour, 13, keyWordThree, keyWordFour);
            MixReverse(ref blockWordOne, ref blockWordTwo, 24, keyWordOne, keyWordTwo);
            block[15] = blockWordSixteen;
            block[14] = blockWordFifteen;
            block[13] = blockWordFourteen;
            block[12] = blockWordThirteen;
            block[11] = blockWordTwelve;
            block[10] = blockWordEleven;
            block[9] = blockWordTen;
            block[8] = blockWordNine;
            block[7] = blockWordEight;
            block[6] = blockWordSeven;
            block[5] = blockWordSix;
            block[4] = blockWordFive;
            block[3] = blockWordFour;
            block[2] = blockWordThree;
            block[1] = blockWordTwo;
            block[0] = blockWordOne;
        }

        /// <summary>
        /// Performs the decryption operation on the specified 256-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be decrypted.
        /// </param>
        [DebuggerHidden]
        private void DecryptTwoHundredFiftySixBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            blockWordOne -= keyWordFour;
            blockWordTwo -= (keyWordFive + tweakWordOne);
            blockWordThree -= (keyWordOne + tweakWordTwo);
            blockWordFour -= (keyWordTwo + 18);
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordThree, (keyWordFour + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordFive + tweakWordOne), (keyWordOne + 17));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordTwo, (keyWordThree + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordFour + tweakWordThree), (keyWordFive + 16));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordOne, (keyWordTwo + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordThree + tweakWordTwo), (keyWordFour + 15));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordFive, (keyWordOne + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordTwo + tweakWordOne), (keyWordThree + 14));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordFour, (keyWordFive + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordOne + tweakWordThree), (keyWordTwo + 13));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordThree, (keyWordFour + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordFive + tweakWordTwo), (keyWordOne + 12));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordTwo, (keyWordThree + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordFour + tweakWordOne), (keyWordFive + 11));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordOne, (keyWordTwo + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordThree + tweakWordThree), (keyWordFour + 10));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordFive, (keyWordOne + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordTwo + tweakWordTwo), (keyWordThree + 9));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordFour, (keyWordFive + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordOne + tweakWordOne), (keyWordTwo + 8));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordThree, (keyWordFour + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordFive + tweakWordThree), (keyWordOne + 7));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordTwo, (keyWordThree + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordOne, (keyWordTwo + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordFive, (keyWordOne + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordFour, (keyWordFive + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordThree, (keyWordFour + tweakWordThree));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordFive + tweakWordOne), (keyWordOne + 2));
            MixReverse(ref blockWordOne, ref blockWordFour, 32);
            MixReverse(ref blockWordThree, ref blockWordTwo, 32);
            MixReverse(ref blockWordOne, ref blockWordTwo, 58);
            MixReverse(ref blockWordThree, ref blockWordFour, 22);
            MixReverse(ref blockWordOne, ref blockWordFour, 46);
            MixReverse(ref blockWordThree, ref blockWordTwo, 12);
            MixReverse(ref blockWordOne, ref blockWordTwo, 25, keyWordTwo, (keyWordThree + tweakWordTwo));
            MixReverse(ref blockWordThree, ref blockWordFour, 33, (keyWordFour + tweakWordThree), (keyWordFive + 1));
            MixReverse(ref blockWordOne, ref blockWordFour, 5);
            MixReverse(ref blockWordThree, ref blockWordTwo, 37);
            MixReverse(ref blockWordOne, ref blockWordTwo, 23);
            MixReverse(ref blockWordThree, ref blockWordFour, 40);
            MixReverse(ref blockWordOne, ref blockWordFour, 52);
            MixReverse(ref blockWordThree, ref blockWordTwo, 57);
            MixReverse(ref blockWordOne, ref blockWordTwo, 14, keyWordOne, (keyWordTwo + tweakWordOne));
            MixReverse(ref blockWordThree, ref blockWordFour, 16, (keyWordThree + tweakWordTwo), keyWordFour);
            block[0] = blockWordOne;
            block[1] = blockWordTwo;
            block[2] = blockWordThree;
            block[3] = blockWordFour;
        }

        /// <summary>
        /// Performs the encryption operation on the specified 512-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be encrypted.
        /// </param>
        [DebuggerHidden]
        private void EncryptFiveHundredTwelveBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var blockWordFive = block[4];
            var blockWordSix = block[5];
            var blockWordSeven = block[6];
            var blockWordEight = block[7];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var keyWordSix = Key[5];
            var keyWordSeven = Key[6];
            var keyWordEight = Key[7];
            var keyWordNine = Key[8];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordOne, keyWordTwo);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordThree, keyWordFour);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordFive, (keyWordSix + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordSeven + tweakWordTwo), keyWordEight);
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordTwo, keyWordThree);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordFour, keyWordFive);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordEight + tweakWordThree), (keyWordNine + 1));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordThree, keyWordFour);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordFive, keyWordSix);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordSeven, (keyWordEight + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordNine + tweakWordOne), (keyWordOne + 2));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordFour, keyWordFive);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordSix, keyWordSeven);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordEight, (keyWordNine + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordFive, keyWordSix);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordSeven, keyWordEight);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordNine, (keyWordOne + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordSix, keyWordSeven);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordEight, keyWordNine);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordOne, (keyWordTwo + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordSeven, keyWordEight);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordNine, keyWordOne);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordTwo, (keyWordThree + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordEight, keyWordNine);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordOne, keyWordTwo);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordThree, (keyWordFour + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordFive + tweakWordThree), (keyWordSix + 7));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordNine, keyWordOne);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordTwo, keyWordThree);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordFour, (keyWordFive + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordSix + tweakWordOne), (keyWordSeven + 8));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordOne, keyWordTwo);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordThree, keyWordFour);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordFive, (keyWordSix + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordSeven + tweakWordTwo), (keyWordEight + 9));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordTwo, keyWordThree);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordFour, keyWordFive);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordEight + tweakWordThree), (keyWordNine + 10));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordThree, keyWordFour);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordFive, keyWordSix);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordSeven, (keyWordEight + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordNine + tweakWordOne), (keyWordOne + 11));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordFour, keyWordFive);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordSix, keyWordSeven);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordEight, (keyWordNine + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordOne + tweakWordTwo), (keyWordTwo + 12));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordFive, keyWordSix);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordSeven, keyWordEight);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordNine, (keyWordOne + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordTwo + tweakWordThree), (keyWordThree + 13));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordSix, keyWordSeven);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordEight, keyWordNine);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordOne, (keyWordTwo + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordThree + tweakWordOne), (keyWordFour + 14));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordSeven, keyWordEight);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordNine, keyWordOne);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordTwo, (keyWordThree + tweakWordOne));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordFour + tweakWordTwo), (keyWordFive + 15));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordTwo, 46, keyWordEight, keyWordNine);
            MixForward(ref blockWordThree, ref blockWordFour, 36, keyWordOne, keyWordTwo);
            MixForward(ref blockWordFive, ref blockWordSix, 19, keyWordThree, (keyWordFour + tweakWordTwo));
            MixForward(ref blockWordSeven, ref blockWordEight, 37, (keyWordFive + tweakWordThree), (keyWordSix + 16));
            MixForward(ref blockWordThree, ref blockWordTwo, 33);
            MixForward(ref blockWordFive, ref blockWordEight, 27);
            MixForward(ref blockWordSeven, ref blockWordSix, 14);
            MixForward(ref blockWordOne, ref blockWordFour, 42);
            MixForward(ref blockWordFive, ref blockWordTwo, 17);
            MixForward(ref blockWordSeven, ref blockWordFour, 49);
            MixForward(ref blockWordOne, ref blockWordSix, 36);
            MixForward(ref blockWordThree, ref blockWordEight, 39);
            MixForward(ref blockWordSeven, ref blockWordTwo, 44);
            MixForward(ref blockWordOne, ref blockWordEight, 9);
            MixForward(ref blockWordThree, ref blockWordSix, 54);
            MixForward(ref blockWordFive, ref blockWordFour, 56);
            MixForward(ref blockWordOne, ref blockWordTwo, 39, keyWordNine, keyWordOne);
            MixForward(ref blockWordThree, ref blockWordFour, 30, keyWordTwo, keyWordThree);
            MixForward(ref blockWordFive, ref blockWordSix, 34, keyWordFour, (keyWordFive + tweakWordThree));
            MixForward(ref blockWordSeven, ref blockWordEight, 24, (keyWordSix + tweakWordOne), (keyWordSeven + 17));
            MixForward(ref blockWordThree, ref blockWordTwo, 13);
            MixForward(ref blockWordFive, ref blockWordEight, 50);
            MixForward(ref blockWordSeven, ref blockWordSix, 10);
            MixForward(ref blockWordOne, ref blockWordFour, 17);
            MixForward(ref blockWordFive, ref blockWordTwo, 25);
            MixForward(ref blockWordSeven, ref blockWordFour, 29);
            MixForward(ref blockWordOne, ref blockWordSix, 39);
            MixForward(ref blockWordThree, ref blockWordEight, 43);
            MixForward(ref blockWordSeven, ref blockWordTwo, 8);
            MixForward(ref blockWordOne, ref blockWordEight, 35);
            MixForward(ref blockWordThree, ref blockWordSix, 56);
            MixForward(ref blockWordFive, ref blockWordFour, 22);
            block[0] = (blockWordOne + keyWordOne);
            block[1] = (blockWordTwo + keyWordTwo);
            block[2] = (blockWordThree + keyWordThree);
            block[3] = (blockWordFour + keyWordFour);
            block[4] = (blockWordFive + keyWordFive);
            block[5] = (blockWordSix + keyWordSix + tweakWordOne);
            block[6] = (blockWordSeven + keyWordSeven + tweakWordTwo);
            block[7] = (blockWordEight + keyWordEight + 18);
        }

        /// <summary>
        /// Performs the encryption operation on the specified 1024-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be encrypted.
        /// </param>
        [DebuggerHidden]
        private void EncryptOneThousandTwentyFourBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var blockWordFive = block[4];
            var blockWordSix = block[5];
            var blockWordSeven = block[6];
            var blockWordEight = block[7];
            var blockWordNine = block[8];
            var blockWordTen = block[9];
            var blockWordEleven = block[10];
            var blockWordTwelve = block[11];
            var blockWordThirteen = block[12];
            var blockWordFourteen = block[13];
            var blockWordFifteen = block[14];
            var blockWordSixteen = block[15];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var keyWordSix = Key[5];
            var keyWordSeven = Key[6];
            var keyWordEight = Key[7];
            var keyWordNine = Key[8];
            var keyWordTen = Key[9];
            var keyWordEleven = Key[10];
            var keyWordTwelve = Key[11];
            var keyWordThirteen = Key[12];
            var keyWordFourteen = Key[13];
            var keyWordFifteen = Key[14];
            var keyWordSixteen = Key[15];
            var keyWordSeventeen = Key[16];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordOne, keyWordTwo);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordThree, keyWordFour);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordFive, keyWordSix);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordSeven, keyWordEight);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordNine, keyWordTen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordThirteen, (keyWordFourteen + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFifteen + tweakWordTwo), keyWordSixteen);
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordTwo, keyWordThree);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordFour, keyWordFive);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordSix, keyWordSeven);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordEight, keyWordNine);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordTen, keyWordEleven);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFourteen, (keyWordFifteen + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSixteen + tweakWordThree), (keyWordSeventeen + 1));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordThree, keyWordFour);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordFive, keyWordSix);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordSeven, keyWordEight);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordNine, keyWordTen);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFifteen, (keyWordSixteen + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSeventeen + tweakWordOne), (keyWordOne + 2));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordFour, keyWordFive);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordSix, keyWordSeven);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordEight, keyWordNine);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordTen, keyWordEleven);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordSixteen, (keyWordSeventeen + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordFive, keyWordSix);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordSeven, keyWordEight);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordNine, keyWordTen);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordSeventeen, (keyWordOne + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordSix, keyWordSeven);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordEight, keyWordNine);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordTen, keyWordEleven);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordOne, (keyWordTwo + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordSeven, keyWordEight);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordNine, keyWordTen);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTwo, (keyWordThree + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordEight, keyWordNine);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordTen, keyWordEleven);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordOne, keyWordTwo);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordThree, (keyWordFour + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordFive + tweakWordThree), (keyWordSix + 7));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordNine, keyWordTen);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTwo, keyWordThree);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFour, (keyWordFive + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSix + tweakWordOne), (keyWordSeven + 8));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordTen, keyWordEleven);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordOne, keyWordTwo);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordThree, keyWordFour);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFive, (keyWordSix + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSeven + tweakWordTwo), (keyWordEight + 9));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordTwo, keyWordThree);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordFour, keyWordFive);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordSix, (keyWordSeven + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordEight + tweakWordThree), (keyWordNine + 10));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordOne, keyWordTwo);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordThree, keyWordFour);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordFive, keyWordSix);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordSeven, (keyWordEight + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordNine + tweakWordOne), (keyWordTen + 11));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordTwo, keyWordThree);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordFour, keyWordFive);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordSix, keyWordSeven);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordEight, (keyWordNine + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTen + tweakWordTwo), (keyWordEleven + 12));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordFourteen, keyWordFifteen);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordOne, keyWordTwo);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordThree, keyWordFour);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordFive, keyWordSix);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordSeven, keyWordEight);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordNine, (keyWordTen + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordEleven + tweakWordThree), (keyWordTwelve + 13));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordFifteen, keyWordSixteen);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordTwo, keyWordThree);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordFour, keyWordFive);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordSix, keyWordSeven);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordEight, keyWordNine);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTen, (keyWordEleven + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordTwelve + tweakWordOne), (keyWordThirteen + 14));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordSixteen, keyWordSeventeen);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordOne, keyWordTwo);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordThree, keyWordFour);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordFive, keyWordSix);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordSeven, keyWordEight);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordNine, keyWordTen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordEleven, (keyWordTwelve + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordThirteen + tweakWordTwo), (keyWordFourteen + 15));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordSeventeen, keyWordOne);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordTwo, keyWordThree);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordFour, keyWordFive);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordSix, keyWordSeven);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordEight, keyWordNine);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTen, keyWordEleven);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordTwelve, (keyWordThirteen + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordFourteen + tweakWordThree), (keyWordFifteen + 16));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordOne, keyWordTwo);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordThree, keyWordFour);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordFive, keyWordSix);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordSeven, keyWordEight);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordNine, keyWordTen);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordThirteen, (keyWordFourteen + tweakWordThree));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordFifteen + tweakWordOne), (keyWordSixteen + 17));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            MixForward(ref blockWordOne, ref blockWordTwo, 24, keyWordTwo, keyWordThree);
            MixForward(ref blockWordThree, ref blockWordFour, 13, keyWordFour, keyWordFive);
            MixForward(ref blockWordFive, ref blockWordSix, 8, keyWordSix, keyWordSeven);
            MixForward(ref blockWordSeven, ref blockWordEight, 47, keyWordEight, keyWordNine);
            MixForward(ref blockWordNine, ref blockWordTen, 8, keyWordTen, keyWordEleven);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 17, keyWordTwelve, keyWordThirteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 22, keyWordFourteen, (keyWordFifteen + tweakWordOne));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 37, (keyWordSixteen + tweakWordTwo), (keyWordSeventeen + 18));
            MixForward(ref blockWordOne, ref blockWordTen, 38);
            MixForward(ref blockWordThree, ref blockWordFourteen, 19);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 10);
            MixForward(ref blockWordFive, ref blockWordSixteen, 55);
            MixForward(ref blockWordEleven, ref blockWordEight, 49);
            MixForward(ref blockWordThirteen, ref blockWordFour, 18);
            MixForward(ref blockWordFifteen, ref blockWordSix, 23);
            MixForward(ref blockWordNine, ref blockWordTwo, 52);
            MixForward(ref blockWordOne, ref blockWordEight, 33);
            MixForward(ref blockWordThree, ref blockWordSix, 4);
            MixForward(ref blockWordFive, ref blockWordFour, 51);
            MixForward(ref blockWordSeven, ref blockWordTwo, 13);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 34);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 41);
            MixForward(ref blockWordNine, ref blockWordTwelve, 59);
            MixForward(ref blockWordEleven, ref blockWordTen, 17);
            MixForward(ref blockWordOne, ref blockWordSixteen, 5);
            MixForward(ref blockWordThree, ref blockWordTwelve, 20);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 48);
            MixForward(ref blockWordFive, ref blockWordTen, 41);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 47);
            MixForward(ref blockWordNine, ref blockWordSix, 28);
            MixForward(ref blockWordEleven, ref blockWordFour, 16);
            MixForward(ref blockWordThirteen, ref blockWordEight, 25);
            MixForward(ref blockWordOne, ref blockWordTwo, 41, keyWordThree, keyWordFour);
            MixForward(ref blockWordThree, ref blockWordFour, 9, keyWordFive, keyWordSix);
            MixForward(ref blockWordFive, ref blockWordSix, 37, keyWordSeven, keyWordEight);
            MixForward(ref blockWordSeven, ref blockWordEight, 31, keyWordNine, keyWordTen);
            MixForward(ref blockWordNine, ref blockWordTen, 12, keyWordEleven, keyWordTwelve);
            MixForward(ref blockWordEleven, ref blockWordTwelve, 47, keyWordThirteen, keyWordFourteen);
            MixForward(ref blockWordThirteen, ref blockWordFourteen, 44, keyWordFifteen, (keyWordSixteen + tweakWordTwo));
            MixForward(ref blockWordFifteen, ref blockWordSixteen, 30, (keyWordSeventeen + tweakWordThree), (keyWordOne + 19));
            MixForward(ref blockWordOne, ref blockWordTen, 16);
            MixForward(ref blockWordThree, ref blockWordFourteen, 34);
            MixForward(ref blockWordSeven, ref blockWordTwelve, 56);
            MixForward(ref blockWordFive, ref blockWordSixteen, 51);
            MixForward(ref blockWordEleven, ref blockWordEight, 4);
            MixForward(ref blockWordThirteen, ref blockWordFour, 53);
            MixForward(ref blockWordFifteen, ref blockWordSix, 42);
            MixForward(ref blockWordNine, ref blockWordTwo, 41);
            MixForward(ref blockWordOne, ref blockWordEight, 31);
            MixForward(ref blockWordThree, ref blockWordSix, 44);
            MixForward(ref blockWordFive, ref blockWordFour, 47);
            MixForward(ref blockWordSeven, ref blockWordTwo, 46);
            MixForward(ref blockWordThirteen, ref blockWordSixteen, 19);
            MixForward(ref blockWordFifteen, ref blockWordFourteen, 42);
            MixForward(ref blockWordNine, ref blockWordTwelve, 44);
            MixForward(ref blockWordEleven, ref blockWordTen, 25);
            MixForward(ref blockWordOne, ref blockWordSixteen, 9);
            MixForward(ref blockWordThree, ref blockWordTwelve, 48);
            MixForward(ref blockWordSeven, ref blockWordFourteen, 35);
            MixForward(ref blockWordFive, ref blockWordTen, 52);
            MixForward(ref blockWordFifteen, ref blockWordTwo, 23);
            MixForward(ref blockWordNine, ref blockWordSix, 31);
            MixForward(ref blockWordEleven, ref blockWordFour, 37);
            MixForward(ref blockWordThirteen, ref blockWordEight, 20);
            block[0] = (blockWordOne + keyWordFour);
            block[1] = (blockWordTwo + keyWordFive);
            block[2] = (blockWordThree + keyWordSix);
            block[3] = (blockWordFour + keyWordSeven);
            block[4] = (blockWordFive + keyWordEight);
            block[5] = (blockWordSix + keyWordNine);
            block[6] = (blockWordSeven + keyWordTen);
            block[7] = (blockWordEight + keyWordEleven);
            block[8] = (blockWordNine + keyWordTwelve);
            block[9] = (blockWordTen + keyWordThirteen);
            block[10] = (blockWordEleven + keyWordFourteen);
            block[11] = (blockWordTwelve + keyWordFifteen);
            block[12] = (blockWordThirteen + keyWordSixteen);
            block[13] = (blockWordFourteen + keyWordSeventeen + tweakWordThree);
            block[14] = (blockWordFifteen + keyWordOne + tweakWordOne);
            block[15] = (blockWordSixteen + keyWordTwo + 20);
        }

        /// <summary>
        /// Performs the encryption operation on the specified 256-bit block.
        /// </summary>
        /// <param name="block">
        /// The block to be encrypted.
        /// </param>
        [DebuggerHidden]
        private void EncryptTwoHundredFiftySixBits(ref UInt64[] block)
        {
            var blockWordOne = block[0];
            var blockWordTwo = block[1];
            var blockWordThree = block[2];
            var blockWordFour = block[3];
            var keyWordOne = Key[0];
            var keyWordTwo = Key[1];
            var keyWordThree = Key[2];
            var keyWordFour = Key[3];
            var keyWordFive = Key[4];
            var tweakWordOne = Tweak[0];
            var tweakWordTwo = Tweak[1];
            var tweakWordThree = Tweak[2];
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordOne, (keyWordTwo + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordThree + tweakWordTwo), keyWordFour);
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordTwo, (keyWordThree + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordFour + tweakWordThree), (keyWordFive + 1));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordThree, (keyWordFour + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordFive + tweakWordOne), (keyWordOne + 2));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordFour, (keyWordFive + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordOne + tweakWordTwo), (keyWordTwo + 3));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordFive, (keyWordOne + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordTwo + tweakWordThree), (keyWordThree + 4));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordOne, (keyWordTwo + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordThree + tweakWordOne), (keyWordFour + 5));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordTwo, (keyWordThree + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordFour + tweakWordTwo), (keyWordFive + 6));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordThree, (keyWordFour + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordFive + tweakWordThree), (keyWordOne + 7));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordFour, (keyWordFive + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordOne + tweakWordOne), (keyWordTwo + 8));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordFive, (keyWordOne + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordTwo + tweakWordTwo), (keyWordThree + 9));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordOne, (keyWordTwo + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordThree + tweakWordThree), (keyWordFour + 10));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordTwo, (keyWordThree + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordFour + tweakWordOne), (keyWordFive + 11));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordThree, (keyWordFour + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordFive + tweakWordTwo), (keyWordOne + 12));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordFour, (keyWordFive + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordOne + tweakWordThree), (keyWordTwo + 13));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordFive, (keyWordOne + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordTwo + tweakWordOne), (keyWordThree + 14));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordOne, (keyWordTwo + tweakWordOne));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordThree + tweakWordTwo), (keyWordFour + 15));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            MixForward(ref blockWordOne, ref blockWordTwo, 14, keyWordTwo, (keyWordThree + tweakWordTwo));
            MixForward(ref blockWordThree, ref blockWordFour, 16, (keyWordFour + tweakWordThree), (keyWordFive + 16));
            MixForward(ref blockWordOne, ref blockWordFour, 52);
            MixForward(ref blockWordThree, ref blockWordTwo, 57);
            MixForward(ref blockWordOne, ref blockWordTwo, 23);
            MixForward(ref blockWordThree, ref blockWordFour, 40);
            MixForward(ref blockWordOne, ref blockWordFour, 5);
            MixForward(ref blockWordThree, ref blockWordTwo, 37);
            MixForward(ref blockWordOne, ref blockWordTwo, 25, keyWordThree, (keyWordFour + tweakWordThree));
            MixForward(ref blockWordThree, ref blockWordFour, 33, (keyWordFive + tweakWordOne), (keyWordOne + 17));
            MixForward(ref blockWordOne, ref blockWordFour, 46);
            MixForward(ref blockWordThree, ref blockWordTwo, 12);
            MixForward(ref blockWordOne, ref blockWordTwo, 58);
            MixForward(ref blockWordThree, ref blockWordFour, 22);
            MixForward(ref blockWordOne, ref blockWordFour, 32);
            MixForward(ref blockWordThree, ref blockWordTwo, 32);
            block[0] = (blockWordOne + keyWordFour);
            block[1] = (blockWordTwo + keyWordFive + tweakWordOne);
            block[2] = (blockWordThree + keyWordOne + tweakWordTwo);
            block[3] = (blockWordFour + keyWordTwo + 18);
        }

        /// <summary>
        /// Initializes the initialization vector.
        /// </summary>
        /// <param name="initializationVector">
        /// The initialization vector.
        /// </param>
        [DebuggerHidden]
        private void InitializeInitializationVector(Byte[] initializationVector)
        {
            var initializationVectorLength = InitializationVector.Length;

            for (var i = 0; i < initializationVectorLength; i++)
            {
                InitializationVector[i] = BitConverter.ToUInt64(initializationVector, (i * 8));
            }
        }

        /// <summary>
        /// Initializes the key.
        /// </summary>
        /// <param name="key">
        /// The private key.
        /// </param>
        [DebuggerHidden]
        private void InitializeKey(Byte[] key)
        {
            var keyPositionCeiling = (Key.Length - 1);

            for (var i = 0; i < keyPositionCeiling; i++)
            {
                Key[i] = BitConverter.ToUInt64(key, (i * 8));
            }

            var parityWord = KeyConstant;
            var keyIndex = 0;

            while (keyIndex < (Key.Length - 1))
            {
                parityWord ^= Key[keyIndex];
                keyIndex++;
            }

            Key[keyIndex] = parityWord;
        }

        /// <summary>
        /// Initializes the tweak array.
        /// </summary>
        /// <param name="tweakWordOne">
        /// The first 64-bit tweak word.
        /// </param>
        /// <param name="tweakWordTwo">
        /// The second 64-bit tweak word.
        /// </param>
        [DebuggerHidden]
        private void InitializeTweak(UInt64 tweakWordOne, UInt64 tweakWordTwo)
        {
            Tweak[0] = tweakWordOne;
            Tweak[1] = tweakWordTwo;
            Tweak[2] = (tweakWordOne ^ tweakWordTwo);
        }

        /// <summary>
        /// Gets the number of 64-bit words per block.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 BlockWordCount => (BlockSizeInBytes / 8);

        /// <summary>
        /// Gets the number of 64-bit words per key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 KeyWordCount => ((KeyLength / 64) + 1);

        /// <summary>
        /// Represents the initialization vector used for the transform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly UInt64[] InitializationVector;

        /// <summary>
        /// Represents the private key used for the transform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly UInt64[] Key;

        /// <summary>
        /// Represents the key constant.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const UInt64 KeyConstant = 0x1bd11bdaa9fc1a22;

        /// <summary>
        /// Represents the length of the tweak array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 TweakArrayLength = 3;

        /// <summary>
        /// Represents a message template for exceptions that are raised when a given block size is unsupported.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String UnsupportedBlockSizeExceptionMessageTemplate = "The specified block size, {0}, is not supported.";

        /// <summary>
        /// Represents a message template for exceptions that are raised when a given cipher mode is unsupported.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String UnsupportedCipherModeExceptionMessageTemplate = "The specified cipher mode, {0}, is not supported.";

        /// <summary>
        /// Represents the length, in bits, of the key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 KeyLength;

        /// <summary>
        /// Represents the tweak array used for the transform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64[] Tweak;
    }
}