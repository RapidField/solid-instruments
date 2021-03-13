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

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public readonly override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is Number number)
            {
                return Equals(number);
            }
            else if (obj is Int32 int32Value)
            {
                return Equals(int32Value);
            }
            else if (obj is Double doubleValue)
            {
                return Equals(doubleValue);
            }
            else if (obj is Decimal decimalValue)
            {
                return Equals(decimalValue);
            }
            else if (obj is Int64 int64Value)
            {
                return Equals(int64Value);
            }
            else if (obj is Single singleValue)
            {
                return Equals(singleValue);
            }
            else if (obj is BigInteger bigIntegerValue)
            {
                return Equals(bigIntegerValue);
            }
            else if (obj is BigRational bigRationalValue)
            {
                return Equals(bigRationalValue);
            }
            else if (obj is Int16 int16Value)
            {
                return Equals(int16Value);
            }
            else if (obj is Byte byteValue)
            {
                return Equals(byteValue);
            }
            else if (obj is UInt64 uInt64Value)
            {
                return Equals(uInt64Value);
            }
            else if (obj is UInt32 uInt32Value)
            {
                return Equals(uInt32Value);
            }
            else if (obj is UInt16 uInt16Value)
            {
                return Equals(uInt16Value);
            }
            else if (obj is SByte sByteValue)
            {
                return Equals(sByteValue);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Number" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Number" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public readonly Boolean Equals(Number other) => Format switch
        {
            NumericDataFormat.Byte => other.Equals(ToByte()),
            NumericDataFormat.SByte => other.Equals(ToSByte()),
            NumericDataFormat.UInt16 => other.Equals(ToUInt16()),
            NumericDataFormat.Int16 => other.Equals(ToInt16()),
            NumericDataFormat.UInt32 => other.Equals(ToUInt32()),
            NumericDataFormat.Int32 => other.Equals(ToInt32()),
            NumericDataFormat.UInt64 => other.Equals(ToUInt64()),
            NumericDataFormat.Int64 => other.Equals(ToInt64()),
            NumericDataFormat.Single => other.Equals(ToSingle()),
            NumericDataFormat.Double => other.Equals(ToDouble()),
            NumericDataFormat.Decimal => other.Equals(ToDecimal()),
            NumericDataFormat.BigInteger => other.Equals(ToBigInteger()),
            NumericDataFormat.BigRational => other.Equals(ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public readonly override Int32 GetHashCode() => Compress().ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Byte" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Byte" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Byte other) => Format switch
        {
            NumericDataFormat.Byte => ToByte().Equals(other),
            NumericDataFormat.SByte => ToInt16().Equals(other.ToInt16()),
            NumericDataFormat.UInt16 => ToUInt16().Equals(other.ToUInt16()),
            NumericDataFormat.Int16 => ToInt16().Equals(other.ToInt16()),
            NumericDataFormat.UInt32 => ToUInt32().Equals(other.ToUInt32()),
            NumericDataFormat.Int32 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt64 => ToUInt64().Equals(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().Equals(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="SByte" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SByte" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(SByte other) => Format switch
        {
            NumericDataFormat.Byte => ToInt16().Equals(other.ToInt16()),
            NumericDataFormat.SByte => ToSByte().Equals(other),
            NumericDataFormat.UInt16 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.Int16 => ToInt16().Equals(other.ToInt16()),
            NumericDataFormat.UInt32 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt64 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().Equals(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="UInt16" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt16" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(UInt16 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt16().Equals(other),
            NumericDataFormat.SByte => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt16 => ToUInt16().Equals(other),
            NumericDataFormat.Int16 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt32 => ToUInt32().Equals(other.ToUInt32()),
            NumericDataFormat.Int32 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt64 => ToUInt64().Equals(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().Equals(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Int16" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int16" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Int16 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt16().Equals(other),
            NumericDataFormat.SByte => ToInt16().Equals(other),
            NumericDataFormat.UInt16 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.Int16 => ToInt16().Equals(other),
            NumericDataFormat.UInt32 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().Equals(other.ToInt32()),
            NumericDataFormat.UInt64 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToSingle().Equals(other.ToSingle()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="UInt32" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt32" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(UInt32 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt32().Equals(other),
            NumericDataFormat.SByte => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.UInt16 => ToUInt32().Equals(other),
            NumericDataFormat.Int16 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.UInt32 => ToUInt32().Equals(other),
            NumericDataFormat.Int32 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.UInt64 => ToUInt64().Equals(other.ToUInt64()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Int32" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int32" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Int32 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt32().Equals(other),
            NumericDataFormat.SByte => ToInt32().Equals(other),
            NumericDataFormat.UInt16 => ToInt32().Equals(other),
            NumericDataFormat.Int16 => ToInt32().Equals(other),
            NumericDataFormat.UInt32 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Int32 => ToInt32().Equals(other),
            NumericDataFormat.UInt64 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().Equals(other.ToInt64()),
            NumericDataFormat.Single => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="UInt64" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="UInt64" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(UInt64 other) => Format switch
        {
            NumericDataFormat.Byte => ToUInt64().Equals(other),
            NumericDataFormat.SByte => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.UInt16 => ToUInt64().Equals(other),
            NumericDataFormat.Int16 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.UInt32 => ToUInt64().Equals(other),
            NumericDataFormat.Int32 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.UInt64 => ToUInt64().Equals(other),
            NumericDataFormat.Int64 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.Single => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.Double => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Int64" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Int64" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Int64 other) => Format switch
        {
            NumericDataFormat.Byte => ToInt64().Equals(other),
            NumericDataFormat.SByte => ToInt64().Equals(other),
            NumericDataFormat.UInt16 => ToInt64().Equals(other),
            NumericDataFormat.Int16 => ToInt64().Equals(other),
            NumericDataFormat.UInt32 => ToInt64().Equals(other),
            NumericDataFormat.Int32 => ToInt64().Equals(other),
            NumericDataFormat.UInt64 => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.Int64 => ToInt64().Equals(other),
            NumericDataFormat.Single => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.Double => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Single" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Single" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Single other) => Format switch
        {
            NumericDataFormat.Byte => ToSingle().Equals(other),
            NumericDataFormat.SByte => ToSingle().Equals(other),
            NumericDataFormat.UInt16 => ToSingle().Equals(other),
            NumericDataFormat.Int16 => ToSingle().Equals(other),
            NumericDataFormat.UInt32 => ToSingle().Equals(other),
            NumericDataFormat.Int32 => ToSingle().Equals(other),
            NumericDataFormat.UInt64 => ToSingle().Equals(other),
            NumericDataFormat.Int64 => ToSingle().Equals(other),
            NumericDataFormat.Single => ToSingle().Equals(other),
            NumericDataFormat.Double => ToDouble().Equals(other.ToDouble()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other.ToDecimal()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Double" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Double" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Double other) => Format switch
        {
            NumericDataFormat.Byte => ToDouble().Equals(other),
            NumericDataFormat.SByte => ToDouble().Equals(other),
            NumericDataFormat.UInt16 => ToDouble().Equals(other),
            NumericDataFormat.Int16 => ToDouble().Equals(other),
            NumericDataFormat.UInt32 => ToDouble().Equals(other),
            NumericDataFormat.Int32 => ToDouble().Equals(other),
            NumericDataFormat.UInt64 => ToDouble().Equals(other),
            NumericDataFormat.Int64 => ToDouble().Equals(other),
            NumericDataFormat.Single => ToDouble().Equals(other),
            NumericDataFormat.Double => ToDouble().Equals(other),
            NumericDataFormat.Decimal => ToBigRational().Equals(other.ToBigRational()),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="Decimal" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Decimal" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(Decimal other) => Format switch
        {
            NumericDataFormat.Byte => ToDecimal().Equals(other),
            NumericDataFormat.SByte => ToDecimal().Equals(other),
            NumericDataFormat.UInt16 => ToDecimal().Equals(other),
            NumericDataFormat.Int16 => ToDecimal().Equals(other),
            NumericDataFormat.UInt32 => ToDecimal().Equals(other),
            NumericDataFormat.Int32 => ToDecimal().Equals(other),
            NumericDataFormat.UInt64 => ToDecimal().Equals(other),
            NumericDataFormat.Int64 => ToDecimal().Equals(other),
            NumericDataFormat.Single => ToDecimal().Equals(other),
            NumericDataFormat.Double => ToBigRational().Equals(other.ToBigRational()),
            NumericDataFormat.Decimal => ToDecimal().Equals(other),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other.ToBigInteger()),
            NumericDataFormat.BigRational => ToBigRational().Equals(other.ToBigRational()),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="BigInteger" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="BigInteger" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(BigInteger other) => Format switch
        {
            NumericDataFormat.Byte => ToBigInteger().Equals(other),
            NumericDataFormat.SByte => ToBigInteger().Equals(other),
            NumericDataFormat.UInt16 => ToBigInteger().Equals(other),
            NumericDataFormat.Int16 => ToBigInteger().Equals(other),
            NumericDataFormat.UInt32 => ToBigInteger().Equals(other),
            NumericDataFormat.Int32 => ToBigInteger().Equals(other),
            NumericDataFormat.UInt64 => ToBigInteger().Equals(other),
            NumericDataFormat.Int64 => ToBigInteger().Equals(other),
            NumericDataFormat.Single => ToBigInteger().Equals(other),
            NumericDataFormat.Double => ToBigInteger().Equals(other),
            NumericDataFormat.Decimal => ToBigInteger().Equals(other),
            NumericDataFormat.BigInteger => ToBigInteger().Equals(other),
            NumericDataFormat.BigRational => ToBigRational().Equals(other),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };

        /// <summary>
        /// Determines whether or not the current <see cref="Number" /> is equal to the specified <see cref="BigRational" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="BigRational" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        [DebuggerHidden]
        private readonly Boolean Equals(BigRational other) => Format switch
        {
            NumericDataFormat.Byte => ToBigRational().Equals(other),
            NumericDataFormat.SByte => ToBigRational().Equals(other),
            NumericDataFormat.UInt16 => ToBigRational().Equals(other),
            NumericDataFormat.Int16 => ToBigRational().Equals(other),
            NumericDataFormat.UInt32 => ToBigRational().Equals(other),
            NumericDataFormat.Int32 => ToBigRational().Equals(other),
            NumericDataFormat.UInt64 => ToBigRational().Equals(other),
            NumericDataFormat.Int64 => ToBigRational().Equals(other),
            NumericDataFormat.Single => ToBigRational().Equals(other),
            NumericDataFormat.Double => ToBigRational().Equals(other),
            NumericDataFormat.Decimal => ToBigRational().Equals(other),
            NumericDataFormat.BigInteger => ToBigRational().Equals(other),
            NumericDataFormat.BigRational => ToBigRational().Equals(other),
            _ => throw new UnsupportedSpecificationException(UnsupportedFormatExceptionMessage)
        };
    }
}