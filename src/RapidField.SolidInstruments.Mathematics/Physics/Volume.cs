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
    /// Represents a quantitative measurement of three-dimensional space.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct Volume : IComparable<Volume>, IEquatable<Volume>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Volume" /> structure.
        /// </summary>
        /// <param name="cubicMeters">
        /// A volume measurement expressed in cubic meters.
        /// </param>
        public Volume(Double cubicMeters)
            : this(Convert.ToDecimal(cubicMeters))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Volume" /> structure.
        /// </summary>
        /// <param name="cubicMeters">
        /// A volume measurement expressed in cubic meters.
        /// </param>
        public Volume(Decimal cubicMeters)
        {
            CubicMeters = cubicMeters;
        }

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic centimeters.
        /// </summary>
        public static Volume FromCubicCentimeters(Double value) => FromCubicCentimeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic centimeters.
        /// </summary>
        public static Volume FromCubicCentimeters(Decimal value) => new Volume(value / CubicCentimetersPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic feet.
        /// </summary>
        public static Volume FromCubicFeet(Double value) => FromCubicFeet(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic feet.
        /// </summary>
        public static Volume FromCubicFeet(Decimal value) => new Volume(value / CubicFeetPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic inches.
        /// </summary>
        public static Volume FromCubicInches(Double value) => FromCubicInches(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic inches.
        /// </summary>
        public static Volume FromCubicInches(Decimal value) => new Volume(value / CubicInchesPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic kilometers.
        /// </summary>
        public static Volume FromCubicKilometers(Double value) => FromCubicKilometers(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic kilometers.
        /// </summary>
        public static Volume FromCubicKilometers(Decimal value) => new Volume(value / CubicKilometersPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic meters.
        /// </summary>
        public static Volume FromCubicMeters(Double value) => FromCubicMeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic meters.
        /// </summary>
        public static Volume FromCubicMeters(Decimal value) => new Volume(value);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic miles.
        /// </summary>
        public static Volume FromCubicMiles(Double value) => FromCubicMiles(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic miles.
        /// </summary>
        public static Volume FromCubicMiles(Decimal value) => new Volume(value / CubicMilesPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic millimeters.
        /// </summary>
        public static Volume FromCubicMillimeters(Double value) => FromCubicMillimeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic millimeters.
        /// </summary>
        public static Volume FromCubicMillimeters(Decimal value) => new Volume(value / CubicMillimetersPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic yards.
        /// </summary>
        public static Volume FromCubicYards(Double value) => FromCubicYards(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of cubic yards.
        /// </summary>
        public static Volume FromCubicYards(Decimal value) => new Volume(value / CubicYardsPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of gallons.
        /// </summary>
        public static Volume FromGallons(Double value) => FromGallons(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of gallons.
        /// </summary>
        public static Volume FromGallons(Decimal value) => new Volume(value / GallonsPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of liters.
        /// </summary>
        public static Volume FromLiters(Double value) => FromLiters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of liters.
        /// </summary>
        public static Volume FromLiters(Decimal value) => new Volume(value / LitersPerCubicMeter);

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of milliliters.
        /// </summary>
        public static Volume FromMilliliters(Double value) => FromMilliliters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Volume" /> that represents the specified number of milliliters.
        /// </summary>
        public static Volume FromMilliliters(Decimal value) => new Volume(value / MillilitersPerCubicMeter);

        /// <summary>
        /// Determines the difference between two <see cref="Volume" /> values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first <see cref="Volume" /> instance.
        /// </param>
        /// <param name="minuend">
        /// The second <see cref="Volume" /> instance.
        /// </param>
        /// <returns>
        /// The area between the specified <see cref="Volume" /> instances.
        /// </returns>
        public static Volume operator -(Volume subtrahend, Volume minuend) => new Volume(subtrahend.CubicMeters - minuend.CubicMeters);

        /// <summary>
        /// Negates the specified <see cref="Volume" /> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Volume" /> instance.
        /// </param>
        /// <returns>
        /// The negative of the specified <see cref="Volume" /> instance.
        /// </returns>
        public static Volume operator -(Volume value) => new Volume(-value.CubicMeters);

        /// <summary>
        /// Determines whether or not two specified <see cref="Volume" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Volume a, Volume b) => a == b == false;

        /// <summary>
        /// Determines the product of an <see cref="Volume" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Volume" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Volume" /> and the specified multiplier.
        /// </returns>
        public static Volume operator *(Volume multiplicand, Double multiplier) => multiplicand * Convert.ToDecimal(multiplier);

        /// <summary>
        /// Determines the product of an <see cref="Volume" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Volume" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Volume" /> and the specified multiplier.
        /// </returns>
        public static Volume operator *(Volume multiplicand, Decimal multiplier) => new Volume(multiplicand.CubicMeters * multiplier);

        /// <summary>
        /// Determines the quotient of an <see cref="Volume" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Volume" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Volume" /> and the specified divisor.
        /// </returns>
        public static Volume operator /(Volume dividend, Double divisor) => dividend / Convert.ToDecimal(divisor);

        /// <summary>
        /// Determines the quotient of an <see cref="Volume" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Volume" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Volume" /> and the specified divisor.
        /// </returns>
        public static Volume operator /(Volume dividend, Decimal divisor) => new Volume(dividend.CubicMeters / divisor);

        /// <summary>
        /// Determines the sum of two <see cref="Volume" /> values.
        /// </summary>
        /// <param name="augend">
        /// The first <see cref="Volume" /> instance.
        /// </param>
        /// <param name="addend">
        /// The second <see cref="Volume" /> instance.
        /// </param>
        /// <returns>
        /// The sum of the specified <see cref="Volume" /> instances.
        /// </returns>
        public static Volume operator +(Volume augend, Volume addend) => new Volume(augend.CubicMeters + addend.CubicMeters);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Volume" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Volume a, Volume b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Volume" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Volume a, Volume b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="Volume" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Volume a, Volume b) => a.Equals(b);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Volume" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Volume a, Volume b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Volume" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Volume" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Volume" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Volume a, Volume b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Volume" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a area value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Volume" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a area value.
        /// </exception>
        public static Volume Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Volume" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a area value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out Volume result)
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
        /// Compares the current <see cref="Volume" /> to the supplied object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Volume" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Volume other) => CubicMeters.CompareTo(other.CubicMeters);

        /// <summary>
        /// Determines whether or not the current <see cref="Volume" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is Volume volume)
            {
                return Equals(volume);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Volume" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Volume" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Volume other) => CubicMeters == other.CubicMeters;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => CubicMeters.GetHashCode();

        /// <summary>
        /// Converts the current <see cref="Volume" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Volume" />.
        /// </returns>
        public Byte[] ToByteArray() => CubicMeters.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="Volume" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Volume" />.
        /// </returns>
        public override String ToString() => $"{CubicMeters} {CubicMetersUnitOfMeasureSymbol}";

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Volume" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a area value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Volume" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a area value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Volume result, Boolean raiseExceptionOnFail)
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
                    if (numericSubstringIsComplete == false)
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
                    case CubicCentimetersUnitOfMeasureCymbol:

                        result = FromCubicCentimeters(numericValue);
                        break;

                    case CubicFeetUnitOfMeasureSymbol:

                        result = FromCubicFeet(numericValue);
                        break;

                    case CubicInchesUnitOfMeasureSymbol:

                        result = FromCubicInches(numericValue);
                        break;

                    case CubicKilometersUnitOfMeasureSymbol:

                        result = FromCubicKilometers(numericValue);
                        break;

                    case CubicMetersUnitOfMeasureSymbol:

                        result = FromCubicMeters(numericValue);
                        break;

                    case CubicMilesUnitOfMeasureSymbol:

                        result = FromCubicMiles(numericValue);
                        break;

                    case CubicMillimetersUnitOfMeasureSymbol:

                        result = FromCubicMillimeters(numericValue);
                        break;

                    case CubicYardsUnitOfMeasureSymbol:

                        result = FromCubicYards(numericValue);
                        break;

                    case GallonsUnitOfMeasureSymbol:

                        result = FromGallons(numericValue);
                        break;

                    case LitersUnitOfMeasureSymbol:

                        result = FromLiters(numericValue);
                        break;

                    case MillilitersUnitOfMeasureSymbol:

                        result = FromMilliliters(numericValue);
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
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic centimeters.
        /// </summary>
        public Decimal TotalCubicCentimeters => CubicMeters * CubicCentimetersPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic feet.
        /// </summary>
        public Decimal TotalCubicFeet => CubicMeters * CubicFeetPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic inches.
        /// </summary>
        public Decimal TotalCubicInches => CubicMeters * CubicInchesPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic kilometers.
        /// </summary>
        public Decimal TotalCubicKilometers => CubicMeters * CubicKilometersPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic meters.
        /// </summary>
        public Decimal TotalCubicMeters => CubicMeters;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic miles.
        /// </summary>
        public Decimal TotalCubicMiles => CubicMeters * CubicMilesPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic millimeters.
        /// </summary>
        public Decimal TotalCubicMillimeters => CubicMeters * CubicMillimetersPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in cubic yards.
        /// </summary>
        public Decimal TotalCubicYards => CubicMeters * CubicYardsPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in gallons.
        /// </summary>
        public Decimal TotalGallons => CubicMeters * GallonsPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in liters.
        /// </summary>
        public Decimal TotalLiters => CubicMeters * LitersPerCubicMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Volume" /> expressed in milliliters.
        /// </summary>
        public Decimal TotalMilliliters => CubicMeters * MillilitersPerCubicMeter;

        /// <summary>
        /// Represents the zero <see cref="Volume" /> value.
        /// </summary>
        public static readonly Volume Zero = new Volume(0m);

        /// <summary>
        /// Represents the number of cubic centimeters in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicCentimetersPerCubicMeter = 1000000m;

        /// <summary>
        /// Represents the textual symbol for a cubic centimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicCentimetersUnitOfMeasureCymbol = "ccm";

        /// <summary>
        /// Represents the number of cubic feet in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicFeetPerCubicMeter = 35.31466672148859m;

        /// <summary>
        /// Represents the textual symbol for a cubic foot.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicFeetUnitOfMeasureSymbol = "cbft";

        /// <summary>
        /// Represents the number of cubic inches in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicInchesPerCubicMeter = 61023.74409473228m;

        /// <summary>
        /// Represents the textual symbol for a cubic inch.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicInchesUnitOfMeasureSymbol = "cbin";

        /// <summary>
        /// Represents the number of cubic kilometers in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicKilometersPerCubicMeter = 0.000000001m;

        /// <summary>
        /// Represents the textual symbol for a cubic kilometer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicKilometersUnitOfMeasureSymbol = "cbkm";

        /// <summary>
        /// Represents the textual symbol for a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicMetersUnitOfMeasureSymbol = "cbm";

        /// <summary>
        /// Represents the number of cubic miles in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicMilesPerCubicMeter = 0.000000000239913m;

        /// <summary>
        /// Represents the textual symbol for a cubic mile.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicMilesUnitOfMeasureSymbol = "cbmi";

        /// <summary>
        /// Represents the number of cubic millimeters in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicMillimetersPerCubicMeter = 1000000000m;

        /// <summary>
        /// Represents the textual symbol for a cubic millimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicMillimetersUnitOfMeasureSymbol = "cbmm";

        /// <summary>
        /// Represents the number of cubic millimeters in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CubicYardsPerCubicMeter = 1.307951m;

        /// <summary>
        /// Represents the textual symbol for a cubic yard.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CubicYardsUnitOfMeasureSymbol = "cbyd";

        /// <summary>
        /// Represents the number of gallons in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal GallonsPerCubicMeter = 264.172053m;

        /// <summary>
        /// Represents the textual symbol for a gallon.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GallonsUnitOfMeasureSymbol = "g";

        /// <summary>
        /// Represents the number of liters in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal LitersPerCubicMeter = 1000m;

        /// <summary>
        /// Represents the textual symbol for a liter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String LitersUnitOfMeasureSymbol = "l";

        /// <summary>
        /// Represents the number of milliliters in a cubic meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MillilitersPerCubicMeter = 1000000m;

        /// <summary>
        /// Represents the textual symbol for a milliliter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MillilitersUnitOfMeasureSymbol = "ml";

        /// <summary>
        /// Represents a message template for format exceptions raised by <see cref="Parse(String, out Volume, Boolean)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as a volume.";

        /// <summary>
        /// Represents the value of the current <see cref="Volume" /> expressed in cubic meters.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Decimal CubicMeters;
    }
}