// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RapidField.SolidInstruments.Mathematics.Physics
{
    /// <summary>
    /// Represents a measurement of the separation between two points in physical space.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct Length : IComparable<Length>, IEquatable<Length>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Length" /> structure.
        /// </summary>
        /// <param name="meters">
        /// A distance measurement expressed in meters.
        /// </param>
        public Length(Double meters)
            : this(meters.ToDecimal())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Length" /> structure.
        /// </summary>
        /// <param name="meters">
        /// A distance measurement expressed in meters.
        /// </param>
        public Length(Decimal meters)
        {
            Meters = meters;
        }

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of centimeters.
        /// </summary>
        public static Length FromCentimeters(Double value) => FromCentimeters(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of centimeters.
        /// </summary>
        public static Length FromCentimeters(Decimal value) => new(value / CentimetersPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of feet.
        /// </summary>
        public static Length FromFeet(Double value) => FromFeet(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of feet.
        /// </summary>
        public static Length FromFeet(Decimal value) => new(value / FeetPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of inches.
        /// </summary>
        public static Length FromInches(Double value) => FromInches(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of inches.
        /// </summary>
        public static Length FromInches(Decimal value) => new(value / InchesPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of kilometers.
        /// </summary>
        public static Length FromKilometers(Double value) => FromKilometers(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of kilometers.
        /// </summary>
        public static Length FromKilometers(Decimal value) => new(value / KilometersPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of light-years.
        /// </summary>
        public static Length FromLightYears(Double value) => FromLightYears(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of light-years.
        /// </summary>
        public static Length FromLightYears(Decimal value) => new(value / LightYearsPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of meters.
        /// </summary>
        public static Length FromMeters(Double value) => FromMeters(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of meters.
        /// </summary>
        public static Length FromMeters(Decimal value) => new(value);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of miles.
        /// </summary>
        public static Length FromMiles(Double value) => FromMiles(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of miles.
        /// </summary>
        public static Length FromMiles(Decimal value) => new(value / MilesPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of millimeters.
        /// </summary>
        public static Length FromMillimeters(Double value) => FromMillimeters(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of millimeters.
        /// </summary>
        public static Length FromMillimeters(Decimal value) => new(value / MillimetersPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of nanometers.
        /// </summary>
        public static Length FromNanometers(Double value) => FromNanometers(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of nanometers.
        /// </summary>
        public static Length FromNanometers(Decimal value) => new(value / NanometersPerMeter);

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of yards.
        /// </summary>
        public static Length FromYards(Double value) => FromYards(value.ToDecimal());

        /// <summary>
        /// Returns a <see cref="Length" /> that represents the specified number of yards.
        /// </summary>
        public static Length FromYards(Decimal value) => new(value / YardsPerMeter);

        /// <summary>
        /// Determines the difference between two <see cref="Length" /> values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first <see cref="Length" /> instance.
        /// </param>
        /// <param name="minuend">
        /// The second <see cref="Length" /> instance.
        /// </param>
        /// <returns>
        /// The difference between the specified <see cref="Length" /> instances.
        /// </returns>
        public static Length operator -(Length subtrahend, Length minuend) => new(subtrahend.Meters - minuend.Meters);

        /// <summary>
        /// Negates the specified <see cref="Length" /> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Length" /> instance.
        /// </param>
        /// <returns>
        /// The negative of the specified <see cref="Length" /> instance.
        /// </returns>
        public static Length operator -(Length value) => new(-value.Meters);

        /// <summary>
        /// Determines whether or not two specified <see cref="Length" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Length a, Length b) => (a == b) is false;

        /// <summary>
        /// Determines the product of a <see cref="Length" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Length" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Length" /> and the specified multiplier.
        /// </returns>
        public static Length operator *(Length multiplicand, Double multiplier) => multiplicand * multiplier.ToDecimal();

        /// <summary>
        /// Determines the product of a <see cref="Length" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Length" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Length" /> and the specified multiplier.
        /// </returns>
        public static Length operator *(Length multiplicand, Decimal multiplier) => new(multiplicand.Meters * multiplier);

        /// <summary>
        /// Determines the quotient of a <see cref="Length" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Length" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Length" /> and the specified divisor.
        /// </returns>
        public static Length operator /(Length dividend, Double divisor) => dividend / divisor.ToDecimal();

        /// <summary>
        /// Determines the quotient of a <see cref="Length" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Length" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Length" /> and the specified divisor.
        /// </returns>
        public static Length operator /(Length dividend, Decimal divisor) => new(dividend.Meters / divisor);

        /// <summary>
        /// Determines the sum of two <see cref="Length" /> values.
        /// </summary>
        /// <param name="augend">
        /// The first <see cref="Length" /> instance.
        /// </param>
        /// <param name="addend">
        /// The second <see cref="Length" /> instance.
        /// </param>
        /// <returns>
        /// The sum of the specified <see cref="Length" /> instances.
        /// </returns>
        public static Length operator +(Length augend, Length addend) => new(augend.Meters + addend.Meters);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Length" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Length a, Length b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Length" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Length a, Length b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="Length" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Length a, Length b) => a.Equals(b);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Length" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Length a, Length b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Length" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Length" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Length" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Length a, Length b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a distance value to its <see cref="Length" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a distance value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Length" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a distance value.
        /// </exception>
        public static Length Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a distance value to its <see cref="Length" /> equivalent.
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a distance value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out Length result)
        {
            if (Parse(input, out var value, false))
            {
                result = value;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Compares the current <see cref="Length" /> to the supplied object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Length" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Length other) => Meters.CompareTo(other.Meters);

        /// <summary>
        /// Determines whether or not the current <see cref="Length" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is Length length)
            {
                return Equals(length);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Length" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Length" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Length other) => Meters == other.Meters;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => Meters.GetHashCode();

        /// <summary>
        /// Converts the current <see cref="Length" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Length" />.
        /// </returns>
        public Byte[] ToByteArray() => Meters.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="Length" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Length" />.
        /// </returns>
        public override String ToString() => $"{Meters} {MetersUnitOfMeasureSymbol}";

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a distance value to its <see cref="Length" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a distance value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Length" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a distance value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Length result, Boolean raiseExceptionOnFail)
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
                result = default;
                return false;
            }

            var processedString = input.Solidify().ToLower();

            if (processedString.Length < 2)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input));
                }

                result = default;
                return false;
            }

            try
            {
                var numericSubstringBuilder = new StringBuilder();
                var unitOfMeasureSubstringBuilder = new StringBuilder();
                var numericSubstringIsComplete = false;

                foreach (var character in processedString)
                {
                    if (numericSubstringIsComplete is false)
                    {
                        if (character.IsNumeric() || character == '.')
                        {
                            numericSubstringBuilder.Append(character);
                            continue;
                        }

                        numericSubstringIsComplete = true;
                    }

                    if (character.IsAlphabetic())
                    {
                        unitOfMeasureSubstringBuilder.Append(character);
                        continue;
                    }
                    else if (character.IsSymbolic())
                    {
                        continue;
                    }
                    else if (raiseExceptionOnFail)
                    {
                        throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input));
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }

                Decimal numericValue;
                String unitOfMeasureSubstring;

                if (raiseExceptionOnFail)
                {
                    numericValue = Decimal.Parse(numericSubstringBuilder.ToString());
                    unitOfMeasureSubstring = unitOfMeasureSubstringBuilder.ToString();
                }
                else if (Decimal.TryParse(numericSubstringBuilder.ToString(), out numericValue) && unitOfMeasureSubstringBuilder.Length > 0)
                {
                    unitOfMeasureSubstring = unitOfMeasureSubstringBuilder.ToString();
                }
                else
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input));
                }

                switch (unitOfMeasureSubstring)
                {
                    case CentimetersUnitOfMeasureSymbol:

                        result = FromCentimeters(numericValue);
                        break;

                    case FeetUnitOfMeasureSymbol:

                        result = FromFeet(numericValue);
                        break;

                    case InchesUnitOfMeasureSymbol:

                        result = FromInches(numericValue);
                        break;

                    case KilometersUnitOfMeasureSymbol:

                        result = FromKilometers(numericValue);
                        break;

                    case LightYearsUnitOfMeasureSymbol:

                        result = FromLightYears(numericValue);
                        break;

                    case MetersUnitOfMeasureSymbol:

                        result = FromMeters(numericValue);
                        break;

                    case MilesUnitOfMeasureSymbol:

                        result = FromMiles(numericValue);
                        break;

                    case MillimetersUnitOfMeasureSymbol:

                        result = FromMillimeters(numericValue);
                        break;

                    case NanometersUnitOfMeasureSymbol:

                        result = FromNanometers(numericValue);
                        break;

                    case YardsUnitOfMeasureSymbol:

                        result = FromYards(numericValue);
                        break;

                    default:

                        if (raiseExceptionOnFail)
                        {
                            throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input));
                        }

                        result = default;
                        return false;
                }
            }
            catch (FormatException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input), exception);
            }
            catch (OverflowException exception)
            {
                throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input), exception);
            }

            return true;
        }

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in centimeters.
        /// </summary>
        public Decimal TotalCentimeters => Meters * CentimetersPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in feet.
        /// </summary>
        public Decimal TotalFeet => Meters * FeetPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in inches.
        /// </summary>
        public Decimal TotalInches => Meters * InchesPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in kilometers.
        /// </summary>
        public Decimal TotalKilometers => Meters * KilometersPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in light-years.
        /// </summary>
        public Decimal TotalLightYears => Meters * LightYearsPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in meters.
        /// </summary>
        public Decimal TotalMeters => Meters;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in miles.
        /// </summary>
        public Decimal TotalMiles => Meters * MilesPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in millimeters.
        /// </summary>
        public Decimal TotalMillimeters => Meters * MillimetersPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in nanometers.
        /// </summary>
        public Decimal TotalNanometers => Meters * NanometersPerMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Length" /> expressed in yards.
        /// </summary>
        public Decimal TotalYards => Meters * FeetPerMeter / FeetPerYard;

        /// <summary>
        /// Represents the zero <see cref="Length" /> value.
        /// </summary>
        public static readonly Length Zero = new(0m);

        /// <summary>
        /// Represents the number of centimeters in an foot.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CentimetersPerFoot = CentimetersPerInch * InchesPerFoot;

        /// <summary>
        /// Represents the number of centimeters in an inch.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CentimetersPerInch = 2.54m;

        /// <summary>
        /// Represents the number of centimeters in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CentimetersPerMeter = 100m;

        /// <summary>
        /// Represents the textual symbol for a centimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CentimetersUnitOfMeasureSymbol = "cm";

        /// <summary>
        /// Represents the number of feet in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal FeetPerMeter = CentimetersPerMeter / CentimetersPerFoot;

        /// <summary>
        /// Represents the number of feet in a mile.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal FeetPerMile = 5280m;

        /// <summary>
        /// Represents the number of feet in a yard.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal FeetPerYard = 3m;

        /// <summary>
        /// Represents the textual symbol for a foot.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String FeetUnitOfMeasureSymbol = "ft";

        /// <summary>
        /// Represents the number of inches in a foot.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal InchesPerFoot = 12m;

        /// <summary>
        /// Represents the number of inches in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal InchesPerMeter = CentimetersPerMeter / CentimetersPerInch;

        /// <summary>
        /// Represents the textual symbol for an inch.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String InchesUnitOfMeasureSymbol = "in";

        /// <summary>
        /// Represents the number of kilometers in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal KilometersPerMeter = 0.001m;

        /// <summary>
        /// Represents the textual symbol for a kilometer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String KilometersUnitOfMeasureSymbol = "km";

        /// <summary>
        /// Represents the number of light-years in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal LightYearsPerMeter = 1m / MetersPerLightYear;

        /// <summary>
        /// Represents the textual symbol for a light-year.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String LightYearsUnitOfMeasureSymbol = "ly";

        /// <summary>
        /// Represents the number of meters in a light-year.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MetersPerLightYear = 9460730472580800m;

        /// <summary>
        /// Represents the textual symbol for a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MetersUnitOfMeasureSymbol = "m";

        /// <summary>
        /// Represents the number of miles in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MilesPerMeter = FeetPerMeter / FeetPerMile;

        /// <summary>
        /// Represents the textual symbol for a mile.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MilesUnitOfMeasureSymbol = "mi";

        /// <summary>
        /// Represents the number of millimeters in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MillimetersPerMeter = 1000m;

        /// <summary>
        /// Represents the textual symbol for a millimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MillimetersUnitOfMeasureSymbol = "mm";

        /// <summary>
        /// Represents the number of nanometers in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal NanometersPerMeter = 1000000000m;

        /// <summary>
        /// Represents the textual symbol for a nanometer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String NanometersUnitOfMeasureSymbol = "nm";

        /// <summary>
        /// Represents a message template for format exceptions raised by <see cref="Parse(String, out Length, Boolean)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as a length.";

        /// <summary>
        /// Represents the number of yards in a meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal YardsPerMeter = FeetPerMeter / FeetPerYard;

        /// <summary>
        /// Represents the textual symbol for a yard.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String YardsUnitOfMeasureSymbol = "yd";

        /// <summary>
        /// Represents the value of the current <see cref="Length" /> expressed in meters.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Decimal Meters;
    }
}