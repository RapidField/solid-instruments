// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Pack = 1)]
    public readonly partial struct Number : IComparable<Number>, IEquatable<Number>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Byte value)
            : this(new[] { value }, NumericDataFormat.Byte)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(SByte value)
             : this((Byte[])(Array)(new[] { value }), NumericDataFormat.SByte)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(UInt16 value)
            : this(value.ToByteArray(), NumericDataFormat.UInt16)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Int16 value)
            : this(value.ToByteArray(), NumericDataFormat.Int16)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(UInt32 value)
            : this(value.ToByteArray(), NumericDataFormat.UInt32)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Int32 value)
            : this(value.ToByteArray(), NumericDataFormat.Int32)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(UInt64 value)
            : this(value.ToByteArray(), NumericDataFormat.UInt64)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Int64 value)
            : this(value.ToByteArray(), NumericDataFormat.Int64)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Single value)
            : this(value.ToByteArray(), NumericDataFormat.Single)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Double value)
            : this(value.ToByteArray(), NumericDataFormat.Double)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(Decimal value)
            : this(value.ToByteArray(), NumericDataFormat.Decimal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(BigInteger value)
            : this(value.ToByteArray(), NumericDataFormat.BigInteger)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// The value of the number.
        /// </param>
        [DebuggerHidden]
        private Number(BigRational value)
            : this(value.ToByteArray(), NumericDataFormat.BigRational)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number" /> structure.
        /// </summary>
        /// <param name="value">
        /// An array of bytes representing the number.
        /// </param>
        /// <param name="format">
        /// The numeric type represented by <paramref name="value" />.
        /// </param>
        [DebuggerHidden]
        private Number(Byte[] value, NumericDataFormat format)
        {
            Format = format;
            Value = new(value);
        }

        /// <summary>
        /// Returns the number of significant figures in the value of the current <see cref="Number" />.
        /// </summary>
        /// <returns>
        /// The number of significant figures in the value of the current <see cref="Number" />.
        /// </returns>
        public Int32 CountSignificantFigures() => Format switch
        {
            NumericDataFormat.Byte => ToByte().CountSignificantFigures(),
            NumericDataFormat.SByte => ToSByte().CountSignificantFigures(),
            NumericDataFormat.UInt16 => ToUInt16().CountSignificantFigures(),
            NumericDataFormat.Int16 => ToInt16().CountSignificantFigures(),
            NumericDataFormat.UInt32 => ToUInt32().CountSignificantFigures(),
            NumericDataFormat.Int32 => ToInt32().CountSignificantFigures(),
            NumericDataFormat.UInt64 => ToUInt64().CountSignificantFigures(),
            NumericDataFormat.Int64 => ToInt64().CountSignificantFigures(),
            NumericDataFormat.Single => ToSingle().CountSignificantFigures(),
            NumericDataFormat.Double => ToDouble().CountSignificantFigures(),
            NumericDataFormat.Decimal => ToDecimal().CountSignificantFigures(),
            NumericDataFormat.BigInteger => ToBigInteger().CountSignificantFigures(),
            NumericDataFormat.BigRational => ToBigRational().CountSignificantFigures(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is an integer (a number in the series {
        /// ..., -2, -1, 0, 1, 2, ... }).
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Number" /> is an integer, otherwise <see langword="false" />.
        /// </returns>
        public Boolean IsInteger => Format switch
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
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Number" /> is less than zero, otherwise <see langword="false" />.
        /// </returns>
        public Boolean IsNegative => Format switch
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
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Number" /> is greater than zero, otherwise <see langword="false" />.
        /// </returns>
        public Boolean IsPositive => Format switch
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
        /// Gets a value indicating whether or not the current <see cref="Number" /> value is a rational number (not an integer).
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Number" /> is a rational number, otherwise <see langword="false" />.
        /// </returns>
        public Boolean IsRational => IsInteger is false;

        /// <summary>
        /// Rounds the current <see cref="Number" /> to the specified number of fractional digits.
        /// </summary>
        /// <remarks>
        /// This method rounds away from zero by default.
        /// </remarks>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Number" /> to.
        /// </param>
        /// <returns>
        /// The current <see cref="Number" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 15.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Number" />.
        /// </exception>
        public Number RoundedTo(Int32 digits) => RoundedTo(digits, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Rounds the current <see cref="Number" /> to the specified number of fractional digits.
        /// </summary>
        /// <param name="digits">
        /// A number of fractional digits to round the current <see cref="Number" /> to.
        /// </param>
        /// <param name="midpointRoundingMode">
        /// A specification for how to round if the current value is midway between two numbers.
        /// </param>
        /// <returns>
        /// The current <see cref="Number" /> rounded to the specified number or digits.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="midpointRoundingMode" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits" /> is less than zero or greater than 28.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The result is outside the range of a <see cref="Number" />.
        /// </exception>
        public Number RoundedTo(Int32 digits, MidpointRounding midpointRoundingMode) => Format switch
        {
            NumericDataFormat.Byte => this,
            NumericDataFormat.SByte => this,
            NumericDataFormat.UInt16 => Compress(),
            NumericDataFormat.Int16 => Compress(),
            NumericDataFormat.UInt32 => Compress(),
            NumericDataFormat.Int32 => Compress(),
            NumericDataFormat.UInt64 => Compress(),
            NumericDataFormat.Int64 => Compress(),
            NumericDataFormat.Single => IsInteger ? Compress() : new Number(ToDecimal().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.Double => IsInteger ? Compress() : new Number(ToDecimal().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.Decimal => IsInteger ? Compress() : new Number(ToDecimal().RoundedTo(digits, midpointRoundingMode)).Compress(),
            NumericDataFormat.BigInteger => Compress(),
            NumericDataFormat.BigRational => IsInteger ? Compress() : new Number(ToBigRational().RoundedTo(digits, midpointRoundingMode)).Compress(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Gets an exception message that indicates that <see cref="Format" /> is an unsupported numeric data format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String UnsupportedFormatExceptionMessage => $"The specified numeric data format, {Format}, is not supported.";

        /// <summary>
        /// Represents the numeric type represented by the bytes constituting <see cref="Value" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        internal readonly NumericDataFormat Format;

        /// <summary>
        /// Represents the bytes constituting the value of the current <see cref="Number" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(1)]
        private readonly Memory<Byte> Value;
    }
}