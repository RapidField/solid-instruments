// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents the span of a contiguous period of time with specific start and end points.
    /// </summary>
    [DataContract]
    public sealed class DateTimeRange : ICloneable, IEquatable<DateTimeRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange" /> class.
        /// </summary>
        /// <param name="start">
        /// A starting <see cref="DateTime" /> for the range.
        /// </param>
        /// <param name="end">
        /// An ending <see cref="DateTime" /> for the range.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start" /> is later than <paramref name="end" />.
        /// </exception>
        public DateTimeRange(DateTime start, DateTime end)
            : this(start, end, DefaultGranularity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRange" /> class.
        /// </summary>
        /// <param name="start">
        /// A starting <see cref="DateTime" /> for the range.
        /// </param>
        /// <param name="end">
        /// An ending <see cref="DateTime" /> for the range.
        /// </param>
        /// <param name="granularity">
        /// A value that specifies how granular the range should be.
        /// </param>
        /// <exception cref="ArgumentException">
        /// One of the <see cref="DateTime" /> s has a <see cref="DateTimeKind" /> equal to <see cref="DateTimeKind.Unspecified" />
        /// and the other specifies a kind.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start" /> is later than <paramref name="end" /> -or- <paramref name="granularity" /> is equal to
        /// <see cref="DateTimeRangeGranularity.Unspecified" />.
        /// </exception>
        public DateTimeRange(DateTime start, DateTime end, DateTimeRangeGranularity granularity)
        {
            if (start.Kind == end.Kind)
            {
                Initialize(start, end, granularity);
            }
            else if (start.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException($"The constructor for {nameof(DateTimeRange)} cannot accept a {nameof(DateTime)} with unspecified kind as an argument for {nameof(start)} if the argument for {nameof(end)} specifies a kind.");
            }
            else if (end.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException($"The constructor for {nameof(DateTimeRange)} cannot accept a {nameof(DateTime)} with unspecified kind as an argument for {nameof(end)} if the argument for {nameof(start)} specifies a kind.");
            }
            else if (start.Kind == DateTimeKind.Utc)
            {
                Initialize(start, end.ToUniversalTime(), granularity);
            }
            else if (end.Kind == DateTimeKind.Utc)
            {
                Initialize(start.ToUniversalTime(), end, granularity);
            }
        }

        /// <summary>
        /// Determine whether or not two specified <see cref="DateTimeRange" /> instances are not equal.
        /// </summary>
        /// <param name="dateTimeRangeOne">
        /// The first <see cref="DateTimeRange" /> instance to compare.
        /// </param>
        /// <param name="dateTimeRangeTwo">
        /// The second <see cref="DateTimeRange" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(DateTimeRange dateTimeRangeOne, DateTimeRange dateTimeRangeTwo) => (dateTimeRangeOne == dateTimeRangeTwo) == false;

        /// <summary>
        /// Determines whether or not two specified <see cref="DateTimeRange" /> instances are equal.
        /// </summary>
        /// <param name="dateTimeRangeOne">
        /// The first <see cref="DateTimeRange" /> instance to compare.
        /// </param>
        /// <param name="dateTimeRangeTwo">
        /// The second <see cref="DateTimeRange" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(DateTimeRange dateTimeRangeOne, DateTimeRange dateTimeRangeTwo)
        {
            if ((Object)dateTimeRangeOne is null && (Object)dateTimeRangeTwo is null)
            {
                return true;
            }
            else if ((Object)dateTimeRangeOne is null || (Object)dateTimeRangeTwo is null)
            {
                return false;
            }

            return dateTimeRangeOne.Equals(dateTimeRangeTwo);
        }

        /// <summary>
        /// Converts the string representation of a contiguous period of time to its <see cref="DateTimeRange" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing start and end points for a contiguous period of time.
        /// </param>
        /// <returns>
        /// A <see cref="DateTimeRange" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The start point of the specified range is later than the end point.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> is incorrectly formatted.
        /// </exception>
        public static DateTimeRange Parse(String input)
        {
            if (Parse(input, out var start, out var end, out var granularity, true))
            {
                return new DateTimeRange(start, end, granularity);
            }

            return null;
        }

        /// <summary>
        /// Converts the string representation of a contiguous period of time to its <see cref="DateTimeRange" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing start and end points for a contiguous period of time.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out DateTimeRange result)
        {
            if (Parse(input, out var start, out var end, out var granularity, false) == false)
            {
                result = null;
                return false;
            }
            else if ((start.Kind != end.Kind) && (start.Kind == DateTimeKind.Unspecified || end.Kind == DateTimeKind.Unspecified))
            {
                result = null;
                return false;
            }
            else if (end < start)
            {
                result = null;
                return false;
            }

            result = new DateTimeRange(start, end, granularity);
            return true;
        }

        /// <summary>
        /// Creates a new <see cref="DateTimeRange" /> that is an identical copy of the current <see cref="DateTimeRange" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="DateTimeRange" /> that is an identical copy of the current <see cref="DateTimeRange" />.
        /// </returns>
        public Object Clone() => new DateTimeRange(Start, End, Granularity);

        /// <summary>
        /// Indicates whether or not the provided <see cref="DateTime" /> is within the bounds of the current
        /// <see cref="DateTimeRange" />.
        /// </summary>
        /// <param name="dateTime">
        /// A <see cref="DateTime" /> to be evaluated against the current range.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided value is within the bounds of the current range, otherwise
        /// <see langword="false" />.
        /// </returns>
        public Boolean Contains(DateTime dateTime) => (dateTime >= Start && dateTime <= End);

        /// <summary>
        /// Indicates whether or not the start and end points of the provided <see cref="DateTimeRange" /> are within the bounds of
        /// the current <see cref="DateTimeRange" />.
        /// </summary>
        /// <param name="dateTimeRange">
        /// A <see cref="DateTimeRange" /> to be evaluated against the current range.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the provided range is within the bounds of the current range, otherwise
        /// <see langword="false" />.
        /// </returns>
        public Boolean Contains(DateTimeRange dateTimeRange) => (dateTimeRange.Start >= Start && dateTimeRange.End <= End);

        /// <summary>
        /// Determines whether or not the current <see cref="DateTimeRange" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }

            DateTimeRange typedObject;
            typedObject = obj as DateTimeRange;

            if (typedObject is null)
            {
                return false;
            }

            return Equals(typedObject);
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="DateTimeRange" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="DateTimeRange" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(DateTimeRange other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (Start != other.Start)
            {
                return false;
            }
            else if (End != other.End)
            {
                return false;
            }
            else if (Midpoint != other.Midpoint)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Indicates whether or not the provided <see cref="DateTimeRange" /> overlaps the current <see cref="DateTimeRange" />.
        /// </summary>
        /// <param name="dateTimeRange">
        /// A <see cref="DateTimeRange" /> to be evaluated against the current range.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified range overlaps the current range, otherwise <see langword="false" />.
        /// </returns>
        public Boolean Overlaps(DateTimeRange dateTimeRange)
        {
            if (Midpoint < dateTimeRange.Midpoint)
            {
                return End > dateTimeRange.Start;
            }
            else if (Midpoint > dateTimeRange.Midpoint)
            {
                return Start < dateTimeRange.End;
            }
            else
            {
                return Length > TimeSpan.Zero || dateTimeRange.Length > TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Constrains the granularity of the current <see cref="DateTimeRange" /> using the provided value.
        /// </summary>
        /// <remarks>
        /// This method can only be used to make the range equally or less granular than its existing state.
        /// </remarks>
        /// <param name="granularity">
        /// A value that specifies how granular the current <see cref="DateTimeRange" /> should be.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="granularity" /> is more granular than the existing value for the current instance.
        /// </exception>
        public void Quantize(DateTimeRangeGranularity granularity)
        {
            var currentGranularityRanking = (Int32)Granularity;
            var providedGranularityRanking = (Int32)granularity;

            if (currentGranularityRanking > providedGranularityRanking)
            {
                throw new InvalidOperationException($"{nameof(Quantize)} cannot be invoked using an argument for {nameof(granularity)} that is more granular than the target's {nameof(Granularity)} property.");
            }
            else if (currentGranularityRanking < providedGranularityRanking)
            {
                Initialize(Start, End, granularity);
            }
        }

        /// <summary>
        /// Converts the current <see cref="DateTimeRange" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="DateTimeRange" />.
        /// </returns>
        public Byte[] ToByteArray()
        {
            var bytes = new List<Byte>();
            bytes.AddRange(Start.ToByteArray(true));
            bytes.AddRange(End.ToByteArray(true));
            bytes.AddRange(Granularity.ToByteArray());
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeRange" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DateTimeRange" />.
        /// </returns>
        public override String ToString() => $"{Start.ToFullDetailString()} | {End.ToFullDetailString()} | {Granularity}";

        /// <summary>
        /// Converts the string representation of a contiguous period of time to its <see cref="DateTimeRange" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing start and end points for a contiguous period of time.
        /// </param>
        /// <param name="start">
        /// A reference to the start <see cref="DateTime" /> component of the resulting <see cref="DateTimeRange" />.
        /// </param>
        /// <param name="end">
        /// A reference to the end <see cref="DateTime" /> component of the resulting <see cref="DateTimeRange" />.
        /// </param>
        /// <param name="granularity">
        /// A reference to the <see cref="DateTimeRangeGranularity" /> component of the resulting <see cref="DateTimeRange" />.
        /// </param>
        /// <param name="raiseExceptionOnFail">
        /// A value indicating whether or not an exception should be raised if the parse operation fails.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> is incorrectly formatted.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out DateTime start, out DateTime end, out DateTimeRangeGranularity granularity, Boolean raiseExceptionOnFail)
        {
            if (raiseExceptionOnFail)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }
                else if (input.Length == 0)
                {
                    throw new ArgumentEmptyException(nameof(input));
                }
            }
            else if (input.IsNullOrEmpty())
            {
                start = default;
                end = default;
                granularity = DateTimeRangeGranularity.Unspecified;
                return false;
            }

            try
            {
                var rawValueSubstrings = input.Compress().Split('|');

                if (rawValueSubstrings.Length < 3)
                {
                    if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(DateTimeRange)));
                    }
                    else
                    {
                        start = default;
                        end = default;
                        granularity = DateTimeRangeGranularity.Unspecified;
                        return false;
                    }
                }
                else if (raiseExceptionOnFail)
                {
                    start = DateTimeExtensions.ParseExtendedFormatDateTimeString(rawValueSubstrings[0]);
                    end = DateTimeExtensions.ParseExtendedFormatDateTimeString(rawValueSubstrings[1]);
                    granularity = (DateTimeRangeGranularity)Enum.Parse(typeof(DateTimeRangeGranularity), rawValueSubstrings[2]);
                    return true;
                }
                else if (DateTimeExtensions.TryParseExtendedFormatDateTimeString(rawValueSubstrings[0], out start) == false)
                {
                    start = default;
                    end = default;
                    granularity = DateTimeRangeGranularity.Unspecified;
                    return false;
                }
                else if (DateTimeExtensions.TryParseExtendedFormatDateTimeString(rawValueSubstrings[1], out end) == false)
                {
                    start = default;
                    end = default;
                    granularity = DateTimeRangeGranularity.Unspecified;
                    return false;
                }
                else if (Enum.TryParse(rawValueSubstrings[2], out granularity) == false)
                {
                    return false;
                }

                return true;
            }
            catch (ArgumentException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(DateTimeRange)), exception);
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(DateTimeRange)), exception);
            }
            catch (FormatException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(DateTimeRange)), exception);
            }
        }

        /// <summary>
        /// Calculates the total number of days that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in days.
        /// </returns>
        [DebuggerHidden]
        private Int32 CalculateLengthInDays() => Convert.ToInt32(Math.Floor(Length.TotalDays));

        /// <summary>
        /// Calculates the total number of hours that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in hours.
        /// </returns>
        [DebuggerHidden]
        private Int32 CalculateLengthInHours() => Convert.ToInt32(Math.Floor(Length.TotalHours));

        /// <summary>
        /// Calculates the total number of milliseconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in milliseconds.
        /// </returns>
        [DebuggerHidden]
        private Int64 CalculateLengthInMilliseconds() => Convert.ToInt64(Math.Floor(Length.TotalMilliseconds));

        /// <summary>
        /// Calculates the total number of minutes that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in minutes.
        /// </returns>
        [DebuggerHidden]
        private Int64 CalculateLengthInMinutes() => Convert.ToInt64(Math.Floor(Length.TotalMinutes));

        /// <summary>
        /// Calculates the total number of months that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in months.
        /// </returns>
        [DebuggerHidden]
        private Int32 CalculateLengthInMonths()
        {
            if (PrecalculatedLengthInMonths.HasValue)
            {
                return PrecalculatedLengthInMonths.Value;
            }

            CalculateLengthInMonthsAndYears(out var lengthInMonths, out var lengthInYears);
            PrecalculatedLengthInYears = lengthInYears;
            return lengthInMonths;
        }

        /// <summary>
        /// Calculates the total number of months and years (calendar date to calendar date) that the current
        /// <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <param name="lengthInMonths">
        /// The resulting length, in months.
        /// </param>
        /// <param name="lengthInYears">
        /// The resulting length, in years.
        /// </param>
        [DebuggerHidden]
        private void CalculateLengthInMonthsAndYears(out Int32 lengthInMonths, out Int32 lengthInYears)
        {
            Boolean endPrecedesStartDuringMonth;
            Boolean endPrecedesStartDuringYear;

            {
                // The last days of each calendar month are treated equally for the purpose of this calculation.
                var startAndEndPointsAreSameDayOfMonth = ((Start.IsLastDayOfMonth() && End.IsLastDayOfMonth()) || (Start.Day == End.Day));

                if (startAndEndPointsAreSameDayOfMonth)
                {
                    // The end time precedes the start time, and therefore precedes it during its month.
                    endPrecedesStartDuringMonth = End.TimeOfDay < Start.TimeOfDay;
                }
                else
                {
                    // The end day precedes the start time, and therefore precedes it during its month.
                    endPrecedesStartDuringMonth = End.Day < Start.Day;
                }
            }

            if (End.Month < Start.Month)
            {
                // The end month precedes the start month during the year.
                endPrecedesStartDuringYear = true;
            }
            else if (End.Month == Start.Month)
            {
                // The end months and start month are the same, but the end day and time precedes the start day and time.
                endPrecedesStartDuringYear = endPrecedesStartDuringMonth == true;
            }
            else
            {
                // The end month follows the start month during the year.
                endPrecedesStartDuringYear = false;
            }

            lengthInYears = End.Year - Start.Year;
            lengthInMonths = 0;

            if (lengthInYears < 1)
            {
                // Less than a full year has elapsed.
                lengthInYears = 0;
            }
            else if (endPrecedesStartDuringYear)
            {
                // Correct downward. The anniversary has not passed.
                lengthInYears--;
            }

            // Add months calculated from full years.
            lengthInMonths += (lengthInYears * 12);

            if (endPrecedesStartDuringYear)
            {
                // Add the difference in months crossing the New Year.
                lengthInMonths += 12 - Start.Month;
                lengthInMonths += End.Month;
            }
            else
            {
                // Add the difference in months within the year.
                lengthInMonths += End.Month - Start.Month;
            }

            if (lengthInMonths < 1)
            {
                // Less than a full month has elapsed.
                lengthInMonths = 0;
            }
            else if (endPrecedesStartDuringMonth)
            {
                // Correct downward. The mensiversary has not passed.
                lengthInMonths--;
            }
        }

        /// <summary>
        /// Calculates the total number of seconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in seconds.
        /// </returns>
        [DebuggerHidden]
        private Int64 CalculateLengthInSeconds() => Convert.ToInt64(Math.Floor(Length.TotalSeconds));

        /// <summary>
        /// Calculates the total number of weeks that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in weeks.
        /// </returns>
        [DebuggerHidden]
        private Int32 CalculateLengthInWeeks() => Convert.ToInt32(Math.Floor(Length.TotalDays / 7));

        /// <summary>
        /// Calculates the total number of years (calendar date to calendar date) that the current <see cref="DateTimeRange" />
        /// spans.
        /// </summary>
        /// <returns>
        /// The resulting length, in years.
        /// </returns>
        [DebuggerHidden]
        private Int32 CalculateLengthInYears()
        {
            if (PrecalculatedLengthInYears.HasValue)
            {
                return PrecalculatedLengthInYears.Value;
            }

            CalculateLengthInMonthsAndYears(out var lengthInMonths, out var lengthInYears);
            PrecalculatedLengthInMonths = lengthInMonths;
            return lengthInYears;
        }

        /// <summary>
        /// Calculates the midpoint <see cref="DateTime" /> for the current range.
        /// </summary>
        /// <returns>
        /// The resulting midpoint.
        /// </returns>
        [DebuggerHidden]
        private DateTime CalculateMidpoint() => Start.Add(new TimeSpan(Length == TimeSpan.Zero ? 0 : (Length.Ticks / 2)));

        /// <summary>
        /// Initializes or reinitializes the instance using the specified serialized representation.
        /// </summary>
        /// <param name="input">
        /// A string representation of a <see cref="DateTimeRange" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The start <see cref="DateTime" /> is later than the end <see cref="DateTime" /> -or- the granularity is equal to
        /// <see cref="DateTimeRangeGranularity.Unspecified" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> is incorrectly formatted.
        /// </exception>
        [DebuggerHidden]
        private void Initialize(String input)
        {
            Parse(input, out var start, out var end, out var granularity, true);
            Initialize(start, end, granularity);
        }

        /// <summary>
        /// Initializes or reinitializes the instance using the specified arguments.
        /// </summary>
        /// <param name="start">
        /// A starting <see cref="DateTime" /> for the range.
        /// </param>
        /// <param name="end">
        /// An ending <see cref="DateTime" /> for the range.
        /// </param>
        /// <param name="granularity">
        /// A value that specifies how granular the range should be.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start" /> is later than <paramref name="end" /> -or- <paramref name="granularity" /> is equal to
        /// <see cref="DateTimeRangeGranularity.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private void Initialize(DateTime start, DateTime end, DateTimeRangeGranularity granularity)
        {
            end.RejectIf().IsLessThan(start, nameof(end), nameof(start));
            Granularity = granularity.RejectIf().IsEqualToValue(DateTimeRangeGranularity.Unspecified, nameof(granularity));
            Start = start.Quantize(granularity);
            End = end.Quantize(granularity);
            Length = (End - Start);
            LazyLengthInDays = new Lazy<Int32>(CalculateLengthInDays, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInHours = new Lazy<Int32>(CalculateLengthInHours, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInMilliseconds = new Lazy<Int64>(CalculateLengthInMilliseconds, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInMinutes = new Lazy<Int64>(CalculateLengthInMinutes, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInMonths = new Lazy<Int32>(CalculateLengthInMonths, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInSeconds = new Lazy<Int64>(CalculateLengthInSeconds, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInWeeks = new Lazy<Int32>(CalculateLengthInWeeks, LazyThreadSafetyMode.PublicationOnly);
            LazyLengthInYears = new Lazy<Int32>(CalculateLengthInYears, LazyThreadSafetyMode.PublicationOnly);
            LazyMidpoint = new Lazy<DateTime>(CalculateMidpoint, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTimeRange" /> to its equivalent serialized string representation.
        /// </summary>
        /// <returns>
        /// A serialized string representation of the current <see cref="DateTimeRange" />.
        /// </returns>
        [DebuggerHidden]
        private String ToSerializedString() => $"{Start.ToString(DateTimeSerializationFormatString)}|{End.ToString(DateTimeSerializationFormatString)}|{Granularity}";

        /// <summary>
        /// Gets the ending <see cref="DateTime" /> for the current range.
        /// </summary>
        [IgnoreDataMember]
        public DateTime End
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the granularity of the current <see cref="DateTimeRange" />.
        /// </summary>
        [IgnoreDataMember]
        public DateTimeRangeGranularity Granularity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan" /> representing the duration of the current <see cref="DateTimeRange" />.
        /// </summary>
        [IgnoreDataMember]
        public TimeSpan Length
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of days that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int32 LengthInDays => LazyLengthInDays.Value;

        /// <summary>
        /// Gets the total number of hours that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int32 LengthInHours => LazyLengthInHours.Value;

        /// <summary>
        /// Gets the total number of milliseconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int64 LengthInMilliseconds => LazyLengthInMilliseconds.Value;

        /// <summary>
        /// Gets the total number of minutes that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int64 LengthInMinutes => LazyLengthInMinutes.Value;

        /// <summary>
        /// Gets the total number of months that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int32 LengthInMonths => LazyLengthInMonths.Value;

        /// <summary>
        /// Gets the total number of seconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int64 LengthInSeconds => LazyLengthInSeconds.Value;

        /// <summary>
        /// Gets the total number of weeks (7 day periods) that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int32 LengthInWeeks => LazyLengthInWeeks.Value;

        /// <summary>
        /// Gets the total number of years (calendar date to calendar date) that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [IgnoreDataMember]
        public Int32 LengthInYears => LazyLengthInYears.Value;

        /// <summary>
        /// Gets the midpoint <see cref="DateTime" /> for the current range.
        /// </summary>
        [IgnoreDataMember]
        public DateTime Midpoint => LazyMidpoint.Value;

        /// <summary>
        /// Gets the starting <see cref="DateTime" /> for the current range.
        /// </summary>
        [IgnoreDataMember]
        public DateTime Start
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a serialized representation of the current <see cref="DateTimeRange" />.
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Value")]
        private String SerializationValue
        {
            [DebuggerHidden]
            get => ToSerializedString();
            [DebuggerHidden]
            set => Initialize(value);
        }

        /// <summary>
        /// Represents the <see cref="DateTime" /> format string that is used for serialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeSerializationFormatString = "o";

        /// <summary>
        /// Represents the default granularity for date time ranges.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const DateTimeRangeGranularity DefaultGranularity = DateTimeRangeGranularity.Exact;

        /// <summary>
        /// Represents a message template for exceptions that are raised when a string parse operation fails.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as a {1}.";

        /// <summary>
        /// Represents a lazily-initialized total number of days that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int32> LazyLengthInDays;

        /// <summary>
        /// Represents a lazily-initialized total number of hours that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int32> LazyLengthInHours;

        /// <summary>
        /// Represents a lazily-initialized total number of milliseconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int64> LazyLengthInMilliseconds;

        /// <summary>
        /// Represents a lazily-initialized total number of minutes that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int64> LazyLengthInMinutes;

        /// <summary>
        /// Represents a lazily-initialized total number of months that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int32> LazyLengthInMonths;

        /// <summary>
        /// Represents a lazily-initialized total number of seconds that the current <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int64> LazyLengthInSeconds;

        /// <summary>
        /// Represents a lazily-initialized total number of weeks (7 day periods) that the current <see cref="DateTimeRange" />
        /// spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int32> LazyLengthInWeeks;

        /// <summary>
        /// Represents a lazily-initialized total number of years (calendar date to calendar date) that the current
        /// <see cref="DateTimeRange" /> spans.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<Int32> LazyLengthInYears;

        /// <summary>
        /// Represents a lazily-initialized midpoint <see cref="DateTime" /> for the current range.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Lazy<DateTime> LazyMidpoint;

        /// <summary>
        /// Represents a pre-calculated total number of months that the current <see cref="DateTimeRange" /> spans, or
        /// <see langword="null" /> if the value has not been calculated.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Int32? PrecalculatedLengthInMonths = null;

        /// <summary>
        /// Represents a pre-calculated total number of years (calendar date to calendar date) that the current
        /// <see cref="DateTimeRange" /> spans, or <see langword="null" /> if the value has not been calculated.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Int32? PrecalculatedLengthInYears = null;
    }
}