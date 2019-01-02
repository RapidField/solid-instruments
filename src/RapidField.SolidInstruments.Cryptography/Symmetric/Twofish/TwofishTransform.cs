// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Symmetric.Twofish
{
    /// <summary>
    /// Performs a cryptographic transformation of data using the Twofish algorithm.
    /// </summary>
    internal sealed class TwofishTransform : CryptographicTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwofishTransform" /> class.
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
        public TwofishTransform(Int32 keySize, Byte[] key, Byte[] initializationVector, Int32 blockSize, CipherMode mode, PaddingMode paddingMode, EncryptionDirection encryptionDirection)
            : base(blockSize, mode, paddingMode, encryptionDirection)
        {
            OutputWhitening = ((InputWhitening + BlockSizeInBytes) / 4);
            RoundSubkeys = ((OutputWhitening + BlockSizeInBytes) / 4);
            TotalSubkeys = ((RoundSubkeys + 2) * MaximumRoundCount);
            InitializationVector = new UInt32[initializationVector is null ? 0 : (initializationVector.Length / 4)];
            KeyLength = keySize;
            Key = new UInt32[KeyWordCount];
            RoundCountOptions = new Int32[] { 0, RoundCountForOneHundredTwentyEightBitKeys, RoundCountForOneHundredNinetyTwoBitKeys, RoundCountForTwoHundredFiftySixBitKeys };
            RoundCount = RoundCountOptions[(KeyLength - 1) / 64];
            SubstitutionBoxKeys = new UInt32[(MaximumKeyBitLength / 64)];
            Subkeys = new UInt32[TotalSubkeys];

            for (var i = 0; i < KeyWordCount; i++)
            {
                Key[i] = BitConverter.ToUInt32(key, (i * 4));
            }

            for (var i = 0; i < InitializationVector.Length; i++)
            {
                InitializationVector[i] = BitConverter.ToUInt32(initializationVector, (i * 4));
            }

            InitializeKey(KeyLength, Key);
        }

        /// <summary>
        /// Performs the decryption operation on the specified block.
        /// </summary>
        /// <param name="block">
        /// The block to decrypt.
        /// </param>
        [DebuggerHidden]
        protected override void DecryptBlock(Byte[] block)
        {
            var blockWords = new UInt32[BlockWordCount];

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] = BitConverter.ToUInt32(block, (i * 4));
            }

            var buffer = new UInt32[BlockWordCount];

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

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] ^= Subkeys[OutputWhitening + i];
            }

            {
                var tweakWordOne = default(UInt32);
                var tweakWordTwo = default(UInt32);

                for (var i = (RoundCount - 1); i >= 0; i--)
                {
                    tweakWordOne = PerformFunctionThirtyTwo(blockWords[0], SubstitutionBoxKeys, KeyLength);
                    tweakWordTwo = PerformFunctionThirtyTwo(RotateLeft(blockWords[1], 8), SubstitutionBoxKeys, KeyLength);
                    blockWords[2] = RotateLeft(blockWords[2], 1);
                    blockWords[2] ^= tweakWordOne + tweakWordTwo + Subkeys[RoundSubkeys + 2 * i];
                    blockWords[3] ^= tweakWordOne + 2 * tweakWordTwo + Subkeys[RoundSubkeys + 2 * i + 1];
                    blockWords[3] = RotateRight(blockWords[3], 1);

                    if (i > 0)
                    {
                        tweakWordOne = blockWords[0];
                        blockWords[0] = blockWords[2];
                        blockWords[2] = tweakWordOne;
                        tweakWordTwo = blockWords[1];
                        blockWords[1] = blockWords[3];
                        blockWords[3] = tweakWordTwo;
                    }
                }
            }

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] ^= Subkeys[InputWhitening + i];

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

                for (var j = 0; j < 4; j++)
                {
                    block[((i * BlockWordCount) + j)] = currentWord[j];
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="TwofishTransform" />.
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
        /// The block to encrypt.
        /// </param>
        [DebuggerHidden]
        protected override void EncryptBlock(Byte[] block)
        {
            var blockWords = new UInt32[BlockWordCount];

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] = BitConverter.ToUInt32(block, (i * 4));
            }

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] ^= Subkeys[InputWhitening + i];

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

            {
                var tweakWordOne = default(UInt32);
                var tweakWordTwo = default(UInt32);
                var tweakWordThree = default(UInt32);

                for (var i = 0; i < RoundCount; i++)
                {
                    tweakWordOne = PerformFunctionThirtyTwo(blockWords[0], SubstitutionBoxKeys, KeyLength);
                    tweakWordTwo = PerformFunctionThirtyTwo(RotateLeft(blockWords[1], 8), SubstitutionBoxKeys, KeyLength);
                    blockWords[3] = RotateLeft(blockWords[3], 1);
                    blockWords[2] ^= tweakWordOne + tweakWordTwo + Subkeys[RoundSubkeys + 2 * i];
                    blockWords[3] ^= tweakWordOne + 2 * tweakWordTwo + Subkeys[RoundSubkeys + 2 * i + 1];
                    blockWords[2] = RotateRight(blockWords[2], 1);

                    if (i < (RoundCount - 1))
                    {
                        tweakWordThree = blockWords[0];
                        blockWords[0] = blockWords[2];
                        blockWords[2] = tweakWordThree;
                        tweakWordThree = blockWords[1];
                        blockWords[1] = blockWords[3];
                        blockWords[3] = tweakWordThree;
                    }
                }
            }

            for (var i = 0; i < BlockWordCount; i++)
            {
                blockWords[i] ^= Subkeys[OutputWhitening + i];

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

                for (var j = 0; j < 4; j++)
                {
                    block[((i * BlockWordCount) + j)] = currentWord[j];
                }
            }
        }

        /// <summary>
        /// Extracts the fourth byte from the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value from which to extract a byte.
        /// </param>
        /// <returns>
        /// The extracted byte.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Byte ExtractByteFour(UInt32 value) => (Byte)((value >> 24));

        /// <summary>
        /// Extracts the first byte from the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value from which to extract a byte.
        /// </param>
        /// <returns>
        /// The extracted byte.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Byte ExtractByteOne(UInt32 value) => (Byte)(value);

        /// <summary>
        /// Extracts the third byte from the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value from which to extract a byte.
        /// </param>
        /// <returns>
        /// The extracted byte.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Byte ExtractByteThree(UInt32 value) => (Byte)((value >> 16));

        /// <summary>
        /// Extracts the second byte from the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value from which to extract a byte.
        /// </param>
        /// <returns>
        /// The extracted byte.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Byte ExtractByteTwo(UInt32 value) => (Byte)((value >> 8));

        /// <summary>
        /// Perform linear feedback shift one on the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value for the operation.
        /// </param>
        /// <returns>
        /// The result of the shift operation.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 LinearFeedbackShiftRegisterOne(Int32 value) => (((value) >> 1) ^ ((((value) & 0x01) == 0x01) ? 0x169 / 2 : 0));

        /// <summary>
        /// Perform linear feedback shift two on the specified 32-bit value.
        /// </summary>
        /// <param name="value">
        /// The value for the operation.
        /// </param>
        /// <returns>
        /// The result of the shift operation.
        /// </returns>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 LinearFeedbackShiftRegisterTwo(Int32 value) => (((value) >> 2) ^ ((((value) & 0x02) == 0x02) ? 0x169 / 2 : 0) ^ ((((value) & 0x01) == 0x01) ? 0x169 / 4 : 0));

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M00(Int32 value) => Mul1(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M01(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M02(Int32 value) => MulX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M03(Int32 value) => MulX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M10(Int32 value) => MulX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M11(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M12(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M13(Int32 value) => Mul1(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M20(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M21(Int32 value) => MulX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M22(Int32 value) => Mul1(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M23(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M30(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M31(Int32 value) => Mul1(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M32(Int32 value) => MulY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 M33(Int32 value) => MulX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 Mul1(Int32 value) => Mx1(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 MulX(Int32 value) => MxX(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 MulY(Int32 value) => MxY(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 Mx1(Int32 value) => value;

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 MxX(Int32 value) => value ^ LinearFeedbackShiftRegisterTwo(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Int32 MxY(Int32 value) => value ^ LinearFeedbackShiftRegisterOne(value) ^ LinearFeedbackShiftRegisterTwo(value);

        /// <summary>
        /// Processes the supplied 32-bit value using substitution boxes and, subsequently, performs MDS matrix multiplication.
        /// </summary>
        /// <param name="inputValue">
        /// A 32-bit input value.
        /// </param>
        /// <param name="keyWords">
        /// The key words.
        /// </param>
        /// <param name="keyLength">
        /// The total key length.
        /// </param>
        /// <returns>
        /// The result of the keyed permutation applied to the 32-bit value.
        /// </returns>
        [DebuggerHidden]
        private static UInt32 PerformFunctionThirtyTwo(UInt32 inputValue, UInt32[] keyWords, Int32 keyLength)
        {
            var byteArray = new[]
            {
                ExtractByteOne(inputValue),
                ExtractByteTwo(inputValue),
                ExtractByteThree(inputValue),
                ExtractByteFour(inputValue)
            };

            switch (((keyLength + 63) / 64) & 3)
            {
                case 0:

                    byteArray[0] = (Byte)(SubstitutionBoxes[PermutationLookupIndex04, byteArray[0]] ^ ExtractByteOne(keyWords[3]));
                    byteArray[1] = (Byte)(SubstitutionBoxes[PermutationLookupIndex14, byteArray[1]] ^ ExtractByteTwo(keyWords[3]));
                    byteArray[2] = (Byte)(SubstitutionBoxes[PermutationLookupIndex24, byteArray[2]] ^ ExtractByteThree(keyWords[3]));
                    byteArray[3] = (Byte)(SubstitutionBoxes[PermutationLookupIndex34, byteArray[3]] ^ ExtractByteFour(keyWords[3]));
                    goto case 3;

                case 3:

                    byteArray[0] = (Byte)(SubstitutionBoxes[PermutationLookupIndex03, byteArray[0]] ^ ExtractByteOne(keyWords[2]));
                    byteArray[1] = (Byte)(SubstitutionBoxes[PermutationLookupIndex13, byteArray[1]] ^ ExtractByteTwo(keyWords[2]));
                    byteArray[2] = (Byte)(SubstitutionBoxes[PermutationLookupIndex23, byteArray[2]] ^ ExtractByteThree(keyWords[2]));
                    byteArray[3] = (Byte)(SubstitutionBoxes[PermutationLookupIndex33, byteArray[3]] ^ ExtractByteFour(keyWords[2]));
                    goto case 2;

                case 2:

                    byteArray[0] = SubstitutionBoxes[PermutationLookupIndex00, SubstitutionBoxes[PermutationLookupIndex01, SubstitutionBoxes[PermutationLookupIndex02, byteArray[0]] ^ ExtractByteOne(keyWords[1])] ^ ExtractByteOne(keyWords[0])];
                    byteArray[1] = SubstitutionBoxes[PermutationLookupIndex10, SubstitutionBoxes[PermutationLookupIndex11, SubstitutionBoxes[PermutationLookupIndex12, byteArray[1]] ^ ExtractByteTwo(keyWords[1])] ^ ExtractByteTwo(keyWords[0])];
                    byteArray[2] = SubstitutionBoxes[PermutationLookupIndex20, SubstitutionBoxes[PermutationLookupIndex21, SubstitutionBoxes[PermutationLookupIndex22, byteArray[2]] ^ ExtractByteThree(keyWords[1])] ^ ExtractByteThree(keyWords[0])];
                    byteArray[3] = SubstitutionBoxes[PermutationLookupIndex30, SubstitutionBoxes[PermutationLookupIndex31, SubstitutionBoxes[PermutationLookupIndex32, byteArray[3]] ^ ExtractByteFour(keyWords[1])] ^ ExtractByteFour(keyWords[0])];
                    break;
            }

            return (UInt32)((M00(byteArray[0]) ^ M01(byteArray[1]) ^ M02(byteArray[2]) ^ M03(byteArray[3]))) ^ (UInt32)((M10(byteArray[0]) ^ M11(byteArray[1]) ^ M12(byteArray[2]) ^ M13(byteArray[3])) << 8) ^ (UInt32)((M20(byteArray[0]) ^ M21(byteArray[1]) ^ M22(byteArray[2]) ^ M23(byteArray[3])) << 16) ^ (UInt32)((M30(byteArray[0]) ^ M31(byteArray[1]) ^ M32(byteArray[2]) ^ M33(byteArray[3])) << 24);
        }

        /// <summary>
        /// Produces a key substitution box word from two key words.
        /// </summary>
        /// <param name="keyWordOne">
        /// The first key word.
        /// </param>
        /// <param name="keyWordTwo">
        /// The second key word.
        /// </param>
        /// <returns>
        /// The remainder polynomial result.
        /// </returns>
        [DebuggerHidden]
        private static UInt32 ReedSolomonEncode(UInt32 keyWordOne, UInt32 keyWordTwo)
        {
            var encodedResult = default(UInt32);

            for (var i = encodedResult = 0; i < 2; i++)
            {
                encodedResult ^= (i > 0) ? keyWordOne : keyWordTwo;

                for (var j = 0; j < 4; j++)
                {
                    var shiftedByte = (Byte)(encodedResult >> 24);
                    var gTwo = (UInt32)(((shiftedByte << 1) ^ (((shiftedByte & 0x80) == 0x80) ? 0x14d : 0)) & 0xff);
                    var gThree = (UInt32)(((shiftedByte >> 1) & 0x7f) ^ (((shiftedByte & 1) == 1) ? 0x14d >> 1 : 0) ^ gTwo);
                    encodedResult = (encodedResult << 8) ^ (gThree << 24) ^ (gTwo << 16) ^ (gThree << 8) ^ shiftedByte;
                }
            }

            return encodedResult;
        }

        /// <summary>
        /// Rotates the specified 32-bit value left by the specified number of bits.
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
        private static UInt32 RotateLeft(UInt32 value, Int32 bitCount) => (((value) << ((bitCount) & 0x1f)) | (value) >> (32 - ((bitCount) & 0x1f)));

        /// <summary>
        /// Rotates the specified 32-bit value right by the specified number of bits.
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
        private static UInt32 RotateRight(UInt32 value, Int32 bitCount) => (((value) >> ((bitCount) & 0x1f)) | ((value) << (32 - ((bitCount) & 0x1f))));

        /// <summary>
        /// Initializes the key using 32-bit key words.
        /// </summary>
        /// <param name="keyLength">
        /// The key length.
        /// </param>
        /// <param name="keyWords">
        /// The key words.
        /// </param>
        [DebuggerHidden]
        private void InitializeKey(Int32 keyLength, UInt32[] keyWords)
        {
            var tweakWordOne = default(UInt32);
            var tweakWordTwo = default(UInt32);
            var evenKeyWords = new UInt32[MaximumKeyBitLength / 64];
            var oddKeyWords = new UInt32[MaximumKeyBitLength / 64];
            var keywordCountRounded = (keyLength + 63) / 64;

            for (var i = 0; i < keywordCountRounded; i++)
            {
                evenKeyWords[i] = keyWords[2 * i];
                oddKeyWords[i] = keyWords[2 * i + 1];
                SubstitutionBoxKeys[keywordCountRounded - 1 - i] = ReedSolomonEncode(evenKeyWords[i], oddKeyWords[i]);
            }

            var subkeyCount = (RoundSubkeys + 2 * RoundCount);

            for (var i = 0; i < subkeyCount / 2; i++)
            {
                tweakWordOne = PerformFunctionThirtyTwo((UInt32)(i * 0x02020202u), evenKeyWords, keyLength);
                tweakWordTwo = PerformFunctionThirtyTwo((UInt32)(i * 0x02020202u + 0x01010101u), oddKeyWords, keyLength);
                tweakWordTwo = RotateLeft(tweakWordTwo, 8);
                Subkeys[2 * i] = tweakWordOne + tweakWordTwo;
                Subkeys[2 * i + 1] = RotateLeft(tweakWordOne + 2 * tweakWordTwo, 9);
            }
        }

        /// <summary>
        /// Gets the number of 32-bit words per block.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 BlockWordCount => (BlockSizeInBytes / 4);

        /// <summary>
        /// Gets the number of 32-bit words per key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 KeyWordCount => (KeyLength / 32);

        /// <summary>
        /// Represents the initialization vector used for the transform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly UInt32[] InitializationVector;

        /// <summary>
        /// Represents the private key used for the transform.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly UInt32[] Key;

        /// <summary>
        /// Represents the input whitening value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 InputWhitening = 0;

        /// <summary>
        /// Represents the maximum allowable key bit-length.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumKeyBitLength = 256;

        /// <summary>
        /// Represents the maximum number of allowable rounds.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumRoundCount = 16;

        /// <summary>
        /// Represents the value for lookup index 0.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex00 = 1;

        /// <summary>
        /// Represents the value for lookup index 1.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex01 = 0;

        /// <summary>
        /// Represents the value for lookup index 2.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex02 = 0;

        /// <summary>
        /// Represents the value for lookup index 3.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex03 = (PermutationLookupIndex01 ^ 1);

        /// <summary>
        /// Represents the value for lookup index 4.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex04 = 1;

        /// <summary>
        /// Represents the value for lookup index 10.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex10 = 0;

        /// <summary>
        /// Represents the value for lookup index 11.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex11 = 0;

        /// <summary>
        /// Represents the value for lookup index 12.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex12 = 1;

        /// <summary>
        /// Represents the value for lookup index 13.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex13 = (PermutationLookupIndex11 ^ 1);

        /// <summary>
        /// Represents the value for lookup index 14.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex14 = 0;

        /// <summary>
        /// Represents the value for lookup index 20.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex20 = 1;

        /// <summary>
        /// Represents the value for lookup index 21.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex21 = 1;

        /// <summary>
        /// Represents the value for lookup index 22.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex22 = 0;

        /// <summary>
        /// Represents the value for lookup index 23.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex23 = (PermutationLookupIndex21 ^ 1);

        /// <summary>
        /// Represents the value for lookup index 24.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex24 = 0;

        /// <summary>
        /// Represents the value for lookup index 30.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex30 = 0;

        /// <summary>
        /// Represents the value for lookup index 31.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex31 = 1;

        /// <summary>
        /// Represents the value for lookup index 32.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex32 = 1;

        /// <summary>
        /// Represents the value for lookup index 33.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex33 = (PermutationLookupIndex31 ^ 1);

        /// <summary>
        /// Represents the value for lookup index 34.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PermutationLookupIndex34 = 1;

        /// <summary>
        /// Represents the number of rounds for 192-bit keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 RoundCountForOneHundredNinetyTwoBitKeys = 16;

        /// <summary>
        /// Represents the number of rounds for 128-bit keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 RoundCountForOneHundredTwentyEightBitKeys = 16;

        /// <summary>
        /// Represents the number of rounds for 256-bit keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 RoundCountForTwoHundredFiftySixBitKeys = 16;

        /// <summary>
        /// Represents a message template for exceptions that are raised when a given cipher mode is unsupported.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String UnsupportedCipherModeExceptionMessageTemplate = "The specified cipher mode, {0}, is not supported.";

        /// <summary>
        /// Represents fixed 8x8 permutation substitution boxes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Byte[,] SubstitutionBoxes =
        {
            {
                0xa9, 0x67, 0xb3, 0xe8, 0x04, 0xfd, 0xa3, 0x76,
                0x9a, 0x92, 0x80, 0x78, 0xe4, 0xdd, 0xd1, 0x38,
                0x0d, 0xc6, 0x35, 0x98, 0x18, 0xf7, 0xec, 0x6c,
                0x43, 0x75, 0x37, 0x26, 0xfa, 0x13, 0x94, 0x48,
                0xf2, 0xd0, 0x8b, 0x30, 0x84, 0x54, 0xdf, 0x23,
                0x19, 0x5b, 0x3d, 0x59, 0xf3, 0xae, 0xa2, 0x82,
                0x63, 0x01, 0x83, 0x2e, 0xd9, 0x51, 0x9b, 0x7c,
                0xa6, 0xeb, 0xa5, 0xbe, 0x16, 0x0c, 0xe3, 0x61,
                0xc0, 0x8c, 0x3a, 0xf5, 0x73, 0x2c, 0x25, 0x0b,
                0xbb, 0x4e, 0x89, 0x6b, 0x53, 0x6a, 0xb4, 0xf1,
                0xe1, 0xe6, 0xbd, 0x45, 0xe2, 0xf4, 0xb6, 0x66,
                0xcc, 0x95, 0x03, 0x56, 0xd4, 0x1c, 0x1e, 0xd7,
                0xfb, 0xc3, 0x8e, 0xb5, 0xe9, 0xcf, 0xbf, 0xba,
                0xea, 0x77, 0x39, 0xaf, 0x33, 0xc9, 0x62, 0x71,
                0x81, 0x79, 0x09, 0xad, 0x24, 0xcd, 0xf9, 0xd8,
                0xe5, 0xc5, 0xb9, 0x4d, 0x44, 0x08, 0x86, 0xe7,
                0xa1, 0x1d, 0xaa, 0xed, 0x06, 0x70, 0xb2, 0xd2,
                0x41, 0x7b, 0xa0, 0x11, 0x31, 0xc2, 0x27, 0x90,
                0x20, 0xf6, 0x60, 0xff, 0x96, 0x5c, 0xb1, 0xab,
                0x9e, 0x9c, 0x52, 0x1b, 0x5f, 0x93, 0x0a, 0xef,
                0x91, 0x85, 0x49, 0xee, 0x2d, 0x4f, 0x8f, 0x3b,
                0x47, 0x87, 0x6d, 0x46, 0xd6, 0x3e, 0x69, 0x64,
                0x2a, 0xce, 0xcb, 0x2f, 0xfc, 0x97, 0x05, 0x7a,
                0xac, 0x7f, 0xd5, 0x1a, 0x4b, 0x0e, 0xa7, 0x5a,
                0x28, 0x14, 0x3f, 0x29, 0x88, 0x3c, 0x4c, 0x02,
                0xb8, 0xda, 0xb0, 0x17, 0x55, 0x1f, 0x8a, 0x7d,
                0x57, 0xc7, 0x8d, 0x74, 0xb7, 0xc4, 0x9f, 0x72,
                0x7e, 0x15, 0x22, 0x12, 0x58, 0x07, 0x99, 0x34,
                0x6e, 0x50, 0xde, 0x68, 0x65, 0xbc, 0xdb, 0xf8,
                0xc8, 0xa8, 0x2b, 0x40, 0xdc, 0xfe, 0x32, 0xa4,
                0xca, 0x10, 0x21, 0xf0, 0xd3, 0x5d, 0x0f, 0x00,
                0x6f, 0x9d, 0x36, 0x42, 0x4a, 0x5e, 0xc1, 0xe0
            },
            {
                0x75, 0xf3, 0xc6, 0xf4, 0xdb, 0x7b, 0xfb, 0xc8,
                0x4a, 0xd3, 0xe6, 0x6b, 0x45, 0x7d, 0xe8, 0x4b,
                0xd6, 0x32, 0xd8, 0xfd, 0x37, 0x71, 0xf1, 0xe1,
                0x30, 0x0f, 0xf8, 0x1b, 0x87, 0xfa, 0x06, 0x3f,
                0x5e, 0xba, 0xae, 0x5b, 0x8a, 0x00, 0xbc, 0x9d,
                0x6d, 0xc1, 0xb1, 0x0e, 0x80, 0x5d, 0xd2, 0xd5,
                0xa0, 0x84, 0x07, 0x14, 0xb5, 0x90, 0x2c, 0xa3,
                0xb2, 0x73, 0x4c, 0x54, 0x92, 0x74, 0x36, 0x51,
                0x38, 0xb0, 0xbd, 0x5a, 0xfc, 0x60, 0x62, 0x96,
                0x6c, 0x42, 0xf7, 0x10, 0x7c, 0x28, 0x27, 0x8c,
                0x13, 0x95, 0x9c, 0xc7, 0x24, 0x46, 0x3b, 0x70,
                0xca, 0xe3, 0x85, 0xcb, 0x11, 0xd0, 0x93, 0xb8,
                0xa6, 0x83, 0x20, 0xff, 0x9f, 0x77, 0xc3, 0xcc,
                0x03, 0x6f, 0x08, 0xbf, 0x40, 0xe7, 0x2b, 0xe2,
                0x79, 0x0c, 0xaa, 0x82, 0x41, 0x3a, 0xea, 0xb9,
                0xe4, 0x9a, 0xa4, 0x97, 0x7e, 0xda, 0x7a, 0x17,
                0x66, 0x94, 0xa1, 0x1d, 0x3d, 0xf0, 0xde, 0xb3,
                0x0b, 0x72, 0xa7, 0x1c, 0xef, 0xd1, 0x53, 0x3e,
                0x8f, 0x33, 0x26, 0x5f, 0xec, 0x76, 0x2a, 0x49,
                0x81, 0x88, 0xee, 0x21, 0xc4, 0x1a, 0xeb, 0xd9,
                0xc5, 0x39, 0x99, 0xcd, 0xad, 0x31, 0x8b, 0x01,
                0x18, 0x23, 0xdd, 0x1f, 0x4e, 0x2d, 0xf9, 0x48,
                0x4f, 0xf2, 0x65, 0x8e, 0x78, 0x5c, 0x58, 0x19,
                0x8d, 0xe5, 0x98, 0x57, 0x67, 0x7f, 0x05, 0x64,
                0xaf, 0x63, 0xb6, 0xfe, 0xf5, 0xb7, 0x3c, 0xa5,
                0xce, 0xe9, 0x68, 0x44, 0xe0, 0x4d, 0x43, 0x69,
                0x29, 0x2e, 0xac, 0x15, 0x59, 0xa8, 0x0a, 0x9e,
                0x6e, 0x47, 0xdf, 0x34, 0x35, 0x6a, 0xcf, 0xdc,
                0x22, 0xc9, 0xc0, 0x9b, 0x89, 0xd4, 0xed, 0xab,
                0x12, 0xa2, 0x0d, 0x52, 0xbb, 0x02, 0x2f, 0xa9,
                0xd7, 0x61, 0x1e, 0xb4, 0x50, 0x04, 0xf6, 0xc2,
                0x16, 0x25, 0x86, 0x56, 0x55, 0x09, 0xbe, 0x91
            }
        };

        /// <summary>
        /// Represents the length, in bits, of the key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 KeyLength;

        /// <summary>
        /// Represents the output whitening value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 OutputWhitening;

        /// <summary>
        /// Represents the number of rounds for the cipher.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 RoundCount;

        /// <summary>
        /// Represents the possible round counts for various key sizes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32[] RoundCountOptions;

        /// <summary>
        /// Represents the number of subkeys per round.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 RoundSubkeys;

        /// <summary>
        /// Represents the round subkeys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt32[] Subkeys;

        /// <summary>
        /// Represents the key bits used for substitution boxes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt32[] SubstitutionBoxKeys;

        /// <summary>
        /// Represents the total number of subkeys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int32 TotalSubkeys;
    }
}