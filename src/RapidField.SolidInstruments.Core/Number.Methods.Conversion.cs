﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number
    {
        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a numeric value to its <see cref="Number" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a numeric value to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Number" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a numeric value.
        /// </exception>
        public static Number Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return value;
            }

            return new(0);
        }

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the rational approximation of the specified number.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(Single number) => new(number.ToBigRational());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the rational approximation of the specified number.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(Double number) => new(number.ToBigRational());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the rational approximation of the specified number.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(Decimal number) => new(number.ToBigRational());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified integer rationally.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(Int64 number) => RationalRepresentationOf(number.ToBigInteger());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified integer rationally.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(UInt64 number) => RationalRepresentationOf(number.ToBigInteger());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified integer rationally.
        /// </summary>
        /// <param name="number">
        /// The number to rationally approximate.
        /// </param>
        /// <returns>
        /// The rational approximation of <paramref name="number" />.
        /// </returns>
        public static Number RationalRepresentationOf(BigInteger number) => RationalRepresentationOf(number, BigInteger.One);

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified rational number.
        /// </summary>
        /// <param name="numerator">
        /// The integral numerator.
        /// </param>
        /// <param name="denominator">
        /// The integral denominator.
        /// </param>
        /// <returns>
        /// The specified rational number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="denominator" /> is equal to zero (0).
        /// </exception>
        public static Number RationalRepresentationOf(Int64 numerator, Int64 denominator) => RationalRepresentationOf(numerator.ToBigInteger(), denominator.ToBigInteger());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified rational number.
        /// </summary>
        /// <param name="numerator">
        /// The integral numerator.
        /// </param>
        /// <param name="denominator">
        /// The integral denominator.
        /// </param>
        /// <returns>
        /// The specified rational number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="denominator" /> is equal to zero (0).
        /// </exception>
        public static Number RationalRepresentationOf(UInt64 numerator, UInt64 denominator) => RationalRepresentationOf(numerator.ToBigInteger(), denominator.ToBigInteger());

        /// <summary>
        /// Initializes a new <see cref="Number" /> which represents the specified rational number.
        /// </summary>
        /// <param name="numerator">
        /// The integral numerator.
        /// </param>
        /// <param name="denominator">
        /// The integral denominator.
        /// </param>
        /// <returns>
        /// The specified rational number.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="denominator" /> is equal to zero (0).
        /// </exception>
        public static Number RationalRepresentationOf(BigInteger numerator, BigInteger denominator) => new(new BigRational(numerator, denominator.RejectIf().IsEqualToValue(BigInteger.Zero, nameof(denominator))));

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a numeric value to its <see cref="Number" /> equivalent.
        /// The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a numeric value to convert.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out Number result)
        {
            if (Parse(input, out var value, false))
            {
                result = value;
                return true;
            }

            result = new(0);
            return false;
        }

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="BigInteger" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="BigInteger" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a
        /// <see cref="BigInteger" />.
        /// </exception>
        public readonly BigInteger ToBigInteger() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToBigInteger(),
            NumericDataFormat.SByte => ToSByte().ToBigInteger(),
            NumericDataFormat.UInt16 => ToUInt16().ToBigInteger(),
            NumericDataFormat.Int16 => ToInt16().ToBigInteger(),
            NumericDataFormat.UInt32 => ToUInt32().ToBigInteger(),
            NumericDataFormat.Int32 => ToInt32().ToBigInteger(),
            NumericDataFormat.UInt64 => ToUInt64().ToBigInteger(),
            NumericDataFormat.Int64 => ToInt64().ToBigInteger(),
            NumericDataFormat.Single => IsInteger ? ToSingle().ToBigInteger() : throw new OverflowException(OverflowExceptionMessage),
            NumericDataFormat.Double => IsInteger ? ToDouble().ToBigInteger() : throw new OverflowException(OverflowExceptionMessage),
            NumericDataFormat.Decimal => IsInteger ? ToDecimal().ToBigInteger() : throw new OverflowException(OverflowExceptionMessage),
            NumericDataFormat.BigInteger => new BigInteger(Value.Span),
            NumericDataFormat.BigRational => IsInteger ? ToBigRational().ToBigInteger() : throw new OverflowException(OverflowExceptionMessage),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="BigRational" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="BigRational" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a
        /// <see cref="BigRational" />.
        /// </exception>
        public readonly BigRational ToBigRational() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToBigRational(),
            NumericDataFormat.SByte => ToSByte().ToBigRational(),
            NumericDataFormat.UInt16 => ToUInt16().ToBigRational(),
            NumericDataFormat.Int16 => ToInt16().ToBigRational(),
            NumericDataFormat.UInt32 => ToUInt32().ToBigRational(),
            NumericDataFormat.Int32 => ToInt32().ToBigRational(),
            NumericDataFormat.UInt64 => ToUInt64().ToBigRational(),
            NumericDataFormat.Int64 => ToInt64().ToBigRational(),
            NumericDataFormat.Single => ToSingle().ToBigRational(),
            NumericDataFormat.Double => ToDouble().ToBigRational(),
            NumericDataFormat.Decimal => ToDecimal().ToBigRational(),
            NumericDataFormat.BigInteger => ToBigInteger().ToBigRational(),
            NumericDataFormat.BigRational => BigRationalFromByteArray(Value.ToArray()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Byte" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Byte" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Byte" />.
        /// </exception>
        public readonly Byte ToByte() => Format switch
        {
            NumericDataFormat.Byte => Value.Span[0],
            NumericDataFormat.SByte => ToSByte().ToByte(),
            NumericDataFormat.UInt16 => ToUInt16().ToByte(),
            NumericDataFormat.Int16 => ToInt16().ToByte(),
            NumericDataFormat.UInt32 => ToUInt32().ToByte(),
            NumericDataFormat.Int32 => ToInt32().ToByte(),
            NumericDataFormat.UInt64 => ToUInt64().ToByte(),
            NumericDataFormat.Int64 => ToInt64().ToByte(),
            NumericDataFormat.Single => ToSingle().ToByte(),
            NumericDataFormat.Double => ToDouble().ToByte(),
            NumericDataFormat.Decimal => ToDecimal().ToByte(),
            NumericDataFormat.BigInteger => ToBigInteger().ToByte(),
            NumericDataFormat.BigRational => ToBigRational().ToByte(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="Number" />.
        /// </returns>
        public readonly Byte[] ToByteArray()
        {
            var formatLengthInBytes = sizeof(NumericDataFormat);
            var valueLengthInBytes = Value.Length;
            var byteArray = new Byte[formatLengthInBytes + valueLengthInBytes];
            byteArray[0] = (Byte)Format;
            Buffer.BlockCopy(Value.ToArray(), 0, byteArray, formatLengthInBytes, valueLengthInBytes);
            return byteArray;
        }

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Decimal" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Decimal" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Decimal" />.
        /// </exception>
        public readonly Decimal ToDecimal() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToDecimal(),
            NumericDataFormat.SByte => ToSByte().ToDecimal(),
            NumericDataFormat.UInt16 => ToUInt16().ToDecimal(),
            NumericDataFormat.Int16 => ToInt16().ToDecimal(),
            NumericDataFormat.UInt32 => ToUInt32().ToDecimal(),
            NumericDataFormat.Int32 => ToInt32().ToDecimal(),
            NumericDataFormat.UInt64 => ToUInt64().ToDecimal(),
            NumericDataFormat.Int64 => ToInt64().ToDecimal(),
            NumericDataFormat.Single => ToSingle().ToDecimal(),
            NumericDataFormat.Double => ToDouble().ToDecimal(),
            NumericDataFormat.Decimal => new Decimal(new[] { BitConverter.ToInt32(Value.Slice(0, 4).Span), BitConverter.ToInt32(Value.Slice(4, 4).Span), BitConverter.ToInt32(Value.Slice(8, 4).Span), BitConverter.ToInt32(Value.Slice(12, 4).Span) }),
            NumericDataFormat.BigInteger => ToBigInteger().ToDecimal(),
            NumericDataFormat.BigRational => ToBigRational().ToDecimal(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Double" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Double" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Double" />.
        /// </exception>
        public readonly Double ToDouble() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToDouble(),
            NumericDataFormat.SByte => ToSByte().ToDouble(),
            NumericDataFormat.UInt16 => ToUInt16().ToDouble(),
            NumericDataFormat.Int16 => ToInt16().ToDouble(),
            NumericDataFormat.UInt32 => ToUInt32().ToDouble(),
            NumericDataFormat.Int32 => ToInt32().ToDouble(),
            NumericDataFormat.UInt64 => ToUInt64().ToDouble(),
            NumericDataFormat.Int64 => ToInt64().ToDouble(),
            NumericDataFormat.Single => ToSingle().ToDouble(),
            NumericDataFormat.Double => BitConverter.ToDouble(Value.Span),
            NumericDataFormat.Decimal => ToDecimal().RoundedTo(SignificantFiguresLimitForDouble).ToDouble(),
            NumericDataFormat.BigInteger => ToBigInteger().ToDouble(),
            NumericDataFormat.BigRational => ToBigRational().RoundedTo(SignificantFiguresLimitForDouble).ToDouble(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Int16" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Int16" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Int16" />.
        /// </exception>
        public readonly Int16 ToInt16() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToInt16(),
            NumericDataFormat.SByte => ToSByte().ToInt16(),
            NumericDataFormat.UInt16 => ToUInt16().ToInt16(),
            NumericDataFormat.Int16 => BitConverter.ToInt16(Value.Span),
            NumericDataFormat.UInt32 => ToUInt32().ToInt16(),
            NumericDataFormat.Int32 => ToInt32().ToInt16(),
            NumericDataFormat.UInt64 => ToUInt64().ToInt16(),
            NumericDataFormat.Int64 => ToInt64().ToInt16(),
            NumericDataFormat.Single => ToSingle().ToInt16(),
            NumericDataFormat.Double => ToDouble().ToInt16(),
            NumericDataFormat.Decimal => ToDecimal().ToInt16(),
            NumericDataFormat.BigInteger => ToBigInteger().ToInt16(),
            NumericDataFormat.BigRational => ToBigRational().ToInt16(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Int32" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Int32" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Int32" />.
        /// </exception>
        public readonly Int32 ToInt32() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToInt32(),
            NumericDataFormat.SByte => ToSByte().ToInt32(),
            NumericDataFormat.UInt16 => ToUInt16().ToInt32(),
            NumericDataFormat.Int16 => ToInt16().ToInt32(),
            NumericDataFormat.UInt32 => ToUInt32().ToInt32(),
            NumericDataFormat.Int32 => BitConverter.ToInt32(Value.Span),
            NumericDataFormat.UInt64 => ToUInt64().ToInt32(),
            NumericDataFormat.Int64 => ToInt64().ToInt32(),
            NumericDataFormat.Single => ToSingle().ToInt32(),
            NumericDataFormat.Double => ToDouble().ToInt32(),
            NumericDataFormat.Decimal => ToDecimal().ToInt32(),
            NumericDataFormat.BigInteger => ToBigInteger().ToInt32(),
            NumericDataFormat.BigRational => ToBigRational().ToInt32(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Int64" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Int64" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Int64" />.
        /// </exception>
        public readonly Int64 ToInt64() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToInt64(),
            NumericDataFormat.SByte => ToSByte().ToInt64(),
            NumericDataFormat.UInt16 => ToUInt16().ToInt64(),
            NumericDataFormat.Int16 => ToInt16().ToInt64(),
            NumericDataFormat.UInt32 => ToUInt32().ToInt64(),
            NumericDataFormat.Int32 => ToInt32().ToInt64(),
            NumericDataFormat.UInt64 => ToUInt64().ToInt64(),
            NumericDataFormat.Int64 => BitConverter.ToInt64(Value.Span),
            NumericDataFormat.Single => ToSingle().ToInt64(),
            NumericDataFormat.Double => ToDouble().ToInt64(),
            NumericDataFormat.Decimal => ToDecimal().ToInt64(),
            NumericDataFormat.BigInteger => ToBigInteger().ToInt64(),
            NumericDataFormat.BigRational => ToBigRational().ToInt64(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent rational representation.
        /// </summary>
        /// <returns>
        /// The resulting rational <see cref="Number" />.
        /// </returns>
        public readonly Number ToRationalNumber() => Format switch
        {
            NumericDataFormat.Byte => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.SByte => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.UInt16 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.Int16 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.UInt32 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.Int32 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.UInt64 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.Int64 => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.Single => RationalRepresentationOf(ToSingle()),
            NumericDataFormat.Double => RationalRepresentationOf(ToDouble()),
            NumericDataFormat.Decimal => RationalRepresentationOf(ToDecimal()),
            NumericDataFormat.BigInteger => RationalRepresentationOf(ToBigInteger()),
            NumericDataFormat.BigRational => this,
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="SByte" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="SByte" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for an <see cref="SByte" />.
        /// </exception>
        public readonly SByte ToSByte() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToSByte(),
            NumericDataFormat.SByte => unchecked((SByte)Value.Span[0]),
            NumericDataFormat.UInt16 => ToUInt16().ToSByte(),
            NumericDataFormat.Int16 => ToInt16().ToSByte(),
            NumericDataFormat.UInt32 => ToUInt32().ToSByte(),
            NumericDataFormat.Int32 => ToInt32().ToSByte(),
            NumericDataFormat.UInt64 => ToUInt64().ToSByte(),
            NumericDataFormat.Int64 => ToInt64().ToSByte(),
            NumericDataFormat.Single => ToSingle().ToSByte(),
            NumericDataFormat.Double => ToDouble().ToSByte(),
            NumericDataFormat.Decimal => ToDecimal().ToSByte(),
            NumericDataFormat.BigInteger => ToBigInteger().ToSByte(),
            NumericDataFormat.BigRational => ToBigRational().ToSByte(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="Single" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="Single" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="Single" />.
        /// </exception>
        public readonly Single ToSingle() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToSingle(),
            NumericDataFormat.SByte => ToSByte().ToSingle(),
            NumericDataFormat.UInt16 => ToUInt16().ToSingle(),
            NumericDataFormat.Int16 => ToInt16().ToSingle(),
            NumericDataFormat.UInt32 => ToUInt32().ToSingle(),
            NumericDataFormat.Int32 => ToInt32().ToSingle(),
            NumericDataFormat.UInt64 => ToUInt64().ToSingle(),
            NumericDataFormat.Int64 => ToInt64().ToSingle(),
            NumericDataFormat.Single => BitConverter.ToSingle(Value.Span),
            NumericDataFormat.Double => ToDouble().RoundedTo(SignificantFiguresLimitForSingle).ToSingle(),
            NumericDataFormat.Decimal => ToDecimal().RoundedTo(SignificantFiguresLimitForSingle).ToSingle(),
            NumericDataFormat.BigInteger => ToBigInteger().ToSingle(),
            NumericDataFormat.BigRational => ToBigRational().RoundedTo(SignificantFiguresLimitForSingle).ToSingle(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the value of the current <see cref="Number" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Number" />.
        /// </returns>
        public readonly override String ToString() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToString(),
            NumericDataFormat.SByte => ToSByte().ToString(),
            NumericDataFormat.UInt16 => ToUInt16().ToString(),
            NumericDataFormat.Int16 => ToInt16().ToString(),
            NumericDataFormat.UInt32 => ToUInt32().ToString(),
            NumericDataFormat.Int32 => ToInt32().ToString(),
            NumericDataFormat.UInt64 => ToUInt64().ToString(),
            NumericDataFormat.Int64 => ToInt64().ToString(),
            NumericDataFormat.Single => ToSingle().ToString(),
            NumericDataFormat.Double => ToDouble().ToString(),
            NumericDataFormat.Decimal => ToDecimal().ToString(),
            NumericDataFormat.BigInteger => ToBigInteger().ToString(),
            NumericDataFormat.BigRational => ToBigRational().ToString(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="UInt16" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="UInt16" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="UInt16" />.
        /// </exception>
        public readonly UInt16 ToUInt16() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToUInt16(),
            NumericDataFormat.SByte => ToSByte().ToUInt16(),
            NumericDataFormat.UInt16 => BitConverter.ToUInt16(Value.Span),
            NumericDataFormat.Int16 => ToInt16().ToUInt16(),
            NumericDataFormat.UInt32 => ToUInt32().ToUInt16(),
            NumericDataFormat.Int32 => ToInt32().ToUInt16(),
            NumericDataFormat.UInt64 => ToUInt64().ToUInt16(),
            NumericDataFormat.Int64 => ToInt64().ToUInt16(),
            NumericDataFormat.Single => ToSingle().ToUInt16(),
            NumericDataFormat.Double => ToDouble().ToUInt16(),
            NumericDataFormat.Decimal => ToDecimal().ToUInt16(),
            NumericDataFormat.BigInteger => ToBigInteger().ToUInt16(),
            NumericDataFormat.BigRational => ToBigRational().ToUInt16(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="UInt32" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="UInt32" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="UInt32" />.
        /// </exception>
        public readonly UInt32 ToUInt32() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToUInt32(),
            NumericDataFormat.SByte => ToSByte().ToUInt32(),
            NumericDataFormat.UInt16 => ToUInt16().ToUInt32(),
            NumericDataFormat.Int16 => ToInt16().ToUInt32(),
            NumericDataFormat.UInt32 => BitConverter.ToUInt32(Value.Span),
            NumericDataFormat.Int32 => ToInt32().ToUInt32(),
            NumericDataFormat.UInt64 => ToUInt64().ToUInt32(),
            NumericDataFormat.Int64 => ToInt64().ToUInt32(),
            NumericDataFormat.Single => ToSingle().ToUInt32(),
            NumericDataFormat.Double => ToDouble().ToUInt32(),
            NumericDataFormat.Decimal => ToDecimal().ToUInt32(),
            NumericDataFormat.BigInteger => ToBigInteger().ToUInt32(),
            NumericDataFormat.BigRational => ToBigRational().ToUInt32(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the current <see cref="Number" /> to its equivalent <see cref="UInt64" /> value.
        /// </summary>
        /// <returns>
        /// The resulting <see cref="UInt64" /> value.
        /// </returns>
        /// <exception cref="OverflowException">
        /// The value of the current <see cref="Number" /> is outside of the allowable range of values for a <see cref="UInt64" />.
        /// </exception>
        public readonly UInt64 ToUInt64() => Format switch
        {
            NumericDataFormat.Byte => ToByte().ToUInt64(),
            NumericDataFormat.SByte => ToSByte().ToUInt64(),
            NumericDataFormat.UInt16 => ToUInt16().ToUInt64(),
            NumericDataFormat.Int16 => ToInt16().ToUInt64(),
            NumericDataFormat.UInt32 => ToUInt32().ToUInt64(),
            NumericDataFormat.Int32 => ToInt32().ToUInt64(),
            NumericDataFormat.UInt64 => BitConverter.ToUInt64(Value.Span),
            NumericDataFormat.Int64 => ToInt64().ToUInt64(),
            NumericDataFormat.Single => ToSingle().ToUInt64(),
            NumericDataFormat.Double => ToDouble().ToUInt64(),
            NumericDataFormat.Decimal => ToDecimal().ToUInt64(),
            NumericDataFormat.BigInteger => ToBigInteger().ToUInt64(),
            NumericDataFormat.BigRational => ToBigRational().ToUInt64(),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Converts the specified array of bytes to a <see cref="BigRational" />.
        /// </summary>
        /// <param name="value">
        /// The array of bytes to convert.
        /// </param>
        /// <returns>
        /// The resulting <see cref="BigRational" />.
        /// </returns>
        [DebuggerHidden]
        internal static BigRational BigRationalFromByteArray(Byte[] value) => BigRational.Parse(BigRationalFormatEncoding.GetString(value));

        /// <summary>
        /// Converts the specified <see cref="BigRational" /> to an array of bytes.
        /// </summary>
        /// <param name="value">
        /// The <see cref="BigRational" /> to convert.
        /// </param>
        /// <returns>
        /// An array of bytes representing <paramref name="value" />.
        /// </returns>
        [DebuggerHidden]
        internal static Byte[] BigRationalToByteArray(BigRational value) => BigRationalFormatEncoding.GetBytes(value.ToString());

        /// <summary>
        /// Attempts to reduce the memory footprint of the current <see cref="Number" /> by converting its underlying value to a
        /// smaller data format, if possible.
        /// </summary>
        /// <returns>
        /// The resulting numeric value.
        /// </returns>
        [DebuggerHidden]
        internal readonly Number Compress() => Format switch
        {
            NumericDataFormat.Byte => this,
            NumericDataFormat.SByte => this,
            NumericDataFormat.UInt16 => Compress(1),
            NumericDataFormat.Int16 => Compress(1),
            NumericDataFormat.UInt32 => Compress(1),
            NumericDataFormat.Int32 => Compress(1),
            NumericDataFormat.UInt64 => Compress(2),
            NumericDataFormat.Int64 => Compress(2),
            NumericDataFormat.Single => Compress(2),
            NumericDataFormat.Double => Compress(2),
            NumericDataFormat.Decimal => Compress(3),
            NumericDataFormat.BigInteger => Compress(3),
            NumericDataFormat.BigRational => Compress(4),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Returns the largest possible value for the specified numeric data format.
        /// </summary>
        /// <param name="format">
        /// The numeric data format to evaluate.
        /// </param>
        /// <returns>
        /// The largest possible value for the specified numeric data format.
        /// </returns>
        [DebuggerHidden]
        private static Number MaxValueOf(NumericDataFormat format) => format switch
        {
            NumericDataFormat.Byte => MaxValueOfByte,
            NumericDataFormat.SByte => MaxValueOfSByte,
            NumericDataFormat.UInt16 => MaxValueOfUInt16,
            NumericDataFormat.Int16 => MaxValueOfInt16,
            NumericDataFormat.UInt32 => MaxValueOfUInt32,
            NumericDataFormat.Int32 => MaxValueOfInt32,
            NumericDataFormat.UInt64 => MaxValueOfUInt64,
            NumericDataFormat.Int64 => MaxValueOfInt64,
            NumericDataFormat.Single => MaxValueOfSingle,
            NumericDataFormat.Double => MaxValueOfDouble,
            NumericDataFormat.Decimal => MaxValueOfDecimal,
            _ => throw new UnsupportedSpecificationException($"The maximum value of the numeric data format, {format}, is undefined.")
        };

        /// <summary>
        /// Returns the smallest possible value for the specified numeric data format.
        /// </summary>
        /// <param name="format">
        /// The numeric data format to evaluate.
        /// </param>
        /// <returns>
        /// The smallest possible value for the specified numeric data format.
        /// </returns>
        [DebuggerHidden]
        private static Number MinValueOf(NumericDataFormat format) => format switch
        {
            NumericDataFormat.Byte => MinValueOfByte,
            NumericDataFormat.SByte => MinValueOfSByte,
            NumericDataFormat.UInt16 => MinValueOfUInt16,
            NumericDataFormat.Int16 => MinValueOfInt16,
            NumericDataFormat.UInt32 => MinValueOfUInt32,
            NumericDataFormat.Int32 => MinValueOfInt32,
            NumericDataFormat.UInt64 => MinValueOfUInt64,
            NumericDataFormat.Int64 => MinValueOfInt64,
            NumericDataFormat.Single => MinValueOfSingle,
            NumericDataFormat.Double => MinValueOfDouble,
            NumericDataFormat.Decimal => MinValueOfDecimal,
            _ => throw new UnsupportedSpecificationException($"The minimum value of the numeric data format, {format}, is undefined.")
        };

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a numeric value to its <see cref="Number" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a numeric value to convert.
        /// </param>
        /// <param name="result">
        /// The resulting <see cref="Number" /> value, or <see cref="Zero" /> if the operation is unsuccessful.
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
        /// <paramref name="input" /> does not contain a valid representation of a numeric value.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Number result, Boolean raiseExceptionOnFail)
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
                result = new(0);
                return false;
            }

            var processedString = input.Trim();

            if (processedString.Length > 0)
            {
                if (Double.TryParse(processedString, out var doubleResult))
                {
                    result = new Number(doubleResult).Compress();
                    return true;
                }
                else if (Decimal.TryParse(processedString, out var decimalResult))
                {
                    result = new Number(decimalResult).Compress();
                    return true;
                }
                else if (BigInteger.TryParse(processedString, out var bigIntegerResult))
                {
                    result = new Number(bigIntegerResult).Compress();
                    return true;
                }
                else if (BigRational.TryParse(processedString, out var bigRationalResult))
                {
                    result = new Number(bigRationalResult);
                    return true;
                }
            }

            result = new(0);
            return raiseExceptionOnFail ? throw new FormatException("The specified string does not contain a valid representation of a numeric value.") : false;
        }

        /// <summary>
        /// Attempts to reduce the memory footprint of the current <see cref="Number" /> by converting its underlying value to a
        /// smaller data format, if possible.
        /// </summary>
        /// <param name="permutationCount">
        /// The maximum number of recursive compression operations to perform.
        /// </param>
        /// <returns>
        /// The resulting numeric value.
        /// </returns>
        [DebuggerHidden]
        private readonly Number Compress(UInt32 permutationCount) => permutationCount <= 0 ? this : Format switch
        {
            NumericDataFormat.Byte => this,
            NumericDataFormat.SByte => this,
            NumericDataFormat.UInt16 => IsValidAs(NumericDataFormat.Byte) ? new(ToByte()) : this,
            NumericDataFormat.Int16 => IsValidAs(NumericDataFormat.SByte) ? new(ToSByte()) : this,
            NumericDataFormat.UInt32 => IsValidAs(NumericDataFormat.UInt16) ? new Number(ToUInt16()).Compress(permutationCount - 1) : this,
            NumericDataFormat.Int32 => IsValidAs(NumericDataFormat.Int16) ? new Number(ToInt16()).Compress(permutationCount - 1) : this,
            NumericDataFormat.UInt64 => IsValidAs(NumericDataFormat.UInt32) ? new Number(ToUInt32()).Compress(permutationCount - 1) : this,
            NumericDataFormat.Int64 => IsValidAs(NumericDataFormat.Int32) ? new Number(ToInt32()).Compress(permutationCount - 1) : this,
            NumericDataFormat.Single => IsValidAs(NumericDataFormat.Int16) ? new Number(ToInt16()).Compress(permutationCount - 1) : this,
            NumericDataFormat.Double => IsValidAs(NumericDataFormat.Single) ? new Number(ToSingle()).Compress(permutationCount - 1) : this,
            NumericDataFormat.Decimal => IsValidAs(NumericDataFormat.Double) ? new Number(ToDouble()).Compress(permutationCount - 1) : this,
            NumericDataFormat.BigInteger => IsValidAs(NumericDataFormat.Int64) ? new Number(ToInt64()).Compress(permutationCount - 1) : this,
            NumericDataFormat.BigRational => IsValidAs(NumericDataFormat.BigInteger) ? new Number(ToBigInteger()).Compress(permutationCount - 1) : (IsValidAs(NumericDataFormat.Decimal) ? new Number(ToDecimal()).Compress(permutationCount - 1) : this),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is valid for the specified numeric data format.
        /// </summary>
        /// <param name="format">
        /// The numeric data format to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the number can be represented using <paramref name="format" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean IsValidAs(NumericDataFormat format) => Format == format ? true : format switch
        {
            NumericDataFormat.Byte => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.SByte => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.UInt16 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Int16 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.UInt32 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Int32 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.UInt64 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Int64 => IsInteger && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Single => CountSignificantFigures() <= SignificantFiguresLimitForSingle && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Double => CountSignificantFigures() <= SignificantFiguresLimitForDouble && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.Decimal => CountSignificantFigures() <= SignificantFiguresLimitForDecimal && this >= MinValueOf(format) && this <= MaxValueOf(format),
            NumericDataFormat.BigInteger => IsInteger,
            NumericDataFormat.BigRational => true,
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Represents the upper allowable limit of significant figures for conversion to a <see cref="Decimal" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SignificantFiguresLimitForDecimal = 28;

        /// <summary>
        /// Represents the upper allowable limit of significant figures for conversion to a <see cref="Double" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SignificantFiguresLimitForDouble = 16;

        /// <summary>
        /// Represents the upper allowable limit of significant figures for conversion to a <see cref="Single" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 SignificantFiguresLimitForSingle = 8;

        /// <summary>
        /// Represents an exception message that indicates that a numeric data format conversion failed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String OverflowExceptionMessage = "A numeric data format conversion failed. The value was either too small or too large.";

        /// <summary>
        /// Represents the encoding that is used to format binary representations of <see cref="BigRational" /> values.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Encoding BigRationalFormatEncoding = Encoding.UTF8;

        /// <summary>
        /// Represents the largest possible value of a <see cref="Byte" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfByte = new(Byte.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Decimal" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfDecimal = new(Decimal.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Double" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfDouble = new(Double.MaxValue);

        /// <summary>
        /// Represents the largest possible value of an <see cref="Int16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfInt16 = new(Int16.MaxValue);

        /// <summary>
        /// Represents the largest possible value of an <see cref="Int32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfInt32 = new(Int32.MaxValue);

        /// <summary>
        /// Represents the largest possible value of an <see cref="Int64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfInt64 = new(Int64.MaxValue);

        /// <summary>
        /// Represents the largest possible value of an <see cref="SByte" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfSByte = new(SByte.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Single" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfSingle = new(Single.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="UInt16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfUInt16 = new(UInt16.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="UInt32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfUInt32 = new(UInt32.MaxValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="UInt64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MaxValueOfUInt64 = new(UInt64.MaxValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Byte" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfByte = new(Byte.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Decimal" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfDecimal = new(Decimal.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Double" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfDouble = new(Double.MinValue);

        /// <summary>
        /// Represents the smallest possible value of an <see cref="Int16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfInt16 = new(Int16.MinValue);

        /// <summary>
        /// Represents the smallest possible value of an <see cref="Int32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfInt32 = new(Int32.MinValue);

        /// <summary>
        /// Represents the smallest possible value of an <see cref="Int64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfInt64 = new(Int64.MinValue);

        /// <summary>
        /// Represents the smallest possible value of an <see cref="SByte" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfSByte = new(SByte.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Single" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfSingle = new(Single.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="UInt16" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfUInt16 = new(UInt16.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="UInt32" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfUInt32 = new(UInt32.MinValue);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="UInt64" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Number MinValueOfUInt64 = new(UInt64.MinValue);
    }
}