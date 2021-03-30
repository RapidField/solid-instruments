// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Numerics;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number : IComparable<Number>, IEquatable<Number>
    {
        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is a fractional number (not an integer).
        /// </summary>
        public readonly Boolean IsFractional => IsInteger is false;

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is an integer (a number in the series {
        /// ..., -2, -1, 0, 1, 2, ... }).
        /// </summary>
        public readonly Boolean IsInteger => Format switch
        {
            NumericDataFormat.Byte => true,
            NumericDataFormat.SByte => true,
            NumericDataFormat.UInt16 => true,
            NumericDataFormat.Int16 => true,
            NumericDataFormat.UInt32 => true,
            NumericDataFormat.Int32 => true,
            NumericDataFormat.UInt64 => true,
            NumericDataFormat.Int64 => true,
            NumericDataFormat.Single => ToSingle().IsInteger(),
            NumericDataFormat.Double => ToDouble().IsInteger(),
            NumericDataFormat.Decimal => ToDecimal().IsInteger(),
            NumericDataFormat.BigInteger => true,
            NumericDataFormat.BigRational => ToBigRational().IsInteger(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is less than zero.
        /// </summary>
        public readonly Boolean IsNegative => Format switch
        {
            NumericDataFormat.Byte => false,
            NumericDataFormat.SByte => ToSByte() < 0,
            NumericDataFormat.UInt16 => false,
            NumericDataFormat.Int16 => ToInt16() < 0,
            NumericDataFormat.UInt32 => false,
            NumericDataFormat.Int32 => ToInt32() < 0,
            NumericDataFormat.UInt64 => false,
            NumericDataFormat.Int64 => ToInt64() < 0,
            NumericDataFormat.Single => ToSingle() < 0f,
            NumericDataFormat.Double => ToDouble() < 0d,
            NumericDataFormat.Decimal => ToDecimal() < 0m,
            NumericDataFormat.BigInteger => ToBigInteger() < BigInteger.Zero,
            NumericDataFormat.BigRational => ToBigRational() < BigRational.Zero,
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is greater than zero.
        /// </summary>
        public readonly Boolean IsPositive => Format switch
        {
            NumericDataFormat.Byte => ToByte() > 0,
            NumericDataFormat.SByte => ToSByte() > 0,
            NumericDataFormat.UInt16 => ToUInt16() > 0,
            NumericDataFormat.Int16 => ToInt16() > 0,
            NumericDataFormat.UInt32 => ToUInt32() > 0,
            NumericDataFormat.Int32 => ToInt32() > 0,
            NumericDataFormat.UInt64 => ToUInt64() > 0,
            NumericDataFormat.Int64 => ToInt64() > 0,
            NumericDataFormat.Single => ToSingle() > 0f,
            NumericDataFormat.Double => ToDouble() > 0d,
            NumericDataFormat.Decimal => ToDecimal() > 0m,
            NumericDataFormat.BigInteger => ToBigInteger() > BigInteger.Zero,
            NumericDataFormat.BigRational => ToBigRational() > BigRational.Zero,
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets an exception message that indicates that <see cref="Format" /> is an unsupported numeric data format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly String UnsupportedFormatExceptionMessage => $"The specified numeric data format, {Format}, is not supported.";
    }
}