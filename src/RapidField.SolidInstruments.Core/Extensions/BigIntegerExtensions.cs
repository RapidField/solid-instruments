// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="BigInteger" /> structure with general purpose features.
    /// </summary>
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="BigInteger" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="BigInteger" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this BigInteger target) => target.IsZero ? 0 : target.ToString(GeneralNumericStringFormatSpecifier, CultureInfo.InvariantCulture).TrimStart(NegativeSignCharacter).Trim(ZeroCharacter).Length;

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="BigRational" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigRational ToBigRational(this BigInteger target) => new(target);

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this BigInteger target) => target.ToInt64().ToByte();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Decimal" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal(this BigInteger target) => target.ToInt64().ToDecimal();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Double" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="Double" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this BigInteger target) => target.ToInt64().ToDouble();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this BigInteger target) => target.ToInt64().ToInt16();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this BigInteger target) => target.ToInt64().ToInt32();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="Int64" />.
        /// </exception>
        public static Int64 ToInt64(this BigInteger target)
        {
            try
            {
                return (Int64)target;
            }
            catch (OverflowException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new OverflowException($"The value {target} is outside of the allowable range of values for a 64-bit integer.", exception);
            }
        }

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Number" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="Number" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        public static Number ToNumber(this BigInteger target) => target;

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// An <see cref="SByte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SByte ToSByte(this BigInteger target) => target.ToInt64().ToSByte();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="Single" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="Single" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="Single" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle(this BigInteger target) => target.ToInt64().ToSingle();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this BigInteger target) => target.ToInt64().ToUInt16();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this BigInteger target) => target.ToInt64().ToUInt32();

        /// <summary>
        /// Converts the specified <see cref="BigInteger" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="BigInteger" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this BigInteger target) => target.ToInt64().ToUInt64();

        /// <summary>
        /// Represents the textual format specifier for a general numeric string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GeneralNumericStringFormatSpecifier = "G";

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