// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="BigRational" /> structure with general purpose features.
    /// </summary>
    public static class BigRationalExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="BigRational" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="BigRational" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this BigRational target)
        {
            if (target.IsZero)
            {
                return 0;
            }

            var significantFigureCount = 0;

            foreach (var digit in target.Digits)
            {
                significantFigureCount++;

                if (significantFigureCount == Number.SignificantFiguresLimitForDecimal)
                {
                    break;
                }
            }

            return significantFigureCount;
        }

        /// <summary>
        /// Determines whether or not the current <see cref="BigRational" /> value is an integer (a number in the series { ..., -2,
        /// -1, 0, 1, 2, ... }).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is an integer, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsInteger(this BigRational target) => target.FractionPart.IsZero;

        /// <summary>
        /// Determines whether or not the current <see cref="BigRational" /> value is a rational number (not an integer).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is a rational number, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsRational(this BigRational target) => target.IsInteger() is false;

        /// <summary>
        /// Rounds the current <see cref="BigRational" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="BigRational" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="BigRational" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="BigRational" />.
        /// </exception>
        public static BigRational RoundedTo(this BigRational target, Int32 digits) => target.RoundedTo(digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="BigRational" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="BigRational" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="BigRational" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="BigRational" />.
        /// </exception>
        public static BigRational RoundedTo(this BigRational target, Int32 digits, MidpointRounding midpointRoundingMode)
        {
            if (target.IsInteger() || target.FractionPart.CountSignificantFigures() <= digits)
            {
                return target;
            }

            var wholeNumberString = target.WholePart.ToString();
            var fractionalDigitString = ((Decimal)target.FractionPart).RoundedTo(digits, midpointRoundingMode).ToString().TrimStart('0').TrimStart('.');

            try
            {
                return BigRational.Approximate(Decimal.Parse($"{wholeNumberString}.{fractionalDigitString}"), 13);
            }
            catch (Exception exception)
            {
                throw new OverflowException("The resulting rounded number is an invalid rational number.", exception);
            }
        }

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigInteger" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="BigInteger" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ToBigInteger(this BigRational target) => target.FractionPart.IsZero ? target.WholePart : throw new OverflowException("The specified number is not a valid integer.");

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this BigRational target) => Convert.ToByte(target.ToDecimal());

        /// <summary>
        /// Converts the current <see cref="BigRational" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="BigRational" />.
        /// </returns>
        public static Byte[] ToByteArray(this BigRational target) => Number.BigRationalToByteArray(target);

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Decimal" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal(this BigRational target) => (Decimal)target;

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Double" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="Double" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this BigRational target) => Convert.ToDouble(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this BigRational target) => Convert.ToInt16(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this BigRational target) => Convert.ToInt32(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this BigRational target) => Convert.ToInt64(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// An <see cref="SByte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this BigRational target) => Convert.ToSByte(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="Single" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="Single" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Single" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle(this BigRational target) => Convert.ToSingle(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this BigRational target) => Convert.ToUInt16(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this BigRational target) => Convert.ToUInt32(target.ToDecimal());

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigRational" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this BigRational target) => Convert.ToUInt64(target.ToDecimal());
    }
}