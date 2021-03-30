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
        /// Determines whether or not two specified <see cref="Number" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Number a, Number b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not two specified <see cref="Number" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Number" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Number" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Number a, Number b) => b.Format switch
        {
            NumericDataFormat.Byte => a.Equals(b.ToByte()),
            NumericDataFormat.SByte => a.Equals(b.ToSByte()),
            NumericDataFormat.UInt16 => a.Equals(b.ToUInt16()),
            NumericDataFormat.Int16 => a.Equals(b.ToInt16()),
            NumericDataFormat.UInt32 => a.Equals(b.ToUInt32()),
            NumericDataFormat.Int32 => a.Equals(b.ToInt32()),
            NumericDataFormat.UInt64 => a.Equals(b.ToUInt64()),
            NumericDataFormat.Int64 => a.Equals(b.ToInt64()),
            NumericDataFormat.Single => a.Equals(b.ToSingle()),
            NumericDataFormat.Double => a.Equals(b.ToDouble()),
            NumericDataFormat.Decimal => a.Equals(b.ToDecimal()),
            NumericDataFormat.BigInteger => a.Equals(b.ToBigInteger()),
            NumericDataFormat.BigRational => a.Equals(b.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(b.UnsupportedFormatExceptionMessage)
        };
    }
}