// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Single" /> structure with general purpose features.
    /// </summary>
    public static class SingleExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="Single" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="Single" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this Single target) => target == 0f ? 0 : target.ToComponents().SignificantDigits.Length;

        /// <summary>
        /// Returns the significant digits for the current <see cref="Single" /> expressed as a whole number.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// The significand, or mantissa, for the current <see cref="Single" />.
        /// </returns>
        public static Single GetSignificand(this Single target) => target == 0f ? 0f : Single.Parse(target.ToComponents().Significand);

        /// <summary>
        /// Determines whether or not the current <see cref="Single" /> value is a fractional number (not an integer).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is a fractional number, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsFractional(this Single target) => target.IsInteger() is false;

        /// <summary>
        /// Determines whether or not the current <see cref="Single" /> value is an integer (a number in the series { ..., -2, -1,
        /// 0, 1, 2, ... }).
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="target" /> is an integer, otherwise <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsInteger(this Single target) => target.ToDouble().IsInteger();

        /// <summary>
        /// Rounds the current <see cref="Single" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Single" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Single" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than seven.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Single" />.
        /// </exception>
        public static Single RoundedTo(this Single target, Int32 digits) => target.RoundedTo(digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Single" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Single" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Single" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 7.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Single" />.
        /// </exception>
        public static Single RoundedTo(this Single target, Int32 digits, MidpointRounding midpointRoundingMode) => Convert.ToSingle(Math.Round(target, digits.RejectIf().IsGreaterThan(7, nameof(digits)), midpointRoundingMode));

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigInteger" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="BigInteger" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ToBigInteger(this Single target) => new(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigRational" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigRational ToBigRational(this Single target) => target.IsInteger() ? new(target.ToBigInteger()) : BigRational.Approximate(target, RationalTolerance);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this Single target) => Convert.ToByte(target);

        /// <summary>
        /// Converts the current <see cref="Single" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Single" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte[] ToByteArray(this Single target) => BitConverter.GetBytes(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Decimal" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal(this Single target) => Convert.ToDecimal(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Double" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="Double" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this Single target) => Convert.ToDouble(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Single target) => Convert.ToInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Single target) => Convert.ToInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Single target) => Convert.ToInt64(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="Number" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An <see cref="Number" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        public static Number ToNumber(this Single target) => target;

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// An <see cref="SByte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this Single target) => Convert.ToSByte(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Single target) => Convert.ToUInt16(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Single target) => Convert.ToUInt32(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Single target) => Convert.ToUInt64(target);

        /// <summary>
        /// Converts the specified <see cref="Single" /> to its constituent elements.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Single" />.
        /// </param>
        /// <returns>
        /// The constituent elements of <paramref name="target" />.
        /// </returns>
        [DebuggerHidden]
        internal static FloatingPointNumberComponents ToComponents(this Single target) => new(target);

        /// <summary>
        /// Represents the maximum permissible tolerance for rational approximation during numeric conversion.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Single RationalTolerance = Single.Epsilon * 144f;
    }
}