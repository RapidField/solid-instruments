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
    /// Represents a measurement of the extent of a two-dimensional figure in the plane.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct Area : IComparable<Area>, IEquatable<Area>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Area" /> structure.
        /// </summary>
        /// <param name="squareMeters">
        /// An area measurement expressed in square meters.
        /// </param>
        public Area(Double squareMeters)
            : this(Convert.ToDecimal(squareMeters))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Area" /> structure.
        /// </summary>
        /// <param name="squareMeters">
        /// An area measurement expressed in square meters.
        /// </param>
        public Area(Decimal squareMeters)
        {
            SquareMeters = squareMeters;
        }

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of acres.
        /// </summary>
        public static Area FromAcres(Double value) => FromAcres(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of acres.
        /// </summary>
        public static Area FromAcres(Decimal value) => new(value / AcresPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of hectares.
        /// </summary>
        public static Area FromHectares(Double value) => FromHectares(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of hectares.
        /// </summary>
        public static Area FromHectares(Decimal value) => new(value / HectaresPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square centimeters.
        /// </summary>
        public static Area FromSquareCentimeters(Double value) => FromSquareCentimeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square centimeters.
        /// </summary>
        public static Area FromSquareCentimeters(Decimal value) => new(value / SquareCentimetersPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square feet.
        /// </summary>
        public static Area FromSquareFeet(Double value) => FromSquareFeet(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square feet.
        /// </summary>
        public static Area FromSquareFeet(Decimal value) => new(value / SquareFeetPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square inches.
        /// </summary>
        public static Area FromSquareInches(Double value) => FromSquareInches(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square inches.
        /// </summary>
        public static Area FromSquareInches(Decimal value) => new(value / SquareInchesPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square kilometers.
        /// </summary>
        public static Area FromSquareKilometers(Double value) => FromSquareKilometers(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square kilometers.
        /// </summary>
        public static Area FromSquareKilometers(Decimal value) => new(value / SquareKilometersPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square meters.
        /// </summary>
        public static Area FromSquareMeters(Double value) => FromSquareMeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square meters.
        /// </summary>
        public static Area FromSquareMeters(Decimal value) => new(value);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square miles.
        /// </summary>
        public static Area FromSquareMiles(Double value) => FromSquareMiles(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square miles.
        /// </summary>
        public static Area FromSquareMiles(Decimal value) => new(value / SquareMilesPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square millimeters.
        /// </summary>
        public static Area FromSquareMillimeters(Double value) => FromSquareMillimeters(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square millimeters.
        /// </summary>
        public static Area FromSquareMillimeters(Decimal value) => new(value / SquareMillimetersPerSquareMeter);

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square yards.
        /// </summary>
        public static Area FromSquareYards(Double value) => FromSquareYards(Convert.ToDecimal(value));

        /// <summary>
        /// Returns an <see cref="Area" /> that represents the specified number of square yards.
        /// </summary>
        public static Area FromSquareYards(Decimal value) => new(value / SquareYardsPerSquareMeter);

        /// <summary>
        /// Determines the difference between two <see cref="Area" /> values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first <see cref="Area" /> instance.
        /// </param>
        /// <param name="minuend">
        /// The second <see cref="Area" /> instance.
        /// </param>
        /// <returns>
        /// The area between the specified <see cref="Area" /> instances.
        /// </returns>
        public static Area operator -(Area subtrahend, Area minuend) => new(subtrahend.SquareMeters - minuend.SquareMeters);

        /// <summary>
        /// Negates the specified <see cref="Area" /> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Area" /> instance.
        /// </param>
        /// <returns>
        /// The negative of the specified <see cref="Area" /> instance.
        /// </returns>
        public static Area operator -(Area value) => new(-value.SquareMeters);

        /// <summary>
        /// Determines whether or not two specified <see cref="Area" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Area a, Area b) => (a == b) is false;

        /// <summary>
        /// Determines the product of an <see cref="Area" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Area" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Area" /> and the specified multiplier.
        /// </returns>
        public static Area operator *(Area multiplicand, Double multiplier) => multiplicand * Convert.ToDecimal(multiplier);

        /// <summary>
        /// Determines the product of an <see cref="Area" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Area" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Area" /> and the specified multiplier.
        /// </returns>
        public static Area operator *(Area multiplicand, Decimal multiplier) => new(multiplicand.SquareMeters * multiplier);

        /// <summary>
        /// Determines the quotient of an <see cref="Area" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Area" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Area" /> and the specified divisor.
        /// </returns>
        public static Area operator /(Area dividend, Double divisor) => dividend / Convert.ToDecimal(divisor);

        /// <summary>
        /// Determines the quotient of an <see cref="Area" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Area" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Area" /> and the specified divisor.
        /// </returns>
        public static Area operator /(Area dividend, Decimal divisor) => new(dividend.SquareMeters / divisor);

        /// <summary>
        /// Determines the sum of two <see cref="Area" /> values.
        /// </summary>
        /// <param name="augend">
        /// The first <see cref="Area" /> instance.
        /// </param>
        /// <param name="addend">
        /// The second <see cref="Area" /> instance.
        /// </param>
        /// <returns>
        /// The sum of the specified <see cref="Area" /> instances.
        /// </returns>
        public static Area operator +(Area augend, Area addend) => new(augend.SquareMeters + addend.SquareMeters);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Area" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Area a, Area b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Area" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Area a, Area b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="Area" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Area a, Area b) => a.Equals(b);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Area" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Area a, Area b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Area" /> instance is greater than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Area" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Area" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Area a, Area b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Area" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a area value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Area" /> that is equivalent to <paramref name="input" />.
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
        public static Area Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Area" /> equivalent. The
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
        public static Boolean TryParse(String input, out Area result)
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
        /// Compares the current <see cref="Area" /> to the supplied object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Area" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Area other) => SquareMeters.CompareTo(other.SquareMeters);

        /// <summary>
        /// Determines whether or not the current <see cref="Area" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is Area area)
            {
                return Equals(area);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Area" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Area" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Area other) => SquareMeters == other.SquareMeters;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => SquareMeters.GetHashCode();

        /// <summary>
        /// Converts the current <see cref="Area" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Area" />.
        /// </returns>
        public Byte[] ToByteArray() => SquareMeters.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="Area" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Area" />.
        /// </returns>
        public override String ToString() => $"{SquareMeters} {SquareMetersUnitOfMeasureSymbol}";

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a area value to its <see cref="Area" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a area value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Area" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        private static Boolean Parse(String input, out Area result, Boolean raiseExceptionOnFail)
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
                    case AcresUnitOfMeasureCymbol:

                        result = FromAcres(numericValue);
                        break;

                    case HectaresUnitOfMeasureCymbol:

                        result = FromHectares(numericValue);
                        break;

                    case SquareCentimetersUnitOfMeasureCymbol:

                        result = FromSquareCentimeters(numericValue);
                        break;

                    case SquareFeetUnitOfMeasureSymbol:

                        result = FromSquareFeet(numericValue);
                        break;

                    case SquareInchesUnitOfMeasureSymbol:

                        result = FromSquareInches(numericValue);
                        break;

                    case SquareKilometersUnitOfMeasureSymbol:

                        result = FromSquareKilometers(numericValue);
                        break;

                    case SquareMetersUnitOfMeasureSymbol:

                        result = FromSquareMeters(numericValue);
                        break;

                    case SquareMilesUnitOfMeasureSymbol:

                        result = FromSquareMiles(numericValue);
                        break;

                    case SquareMillimetersUnitOfMeasureSymbol:

                        result = FromSquareMillimeters(numericValue);
                        break;

                    case SquareYardsUnitOfMeasureSymbol:

                        result = FromSquareYards(numericValue);
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
        /// Gets the value of the current <see cref="Area" /> expressed in acres.
        /// </summary>
        public Decimal TotalAcres => SquareMeters * AcresPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in hectares.
        /// </summary>
        public Decimal TotalHectares => SquareMeters * HectaresPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square centimeters.
        /// </summary>
        public Decimal TotalSquareCentimeters => SquareMeters * SquareCentimetersPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square feet.
        /// </summary>
        public Decimal TotalSquareFeet => SquareMeters * SquareFeetPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square inches.
        /// </summary>
        public Decimal TotalSquareInches => SquareMeters * SquareInchesPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square kilometers.
        /// </summary>
        public Decimal TotalSquareKilometers => SquareMeters * SquareKilometersPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square meters.
        /// </summary>
        public Decimal TotalSquareMeters => SquareMeters;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square miles.
        /// </summary>
        public Decimal TotalSquareMiles => SquareMeters * SquareMilesPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square millimeters.
        /// </summary>
        public Decimal TotalSquareMillimeters => SquareMeters * SquareMillimetersPerSquareMeter;

        /// <summary>
        /// Gets the value of the current <see cref="Area" /> expressed in square yards.
        /// </summary>
        public Decimal TotalSquareYards => SquareMeters * SquareYardsPerSquareMeter;

        /// <summary>
        /// Represents the zero <see cref="Area" /> value.
        /// </summary>
        public static readonly Area Zero = new(0m);

        /// <summary>
        /// Represents the number of acres in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal AcresPerSquareMeter = 2.471053814671653e-4m;

        /// <summary>
        /// Represents the textual symbol for an acre.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String AcresUnitOfMeasureCymbol = "ac";

        /// <summary>
        /// Represents the number of hectares in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal HectaresPerSquareMeter = 0.0001m;

        /// <summary>
        /// Represents the textual symbol for a hectare.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String HectaresUnitOfMeasureCymbol = "ha";

        /// <summary>
        /// Represents a message template for format exceptions raised by <see cref="Parse(String, out Area, Boolean)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as an area.";

        /// <summary>
        /// Represents the number of square centimeters in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareCentimetersPerSquareMeter = 10000m;

        /// <summary>
        /// Represents the textual symbol for a square centimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareCentimetersUnitOfMeasureCymbol = "sqcm";

        /// <summary>
        /// Represents the number of square feet in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareFeetPerSquareMeter = 10.76391041670972m;

        /// <summary>
        /// Represents the textual symbol for a square foot.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareFeetUnitOfMeasureSymbol = "sqft";

        /// <summary>
        /// Represents the number of square inches in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareInchesPerSquareMeter = 1550.0031000062m;

        /// <summary>
        /// Represents the textual symbol for a square inch.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareInchesUnitOfMeasureSymbol = "sqin";

        /// <summary>
        /// Represents the number of square kilometers in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareKilometersPerSquareMeter = 0.000001m;

        /// <summary>
        /// Represents the textual symbol for a square kilometer.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareKilometersUnitOfMeasureSymbol = "sqkm";

        /// <summary>
        /// Represents the textual symbol for a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareMetersUnitOfMeasureSymbol = "sqm";

        /// <summary>
        /// Represents the number of square miles in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareMilesPerSquareMeter = 3.861021585424458e-7m;

        /// <summary>
        /// Represents the textual symbol for a square mile.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareMilesUnitOfMeasureSymbol = "sqmi";

        /// <summary>
        /// Represents the number of square millimeters in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareMillimetersPerSquareMeter = 1000000m;

        /// <summary>
        /// Represents the textual symbol for a square millimeter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareMillimetersUnitOfMeasureSymbol = "sqmm";

        /// <summary>
        /// Represents the number of square yards in a square meter.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal SquareYardsPerSquareMeter = 1.19599004630108m;

        /// <summary>
        /// Represents the textual symbol for a square yard.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SquareYardsUnitOfMeasureSymbol = "sqyd";

        /// <summary>
        /// Represents the value of the current <see cref="Area" /> expressed in square meters.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Decimal SquareMeters;
    }
}