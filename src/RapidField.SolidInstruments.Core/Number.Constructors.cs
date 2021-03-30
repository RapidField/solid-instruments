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
             : this(new[] { unchecked((Byte)value) }, NumericDataFormat.SByte)
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
    }
}