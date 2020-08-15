// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Globalization;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="DateTime" /> structure with general purpose features.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the day of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the day of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisDay(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, 0, 0, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the hour of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the hour of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisHour(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, 0, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the millisecond of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the millisecond of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisMillisecond(this DateTime target)
        {
            var thisMillisecond = target.Millisecond;

            while (target.Millisecond == thisMillisecond)
            {
                target = target.Subtract(SingleTick);
            }

            target = target.Add(SingleTick);
            return new DateTime(target.Ticks, target.Kind);
        }

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the minute of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the minute of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisMinute(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the month of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the month of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisMonth(this DateTime target) => new DateTime(target.Year, target.Month, 1, 0, 0, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the second of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the second of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisSecond(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the start of the year of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the start of the year of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime BeginningOfThisYear(this DateTime target) => new DateTime(target.Year, 1, 1, 0, 0, 0, 0, target.Kind);

        /// <summary>
        /// Defines an integer representing the calendar quarter of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="Int32" /> representing the calendar quarter of the current <see cref="DateTime" />.
        /// </returns>
        public static Int32 CalendarQuarter(this DateTime target) => target.Month switch
        {
            1 => 1, // January
            2 => 1, // February
            3 => 1, // March
            4 => 2, // April
            5 => 2, // May
            6 => 2, // June
            7 => 3, // July
            8 => 3, // August
            9 => 3, // September
            10 => 4, // October
            11 => 4, // November
            12 => 4, // December
            _ => default
        };

        /// <summary>
        /// Determines the ordinal position of the current <see cref="DateTime" /> instance's <see cref="DayOfWeek" /> value within
        /// its month.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DayOfWeekMonthlyOrdinal" /> identifying the ordinal position of the current <see cref="DateTime" />
        /// instance's <see cref="DayOfWeek" /> value within its month.
        /// </returns>
        public static DayOfWeekMonthlyOrdinal DayOfWeekOrdinal(this DateTime target)
        {
            if (target.Day < 8)
            {
                return DayOfWeekMonthlyOrdinal.First;
            }
            else if (target.Day < 15)
            {
                return DayOfWeekMonthlyOrdinal.Second;
            }
            else if (target.Day < 22)
            {
                return DayOfWeekMonthlyOrdinal.Third;
            }
            else if (target.Month == target.Add(SevenDays).Month)
            {
                return DayOfWeekMonthlyOrdinal.Fourth;
            }

            return DayOfWeekMonthlyOrdinal.Last;
        }

        /// <summary>
        /// Indicates whether or not the day of the month component of the current <see cref="DateTime" /> is the last day of the
        /// month.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="DateTime" /> represents the last day in the month, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean IsLastDayOfMonth(this DateTime target) => DateTime.DaysInMonth(target.Year, target.Month) == target.Day;

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the day of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the day of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisDay(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, 12, 0, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the hour of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the hour of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisHour(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, 30, 0, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the millisecond of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the millisecond of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisMillisecond(this DateTime target) => target.BeginningOfThisMillisecond().AddTicks(5000);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the minute of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the minute of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisMinute(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 30, 0, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the month of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the month of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisMonth(this DateTime target)
        {
            var numberOfDaysInThisMonth = DateTime.DaysInMonth(target.Year, target.Month);

            switch (numberOfDaysInThisMonth)
            {
                case 28:

                    return new DateTime(target.Year, target.Month, 15, 0, 0, 0, 0, target.Kind);

                case 29:

                    return new DateTime(target.Year, target.Month, 15, 12, 0, 0, 0, target.Kind);

                case 30:

                    return new DateTime(target.Year, target.Month, 16, 0, 0, 0, 0, target.Kind);

                case 31:

                    return new DateTime(target.Year, target.Month, 16, 12, 0, 0, 0, target.Kind);

                default:

                    goto case 31;
            }
        }

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the second of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the second of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisSecond(this DateTime target) => new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, 500, target.Kind);

        /// <summary>
        /// Defines a <see cref="DateTime" /> representing the midpoint of the year of the current <see cref="DateTime" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime" /> representing the midpoint of the year of the current <see cref="DateTime" />.
        /// </returns>
        public static DateTime MidpointOfThisYear(this DateTime target)
        {
            if (DateTime.IsLeapYear(target.Year))
            {
                // Midnight on 1-Jul is the midpoint for leap years.
                return new DateTime(target.Year, 7, 2, 0, 0, 0, 0, target.Kind);
            }

            // Noon on 2-Jul is the midpoint for non-leap years.
            return new DateTime(target.Year, 7, 2, 12, 0, 0, 0, target.Kind);
        }

        /// <summary>
        /// Converts the current <see cref="DateTime" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="DateTime" />.
        /// </returns>
        public static Byte[] ToByteArray(this DateTime target) => target.ToByteArray(true);

        /// <summary>
        /// Converts the current <see cref="DateTime" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <param name="encodeKind">
        /// A value indicating whether or not the result should encode the kind of the current <see cref="DateTime" />. This
        /// argument is <see langword="true" /> by default.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="DateTime" />.
        /// </returns>
        public static Byte[] ToByteArray(this DateTime target, Boolean encodeKind) => encodeKind ? BitConverter.GetBytes(target.ToBinary()) : BitConverter.GetBytes(target.Ticks);

        /// <summary>
        /// Converts the value of the current <see cref="DateTime" /> to its equivalent fully detailed string representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A fully detailed string representation of the current <see cref="DateTime" />.
        /// </returns>
        public static String ToFullDetailString(this DateTime target) => target.ToString(ConstructFullDetailFormatString(target));

        /// <summary>
        /// Converts the value of the current <see cref="DateTime" /> to its equivalent serialized string representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A serialized string representation of the current <see cref="DateTime" />.
        /// </returns>
        public static String ToSerializedString(this DateTime target) => target.ToString(SerializedDateTimeFormat);

        /// <summary>
        /// Converts the value of the current <see cref="DateTime" /> to a time-only representation.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <returns>
        /// A new <see cref="TimeOfDay" /> representing only the time components of the current <see cref="DateTime" />.
        /// </returns>
        public static TimeOfDay ToTimeOfDay(this DateTime target)
        {
            switch (target.Kind)
            {
                case DateTimeKind.Local:

                    return new TimeOfDay(TimeZoneInfo.Local, target.Hour, target.Minute, target.Second, target.Millisecond);

                case DateTimeKind.Utc:

                    return new TimeOfDay(TimeZoneInfo.Utc, target.Hour, target.Minute, target.Second, target.Millisecond);

                case DateTimeKind.Unspecified:

                    goto case DateTimeKind.Local;

                default:

                    throw new ArgumentException($"{target.Kind} is not a supported {nameof(DateTimeKind)}.", nameof(target));
            }
        }

        /// <summary>
        /// Converts the extended-format string representation of a date and time to its <see cref="DateTime" /> equivalent.
        /// </summary>
        /// <param name="dateTimeString">
        /// An extended-format string representation of a date and time.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" />.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="dateTimeString" /> does not contain a valid representation of a date and time.
        /// </exception>
        [DebuggerHidden]
        internal static DateTime ParseExtendedFormatDateTimeString(String dateTimeString)
        {
            if (TryParseExtendedFormatDateTimeString(dateTimeString, out var result))
            {
                return result;
            }

            throw new FormatException($"The specified value, {dateTimeString}, could not be parsed as a {typeof(DateTime).FullName}.");
        }

        /// <summary>
        /// Defines a <see cref="DateTime" /> with granularity constrained by the specified <see cref="DateTimeRangeGranularity" />
        /// value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="DateTime" />.
        /// </param>
        /// <param name="granularity">
        /// A value that specifies how granular the <see cref="DateTime" /> should be.
        /// </param>
        /// <returns>
        /// A quantized <see cref="DateTime" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="granularity" /> is <see cref="DateTimeRangeGranularity.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal static DateTime Quantize(this DateTime target, DateTimeRangeGranularity granularity)
        {
            if (granularity == DateTimeRangeGranularity.Unspecified)
            {
                throw new ArgumentOutOfRangeException(nameof(granularity));
            }

            return granularity switch
            {
                DateTimeRangeGranularity.Exact => target,
                DateTimeRangeGranularity.AccurateToTheMillisecond => target.MidpointOfThisMillisecond(),
                DateTimeRangeGranularity.AccurateToTheSecond => target.MidpointOfThisSecond(),
                DateTimeRangeGranularity.AccurateToTheMinute => target.MidpointOfThisMinute(),
                DateTimeRangeGranularity.AccurateToTheHour => target.MidpointOfThisHour(),
                DateTimeRangeGranularity.AccurateToTheDay => target.MidpointOfThisDay(),
                DateTimeRangeGranularity.AccurateToTheMonth => target.MidpointOfThisMonth(),
                DateTimeRangeGranularity.AccurateToTheYear => target.MidpointOfThisYear(),
                _ => throw new ArgumentException($"{granularity} is not a supported {nameof(DateTimeRangeGranularity)}.", nameof(granularity))
            };
        }

        /// <summary>
        /// Converts the extended-format string representation of a date and time to its <see cref="DateTime" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="dateTimeString">
        /// An extended-format string representation of a date and time.
        /// </param>
        /// <param name="result">
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" /> if the conversion succeeded, or
        /// <see cref="DateTime.MinValue" /> if the conversion failed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="dateTimeString" /> was converted successfully; otherwise,
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean TryParseExtendedFormatDateTimeString(String dateTimeString, out DateTime result)
        {
            var formatProvider = new DateTimeFormatInfo();

            if (TryParseSerializedDateTimeString(dateTimeString, formatProvider, out result))
            {
                return true;
            }
            else if (TryParseFullDetailDateTimeString(dateTimeString, formatProvider, out result))
            {
                return true;
            }

            return DateTime.TryParse(dateTimeString, formatProvider, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind, out result);
        }

        /// <summary>
        /// Converts the fully detailed string representation of a date and time to its <see cref="DateTime" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="dateTimeString">
        /// A fully detailed string representation of a date and time.
        /// </param>
        /// <param name="result">
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" /> if the conversion succeeded, or
        /// <see cref="DateTime.MinValue" /> if the conversion failed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="dateTimeString" /> was converted successfully; otherwise,
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean TryParseFullDetailDateTimeString(String dateTimeString, out DateTime result) => TryParseFullDetailDateTimeString(dateTimeString, new DateTimeFormatInfo(), out result);

        /// <summary>
        /// Converts the fully detailed string representation of a date and time to its <see cref="DateTime" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="dateTimeString">
        /// A fully detailed string representation of a date and time.
        /// </param>
        /// <param name="formatProvider">
        /// An object that supplies culture-specific formatting information about <paramref name="dateTimeString" />.
        /// </param>
        /// <param name="result">
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" /> if the conversion succeeded, or
        /// <see cref="DateTime.MinValue" /> if the conversion failed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="dateTimeString" /> was converted successfully; otherwise,
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean TryParseFullDetailDateTimeString(String dateTimeString, IFormatProvider formatProvider, out DateTime result)
        {
            var dateTimeStringFormat = dateTimeString.Contains(DateTimeZoneUtcFormat) ? FullDetailDateTimeFormatTemplate.ApplyFormat(DateTimeZoneUtcFormat) : FullDetailDateTimeFormatTemplate.ApplyFormat(DateTimeZoneLocalFormat);
            return DateTime.TryParseExact(dateTimeString, dateTimeStringFormat, formatProvider, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind, out result);
        }

        /// <summary>
        /// Converts the serialized string representation of a date and time to its <see cref="DateTime" /> equivalent. The method
        /// returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="dateTimeString">
        /// A serialized string representation of a date and time.
        /// </param>
        /// <param name="result">
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" /> if the conversion succeeded, or
        /// <see cref="DateTime.MinValue" /> if the conversion failed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="dateTimeString" /> was converted successfully; otherwise,
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean TryParseSerializedDateTimeString(String dateTimeString, out DateTime result) => TryParseSerializedDateTimeString(dateTimeString, new DateTimeFormatInfo(), out result);

        /// <summary>
        /// Converts the serialized string representation of a date and time to its <see cref="DateTime" /> equivalent. The method
        /// returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="dateTimeString">
        /// A serialized string representation of a date and time.
        /// </param>
        /// <param name="formatProvider">
        /// An object that supplies culture-specific formatting information about <paramref name="dateTimeString" />.
        /// </param>
        /// <param name="result">
        /// The <see cref="DateTime" /> equivalent to <paramref name="dateTimeString" /> if the conversion succeeded, or
        /// <see cref="DateTime.MinValue" /> if the conversion failed.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="dateTimeString" /> was converted successfully; otherwise,
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        internal static Boolean TryParseSerializedDateTimeString(String dateTimeString, IFormatProvider formatProvider, out DateTime result) => DateTime.TryParseExact(dateTimeString, SerializedDateTimeFormat, formatProvider, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.RoundtripKind, out result);

        /// <summary>
        /// Constructs and returns a detailed format string using the time zone kind of the provided <see cref="DateTime" />.
        /// </summary>
        /// <param name="dateTime">
        /// A <see cref="DateTime" /> object.
        /// </param>
        /// <returns>
        /// A detailed format string using the appropriate time zone format.
        /// </returns>
        [DebuggerHidden]
        private static String ConstructFullDetailFormatString(DateTime dateTime) => dateTime.Kind switch
        {
            DateTimeKind.Utc => FullDetailDateTimeFormatTemplate.ApplyFormat(DateTimeZoneUtcFormat),
            DateTimeKind.Local => FullDetailDateTimeFormatTemplate.ApplyFormat(DateTimeZoneLocalFormat),
            DateTimeKind.Unspecified => FullDetailDateTimeFormatTemplate.ApplyFormat(String.Empty),
            _ => throw new ArgumentException($"{dateTime.Kind} is not a supported {nameof(DateTimeKind)}.", nameof(dateTime))
        };

        /// <summary>
        /// Represents a <see cref="DateTime" /> zone format for local time.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeZoneLocalFormat = " zzz";

        /// <summary>
        /// Represents a <see cref="DateTime" /> zone format for universal time.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DateTimeZoneUtcFormat = " UTC";

        /// <summary>
        /// Represents a <see cref="DateTime" /> format that communicates full detail for the value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String FullDetailDateTimeFormatTemplate = "ddd, dd MMM yyyy hh:mm:ss.fffffff tt{0}";

        /// <summary>
        /// Represents a <see cref="DateTime" /> format that is used for serialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SerializedDateTimeFormat = "o";

        /// <summary>
        /// Represents a <see cref="TimeSpan" /> that is seven days long.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan SevenDays = TimeSpan.FromDays(7);

        /// <summary>
        /// Represents a <see cref="TimeSpan" /> that is one tick long.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan SingleTick = TimeSpan.FromTicks(1);
    }
}