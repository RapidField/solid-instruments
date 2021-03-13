// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Decimal" /> structure with general purpose features.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="Decimal" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="Decimal" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this Decimal target)
        {
            if (target == 0m)
            {
                return 0;
            }

            var absoluteValue = Math.Abs(target);
            var absoluteValueIsLessThanOne = absoluteValue < 1m;
            var targetIsRational = absoluteValue.IsRational();
            var wholeAndFractionalFigures = absoluteValue.ToString(GeneralNumericStringFormatSpecifier, CultureInfo.InvariantCulture).Split(ExponentialPrefixCharacter)[0].Split(DecimalPointCharacter);
            var wholeFigures = targetIsRational ? (absoluteValueIsLessThanOne ? String.Empty : wholeAndFractionalFigures[0].TrimStart(ZeroCharacter)) : wholeAndFractionalFigures[0].Trim(ZeroCharacter);
            var fractionalFigures = targetIsRational ? (absoluteValueIsLessThanOne ? wholeAndFractionalFigures[1].Trim(ZeroCharacter) : wholeAndFractionalFigures[1].TrimEnd(ZeroCharacter)) : String.Empty;
            return wholeFigures.Length + fractionalFigures.Length;
        }

        /// <summary>
        /// Determines whether or not the current <see cref="Decimal" /> value is an integer (a number in the series { ..., -2, -1,
        /// 0, 1, 2, ... }).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is an integer, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsInteger(this Decimal target) => Math.Abs(target % 1m) <= Double.Epsilon.ToDecimal();

        /// <summary>
        /// Determines whether or not the current <see cref="Decimal" /> value is a rational number (not an integer).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is a rational number, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsRational(this Decimal target) => target.IsInteger() is false;

        /// <summary>
        /// Rounds the current <see cref="Decimal" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Decimal" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Decimal" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Decimal" />.
        /// </exception>
        public static Decimal RoundedTo(this Decimal target, Int32 digits) => Math.Round(target, digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Decimal" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Decimal" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Decimal" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Decimal" />.
        /// </exception>
        public static Decimal RoundedTo(this Decimal target, Int32 digits, MidpointRounding midpointRoundingMode) => Math.Round(target, digits, midpointRoundingMode);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigInteger" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ToBigInteger(this Decimal target) => new(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigRational" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigRational ToBigRational(this Decimal target) => target.IsInteger() ? new(target.ToBigInteger()) : BigRational.Approximate(target, 13);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this Decimal target) => Convert.ToByte(target);

        /// <summary>
        /// Converts the current <see cref="Decimal" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Decimal" />.
        /// </returns>
        public static Byte[] ToByteArray(this Decimal target)
        {
            var bytes = new List<Byte>();

            {
                var components = Decimal.GetBits(target);

                foreach (var integer in components)
                {
                    bytes.AddRange(BitConverter.GetBytes(integer));
                }
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Double" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="Double" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this Decimal target) => Convert.ToDouble(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Decimal target) => Convert.ToInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Decimal target) => Convert.ToInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Decimal target) => Convert.ToInt64(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// An <see cref="SByte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this Decimal target) => Convert.ToSByte(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="Single" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="Single" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle(this Decimal target) => Convert.ToSingle(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Decimal target) => Convert.ToUInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Decimal target) => Convert.ToUInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Decimal" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Decimal" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Decimal target) => Convert.ToUInt64(target);

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
        /// Represents the textual format specifier for a general numeric string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GeneralNumericStringFormatSpecifier = "G28";

        /// <summary>
        /// Represents the character "0".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Char ZeroCharacter = '0';
    }
}