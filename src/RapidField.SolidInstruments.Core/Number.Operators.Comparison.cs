// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an immutable whole or fractional number of arbitrary size.
    /// </summary>
    public readonly partial struct Number
    {
        /// <summary>
        /// Determines whether or not a supplied <see cref="Number" /> instance is less than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Number a, Number b) => b.Format switch
        {
            NumericDataFormat.Byte => a.CompareTo(b.ToByte()) < 0,
            NumericDataFormat.SByte => a.CompareTo(b.ToSByte()) < 0,
            NumericDataFormat.UInt16 => a.CompareTo(b.ToUInt16()) < 0,
            NumericDataFormat.Int16 => a.CompareTo(b.ToInt16()) < 0,
            NumericDataFormat.UInt32 => a.CompareTo(b.ToUInt32()) < 0,
            NumericDataFormat.Int32 => a.CompareTo(b.ToInt32()) < 0,
            NumericDataFormat.UInt64 => a.CompareTo(b.ToUInt64()) < 0,
            NumericDataFormat.Int64 => a.CompareTo(b.ToInt64()) < 0,
            NumericDataFormat.Single => a.CompareTo(b.ToSingle()) < 0,
            NumericDataFormat.Double => a.CompareTo(b.ToDouble()) < 0,
            NumericDataFormat.Decimal => a.CompareTo(b.ToDecimal()) < 0,
            NumericDataFormat.BigInteger => a.CompareTo(b.ToBigInteger()) < 0,
            NumericDataFormat.BigRational => a.CompareTo(b.ToBigRational()) < 0,
            _ => throw new UnsupportedSpecificationException(b.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not a supplied <see cref="Number" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is less than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Number a, Number b) => b.Format switch
        {
            NumericDataFormat.Byte => a.CompareTo(b.ToByte()) <= 0,
            NumericDataFormat.SByte => a.CompareTo(b.ToSByte()) <= 0,
            NumericDataFormat.UInt16 => a.CompareTo(b.ToUInt16()) <= 0,
            NumericDataFormat.Int16 => a.CompareTo(b.ToInt16()) <= 0,
            NumericDataFormat.UInt32 => a.CompareTo(b.ToUInt32()) <= 0,
            NumericDataFormat.Int32 => a.CompareTo(b.ToInt32()) <= 0,
            NumericDataFormat.UInt64 => a.CompareTo(b.ToUInt64()) <= 0,
            NumericDataFormat.Int64 => a.CompareTo(b.ToInt64()) <= 0,
            NumericDataFormat.Single => a.CompareTo(b.ToSingle()) <= 0,
            NumericDataFormat.Double => a.CompareTo(b.ToDouble()) <= 0,
            NumericDataFormat.Decimal => a.CompareTo(b.ToDecimal()) <= 0,
            NumericDataFormat.BigInteger => a.CompareTo(b.ToBigInteger()) <= 0,
            NumericDataFormat.BigRational => a.CompareTo(b.ToBigRational()) <= 0,
            _ => throw new UnsupportedSpecificationException(b.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not a supplied <see cref="Number" /> instance is greater than another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Number a, Number b) => b.Format switch
        {
            NumericDataFormat.Byte => a.CompareTo(b.ToByte()) > 0,
            NumericDataFormat.SByte => a.CompareTo(b.ToSByte()) > 0,
            NumericDataFormat.UInt16 => a.CompareTo(b.ToUInt16()) > 0,
            NumericDataFormat.Int16 => a.CompareTo(b.ToInt16()) > 0,
            NumericDataFormat.UInt32 => a.CompareTo(b.ToUInt32()) > 0,
            NumericDataFormat.Int32 => a.CompareTo(b.ToInt32()) > 0,
            NumericDataFormat.UInt64 => a.CompareTo(b.ToUInt64()) > 0,
            NumericDataFormat.Int64 => a.CompareTo(b.ToInt64()) > 0,
            NumericDataFormat.Single => a.CompareTo(b.ToSingle()) > 0,
            NumericDataFormat.Double => a.CompareTo(b.ToDouble()) > 0,
            NumericDataFormat.Decimal => a.CompareTo(b.ToDecimal()) > 0,
            NumericDataFormat.BigInteger => a.CompareTo(b.ToBigInteger()) > 0,
            NumericDataFormat.BigRational => a.CompareTo(b.ToBigRational()) > 0,
            _ => throw new UnsupportedSpecificationException(b.UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not a supplied <see cref="Number" /> instance is greater than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is greater than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Number a, Number b) => b.Format switch
        {
            NumericDataFormat.Byte => a.CompareTo(b.ToByte()) >= 0,
            NumericDataFormat.SByte => a.CompareTo(b.ToSByte()) >= 0,
            NumericDataFormat.UInt16 => a.CompareTo(b.ToUInt16()) >= 0,
            NumericDataFormat.Int16 => a.CompareTo(b.ToInt16()) >= 0,
            NumericDataFormat.UInt32 => a.CompareTo(b.ToUInt32()) >= 0,
            NumericDataFormat.Int32 => a.CompareTo(b.ToInt32()) >= 0,
            NumericDataFormat.UInt64 => a.CompareTo(b.ToUInt64()) >= 0,
            NumericDataFormat.Int64 => a.CompareTo(b.ToInt64()) >= 0,
            NumericDataFormat.Single => a.CompareTo(b.ToSingle()) >= 0,
            NumericDataFormat.Double => a.CompareTo(b.ToDouble()) >= 0,
            NumericDataFormat.Decimal => a.CompareTo(b.ToDecimal()) >= 0,
            NumericDataFormat.BigInteger => a.CompareTo(b.ToBigInteger()) >= 0,
            NumericDataFormat.BigRational => a.CompareTo(b.ToBigRational()) >= 0,
            _ => throw new UnsupportedSpecificationException(b.UnsupportedFormatExceptionMessage)
        };
    }
}