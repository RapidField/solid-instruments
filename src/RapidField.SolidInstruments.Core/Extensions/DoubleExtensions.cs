// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Double" /> structure with general purpose features.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="Double" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="Double" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this Double target) => target == 0d ? 0 : target.ToComponents().SignificantDigits.Length;

        /// <summary>
        /// Returns the significant digits for the current <see cref="Double" /> expressed as a whole number.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// The significand, or mantissa, for the current <see cref="Double" />.
        /// </returns>
        public static Double GetSignificand(this Double target) => target == 0d ? 0d : Double.Parse(target.ToComponents().Significand);

        /// <summary>
        /// Determines whether or not the current <see cref="Double" /> value is an integer (a number in the series { ..., -2, -1,
        /// 0, 1, 2, ... }).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is an integer, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsInteger(this Double target) => Math.Abs(target % 1d) <= Double.Epsilon;

        /// <summary>
        /// Determines whether or not the current <see cref="Double" /> value is a rational number (not an integer).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is a rational number, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsRational(this Double target) => target.IsInteger() is false;

        /// <summary>
        /// Rounds the current <see cref="Double" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Double" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Double" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double RoundedTo(this Double target, Int32 digits) => Math.Round(target, digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Double" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Double" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Double" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double RoundedTo(this Double target, Int32 digits, MidpointRounding midpointRoundingMode) => Math.Round(target, digits, midpointRoundingMode);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigInteger" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="BigInteger" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ToBigInteger(this Double target) => new(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigRational" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigRational ToBigRational(this Double target) => target.IsInteger() ? new(target.ToBigInteger()) : BigRational.Approximate(target, RationalTolerance);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this Double target) => Convert.ToByte(target);

        /// <summary>
        /// Converts the current <see cref="Double" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Double" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte[] ToByteArray(this Double target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal(this Double target) => Convert.ToDecimal(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Double target) => Convert.ToInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Double target) => Convert.ToInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Double target) => Convert.ToInt64(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// An <see cref="SByte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this Double target) => Convert.ToSByte(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="Single" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="Single" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle(this Double target) => Convert.ToSingle(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Double target) => Convert.ToUInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Double target) => Convert.ToUInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Double target) => Convert.ToUInt64(target);

        /// <summary>
        /// Converts the specified <see cref="Double" /> to its constituent elements.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Double" />.
        /// </param>
        /// <returns>
        /// The constituent elements of <paramref name="target" />.
        /// </returns>
        [DebuggerHidden]
        internal static FloatingPointNumberComponents ToComponents(this Double target) => new(target);

        /// <summary>
        /// Represents the maximum permissible tolerance for rational approximation during numeric conversion.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Double RationalTolerance = 0.0000001d;
    }
}