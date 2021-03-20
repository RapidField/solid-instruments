﻿// =================================================================================================================================
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
    /// Extends the <see cref="SByte" /> structure with general purpose features.
    /// </summary>
    public static class SByteExtensions
    {
        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="SByte" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="SByte" />.
        /// </returns>
        public static Int32 CountSignificantFigures(this SByte target) => target == 0 ? 0 : target.ToString(GeneralNumericStringFormatSpecifier, CultureInfo.InvariantCulture).TrimStart(NegativeSignCharacter).Trim(ZeroCharacter).Length;

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigInteger" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger ToBigInteger(this SByte target) => new(target.ToInt32());

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="BigRational" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigRational ToBigRational(this SByte target) => new(target.ToBigInteger());

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// An <see cref="Byte" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Byte ToByte(this SByte target) => Convert.ToByte(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="Decimal" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal(this SByte target) => Convert.ToDecimal(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Double" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="Double" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this SByte target) => Convert.ToDouble(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this SByte target) => Convert.ToInt16(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this SByte target) => Convert.ToInt32(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// An <see cref="Int64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this SByte target) => Convert.ToInt64(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="Single" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="Single" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle(this SByte target) => Convert.ToSingle(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt16" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this SByte target) => Convert.ToUInt16(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt32" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this SByte target) => Convert.ToUInt32(target);

        /// <summary>
        /// Converts the specified <see cref="SByte" /> to an equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="SByte" />.
        /// </param>
        /// <returns>
        /// A <see cref="UInt64" /> value that is equivalent to <paramref name="target" />.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of <paramref name="target" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this SByte target) => Convert.ToUInt64(target);

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