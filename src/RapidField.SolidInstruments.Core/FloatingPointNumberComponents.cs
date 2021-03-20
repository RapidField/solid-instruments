// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents the constituent elements of a floating point number.
    /// </summary>
    internal sealed class FloatingPointNumberComponents
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="value">
        /// The associated number.
        /// </param>
        [DebuggerHidden]
        internal FloatingPointNumberComponents(Decimal value)
            : this(Convert.ToSByte(value == 0m ? 0 : (value < 0m ? -1 : 1)), Math.Abs(value))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="value">
        /// The associated number.
        /// </param>
        [DebuggerHidden]
        internal FloatingPointNumberComponents(Double value)
            : this(Convert.ToSByte(value == 0d ? 0 : (value < 0d ? -1 : 1)), Math.Abs(value))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="value">
        /// The associated number.
        /// </param>
        /// s
        [DebuggerHidden]
        internal FloatingPointNumberComponents(Single value)
            : this(Convert.ToSByte(value == 0f ? 0 : (value < 0f ? -1 : 1)), Math.Abs(value))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="sign">
        /// Positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a negative
        /// number, or zero (0) if the number is zero.
        /// </param>
        /// <param name="absoluteValue">
        /// The absolute value of the associated number.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sign" /> is less than negative one (-1) or greater than one (1).
        /// </exception>
        [DebuggerHidden]
        private FloatingPointNumberComponents(SByte sign, Decimal absoluteValue)
            : this(sign, absoluteValue < 1m, absoluteValue.IsFractional(), absoluteValue.ToString(GeneralDecimalStringFormatSpecifier, CultureInfo.InvariantCulture).Split(ExponentialPrefixCharacter)[0].Split(DecimalPointCharacter))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="sign">
        /// Positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a negative
        /// number, or zero (0) if the number is zero.
        /// </param>
        /// <param name="absoluteValue">
        /// The absolute value of the associated number.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sign" /> is less than negative one (-1) or greater than one (1).
        /// </exception>
        [DebuggerHidden]
        private FloatingPointNumberComponents(SByte sign, Double absoluteValue)
            : this(sign, absoluteValue < 1d, absoluteValue.IsFractional(), absoluteValue.ToString(GeneralDoubleStringFormatSpecifier, CultureInfo.InvariantCulture).Split(ExponentialPrefixCharacter)[0].Split(DecimalPointCharacter))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="sign">
        /// Positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a negative
        /// number, or zero (0) if the number is zero.
        /// </param>
        /// <param name="absoluteValue">
        /// The absolute value of the associated number.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sign" /> is less than negative one (-1) or greater than one (1).
        /// </exception>
        [DebuggerHidden]
        private FloatingPointNumberComponents(SByte sign, Single absoluteValue)
            : this(sign, absoluteValue < 1f, absoluteValue.IsFractional(), absoluteValue.ToString(GeneralSingleStringFormatSpecifier, CultureInfo.InvariantCulture).Split(ExponentialPrefixCharacter)[0].Split(DecimalPointCharacter))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="sign">
        /// Positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a negative
        /// number, or zero (0) if the number is zero.
        /// </param>
        /// <param name="absoluteValueIsLessThanOne">
        /// A value indicating whether or not the associated number's absolute value is less than one (1).
        /// </param>
        /// <param name="isFractional">
        /// A value indicating whether or not the associated number is a fractional number.
        /// </param>
        /// <param name="wholeAndFractionalFigures">
        /// An array of strings containing numeric representations of the whole figures [0] and fractional figures [1] for the
        /// associated number, or just the whole figures if the associated number is a whole number.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="wholeAndFractionalFigures" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="wholeAndFractionalFigures" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sign" /> is less than negative one (-1) or greater than one (1).
        /// </exception>
        [DebuggerHidden]
        private FloatingPointNumberComponents(SByte sign, Boolean absoluteValueIsLessThanOne, Boolean isFractional, String[] wholeAndFractionalFigures)
            : this(sign, isFractional ? (absoluteValueIsLessThanOne ? String.Empty : wholeAndFractionalFigures.RejectIf().IsNullOrEmpty(nameof(wholeAndFractionalFigures)).TargetArgument[0].TrimStart(ZeroCharacter)) : wholeAndFractionalFigures.RejectIf().IsNullOrEmpty(nameof(wholeAndFractionalFigures)).TargetArgument[0].Trim(ZeroCharacter), isFractional ? (absoluteValueIsLessThanOne ? wholeAndFractionalFigures[1].Trim(ZeroCharacter) : wholeAndFractionalFigures[1].TrimEnd(ZeroCharacter)) : String.Empty)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointNumberComponents" /> class.
        /// </summary>
        /// <param name="sign">
        /// Positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a negative
        /// number, or zero (0) if the number is zero.
        /// </param>
        /// <param name="wholeFigures">
        /// The whole figures for the associated number as a string of numeric characters, or <see cref="String.Empty" /> if the
        /// associated number is greater than negative one (-1) -and- less than one (1).
        /// </param>
        /// <param name="fractionalFigures">
        /// The fractional figures for the associated number as a string of numeric characters, or <see cref="String.Empty" /> if
        /// the associated number is a whole number.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fractionalFigures" /> is <see langword="null" /> -or- <paramref name="wholeFigures" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="sign" /> is less than negative one (-1) or greater than one (1).
        /// </exception>
        [DebuggerHidden]
        private FloatingPointNumberComponents(SByte sign, String wholeFigures, String fractionalFigures)
        {
            FractionalFigures = fractionalFigures.RejectIf().IsNull(nameof(fractionalFigures));
            Sign = sign.RejectIf().IsLessThan<SByte>(-1, nameof(sign)).OrIf().IsGreaterThan<SByte>(1, nameof(sign));
            WholeFigures = wholeFigures.RejectIf().IsNull(nameof(wholeFigures));
        }

        /// <summary>
        /// Gets the fractional figures for the associated number as a string of numeric characters, or <see cref="String.Empty" />
        /// if the associated number is a whole number.
        /// </summary>
        public String FractionalFigures
        {
            get;
        }

        /// <summary>
        /// Gets positive one (1) if the associated number is a positive number, negative one (-1) if the associated number is a
        /// negative number, or zero (0) if the number is zero.
        /// </summary>
        public SByte Sign
        {
            get;
        }

        /// <summary>
        /// Gets the significant digits of the associated number expressed as a positive or negative whole number string.
        /// </summary>
        public String Significand => Sign switch
        {
            -1 => $"{NegativeSignCharacter}{SignificantDigits}",
            1 => SignificantDigits,
            _ => $"{ZeroCharacter}"
        };

        /// <summary>
        /// Gets the whole figures for the associated number as a string of numeric characters, or <see cref="String.Empty" /> if
        /// the associated number is greater than negative one (-1) -and- less than one (1).
        /// </summary>
        public String WholeFigures
        {
            get;
        }

        /// <summary>
        /// Gets the significant digits of the associated number expressed as an absolute whole number string, or
        /// <see cref="String.Empty" /> if the associated number is zero (0).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String SignificantDigits => $"{WholeFigures}{FractionalFigures}".Trim(ZeroCharacter);

        /// <summary>
        /// Represents the character ".".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char DecimalPointCharacter = '.';

        /// <summary>
        /// Represents the character that prefixes exponential digits ("E").
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char ExponentialPrefixCharacter = 'E';

        /// <summary>
        /// Represents the textual format specifier for a general <see cref="Decimal" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GeneralDecimalStringFormatSpecifier = "G28";

        /// <summary>
        /// Represents the textual format specifier for a general <see cref="Double" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GeneralDoubleStringFormatSpecifier = "G16";

        /// <summary>
        /// Represents the textual format specifier for a general <see cref="Single" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GeneralSingleStringFormatSpecifier = "G8";

        /// <summary>
        /// Represents the character "-".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char NegativeSignCharacter = '-';

        /// <summary>
        /// Represents the character "0".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char ZeroCharacter = '0';
    }
}