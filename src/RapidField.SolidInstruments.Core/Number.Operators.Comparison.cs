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

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Number" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Number other) => other.Format switch
        {
            NumericDataFormat.Byte => CompareTo(other.ToByte()),
            NumericDataFormat.SByte => CompareTo(other.ToSByte()),
            NumericDataFormat.UInt16 => CompareTo(other.ToUInt16()),
            NumericDataFormat.Int16 => CompareTo(other.ToInt16()),
            NumericDataFormat.UInt32 => CompareTo(other.ToUInt32()),
            NumericDataFormat.Int32 => CompareTo(other.ToInt32()),
            NumericDataFormat.UInt64 => CompareTo(other.ToUInt64()),
            NumericDataFormat.Int64 => CompareTo(other.ToInt64()),
            NumericDataFormat.Single => CompareTo(other.ToSingle()),
            NumericDataFormat.Double => CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Byte" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Byte" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Byte other) => Format switch
        {
            NumericDataFormat.Byte => ToByte().CompareTo(other),
            NumericDataFormat.SByte => ToInt16().CompareTo(other.ToInt16()),
            NumericDataFormat.UInt16 => ToUInt16().CompareTo(other.ToUInt16()),
            NumericDataFormat.Int16 => ToInt16().CompareTo(other.ToInt16()),
            NumericDataFormat.UInt32 => ToUInt32().CompareTo(other.ToUInt32()),
            NumericDataFormat.Int32 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt64 => ToUInt64().CompareTo(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().CompareTo(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="SByte" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SByte" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(SByte other) => Format switch
        {
            NumericDataFormat.Byte => ToInt16().CompareTo(other.ToInt16()),
            NumericDataFormat.SByte => ToSByte().CompareTo(other),
            NumericDataFormat.UInt16 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.Int16 => ToInt16().CompareTo(other.ToInt16()),
            NumericDataFormat.UInt32 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt64 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().CompareTo(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="UInt16" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt16" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(UInt16 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt16().CompareTo(other),
            NumericDataFormat.SByte => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt16 => ToUInt16().CompareTo(other),
            NumericDataFormat.Int16 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt32 => ToUInt32().CompareTo(other.ToUInt32()),
            NumericDataFormat.Int32 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt64 => ToUInt64().CompareTo(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().CompareTo(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Int16" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int16" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Int16 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt16().CompareTo(other),
            NumericDataFormat.SByte => ToInt16().CompareTo(other),
            NumericDataFormat.UInt16 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.Int16 => ToInt16().CompareTo(other),
            NumericDataFormat.UInt32 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().CompareTo(other.ToInt32()),
            NumericDataFormat.UInt64 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().CompareTo(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="UInt32" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt32" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(UInt32 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt32().CompareTo(other),
            NumericDataFormat.SByte => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.UInt16 => ToUInt32().CompareTo(other),
            NumericDataFormat.Int16 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.UInt32 => ToUInt32().CompareTo(other),
            NumericDataFormat.Int32 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.UInt64 => ToUInt64().CompareTo(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Int32" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int32" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Int32 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt32().CompareTo(other),
            NumericDataFormat.SByte => ToInt32().CompareTo(other),
            NumericDataFormat.UInt16 => ToInt32().CompareTo(other),
            NumericDataFormat.Int16 => ToInt32().CompareTo(other),
            NumericDataFormat.UInt32 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().CompareTo(other),
            NumericDataFormat.UInt64 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other.ToInt64()),
            NumericDataFormat.Single => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="UInt64" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt64" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(UInt64 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt64().CompareTo(other),
            NumericDataFormat.SByte => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.UInt16 => ToUInt64().CompareTo(other),
            NumericDataFormat.Int16 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.UInt32 => ToUInt64().CompareTo(other),
            NumericDataFormat.Int32 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.UInt64 => ToUInt64().CompareTo(other),
            NumericDataFormat.Int64 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.Single => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.Double => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Int64" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int64" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Int64 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt64().CompareTo(other),
            NumericDataFormat.SByte => ToInt64().CompareTo(other),
            NumericDataFormat.UInt16 => ToInt64().CompareTo(other),
            NumericDataFormat.Int16 => ToInt64().CompareTo(other),
            NumericDataFormat.UInt32 => ToInt64().CompareTo(other),
            NumericDataFormat.Int32 => ToInt64().CompareTo(other),
            NumericDataFormat.UInt64 => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().CompareTo(other),
            NumericDataFormat.Single => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.Double => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Single" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Single" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Single other) => Format switch
        {
            NumericDataFormat.Byte => ToSingle().CompareTo(other),
            NumericDataFormat.SByte => ToSingle().CompareTo(other),
            NumericDataFormat.UInt16 => ToSingle().CompareTo(other),
            NumericDataFormat.Int16 => ToSingle().CompareTo(other),
            NumericDataFormat.UInt32 => ToSingle().CompareTo(other),
            NumericDataFormat.Int32 => ToSingle().CompareTo(other),
            NumericDataFormat.UInt64 => ToSingle().CompareTo(other),
            NumericDataFormat.Int64 => ToSingle().CompareTo(other),
            NumericDataFormat.Single => ToSingle().CompareTo(other),
            NumericDataFormat.Double => ToDouble().CompareTo(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Double" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Double" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Double other) => Format switch
        {
            NumericDataFormat.Byte => ToDouble().CompareTo(other),
            NumericDataFormat.SByte => ToDouble().CompareTo(other),
            NumericDataFormat.UInt16 => ToDouble().CompareTo(other),
            NumericDataFormat.Int16 => ToDouble().CompareTo(other),
            NumericDataFormat.UInt32 => ToDouble().CompareTo(other),
            NumericDataFormat.Int32 => ToDouble().CompareTo(other),
            NumericDataFormat.UInt64 => ToDouble().CompareTo(other),
            NumericDataFormat.Int64 => ToDouble().CompareTo(other),
            NumericDataFormat.Single => ToDouble().CompareTo(other),
            NumericDataFormat.Double => ToDouble().CompareTo(other),
            NumericDataFormat.Decimal => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.BigInteger => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="Decimal" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Decimal" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(Decimal other) => Format switch
        {
            NumericDataFormat.Byte => ToDecimal().CompareTo(other),
            NumericDataFormat.SByte => ToDecimal().CompareTo(other),
            NumericDataFormat.UInt16 => ToDecimal().CompareTo(other),
            NumericDataFormat.Int16 => ToDecimal().CompareTo(other),
            NumericDataFormat.UInt32 => ToDecimal().CompareTo(other),
            NumericDataFormat.Int32 => ToDecimal().CompareTo(other),
            NumericDataFormat.UInt64 => ToDecimal().CompareTo(other),
            NumericDataFormat.Int64 => ToDecimal().CompareTo(other),
            NumericDataFormat.Single => ToDecimal().CompareTo(other),
            NumericDataFormat.Double => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.Decimal => ToDecimal().CompareTo(other),
            NumericDataFormat.BigInteger => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="BigInteger" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="BigInteger" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(BigInteger other) => Format switch
        {
            NumericDataFormat.Byte => ToBigInteger().CompareTo(other),
            NumericDataFormat.SByte => ToBigInteger().CompareTo(other),
            NumericDataFormat.UInt16 => ToBigInteger().CompareTo(other),
            NumericDataFormat.Int16 => ToBigInteger().CompareTo(other),
            NumericDataFormat.UInt32 => ToBigInteger().CompareTo(other),
            NumericDataFormat.Int32 => ToBigInteger().CompareTo(other),
            NumericDataFormat.UInt64 => ToBigInteger().CompareTo(other),
            NumericDataFormat.Int64 => ToBigInteger().CompareTo(other),
            NumericDataFormat.Single => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.Double => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.Decimal => ToBigRational().CompareTo(other.ToBigRational()),
            NumericDataFormat.BigInteger => ToBigInteger().CompareTo(other),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Compares the current <see cref="Number" /> to the supplied <see cref="BigRational" /> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="BigRational" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is less than the supplied instance; one if this instance is greater than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        [DebuggerHidden]
        private Int32 CompareTo(BigRational other) => Format switch
        {
            NumericDataFormat.Byte => ToBigRational().CompareTo(other),
            NumericDataFormat.SByte => ToBigRational().CompareTo(other),
            NumericDataFormat.UInt16 => ToBigRational().CompareTo(other),
            NumericDataFormat.Int16 => ToBigRational().CompareTo(other),
            NumericDataFormat.UInt32 => ToBigRational().CompareTo(other),
            NumericDataFormat.Int32 => ToBigRational().CompareTo(other),
            NumericDataFormat.UInt64 => ToBigRational().CompareTo(other),
            NumericDataFormat.Int64 => ToBigRational().CompareTo(other),
            NumericDataFormat.Single => ToBigRational().CompareTo(other),
            NumericDataFormat.Double => ToBigRational().CompareTo(other),
            NumericDataFormat.Decimal => ToBigRational().CompareTo(other),
            NumericDataFormat.BigInteger => ToBigRational().CompareTo(other),
            NumericDataFormat.BigRational => ToBigRational().CompareTo(other),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };
    }
}