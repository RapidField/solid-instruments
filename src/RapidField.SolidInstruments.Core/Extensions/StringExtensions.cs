// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Reflection;
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
        public static Boolean IsNullOrEmpty(this String target) => (target is null || target.Length == 0);

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
        public static Boolean IsNullOrWhiteSpace(this String target) => (String.IsNullOrWhiteSpace(target));

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
        /// Converts the current <see cref="String" /> to an equivalent object of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="resultType">
        /// The <see cref="Type" /> of the parsed result.
        /// </param>
        /// <returns>
        /// The parsed result.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The parse operation failed for the specified <paramref name="resultType" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="resultType" /> is not a supported <see cref="Type" />.
        /// </exception>
        public static Object ParseAs(this String target, Type resultType)
        {
            ParseAs(target, resultType, out var result, true);
            return result;
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="BitShiftDirection" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="BitShiftDirection" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out BitShiftDirection result)
        {
            try
            {
                result = (BitShiftDirection)Enum.Parse(typeof(BitShiftDirection), target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(BitShiftDirection)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(BitShiftDirection)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Boolean" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Boolean" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Boolean result)
        {
            try
            {
                result = Boolean.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Boolean)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Boolean)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTime" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="DateTime" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out DateTime result)
        {
            try
            {
                result = DateTime.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTime)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTime)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTimeRange" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="DateTimeRange" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out DateTimeRange result)
        {
            try
            {
                result = DateTimeRange.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTimeRange)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTimeRange)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTimeRangeGranularity" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="DateTimeRangeGranularity" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out DateTimeRangeGranularity result)
        {
            try
            {
                result = (DateTimeRangeGranularity)Enum.Parse(typeof(DateTimeRangeGranularity), target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTimeRangeGranularity)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DateTimeRangeGranularity)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DayOfWeekMonthlyOrdinal" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="DayOfWeekMonthlyOrdinal" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out DayOfWeekMonthlyOrdinal result)
        {
            try
            {
                result = (DayOfWeekMonthlyOrdinal)Enum.Parse(typeof(DayOfWeekMonthlyOrdinal), target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DayOfWeekMonthlyOrdinal)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(DayOfWeekMonthlyOrdinal)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Decimal" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Decimal" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Decimal result)
        {
            try
            {
                result = Decimal.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Decimal)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Decimal)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Decimal)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Double" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Double" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Double result)
        {
            try
            {
                result = Double.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Double)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Double)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Double)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Guid" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Guid" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Guid result)
        {
            try
            {
                result = Guid.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Guid)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Guid)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int16" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as an <see cref="Int16" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Int16 result)
        {
            try
            {
                result = Int16.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int16)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int16)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int16)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int32" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as an <see cref="Int32" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Int32 result)
        {
            try
            {
                result = Int32.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int32)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int32)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int32)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int64" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as an <see cref="Int64" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Int64 result)
        {
            try
            {
                result = Int64.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int64)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int64)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Int64)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="IPAddress" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as an <see cref="IPAddress" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out IPAddress result)
        {
            try
            {
                result = IPAddress.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(IPAddress)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(IPAddress)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Single" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Single" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Single result)
        {
            try
            {
                result = Single.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Single)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Single)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Single)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="TimeOfDay" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="TimeOfDay" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out TimeOfDay result)
        {
            try
            {
                result = TimeOfDay.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(TimeOfDay)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(TimeOfDay)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="TimeSpan" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="TimeSpan" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out TimeSpan result)
        {
            try
            {
                result = TimeSpan.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(TimeSpan)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(TimeSpan)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(TimeSpan)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Type" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Type" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Type result)
        {
            try
            {
                result = Type.GetType(target, true, true);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Type)), nameof(result), exception);
            }
            catch (BadImageFormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Type)), nameof(result), exception);
            }
            catch (IOException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Type)), nameof(result), exception);
            }
            catch (TargetInvocationException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Type)), nameof(result), exception);
            }
            catch (TypeLoadException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Type)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt16" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="UInt16" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out UInt16 result)
        {
            try
            {
                result = UInt16.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt16)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt16)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt16)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt32" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="UInt32" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out UInt32 result)
        {
            try
            {
                result = UInt32.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt32)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt32)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt32)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt64" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="UInt64" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out UInt64 result)
        {
            try
            {
                result = UInt64.Parse(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt64)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt64)), nameof(result), exception);
            }
            catch (OverflowException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(UInt64)), nameof(result), exception);
            }
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Uri" /> equivalent.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The target string cannot be parsed as a <see cref="Uri" />.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "This pattern lends itself to design simplicity.")]
        public static void ParseTo(this String target, out Uri result)
        {
            try
            {
                result = new Uri(target);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Uri)), nameof(result), exception);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(target, nameof(Uri)), nameof(result), exception);
            }
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
        public static Byte[] ToByteArray(this String target, Encoding encoding)
        {
            encoding.RejectIf().IsNull(nameof(encoding));
            return encoding.GetBytes(target);
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to an equivalent object of the specified <see cref="Type" />. The method
        /// returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="resultType">
        /// The <see cref="Type" /> of the parsed result.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseAs(this String target, Type resultType, out Object result) => ParseAs(target, resultType, out result, false);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="BitShiftDirection" /> equivalent. The method returns a value
        /// that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out BitShiftDirection result) => Enum.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Boolean" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Boolean result) => Boolean.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTime" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out DateTime result) => DateTime.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTimeRange" /> equivalent. The method returns a value
        /// that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out DateTimeRange result) => DateTimeRange.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DateTimeRangeGranularity" /> equivalent. The method returns
        /// a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out DateTimeRangeGranularity result) => Enum.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="DayOfWeekMonthlyOrdinal" /> equivalent. The method returns a
        /// value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out DayOfWeekMonthlyOrdinal result) => Enum.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Decimal" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Decimal result) => Decimal.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Double" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Double result) => Double.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Guid" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Guid result) => Guid.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int16" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Int16 result) => Int16.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int32" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Int32 result) => Int32.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Int64" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Int64 result) => Int64.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="IPAddress" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out IPAddress result) => IPAddress.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Single" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Single result) => Single.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="TimeOfDay" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out TimeOfDay result) => TimeOfDay.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="TimeSpan" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out TimeSpan result) => TimeSpan.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Type" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Type result)
        {
            result = (target.IsNullOrEmpty() ? null : Type.GetType(target, false, true));
            return (result is null == false);
        }

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt16" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out UInt16 result) => UInt16.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt32" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out UInt32 result) => UInt32.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="UInt64" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out UInt64 result) => UInt64.TryParse(target, out result);

        /// <summary>
        /// Converts the current <see cref="String" /> to its <see cref="Uri" /> equivalent. The method returns a value that
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="String" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParseTo(this String target, out Uri result) => Uri.TryCreate(target, UriKind.RelativeOrAbsolute, out result);

        /// <summary>
        /// Converts a <see cref="String" /> to an equivalent object of the specified <see cref="Type" />.
        /// </summary>
        /// <param name="stringRepresentation">
        /// The <see cref="String" /> to be parsed.
        /// </param>
        /// <param name="resultType">
        /// The <see cref="Type" /> of the parsed result.
        /// </param>
        /// <param name="result">
        /// The parsed result.
        /// </param>
        /// <param name="raiseExceptionOnFail">
        /// A value indicating whether or not an exception should be raised if the parse operation fails.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The parse operation failed for the specified <paramref name="resultType" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="resultType" /> is not a supported <see cref="Type" />.
        /// </exception>
        [DebuggerHidden]
        private static Boolean ParseAs(String stringRepresentation, Type resultType, out Object result, Boolean raiseExceptionOnFail)
        {
            var operationIsSuccessful = false;

#pragma warning disable IDE0007

            if (resultType == typeof(BitShiftDirection))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out BitShiftDirection typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Boolean))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Boolean typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(DateTime))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out DateTime typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(DateTimeRange))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out DateTimeRange typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(DateTimeRangeGranularity))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out DateTimeRangeGranularity typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(DayOfWeekMonthlyOrdinal))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out DayOfWeekMonthlyOrdinal typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Decimal))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Decimal typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Double))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Double typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Guid))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Guid typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Int16))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Int16 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Int32))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Int32 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Int64))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Int64 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(IPAddress))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out IPAddress typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Single))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Single typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(TimeOfDay))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out TimeOfDay typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(TimeSpan))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out TimeSpan typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Type))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Type typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(UInt16))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out UInt16 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(UInt32))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out UInt32 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(UInt64))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out UInt64 typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(Uri))
            {
                operationIsSuccessful = stringRepresentation.TryParseTo(out Uri typedResult);
                result = typedResult;
            }
            else if (resultType == typeof(String))
            {
                operationIsSuccessful = true;
                result = stringRepresentation;
            }
            else
            {
                result = default(Object);

                if (raiseExceptionOnFail)
                {
                    throw new UnsupportedTypeArgumentException(resultType, nameof(resultType));
                }
            }

#pragma warning restore IDE0007

            if (operationIsSuccessful == false && raiseExceptionOnFail)
            {
                throw new ArgumentException(ParseFailureExceptionMessageTemplate.ApplyFormat(stringRepresentation, resultType.FullName), nameof(resultType));
            }

            return operationIsSuccessful;
        }

        /// <summary>
        /// Represents a message template for exceptions that are raised when a string parse operation fails.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFailureExceptionMessageTemplate = "The target string \"{0}\" could not be parsed as a {1}.";
    }
}