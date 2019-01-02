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
    /// Represents a measurement of the strength of gravitational attraction of a physical body.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct Mass : IComparable<Mass>, IEquatable<Mass>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mass" /> structure.
        /// </summary>
        /// <param name="grams">
        /// A mass measurement expressed in grams.
        /// </param>
        public Mass(Double grams)
            : this(Convert.ToDecimal(grams))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mass" /> structure.
        /// </summary>
        /// <param name="grams">
        /// A mass measurement expressed in grams.
        /// </param>
        public Mass(Decimal grams)
        {
            Grams = grams;
        }

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of centigrams.
        /// </summary>
        public static Mass FromCentigrams(Double value) => FromCentigrams(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of centigrams.
        /// </summary>
        public static Mass FromCentigrams(Decimal value) => new Mass(value / CentigramsPerGram);

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of grams.
        /// </summary>
        public static Mass FromGrams(Double value) => FromGrams(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of grams.
        /// </summary>
        public static Mass FromGrams(Decimal value) => new Mass(value);

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of kilograms.
        /// </summary>
        public static Mass FromKilograms(Double value) => FromKilograms(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of kilograms.
        /// </summary>
        public static Mass FromKilograms(Decimal value) => new Mass(value / KilogramsPerGram);

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of micro-grams.
        /// </summary>
        public static Mass FromMicrograms(Double value) => FromMicrograms(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of micro-grams.
        /// </summary>
        public static Mass FromMicrograms(Decimal value) => new Mass(value / MicrogramsPerGram);

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of milligrams.
        /// </summary>
        public static Mass FromMilligrams(Double value) => FromMilligrams(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of milligrams.
        /// </summary>
        public static Mass FromMilligrams(Decimal value) => new Mass(value / MilligramsPerGram);

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of nanograms.
        /// </summary>
        public static Mass FromNanograms(Double value) => FromNanograms(Convert.ToDecimal(value));

        /// <summary>
        /// Returns a <see cref="Mass" /> that represents the specified number of nanograms.
        /// </summary>
        public static Mass FromNanograms(Decimal value) => new Mass(value / NanogramsPerGram);

        /// <summary>
        /// Determines the difference between two <see cref="Mass" /> values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first <see cref="Mass" /> instance.
        /// </param>
        /// <param name="minuend">
        /// The second <see cref="Mass" /> instance.
        /// </param>
        /// <returns>
        /// The mass between the specified <see cref="Mass" /> instances.
        /// </returns>
        public static Mass operator -(Mass subtrahend, Mass minuend) => new Mass(subtrahend.Grams - minuend.Grams);

        /// <summary>
        /// Negates the specified <see cref="Mass" /> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Mass" /> instance.
        /// </param>
        /// <returns>
        /// The negative of the specified <see cref="Mass" /> instance.
        /// </returns>
        public static Mass operator -(Mass value) => new Mass(-value.Grams);

        /// <summary>
        /// Determines whether or not two specified <see cref="Mass" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Mass a, Mass b) => (a == b) == false;

        /// <summary>
        /// Determines the product of a <see cref="Mass" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Mass" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Mass" /> and the specified multiplier.
        /// </returns>
        public static Mass operator *(Mass multiplicand, Double multiplier) => (multiplicand * Convert.ToDecimal(multiplier));

        /// <summary>
        /// Determines the product of a <see cref="Mass" /> value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A <see cref="Mass" /> instance.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified <see cref="Mass" /> and the specified multiplier.
        /// </returns>
        public static Mass operator *(Mass multiplicand, Decimal multiplier) => new Mass(multiplicand.Grams * multiplier);

        /// <summary>
        /// Determines the quotient of a <see cref="Mass" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Mass" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Mass" /> and the specified divisor.
        /// </returns>
        public static Mass operator /(Mass dividend, Double divisor) => (dividend / Convert.ToDecimal(divisor));

        /// <summary>
        /// Determines the quotient of a <see cref="Mass" /> value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A <see cref="Mass" /> instance.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified <see cref="Mass" /> and the specified divisor.
        /// </returns>
        public static Mass operator /(Mass dividend, Decimal divisor) => new Mass(dividend.Grams / divisor);

        /// <summary>
        /// Determines the sum of two <see cref="Mass" /> values.
        /// </summary>
        /// <param name="augend">
        /// The first <see cref="Mass" /> instance.
        /// </param>
        /// <param name="addend">
        /// The second <see cref="Mass" /> instance.
        /// </param>
        /// <returns>
        /// The sum of the specified <see cref="Mass" /> instances.
        /// </returns>
        public static Mass operator +(Mass augend, Mass addend) => new Mass(augend.Grams + addend.Grams);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Mass" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Mass a, Mass b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Mass" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Mass a, Mass b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="Mass" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Mass a, Mass b) => a.Equals(b);

        /// <summary>
        /// Determines whether or not a supplied <see cref="Mass" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Mass a, Mass b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a supplied <see cref="Mass" /> instance is greater than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Mass" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Mass" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Mass a, Mass b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a mass value to its <see cref="Mass" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a mass value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Mass" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a mass value.
        /// </exception>
        public static Mass Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return default(Mass);
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a mass value to its <see cref="Mass" /> equivalent. The
        /// method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a mass value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out Mass result)
        {
            if (Parse(input, out var value, false))
            {
                result = value;
                return true;
            }

            result = default(Mass);
            return false;
        }

        /// <summary>
        /// Compares the current <see cref="Mass" /> to the supplied object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Mass" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Mass other) => Grams.CompareTo(other.Grams);

        /// <summary>
        /// Determines whether or not the current <see cref="Mass" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is Mass)
            {
                return Equals((Mass)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Mass" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Mass" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Mass other) => (Grams == other.Grams);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => Grams.GetHashCode();

        /// <summary>
        /// Converts the current <see cref="Mass" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Mass" />.
        /// </returns>
        public Byte[] ToByteArray() => Grams.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="Mass" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Mass" />.
        /// </returns>
        public override String ToString() => $"{Grams} {GramsUnitOfMeasureSymbol}";

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a mass value to its <see cref="Mass" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a mass value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Mass" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a mass value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Mass result, Boolean raiseExceptionOnFail)
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
                result = default(Mass);
                return false;
            }

            var processedString = input.Solidify().ToLower();

            if (processedString.Length < 2)
            {
                if (raiseExceptionOnFail)
                {
                    throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input));
                }

                result = default(Mass);
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
                        result = default(Mass);
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
                    case CentigramsUnitOfMeasureSymbol:

                        result = FromCentigrams(numericValue);
                        break;

                    case GramsUnitOfMeasureSymbol:

                        result = FromGrams(numericValue);
                        break;

                    case KilogramsUnitOfMeasureSymbol:

                        result = FromKilograms(numericValue);
                        break;

                    case MicrogramsUnitOfMeasureSymbol:

                        result = FromMicrograms(numericValue);
                        break;

                    case MilligramsUnitOfMeasureSymbol:

                        result = FromMilligrams(numericValue);
                        break;

                    case NanogramsUnitOfMeasureSymbol:

                        result = FromNanograms(numericValue);
                        break;

                    default:

                        if (raiseExceptionOnFail)
                        {
                            throw new FormatException(ParseFormatExceptionMessageTemplate.ApplyFormat(input, nameof(Mass)));
                        }

                        result = default(Mass);
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
        /// Gets the value of the current <see cref="Mass" /> expressed in centigrams.
        /// </summary>
        public Decimal TotalCentigrams => (Grams * CentigramsPerGram);

        /// <summary>
        /// Gets the value of the current <see cref="Mass" /> expressed in grams.
        /// </summary>
        public Decimal TotalGrams => Grams;

        /// <summary>
        /// Gets the value of the current <see cref="Mass" /> expressed in kilograms.
        /// </summary>
        public Decimal TotalKilograms => (Grams * KilogramsPerGram);

        /// <summary>
        /// Gets the value of the current <see cref="Mass" /> expressed in micro-grams.
        /// </summary>
        public Decimal TotalMicrograms => (Grams * MicrogramsPerGram);

        /// <summary>
        /// Gets the value of the current <see cref="Mass" /> expressed in milligrams.
        /// </summary>
        public Decimal TotalMilligrams => (Grams * MilligramsPerGram);

        /// <summary>
        /// Gets the value of the current <see cref="Mass" /> expressed in nanograms.
        /// </summary>
        public Decimal TotalNanograms => (Grams * NanogramsPerGram);

        /// <summary>
        /// Represents the atomic mass unit constant.
        /// </summary>
        public static readonly Mass AtomicUnit = FromKilograms(1.6605402e-27m);

        /// <summary>
        /// Represents the mass of a single electron.
        /// </summary>
        public static readonly Mass OfSingleElectron = FromKilograms(9.1093897e-31m);

        /// <summary>
        /// Represents the mass of a single neutron.
        /// </summary>
        public static readonly Mass OfSingleNeutron = FromKilograms(1.6749286e-27m);

        /// <summary>
        /// Represents the mass of a single proton.
        /// </summary>
        public static readonly Mass OfSingleProtron = FromKilograms(1.6726231e-27m);

        /// <summary>
        /// Represents the zero <see cref="Mass" /> value.
        /// </summary>
        public static readonly Mass Zero = new Mass(0m);

        /// <summary>
        /// Represents the number of centigrams in a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal CentigramsPerGram = 100m;

        /// <summary>
        /// Represents the textual symbol for a centigram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CentigramsUnitOfMeasureSymbol = "cg";

        /// <summary>
        /// Represents the textual symbol for a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GramsUnitOfMeasureSymbol = "g";

        /// <summary>
        /// Represents the number of kilograms in a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal KilogramsPerGram = 0.001m;

        /// <summary>
        /// Represents the textual symbol for a kilogram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String KilogramsUnitOfMeasureSymbol = "kg";

        /// <summary>
        /// Represents the number of micro-grams in a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MicrogramsPerGram = 1000000m;

        /// <summary>
        /// Represents the textual symbol for a micro-gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MicrogramsUnitOfMeasureSymbol = "mcg";

        /// <summary>
        /// Represents the number of milligrams in a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal MilligramsPerGram = 1000m;

        /// <summary>
        /// Represents the textual symbol for a milligram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String MilligramsUnitOfMeasureSymbol = "mg";

        /// <summary>
        /// Represents the number of nanograms in a gram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Decimal NanogramsPerGram = 1000000000m;

        /// <summary>
        /// Represents the textual symbol for a nanogram.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String NanogramsUnitOfMeasureSymbol = "ng";

        /// <summary>
        /// Represents a message template for format exceptions raised by <see cref="Parse(String, out Mass, Boolean)" />
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ParseFormatExceptionMessageTemplate = "The specified string, \"{0}\", could not be parsed as a mass.";

        /// <summary>
        /// Represents the value of the current <see cref="Mass" /> expressed in grams.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Decimal Grams;
    }
}