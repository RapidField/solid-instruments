// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="String" /> class with general purpose features.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces format items in the current <see cref="String" /> with corresponding arguments.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="arguments">
        /// An array of arguments that replace the format items in the current <see cref="String" />.
        /// </param>
        /// <returns>
        /// A copy of the current <see cref="String" /> in which format items have been replaced by corresponding elements in the
        /// <paramref name="arguments" /> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The target <see cref="String" /> is null and/or <paramref name="arguments" /> array is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// The target <see cref="String" /> is not a valid template, or the index of a format item is less than zero, or greater
        /// than or equal to the length of <paramref name="arguments" /> array.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String ApplyFormat(this String target, params Object[] arguments) => String.Format(target, args: arguments);

        /// <summary>
        /// Condenses all solitary and consecutive white spaces within the current <see cref="String" /> into single space
        /// characters.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <returns>
        /// The result of applying compression to the current <see cref="String" />.
        /// </returns>
        public static String Compress(this String target) => target.IsNullOrEmpty() ? target : Regex.Replace(target.Trim(), @"\s+", " ", RegexOptions.None, Regex.InfiniteMatchTimeout);

        /// <summary>
        /// Compresses, trims and shortens the current <see cref="String" /> using the specified maximum length.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="maxLength">
        /// A maximum length to limit the current <see cref="String" /> to.
        /// </param>
        /// <returns>
        /// The result of applying compression and the specified cropping to the current <see cref="String" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxLength" /> is less than zero.
        /// </exception>
        public static String CompressAndCrop(this String target, Int32 maxLength) => target.Compress().Crop(maxLength);

        /// <summary>
        /// Trims and shortens the current <see cref="String" /> using the specified maximum length.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="maxLength">
        /// A maximum length to limit the current <see cref="String" /> to.
        /// </param>
        /// <returns>
        /// The result of applying the specified cropping to the current <see cref="String" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxLength" /> is less than zero.
        /// </exception>
        public static String Crop(this String target, Int32 maxLength) => target.Trim().Substring(0, Math.Min(target.Length, maxLength.RejectIf().IsLessThan(0))).TrimEnd();

        /// <summary>
        /// Indicates whether or not the current <see cref="String" /> is <see langword="null" /> or empty.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="String" /> is <see langword="null" /> or empty, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsNullOrEmpty(this String target) => target is null || target.Length == 0;

        /// <summary>
        /// Indicates whether or not the current <see cref="String" /> is <see langword="null" />, empty or consists only of
        /// white-space characters.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="String" /> is <see langword="null" /> or empty, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsNullOrWhiteSpace(this String target) => String.IsNullOrWhiteSpace(target);

        /// <summary>
        /// Indicates whether or not the specified regular expression pattern finds a match in the current <see cref="String" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="regularExpressionPattern">
        /// A regular expression pattern to search for a match against the current <see cref="String" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the regular expression finds a match; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="regularExpressionPattern" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// A regular expression parsing error occurred.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="regularExpressionPattern" /> is null or the current <see cref="String" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// A timeout occurred while attempting to match the current <see cref="String" /> to the specified regular expression
        /// pattern.
        /// </exception>
        public static Boolean MatchesRegularExpression(this String target, String regularExpressionPattern)
        {
            var regularExpression = new Regex(regularExpressionPattern.RejectIf().IsNullOrEmpty(nameof(regularExpressionPattern)));
            return regularExpression.IsMatch(target);
        }

        /// <summary>
        /// Removes all white space from the current <see cref="String" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <returns>
        /// The result of applying solidification to the current <see cref="String" />.
        /// </returns>
        public static String Solidify(this String target) => target.IsNullOrEmpty() ? target : Regex.Replace(target.Trim(), @"\s+", String.Empty, RegexOptions.None, Regex.InfiniteMatchTimeout);

        /// <summary>
        /// Removes all white spacing and shortens the current <see cref="String" /> using the specified maximum length.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="maxLength">
        /// A maximum length to limit the current <see cref="String" /> to.
        /// </param>
        /// <returns>
        /// The result of applying solidification and the specified cropping to the current <see cref="String" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxLength" /> is less than zero.
        /// </exception>
        public static String SolidifyAndCrop(this String target, Int32 maxLength) => target.Solidify().Crop(maxLength);

        /// <summary>
        /// Converts the current <see cref="String" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="String" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encoding" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="EncoderFallbackException">
        /// The current <see cref="String" /> could not be decoded; a fallback occurred.
        /// </exception>
        public static Byte[] ToByteArray(this String target, Encoding encoding) => encoding.RejectIf().IsNull(nameof(encoding)).TargetArgument.GetBytes(target);
    }
}