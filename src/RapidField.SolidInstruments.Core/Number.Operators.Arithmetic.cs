// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Numerics;
using BigRational = Rationals.Rational;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number
    {
        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Number minuend) => minuend.Format switch
        {
            NumericDataFormat.Byte => subtrahend - minuend.ToByte(),
            NumericDataFormat.SByte => subtrahend - minuend.ToSByte(),
            NumericDataFormat.UInt16 => subtrahend - minuend.ToUInt16(),
            NumericDataFormat.Int16 => subtrahend - minuend.ToInt16(),
            NumericDataFormat.UInt32 => subtrahend - minuend.ToUInt32(),
            NumericDataFormat.Int32 => subtrahend - minuend.ToInt32(),
            NumericDataFormat.UInt64 => subtrahend - minuend.ToUInt64(),
            NumericDataFormat.Int64 => subtrahend - minuend.ToInt64(),
            NumericDataFormat.Single => subtrahend - minuend.ToSingle(),
            NumericDataFormat.Double => subtrahend - minuend.ToDouble(),
            NumericDataFormat.Decimal => subtrahend - minuend.ToDecimal(),
            NumericDataFormat.BigInteger => subtrahend - minuend.ToBigInteger(),
            NumericDataFormat.BigRational => subtrahend - minuend.ToBigRational(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Byte minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt16() - minuend.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt16() - minuend.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, SByte minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt16() - minuend.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt16() - minuend.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, UInt16 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Int16 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt32() - minuend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, UInt32 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Int32 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToInt64() - minuend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, UInt64 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Int64 minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Single minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToDouble() - minuend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Double minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, Decimal minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, BigInteger minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigInteger() - minuend).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the difference between two numeric values.
        /// </summary>
        /// <param name="subtrahend">
        /// The first numeric value.
        /// </param>
        /// <param name="minuend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The difference between the specified numeric values.
        /// </returns>
        public static Number operator -(Number subtrahend, BigRational minuend) => subtrahend.Format switch
        {
            NumericDataFormat.Byte => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.SByte => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.UInt16 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Int16 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.UInt32 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Int32 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.UInt64 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Int64 => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Single => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Double => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.Decimal => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.BigInteger => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            NumericDataFormat.BigRational => new Number(subtrahend.ToBigRational() - minuend).Compress(),
            _ => throw new UnsupportedSpecificationException(subtrahend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Negates the specified numeric value.
        /// </summary>
        /// <param name="value">
        /// A numeric value.
        /// </param>
        /// <returns>
        /// The negative of the specified numeric value.
        /// </returns>
        public static Number operator -(Number value) => value.Format switch
        {
            NumericDataFormat.Byte => new Number(-value.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(-value.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(-value.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(-value.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(-value.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(-value.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(-value.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(-value.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(-value.ToSingle()).Compress(),
            NumericDataFormat.Double => new Number(-value.ToDouble()).Compress(),
            NumericDataFormat.Decimal => new Number(-value.ToDecimal()).Compress(),
            NumericDataFormat.BigInteger => new Number(-value.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(-value.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(value.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Number multiplier) => multiplier.Format switch
        {
            NumericDataFormat.Byte => multiplicand * multiplier.ToByte(),
            NumericDataFormat.SByte => multiplicand * multiplier.ToSByte(),
            NumericDataFormat.UInt16 => multiplicand * multiplier.ToUInt16(),
            NumericDataFormat.Int16 => multiplicand * multiplier.ToInt16(),
            NumericDataFormat.UInt32 => multiplicand * multiplier.ToUInt32(),
            NumericDataFormat.Int32 => multiplicand * multiplier.ToInt32(),
            NumericDataFormat.UInt64 => multiplicand * multiplier.ToUInt64(),
            NumericDataFormat.Int64 => multiplicand * multiplier.ToInt64(),
            NumericDataFormat.Single => multiplicand * multiplier.ToSingle(),
            NumericDataFormat.Double => multiplicand * multiplier.ToDouble(),
            NumericDataFormat.Decimal => multiplicand * multiplier.ToDecimal(),
            NumericDataFormat.BigInteger => multiplicand * multiplier.ToBigInteger(),
            NumericDataFormat.BigRational => multiplicand * multiplier.ToBigRational(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Byte multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt16() * multiplier.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt16() * multiplier.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, SByte multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt16() * multiplier.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt16() * multiplier.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, UInt16 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Int16 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt32() * multiplier.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, UInt32 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Int32 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToInt64() * multiplier.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, UInt64 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Int64 multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Single multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToDouble() * multiplier.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Double multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, Decimal multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, BigInteger multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigInteger() * multiplier).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the product of a numeric value and a multiplier.
        /// </summary>
        /// <param name="multiplicand">
        /// A numeric value.
        /// </param>
        /// <param name="multiplier">
        /// A numeric multiplier.
        /// </param>
        /// <returns>
        /// The product of the specified numeric value and the specified multiplier.
        /// </returns>
        public static Number operator *(Number multiplicand, BigRational multiplier) => multiplicand.Format switch
        {
            NumericDataFormat.Byte => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.SByte => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.UInt16 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Int16 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.UInt32 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Int32 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.UInt64 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Int64 => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Single => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Double => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.Decimal => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.BigInteger => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            NumericDataFormat.BigRational => new Number(multiplicand.ToBigRational() * multiplier).Compress(),
            _ => throw new UnsupportedSpecificationException(multiplicand.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Number divisor) => divisor.Format switch
        {
            NumericDataFormat.Byte => dividend / divisor.ToByte(),
            NumericDataFormat.SByte => dividend / divisor.ToSByte(),
            NumericDataFormat.UInt16 => dividend / divisor.ToUInt16(),
            NumericDataFormat.Int16 => dividend / divisor.ToInt16(),
            NumericDataFormat.UInt32 => dividend / divisor.ToUInt32(),
            NumericDataFormat.Int32 => dividend / divisor.ToInt32(),
            NumericDataFormat.UInt64 => dividend / divisor.ToUInt64(),
            NumericDataFormat.Int64 => dividend / divisor.ToInt64(),
            NumericDataFormat.Single => dividend / divisor.ToSingle(),
            NumericDataFormat.Double => dividend / divisor.ToDouble(),
            NumericDataFormat.Decimal => dividend / divisor.ToDecimal(),
            NumericDataFormat.BigInteger => dividend / divisor.ToBigInteger(),
            NumericDataFormat.BigRational => dividend / divisor.ToBigRational(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Byte divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToSingle() / divisor.ToSingle()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToSingle() / divisor.ToSingle()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, SByte divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToSingle() / divisor.ToSingle()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToSingle() / divisor.ToSingle()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, UInt16 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Int16 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, UInt32 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Int32 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDecimal() / divisor.ToDecimal()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, UInt64 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Int64 divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Single divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToDouble() / divisor.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Double divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, Decimal divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, BigInteger divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the quotient of a numeric value and a divisor.
        /// </summary>
        /// <param name="dividend">
        /// A numeric value.
        /// </param>
        /// <param name="divisor">
        /// A numeric divisor.
        /// </param>
        /// <returns>
        /// The quotient of the specified numeric value and the specified divisor.
        /// </returns>
        public static Number operator /(Number dividend, BigRational divisor) => dividend.Format switch
        {
            NumericDataFormat.Byte => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.SByte => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt16 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int16 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt32 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int32 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.UInt64 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Int64 => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Single => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Double => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.Decimal => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.BigInteger => new Number(dividend.ToBigRational() / divisor).Compress(),
            NumericDataFormat.BigRational => new Number(dividend.ToBigRational() / divisor).Compress(),
            _ => throw new UnsupportedSpecificationException(dividend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Number addend) => addend.Format switch
        {
            NumericDataFormat.Byte => augend + addend.ToByte(),
            NumericDataFormat.SByte => augend + addend.ToSByte(),
            NumericDataFormat.UInt16 => augend + addend.ToUInt16(),
            NumericDataFormat.Int16 => augend + addend.ToInt16(),
            NumericDataFormat.UInt32 => augend + addend.ToUInt32(),
            NumericDataFormat.Int32 => augend + addend.ToInt32(),
            NumericDataFormat.UInt64 => augend + addend.ToUInt64(),
            NumericDataFormat.Int64 => augend + addend.ToInt64(),
            NumericDataFormat.Single => augend + addend.ToSingle(),
            NumericDataFormat.Double => augend + addend.ToDouble(),
            NumericDataFormat.Decimal => augend + addend.ToDecimal(),
            NumericDataFormat.BigInteger => augend + addend.ToBigInteger(),
            NumericDataFormat.BigRational => augend + addend.ToBigRational(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Byte addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt16() + addend.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt16() + addend.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, SByte addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt16() + addend.ToInt16()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt16() + addend.ToInt16()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, UInt16 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Int16 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt32() + addend.ToInt32()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, UInt32 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Int32 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToInt64() + addend.ToInt64()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, UInt64 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Int64 addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend.ToBigInteger()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Single addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToDouble() + addend.ToDouble()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Double addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, Decimal addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Single => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, BigInteger addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.Single => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigInteger() + addend).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend.ToBigRational()).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines the sum of two numeric values.
        /// </summary>
        /// <param name="augend">
        /// The first numeric value.
        /// </param>
        /// <param name="addend">
        /// The second numeric value.
        /// </param>
        /// <returns>
        /// The sum of the specified numeric values.
        /// </returns>
        public static Number operator +(Number augend, BigRational addend) => augend.Format switch
        {
            NumericDataFormat.Byte => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.SByte => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.UInt16 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Int16 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.UInt32 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Int32 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.UInt64 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Int64 => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Single => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Double => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.Decimal => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.BigInteger => new Number(augend.ToBigRational() + addend).Compress(),
            NumericDataFormat.BigRational => new Number(augend.ToBigRational() + addend).Compress(),
            _ => throw new UnsupportedSpecificationException(augend.UnsupportedFormatExceptionMessage)
        };
    }
}