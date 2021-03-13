// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RapidField.SolidInstruments.Mathematics.Physics
{
    /// <summary>
    /// Represents a location on Earth expressed as the intersection of latitude and longitude coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public readonly struct GeographicCoordinates : IEquatable<GeographicCoordinates>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="GeographicCoordinates" /> class.
        /// </summary>
        /// <param name="latitude">
        /// A latitude coordinate within the range 0 to (+/-)90.
        /// </param>
        /// <param name="longitude">
        /// A longitude coordinate within the range 0 to (+/-)180.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="latitude" /> is outside of the 0 to (+/-)90 range and/or <paramref name="longitude" /> is outside of the
        /// 0 to (+/-)180 range.
        /// </exception>
        public GeographicCoordinates(Double latitude, Double longitude)
        {
            Latitude = latitude.RejectIf().IsLessThan(LatitudeMinimumValue, nameof(latitude)).OrIf().IsGreaterThan(LatitudeMaximumValue, nameof(latitude));
            Longitude = longitude.RejectIf().IsLessThan(LongitudeMinimumValue, nameof(longitude)).OrIf().IsGreaterThan(LongitudeMaximumValue, nameof(longitude));
        }

        /// <summary>
        /// Determine whether or not two specified <see cref="GeographicCoordinates" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="GeographicCoordinates" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="GeographicCoordinates" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(GeographicCoordinates a, GeographicCoordinates b) => (a == b) is false;

        /// <summary>
        /// Determine whether or not two specified <see cref="GeographicCoordinates" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="GeographicCoordinates" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="GeographicCoordinates" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(GeographicCoordinates a, GeographicCoordinates b) => a.Equals(b);

        /// <summary>
        /// Convert the string representation of latitude and longitude coordinates to its <see cref="GeographicCoordinates" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing latitude and longitude coordinates separated by a comma.
        /// </param>
        /// <returns>
        /// A <see cref="GeographicCoordinates" /> that is equivalent to the coordinates in <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The parsed latitude is outside of the 0 to (+/-)90 range and/or the parsed longitude is outside of the 0 to (+/-)180
        /// range.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> is incorrectly formatted.
        /// </exception>
        public static GeographicCoordinates Parse(String input)
        {
            Parse(input, out var latitude, out var longitude);
            return new(latitude, longitude);
        }

        /// <summary>
        /// Determine whether or not the current <see cref="GeographicCoordinates" /> is equal to the specified
        /// <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public readonly override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is GeographicCoordinates coordinates)
            {
                return Equals(coordinates);
            }

            return false;
        }

        /// <summary>
        /// Determine whether or not two specified <see cref="GeographicCoordinates" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="GeographicCoordinates" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public readonly Boolean Equals(GeographicCoordinates other)
        {
            if ((Object)other is null)
            {
                return false;
            }
            else if (Latitude != other.Latitude)
            {
                return false;
            }
            else if (Longitude != other.Longitude)
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
        public readonly override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Converts the current <see cref="GeographicCoordinates" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="GeographicCoordinates" />.
        /// </returns>
        public readonly Byte[] ToByteArray()
        {
            var bytes = new List<Byte>();
            bytes.AddRange(Latitude.ToByteArray());
            bytes.AddRange(Longitude.ToByteArray());
            return bytes.ToArray();
        }

        /// <summary>
        /// Convert the value of the current <see cref="GeographicCoordinates" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="GeographicCoordinates" />.
        /// </returns>
        public readonly override String ToString() => $"{Latitude}{CoordinateStringDelimiter}{Longitude}";

        /// <summary>
        /// Convert the string representation of latitude and longitude coordinates to its <see cref="GeographicCoordinates" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing latitude and longitude coordinates separated by a comma.
        /// </param>
        /// <param name="latitude">
        /// A reference to the latitude <see cref="Decimal" /> component of the resulting <see cref="GeographicCoordinates" />.
        /// </param>
        /// <param name="longitude">
        /// A reference to the longitude <see cref="Decimal" /> component of the resulting <see cref="GeographicCoordinates" />.
        /// </param>
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
        private static void Parse(String input, out Double latitude, out Double longitude)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            else if (input.Length == 0)
            {
                throw new ArgumentEmptyException(nameof(input));
            }

            try
            {
                var rawCoordinatesAsStrings = input.Solidify().Split(CoordinateStringDelimiter);
                latitude = Double.Parse(rawCoordinatesAsStrings[0]);
                longitude = Double.Parse(rawCoordinatesAsStrings[1]);
            }
            catch (IndexOutOfRangeException exception)
            {
                throw new FormatException(ParseFormatExceptionMessage, exception);
            }
            catch (FormatException exception)
            {
                throw new FormatException(ParseFormatExceptionMessage, exception);
            }
        }

        /// <summary>
        /// Represents geographic coordinates for Earth's North Pole.
        /// </summary>
        public static readonly GeographicCoordinates NorthPole = new(90d, 0d);

        /// <summary>
        /// Represents geographic coordinates for Earth's South Pole.
        /// </summary>
        public static readonly GeographicCoordinates SouthPole = new(-90d, 0d);

        /// <summary>
        /// Represents the latitude element of the current <see cref="GeographicCoordinates" />.
        /// </summary>
        [FieldOffset(0)]
        public readonly Double Latitude;

        /// <summary>
        /// Represents the longitude element of the current <see cref="GeographicCoordinates" />.
        /// </summary>
        [FieldOffset(8)]
        public readonly Double Longitude;

        /// <summary>
        /// Represents the delimiter character for coordinate strings.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char CoordinateStringDelimiter = ',';

        /// <summary>
        /// Represents the maximum allowed latitude value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double LatitudeMaximumValue = 90d;

        /// <summary>
        /// Represents the minimum allowed latitude value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double LatitudeMinimumValue = -90d;

        /// <summary>
        /// Represents the maximum allowed longitude value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double LongitudeMaximumValue = 180d;

        /// <summary>
        /// Represents the minimum allowed longitude value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double LongitudeMinimumValue = -180d;

        /// <summary>
        /// Represents a message for format exceptions raised by <see cref="Parse(String, out Double, out Double)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessage = "The specified string could not be parsed as geographic coordinates. See the inner exception for details.";
    }
}