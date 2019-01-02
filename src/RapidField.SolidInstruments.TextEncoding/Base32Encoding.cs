﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace RapidField.SolidInstruments.TextEncoding
{
    /// <summary>
    /// Represents base32 character encoding.
    /// </summary>
    public class Base32Encoding : Encoding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Base32Encoding" /> class.
        /// </summary>
        public Base32Encoding()
            : this(DefaultAlphabet)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base32Encoding" /> class.
        /// </summary>
        /// <param name="alphabet">
        /// The encoding alphabet.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The specified alphabet length is not equal to 32.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="alphabet" /> is <see langword="null" />.
        /// </exception>
        protected Base32Encoding(String alphabet)
            : base(Utf8CodePageIdentifier, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback)
        {
            alphabet.RejectIf().IsNull(nameof(alphabet));

            if (alphabet.Length != AlphabetLength)
            {
                throw new ArgumentException($"The specified alphabet must be exactly {AlphabetLength} characters in length.", nameof(alphabet));
            }

            Alphabet = alphabet;
        }

        /// <summary>
        /// Calculates the number of bytes produced by decoding a set of characters from the specified character array.
        /// </summary>
        /// <param name="chars">
        /// The character array containing the set of characters to decode.
        /// </param>
        /// <param name="index">
        /// The index of the first character to decode.
        /// </param>
        /// <param name="count">
        /// The number of characters to decode.
        /// </param>
        /// <returns>
        /// The number of bytes produced by decoding the specified characters.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="chars" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> or <paramref name="count" /> is less than zero. -or- <paramref name="index" /> and
        /// <paramref name="count" /> do not denote a valid range in <paramref name="chars" />.
        /// </exception>
        public override Int32 GetByteCount(Char[] chars, Int32 index, Int32 count)
        {
            chars.RejectIf().IsNull(nameof(chars));
            index.RejectIf().IsLessThan(0, nameof(index));
            count.RejectIf().IsLessThan(0, nameof(count));
            (index + count).RejectIf().IsGreaterThan(chars.Length, nameof(count));
            return GetMaxByteCount(count);
        }

        /// <summary>
        /// Decodes a set of characters from the specified character array into the specified byte array.
        /// </summary>
        /// <param name="chars">
        /// The character array containing the set of characters to decode.
        /// </param>
        /// <param name="charIndex">
        /// The index of the first character to decode.
        /// </param>
        /// <param name="charCount">
        /// The number of characters to decode.
        /// </param>
        /// <param name="bytes">
        /// The byte array to contain the resulting sequence of bytes.
        /// </param>
        /// <param name="byteIndex">
        /// The index at which to start writing the resulting sequence of bytes.
        /// </param>
        /// <returns>
        /// The actual number of bytes written into <paramref name="bytes" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to
        /// accommodate the resulting bytes.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="chars" /> is null or <paramref name="bytes" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero. -or-
        /// <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.
        /// -or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />.
        /// </exception>
        /// <exception cref="DecoderFallbackException">
        /// The specified string contains one or more invalid characters.
        /// </exception>
        public override Int32 GetBytes(Char[] chars, Int32 charIndex, Int32 charCount, Byte[] bytes, Int32 byteIndex)
        {
            chars.RejectIf().IsNull(nameof(chars));
            charIndex.RejectIf().IsLessThan(0, nameof(charIndex));
            charCount.RejectIf().IsLessThan(0, nameof(charCount));
            (charIndex + charCount).RejectIf().IsGreaterThan(chars.Length, nameof(charCount));
            bytes.RejectIf().IsNull(nameof(bytes));
            byteIndex.RejectIf().IsLessThan(0, nameof(byteIndex)).OrIf().IsGreaterThanOrEqualTo(bytes.Length, nameof(byteIndex));

            Byte[] bufferArray;

            {
                var buffer = new BigInteger();
                var finalCharacterIndex = (charCount - 1);

                for (var i = finalCharacterIndex; i >= 0; i--)
                {
                    var key = Alphabet.IndexOf(chars[charIndex + i]);

                    if (key == -1)
                    {
                        throw new DecoderFallbackException("The specified string contains invalid characters.");
                    }

                    // Extend and hydrate the buffer.
                    buffer *= AlphabetLength;
                    buffer += key;
                }

                bufferArray = buffer.ToByteArray();
            }

            // Copy the buffer to the output stream.
            var encodedByteCount = Math.Min((bytes.Length - byteIndex), bufferArray.Length);
            Array.Copy(bufferArray, 0, bytes, byteIndex, encodedByteCount);
            return encodedByteCount;
        }

        /// <summary>
        /// Calculates the number of characters produced by encoding a sequence of bytes from the specified byte array.
        /// </summary>
        /// <param name="bytes">
        /// The byte array containing the sequence of bytes to encode.
        /// </param>
        /// <param name="index">
        /// The index of the first byte to encode.
        /// </param>
        /// <param name="count">
        /// The number of bytes to encode.
        /// </param>
        /// <returns>
        /// The number of characters produced by encoding the specified sequence of bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> or <paramref name="count" /> is less than zero. -or- <paramref name="index" /> and
        /// <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.
        /// </exception>
        public override Int32 GetCharCount(Byte[] bytes, Int32 index, Int32 count)
        {
            index.RejectIf().IsLessThan(0, nameof(index));
            count.RejectIf().IsLessThan(0, nameof(count));
            bytes.RejectIf().IsNull(nameof(bytes));
            (index + count).RejectIf().IsGreaterThan(bytes.Length, nameof(count));
            return GetMaxCharCount(count);
        }

        /// <summary>
        /// Encodes a sequence of bytes from the specified byte array into the specified character array.
        /// </summary>
        /// <param name="bytes">
        /// The byte array containing the sequence of bytes to encode.
        /// </param>
        /// <param name="byteIndex">
        /// The index of the first byte to encode.
        /// </param>
        /// <param name="byteCount">
        /// The number of bytes to encode.
        /// </param>
        /// <param name="chars">
        /// The character array to contain the resulting set of characters.
        /// </param>
        /// <param name="charIndex">
        /// The index at which to start writing the resulting set of characters.
        /// </param>
        /// <returns>
        /// The actual number of characters written into <paramref name="chars" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to
        /// accommodate the resulting characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes" /> is null or <paramref name="chars" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="byteIndex" /> or <paramref name="byteCount" /> or <paramref name="charIndex" /> is less than zero. -or-
        /// <paramref name="byteIndex" /> and <paramref name="byteCount" /> do not denote a valid range in <paramref name="bytes" />.
        /// -or- <paramref name="charIndex" /> is not a valid index in <paramref name="chars" />.
        /// </exception>
        public override Int32 GetChars(Byte[] bytes, Int32 byteIndex, Int32 byteCount, Char[] chars, Int32 charIndex)
        {
            bytes.RejectIf().IsNull(nameof(bytes));
            byteIndex.RejectIf().IsLessThan(0, nameof(byteIndex));
            byteCount.RejectIf().IsLessThan(0, nameof(byteCount));
            (byteIndex + byteCount).RejectIf().IsGreaterThan(bytes.Length, nameof(byteCount));
            chars.RejectIf().IsNull(nameof(chars));
            charIndex.RejectIf().IsLessThan(0, nameof(charIndex)).OrIf().IsGreaterThanOrEqualTo(chars.Length, nameof(charIndex));

            BigInteger buffer;

            {
                var bufferArray = new Byte[byteCount + 1];
                Array.Copy(bytes, byteIndex, bufferArray, 0, byteCount);
                buffer = new BigInteger(bufferArray);
            }

            var encodedCharacterCount = GetMaxCharCount(byteCount);

            for (var i = 0; i < encodedCharacterCount; ++i)
            {
                buffer = BigInteger.DivRem(buffer, AlphabetLength, out var modulus);
                chars[charIndex++] = Alphabet[(Int32)modulus];
            }

            return encodedCharacterCount;
        }

        /// <summary>
        /// Calculates the maximum number of bytes produced by decoding the specified number of characters.
        /// </summary>
        /// <param name="charCount">
        /// The number of characters to decode.
        /// </param>
        /// <returns>
        /// The maximum number of bytes produced by decoding the specified number of characters.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="charCount" /> is less than zero.
        /// </exception>
        public override Int32 GetMaxByteCount(Int32 charCount) => (Int32)Math.Ceiling((Double)(charCount.RejectIf().IsLessThan(0, nameof(charCount)) * BitsPerEncodedCharacter) / BitsPerByte);

        /// <summary>
        /// Calculates the maximum number of characters produced by encoding the specified number of bytes.
        /// </summary>
        /// <param name="byteCount">
        /// The number of bytes to encode.
        /// </param>
        /// <returns>
        /// The maximum number of characters produced by encoding the specified number of bytes.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="byteCount" /> is less than zero.
        /// </exception>
        public override Int32 GetMaxCharCount(Int32 byteCount) => (Int32)Math.Ceiling((Double)(byteCount.RejectIf().IsLessThan(0, nameof(byteCount)) * BitsPerByte) / BitsPerEncodedCharacter);

        /// <summary>
        /// Represents the default base32 encoding specification.
        /// </summary>
        public new static readonly Base32Encoding Default = new Base32Encoding();

        /// <summary>
        /// Represents the z-base-32 encoding specification.
        /// </summary>
        public static readonly Base32Encoding ZBase32 = new ZBase32Encoding();

        /// <summary>
        /// Represents the encoding alphabet.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly String Alphabet;

        /// <summary>
        /// Represents the alphabet length for base32 encoding.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 AlphabetLength = 32;

        /// <summary>
        /// Represents the number of bits in a byte.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 BitsPerByte = 8;

        /// <summary>
        /// Represents the default base32 encoding alphabet.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultAlphabet = "abcdefghijklmnopqrstuvwxyz234567";

        /// <summary>
        /// Represents the code page identifier of the preferred encoding.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 Utf8CodePageIdentifier = 65001;

        /// <summary>
        /// Represents the number of bits that each encoded character represents.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32 BitsPerEncodedCharacter = (Int32)Math.Log(AlphabetLength, 2);
    }
}