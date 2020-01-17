// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a specific time of day in a specific time zone.
    /// </summary>
    [DataContract]
    public sealed class TimeOfDay : ICloneable, IComparable, IComparable<TimeOfDay>, IEquatable<TimeOfDay>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay" /> class.
        /// </summary>
        /// <param name="zone">
        /// The time zone for this instance.
        /// </param>
        /// <param name="hour">
        /// The hour component (0 through 23) of the time represented by this instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="zone" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A specified numeric argument is less than zero or greater than the logical maximum (eg. hour 25).
        /// </exception>
        public TimeOfDay(TimeZoneInfo zone, Int32 hour)
            : this(zone, hour, 0, 0, 0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay" /> class.
        /// </summary>
        /// <param name="zone">
        /// The time zone for this instance.
        /// </param>
        /// <param name="hour">
        /// The hour component (0 through 23) of the time represented by this instance.
        /// </param>
        /// <param name="minute">
        /// The minute component (0 through 59) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="zone" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A specified numeric argument is less than zero or greater than the logical maximum (eg. hour 25).
        /// </exception>
        public TimeOfDay(TimeZoneInfo zone, Int32 hour, Int32 minute)
            : this(zone, hour, minute, 0, 0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay" /> class.
        /// </summary>
        /// <param name="zone">
        /// The time zone for this instance.
        /// </param>
        /// <param name="hour">
        /// The hour component (0 through 23) of the time represented by this instance.
        /// </param>
        /// <param name="minute">
        /// The minute component (0 through 59) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <param name="second">
        /// The second component (0 through 59) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="zone" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A specified numeric argument is less than zero or greater than the logical maximum (eg. hour 25).
        /// </exception>
        public TimeOfDay(TimeZoneInfo zone, Int32 hour, Int32 minute, Int32 second)
            : this(zone, hour, minute, second, 0)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay" /> class.
        /// </summary>
        /// <param name="zone">
        /// The time zone for this instance.
        /// </param>
        /// <param name="hour">
        /// The hour component (0 through 23) of the time represented by this instance.
        /// </param>
        /// <param name="minute">
        /// The minute component (0 through 59) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <param name="second">
        /// The second component (0 through 59) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <param name="millisecond">
        /// The millisecond component (0 through 999) of the time represented by this instance. The default value is zero.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="zone" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A specified numeric argument is less than zero or greater than the logical maximum (eg. hour 25).
        /// </exception>
        public TimeOfDay(TimeZoneInfo zone, Int32 hour, Int32 minute, Int32 second, Int32 millisecond)
        {
            Hour = hour.RejectIf().IsLessThan(0, nameof(hour)).OrIf().IsGreaterThan(23, nameof(hour));
            Millisecond = millisecond.RejectIf().IsLessThan(0, nameof(millisecond)).OrIf().IsGreaterThan(999, nameof(millisecond));
            Minute = minute.RejectIf().IsLessThan(0, nameof(minute)).OrIf().IsGreaterThan(59, nameof(minute));
            Second = second.RejectIf().IsLessThan(0, nameof(second)).OrIf().IsGreaterThan(59, nameof(second));
            Zone = zone.RejectIf().IsNull(nameof(zone));
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="TimeOfDay" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(TimeOfDay a, TimeOfDay b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="TimeOfDay" /> instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(TimeOfDay a, TimeOfDay b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="TimeOfDay" /> instance is less than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(TimeOfDay a, TimeOfDay b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="TimeOfDay" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(TimeOfDay a, TimeOfDay b)
        {
            if ((Object)a is null && (Object)b is null)
            {
                return true;
            }
            else if ((Object)a is null || (Object)b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="TimeOfDay" /> instance is greater than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(TimeOfDay a, TimeOfDay b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="TimeOfDay" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="TimeOfDay" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(TimeOfDay a, TimeOfDay b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its <see cref="TimeOfDay" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a time of day value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="TimeOfDay" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A specified numeric component is less than zero or greater than the logical maximum (eg. hour 25).
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a time of day value.
        /// </exception>
        public static TimeOfDay Parse(String input)
        {
            if (Parse(input, out var zone, out var hour, out var minute, out var second, out var millisecond, true))
            {
                return new TimeOfDay(zone, hour, minute, second, millisecond);
            }

            return null;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its <see cref="TimeOfDay" />
        /// equivalent. The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a time of day value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out TimeOfDay result)
        {
            if (Parse(input, out var zone, out var hour, out var minute, out var second, out var millisecond, false) == false)
            {
                result = null;
                return false;
            }
            else if (hour < 0 || hour > 23)
            {
                result = null;
                return false;
            }
            else if (minute < 0 || minute > 59)
            {
                result = null;
                return false;
            }
            else if (second < 0 || second > 59)
            {
                result = null;
                return false;
            }
            else if (millisecond < 0 || millisecond > 999)
            {
                result = null;
                return false;
            }

            result = new TimeOfDay(zone, hour, minute, second, millisecond);
            return true;
        }

        /// <summary>
        /// Defines a <see cref="TimeOfDay" /> representing the start of the hour of the current <see cref="TimeOfDay" />.
        /// </summary>
        /// <returns>
        /// A <see cref="TimeOfDay" /> representing the start of the hour of the current <see cref="TimeOfDay" />.
        /// </returns>
        public TimeOfDay BeginningOfThisHour() => new TimeOfDay(Zone, Hour, 0, 0, 0);

        /// <summary>
        /// Defines a <see cref="TimeOfDay" /> representing the start of the minute of the current <see cref="TimeOfDay" />.
        /// </summary>
        /// <returns>
        /// A <see cref="TimeOfDay" /> representing the start of the minute of the current <see cref="TimeOfDay" />.
        /// </returns>
        public TimeOfDay BeginningOfThisMinute() => new TimeOfDay(Zone, Hour, Minute, 0, 0);

        /// <summary>
        /// Defines a <see cref="TimeOfDay" /> representing the start of the second of the current <see cref="TimeOfDay" />.
        /// </summary>
        /// <returns>
        /// A <see cref="TimeOfDay" /> representing the start of the second of the current <see cref="TimeOfDay" />.
        /// </returns>
        public TimeOfDay BeginningOfThisSecond() => new TimeOfDay(Zone, Hour, Minute, Second, 0);

        /// <summary>
        /// Creates a new <see cref="TimeOfDay" /> that is an identical copy of the current <see cref="TimeOfDay" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="TimeOfDay" /> that is an identical copy of the current <see cref="TimeOfDay" />.
        /// </returns>
        public Object Clone() => new TimeOfDay(Zone, Hour, Minute, Second, Millisecond);

        /// <summary>
        /// Compares the current <see cref="TimeOfDay" /> to the specified object and returns an indication of their relative
        /// values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="TimeOfDay" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(TimeOfDay other)
        {
            var thisInstanceCountOfMillisecondsPastMidnightUtc = CountOfMillisecondsPastMidnightUtc();
            var otherInstanceCountOfMillisecondsPastMidnightUtc = other.CountOfMillisecondsPastMidnightUtc();

            if (thisInstanceCountOfMillisecondsPastMidnightUtc < otherInstanceCountOfMillisecondsPastMidnightUtc)
            {
                return -1;
            }
            else if (thisInstanceCountOfMillisecondsPastMidnightUtc > otherInstanceCountOfMillisecondsPastMidnightUtc)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Compares the current <see cref="TimeOfDay" /> to the specified object and returns an indication of their relative
        /// values.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Object obj) => obj is TimeOfDay ? CompareTo((TimeOfDay)obj) : GetType().FullName.CompareTo(obj.GetType().FullName);

        /// <summary>
        /// Determines whether or not the current <see cref="TimeOfDay" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is TimeOfDay)
            {
                return Equals((TimeOfDay)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="TimeOfDay" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="TimeOfDay" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(TimeOfDay other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (Millisecond != other.Millisecond)
            {
                return false;
            }
            else if (Second != other.Second)
            {
                return false;
            }
            else if (Minute != other.Minute)
            {
                return false;
            }
            else if (Hour != other.Hour)
            {
                return false;
            }
            else if (Zone.Id != other.Zone.Id)
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
        /// Converts the current <see cref="TimeOfDay" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="TimeOfDay" />.
        /// </returns>
        public Byte[] ToByteArray()
        {
            var bytes = new List<Byte>();
            bytes.AddRange(Hour.ToByteArray());
            bytes.AddRange(Minute.ToByteArray());
            bytes.AddRange(Second.ToByteArray());
            bytes.AddRange(Millisecond.ToByteArray());
            bytes.AddRange(Zone.BaseUtcOffset.ToByteArray());
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the value of the current <see cref="TimeOfDay" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="TimeOfDay" />.
        /// </returns>
        public override String ToString()
        {
            Int32? twelveHourClockFormatHourValue = null;
            String meridiemStringFragment = null;

            switch (Hour)
            {
                case 0:

                    twelveHourClockFormatHourValue = 12;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 1:

                    twelveHourClockFormatHourValue = 1;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 2:

                    twelveHourClockFormatHourValue = 2;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 3:

                    twelveHourClockFormatHourValue = 3;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 4:

                    twelveHourClockFormatHourValue = 4;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 5:

                    twelveHourClockFormatHourValue = 5;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 6:

                    twelveHourClockFormatHourValue = 6;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 7:

                    twelveHourClockFormatHourValue = 7;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 8:

                    twelveHourClockFormatHourValue = 8;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 9:

                    twelveHourClockFormatHourValue = 9;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 10:

                    twelveHourClockFormatHourValue = 10;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 11:

                    twelveHourClockFormatHourValue = 11;
                    meridiemStringFragment = AnteMeridiemStringFragment;
                    break;

                case 12:

                    twelveHourClockFormatHourValue = 12;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 13:

                    twelveHourClockFormatHourValue = 1;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 14:

                    twelveHourClockFormatHourValue = 2;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 15:

                    twelveHourClockFormatHourValue = 3;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 16:

                    twelveHourClockFormatHourValue = 4;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 17:

                    twelveHourClockFormatHourValue = 5;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 18:

                    twelveHourClockFormatHourValue = 6;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 19:

                    twelveHourClockFormatHourValue = 7;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 20:

                    twelveHourClockFormatHourValue = 8;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 21:

                    twelveHourClockFormatHourValue = 9;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 22:

                    twelveHourClockFormatHourValue = 10;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                case 23:

                    twelveHourClockFormatHourValue = 11;
                    meridiemStringFragment = PostMeridiemStringFragment;
                    break;

                default:

                    twelveHourClockFormatHourValue = default(Int32);
                    meridiemStringFragment = null;
                    break;
            }

            return $"{twelveHourClockFormatHourValue.Value}:{Minute.ToString("00")}:{Second.ToString("00")}.{Millisecond.ToString("000")} {meridiemStringFragment} {Zone.Id}";
        }

        /// <summary>
        /// Converts the current <see cref="TimeOfDay" /> to Coordinated Universal Time (UTC) using the base UTC offset.
        /// </summary>
        /// <returns>
        /// The current <see cref="TimeOfDay" /> instance converted to Coordinated Universal Time (UTC) using the base UTC offset.
        /// </returns>
        public TimeOfDay ToUniversalTime()
        {
            if (Zone == TimeZoneInfo.Utc)
            {
                return this;
            }

            var utcHour = Hour + Zone.BaseUtcOffset.Hours;
            var utcMinute = Minute + Zone.BaseUtcOffset.Minutes;

            if (utcMinute < 0)
            {
                // Adjust one hour backward.
                utcHour -= 1;
                utcMinute += 60;
            }
            else if (utcMinute > 59)
            {
                // Adjust one hour forward.
                utcHour += 1;
                utcMinute -= 60;
            }

            if (utcHour < 0)
            {
                // Adjust one day backward.
                utcHour += 24;
            }
            else if (utcHour > 23)
            {
                // Adjust one day forward.
                utcHour -= 24;
            }

            return new TimeOfDay(TimeZoneInfo.Utc, utcHour, utcMinute, Second, Millisecond);
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its <see cref="TimeOfDay" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a time of day value to convert.
        /// </param>
        /// <param name="zone">
        /// A reference to the <see cref="TimeZoneInfo" /> component of the resulting <see cref="TimeOfDay" />.
        /// </param>
        /// <param name="hour">
        /// A reference to the hour component of the resulting <see cref="TimeOfDay" />.
        /// </param>
        /// <param name="minute">
        /// A reference to the minute component of the resulting <see cref="TimeOfDay" />.
        /// </param>
        /// <param name="second">
        /// A reference to the second component of the resulting <see cref="TimeOfDay" />.
        /// </param>
        /// <param name="millisecond">
        /// A reference to the millisecond component of the resulting <see cref="TimeOfDay" />.
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
        /// <paramref name="input" /> does not contain a valid representation of a time of day value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out TimeZoneInfo zone, out Int32 hour, out Int32 minute, out Int32 second, out Int32 millisecond, Boolean raiseExceptionOnFail)
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
                zone = null;
                hour = default;
                minute = default;
                second = default;
                millisecond = default;
                return false;
            }

            var rawValueSubstrings = input.Compress().Split(' ');

            if (rawValueSubstrings.Length < 3)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)));
                }

                zone = null;
                hour = default;
                minute = default;
                second = default;
                millisecond = default;
                return false;
            }

            var timeStringFragment = rawValueSubstrings[0];
            var timeValueSubstrings = timeStringFragment.Split(':');
            String hourString;
            String minuteString;
            String secondString;
            String millisecondString;

            if (timeValueSubstrings.Length != 3)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)));
                }

                zone = null;
                hour = default;
                minute = default;
                second = default;
                millisecond = default;
                return false;
            }

            hourString = timeValueSubstrings[0];
            minuteString = timeValueSubstrings[1];
            var secondAndMillisecondSubstrings = timeValueSubstrings[2].Split('.');

            if (secondAndMillisecondSubstrings.Length != 2)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)));
                }

                zone = null;
                hour = default;
                minute = default;
                second = default;
                millisecond = default;
                return false;
            }

            secondString = secondAndMillisecondSubstrings[0];
            millisecondString = secondAndMillisecondSubstrings[1];

            try
            {
                if (raiseExceptionOnFail)
                {
                    hour = Int32.Parse(hourString);
                    minute = Int32.Parse(minuteString);
                    second = Int32.Parse(secondString);
                    millisecond = Int32.Parse(millisecondString);
                }
                else if (Int32.TryParse(hourString, out hour) == false)
                {
                    zone = null;
                    hour = default;
                    minute = default;
                    second = default;
                    millisecond = default;
                    return false;
                }
                else if (Int32.TryParse(minuteString, out minute) == false)
                {
                    zone = null;
                    hour = default;
                    minute = default;
                    second = default;
                    millisecond = default;
                    return false;
                }
                else if (Int32.TryParse(secondString, out second) == false)
                {
                    zone = null;
                    hour = default;
                    minute = default;
                    second = default;
                    millisecond = default;
                    return false;
                }
                else if (Int32.TryParse(millisecondString, out millisecond) == false)
                {
                    zone = null;
                    hour = default;
                    minute = default;
                    second = default;
                    millisecond = default;
                    return false;
                }

                if (hour == 12)
                {
                    hour = 0;
                }

                if (rawValueSubstrings[1].ToUpper() == PostMeridiemStringFragment)
                {
                    // Adjust for the twelve hour format.
                    hour += 12;
                }

                var zoneStringFragmentCount = (rawValueSubstrings.Length - 2);
                var zoneStringFragment = zoneStringFragmentCount == 1 ? rawValueSubstrings[2] : String.Join(" ", rawValueSubstrings.Skip(2).Take(zoneStringFragmentCount).ToArray(), 0, zoneStringFragmentCount);

                if (raiseExceptionOnFail)
                {
                    // Attempt to match the specified time zone against the list of system time zones. Hard fault upon failure.
                    zone = TimeZoneInfo.FindSystemTimeZoneById(zoneStringFragment);
                }
                else
                {
                    // Attempt to match the specified time zone against the list of system time zones.
                    zone = TimeZoneInfo.GetSystemTimeZones().Where(systemTimeZone => systemTimeZone.Id.ToLower() == zoneStringFragment.ToLower()).FirstOrDefault();

                    if (zone is null)
                    {
                        return false;
                    }
                }
            }
            catch (FormatException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)), exception);
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)), exception);
            }
            catch (InvalidTimeZoneException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(TimeOfDay)), exception);
            }

            return true;
        }

        /// <summary>
        /// Counts how many milliseconds past midnight in the local timezone the current <see cref="TimeOfDay" /> represents.
        /// </summary>
        /// <returns>
        /// The number of milliseconds past midnight in the local timezone that the current instance represents.
        /// </returns>
        [DebuggerHidden]
        private Int32 CountOfMillisecondsPastMidnightLocal() => ((Convert.ToInt32(Hour) * 3600000) + (Convert.ToInt32(Minute) * 60000) + (Convert.ToInt32(Second) * 1000) + Convert.ToInt32(Millisecond));

        /// <summary>
        /// Counts how many milliseconds past midnight in Coordinated Universal Time (UTC) the current <see cref="TimeOfDay" />
        /// represents.
        /// </summary>
        /// <returns>
        /// The number of milliseconds past midnight UTC that the current instance represents.
        /// </returns>
        [DebuggerHidden]
        private Int32 CountOfMillisecondsPastMidnightUtc() => (CountOfMillisecondsPastMidnightLocal() + (Zone.BaseUtcOffset.Hours * 3600000));

        /// <summary>
        /// Initializes or reinitializes the instance using the specified serialized representation.
        /// </summary>
        /// <param name="input">
        /// A string representation of a <see cref="TimeOfDay" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a time of day value.
        /// </exception>
        [DebuggerHidden]
        private void Initialize(String input)
        {
            Parse(input, out var zone, out var hour, out var minute, out var second, out var millisecond, true);
            Zone = zone;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        /// <summary>
        /// Gets a <see cref="TimeOfDay" /> representing the current time of day in the local time zone for this machine.
        /// </summary>
        public static TimeOfDay NowLocal => DateTime.Now.ToTimeOfDay();

        /// <summary>
        /// Gets a <see cref="TimeOfDay" /> representing the current time of day in the Coordinated Universal Time (UTC) Zone.
        /// </summary>
        public static TimeOfDay NowUtc => DateTime.UtcNow.ToTimeOfDay();

        /// <summary>
        /// Gets the hour component (0 through 23) of the time represented by this instance.
        /// </summary>
        [IgnoreDataMember]
        public Int32 Hour
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the millisecond component (0 through 999) of the time represented by this instance.
        /// </summary>
        [IgnoreDataMember]
        public Int32 Millisecond
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the minute component (0 through 59) of the time represented by this instance.
        /// </summary>
        [IgnoreDataMember]
        public Int32 Minute
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the second component (0 through 59) of the time represented by this instance.
        /// </summary>
        [IgnoreDataMember]
        public Int32 Second
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the time zone for the current <see cref="TimeOfDay" />.
        /// </summary>
        [IgnoreDataMember]
        public TimeZoneInfo Zone
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a serialized representation of the current <see cref="TimeOfDay" />.
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Value")]
        private String SerializationValue
        {
            [DebuggerHidden]
            get => ToString();
            [DebuggerHidden]
            set => Initialize(value);
        }

        /// <summary>
        /// Represents the abbreviation for ante-meridian in a <see cref="TimeOfDay" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String AnteMeridiemStringFragment = "AM";

        /// <summary>
        /// Represents a message template for exceptions that are raised when a string parse operation fails.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified value, \"{0}\", could not be parsed as a {1}.";

        /// <summary>
        /// Represents the abbreviation for post-meridian in a <see cref="TimeOfDay" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String PostMeridiemStringFragment = "PM";
    }
}