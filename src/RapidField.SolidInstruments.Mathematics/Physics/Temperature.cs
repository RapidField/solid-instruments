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
    /// Represents a measurement of heat intensity.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct Temperature : IComparable<Temperature>, IEquatable<Temperature>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature" /> structure.
        /// </summary>
        /// <param name="degreesCelsius">
        /// A temperature measurement expressed in degrees Celsius.
        /// </param>
        public Temperature(Double degreesCelsius)
            : this(Convert.ToDecimal(degreesCelsius))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature" /> structure.
        /// </summary>
        /// <param name="degreesCelsius">
        /// A temperature measurement expressed in degrees Celsius.
        /// </param>
        public Temperature(Decimal degreesCelsius)
        {
            DegreesCelsius = degreesCelsius;
        }

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of degrees Fahrenheit.
        /// </summary>
        public static Temperature FromDegreesFahrenheit(Double value) => FromDegreesFahrenheit(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of degrees Fahrenheit.
        /// </summary>
        public static Temperature FromDegreesFahrenheit(Decimal value) => new Temperature((value - 32m) * (5m / 9m));

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of degrees Celsius.
        /// </summary>
        public static Temperature FromDegreesCelsius(Double value) => FromDegreesCelsius(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of degrees Celsius.
        /// </summary>
        public static Temperature FromDegreesCelsius(Decimal value) => new Temperature(value);

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of kelvins.
        /// </summary>
        public static Temperature FromKelvins(Double value) => FromKelvins(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Temperature" /> that represents the specified number of kelvins.
        /// </summary>
        public static Temperature FromKelvins(Decimal value) => new Temperature(value - 273.15m);

        /// <summary>
        /// Determines the difference between two <see cref="Temperature" /> values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="minuend">
        /// The second <see cref="Temperature" /> instance.
        /// </param>
        /// <returns>
        /// The temperature between the specified <see cref="Temperature" /> instances.
        /// </returns>
        public static Temperature operator -(Temperature subtrahend, Temperature minuend) => new Temperature(subtrahend.DegreesCelsius - minuend.DegreesCelsius);

        /// <summary>
        /// Negates the specified <see cref="Temperature" /> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Temperature" /> instance.
        /// </param>
        /// <returns>
        /// The negative of the specified <see cref="Temperature" /> instance.
        /// </returns>
        public static Temperature operator -(Temperature value) => new Temperature(-value.DegreesCelsius);

        /// <summary>
        /// Determines whether or not two specified <see cref="Temperature" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Temperature a, Temperature b) => (a == b) is false;

        /// <summary>
        /// Determines the product of a <see cref="Temperature" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Temperature" /> and the specified multiplier.
        /// </returns>
        public static Temperature operator *(Temperature multiplicand, Double multiplier) => multiplicand * Convert.ToDecimal(multiplier);

        /// <summary>
        /// Determines the product of a <see cref="Temperature" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Temperature" /> and the specified multiplier.
        /// </returns>
        public static Temperature operator *(Temperature multiplicand, Decimal multiplier) => new Temperature(multiplicand.DegreesCelsius * multiplier);

        /// <summary>
        /// Determines the quotient of a <see cref="Temperature" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Temperature" /> and the specified divisor.
        /// </returns>
        public static Temperature operator /(Temperature dividend, Double divisor) => dividend / Convert.ToDecimal(divisor);

        /// <summary>
        /// Determines the quotient of a <see cref="Temperature" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Temperature" /> and the specified divisor.
        /// </returns>
        public static Temperature operator /(Temperature dividend, Decimal divisor) => new Temperature(dividend.DegreesCelsius / divisor);

        /// <summary>
        /// Determines the sum of two <see cref="Temperature" /> values.
        /// </summary>
        /// <param name="augend">
        /// The first <see cref="Temperature" /> instance.
        /// </param>
        /// <param name="addend">
        /// The second <see cref="Temperature" /> instance.
        /// </param>
        /// <returns>
        /// The sum of the specified <see cref="Temperature" /> instances.
        /// </returns>
        public static Temperature operator +(Temperature augend, Temperature addend) => new Temperature(augend.DegreesCelsius + addend.DegreesCelsius);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Temperature" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Temperature a, Temperature b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Temperature" /> instance is less than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Temperature a, Temperature b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines whether or not two specified <see cref="Temperature" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Temperature a, Temperature b) => a.Equals(b);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Temperature" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Temperature a, Temperature b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Temperature" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Temperature" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Temperature a, Temperature b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a temperature value to its <see cref="Temperature" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a temperature value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Temperature" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a temperature value.
        /// </exception>
        public static Temperature Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a temperature value to its <see cref="Temperature" />
        /// equivalent. The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a temperature value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out Temperature result)
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
        /// Compares the current <see cref="Temperature" /> to the supplied object and returns an indication of their relative
        /// values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Temperature" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Temperature other) => DegreesCelsius.CompareTo(other.DegreesCelsius);

        /// <summary>
        /// Determines whether or not the current <see cref="Temperature" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is Temperature temperature)
            {
                return Equals(temperature);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Temperature" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Temperature" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Temperature other) => DegreesCelsius == other.DegreesCelsius;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => DegreesCelsius.GetHashCode();

        /// <summary>
        /// Converts the current <see cref="Temperature" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Temperature" />.
        /// </returns>
        public Byte[] ToByteArray() => DegreesCelsius.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="Temperature" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Temperature" />.
        /// </returns>
        public override String ToString() => $"{DegreesCelsius} {DegreesCelsiusUnitOfMeasureSymbol}";

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a temperature value to its <see cref="Temperature" />
        /// equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a temperature value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Temperature" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a temperature value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Temperature result, Boolean raiseExceptionOnFail)
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

            var processedString = input.Solidify().ToUpper();

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

                    if (character.IsAlphabetic() || character == '°')
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
                    case DegreesCelsiusUnitOfMeasureSymbol:

                        result = FromDegreesCelsius(numericValue);
                        break;

                    case DegreesFahrenheitUnitOfMeasureSymbol:

                        result = FromDegreesFahrenheit(numericValue);
                        break;

                    case KelvinsUnitOfMeasureSymbol:

                        result = FromKelvins(numericValue);
                        break;

                    default:

                        if (raiseExceptionOnFail)
                        {
                            throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(Temperature)));
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
        /// Gets the value of the current <see cref="Temperature" /> expressed in degrees Celsius.
        /// </summary>
        public Decimal TotalDegreesCelsius => DegreesCelsius;

        /// <summary>
        /// Gets the value of the current <see cref="Temperature" /> expressed in degrees Fahrenheit.
        /// </summary>
        public Decimal TotalDegreesFahrenheit => (DegreesCelsius * (9m / 5m)) + 32m;

        /// <summary>
        /// Gets the value of the current <see cref="Temperature" /> expressed in kelvins.
        /// </summary>
        public Decimal TotalKelvins => DegreesCelsius + 273.15m;

        /// <summary>
        /// Represents the temperature at the lower limit of the thermodynamic temperature scale.
        /// </summary>
        public static readonly Temperature AbsoluteZero = FromKelvins(0m);

        /// <summary>
        /// Represents the temperature at which water boils at 1 atm.
        /// </summary>
        public static readonly Temperature BoilingPointOfWater = FromDegreesCelsius(99.9839m);

        /// <summary>
        /// Represents the temperature at which purified water melts.
        /// </summary>
        public static readonly Temperature MeltingPointOfWater = FromDegreesCelsius(-0.0001m);

        /// <summary>
        /// Represents the zero <see cref="Temperature" /> value (0 °C).
        /// </summary>
        public static readonly Temperature Zero = FromDegreesCelsius(0m);

        /// <summary>
        /// Represents the textual symbol for a degree Celsius.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DegreesCelsiusUnitOfMeasureSymbol = "°C";

        /// <summary>
        /// Represents the textual symbol for a degree Fahrenheit.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DegreesFahrenheitUnitOfMeasureSymbol = "°F";

        /// <summary>
        /// Represents the textual symbol for a kelvin.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String KelvinsUnitOfMeasureSymbol = "K";

        /// <summary>
        /// Represents a message template for format exceptions raised by <see cref="Parse(String, out Temperature, Boolean)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as a temperature.";

        /// <summary>
        /// Represents the value of the current <see cref="Temperature" /> expressed in degrees Celsius.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Decimal DegreesCelsius;
    }
}