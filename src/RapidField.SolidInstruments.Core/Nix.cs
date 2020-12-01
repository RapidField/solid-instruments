// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents nothing.
    /// </summary>
    /// <remarks>
    /// The <see cref="Nix" /> type is used to signify that a type parameter is not evaluated. Instances of <see cref="Nix" /> are
    /// used as alternatives to null references and default values, when appropriate.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 1)]
    public struct Nix : ICloneable, IComparable, IComparable<Nix>, IConvertible, IEquatable<Nix>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nix" /> structure.
        /// </summary>
        /// <param name="value">
        /// An ignored value.
        /// </param>
        [DebuggerHidden]
        private Nix(Byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="Nix" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(Nix a, Nix b) => (a == b) is false;

        /// <summary>
        /// Determines whether or not a specified <see cref="Nix" /> instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(Nix a, Nix b) => false;

        /// <summary>
        /// Determines whether or not a specified <see cref="Nix" /> instance is less than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(Nix a, Nix b) => true;

        /// <summary>
        /// Determines whether or not two specified <see cref="Nix" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(Nix a, Nix b) => true;

        /// <summary>
        /// Determines whether or not a specified <see cref="Nix" /> instance is greater than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(Nix a, Nix b) => false;

        /// <summary>
        /// Determines whether or not a specified <see cref="Nix" /> instance is greater than or equal to another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="Nix" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="Nix" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(Nix a, Nix b) => true;

        /// <summary>
        /// Creates a new object that is a copy of the current <see cref="Nix" />.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of the current <see cref="Nix" />.
        /// </returns>
        public Object Clone() => new Nix(0x00);

        /// <summary>
        /// Compares the current <see cref="Nix" /> to the specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Nix" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Nix other) => 0;

        /// <summary>
        /// Compares the current <see cref="Nix" /> to the specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(Object obj) => obj is Nix nix ? CompareTo(nix) : GetType().FullName.CompareTo(obj.GetType().FullName);

        /// <summary>
        /// Determines whether or not the current <see cref="Nix" /> is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj) => obj.GetType() == typeof(Nix);

        /// <summary>
        /// Determines whether or not two specified <see cref="Nix" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Nix" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(Nix other) => true;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => 0;

        /// <summary>
        /// Returns the <see cref="TypeCode" /> for the current <see cref="Nix" /> instance.
        /// </summary>
        /// <returns>
        /// The <see cref="TypeCode" /> for the current <see cref="Nix" /> instance.
        /// </returns>
        public TypeCode GetTypeCode() => TypeCode.Object;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent boolean representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A boolean representation of the current <see cref="Nix" />.
        /// </returns>
        public Boolean ToBoolean(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent 8-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An 8-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public Byte ToByte(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent character representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A character representation of the current <see cref="Nix" />.
        /// </returns>
        public Char ToChar(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent date-time representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A date-time representation of the current <see cref="Nix" />.
        /// </returns>
        public DateTime ToDateTime(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent decimal number representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A decimal number representation of the current <see cref="Nix" />.
        /// </returns>
        public Decimal ToDecimal(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent double-precision floating point number
        /// representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A double-precision floating point number representation of the current <see cref="Nix" />.
        /// </returns>
        public Double ToDouble(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent 16-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A 16-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public Int16 ToInt16(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent 32-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A 32-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public Int32 ToInt32(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent 64-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A 64-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public Int64 ToInt64(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent signed 8-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A signed 8-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public SByte ToSByte(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent single-precision floating point number
        /// representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A single-precision floating point number representation of the current <see cref="Nix" />.
        /// </returns>
        public Single ToSingle(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="Nix" />.
        /// </returns>
        public override String ToString() => String.Empty;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent string representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// A string representation of the current <see cref="Nix" />.
        /// </returns>
        public String ToString(IFormatProvider provider) => String.Empty;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to the specified type with an equivalent value.
        /// </summary>
        /// <param name="conversionType">
        /// The type to which the value of the current <see cref="Nix" /> is converted.
        /// </param>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An <see cref="Object" /> instance of type <paramref name="conversionType" /> whose value is equivalent to the value of
        /// the current <see cref="Nix" />.
        /// </returns>
        public Object ToType(Type conversionType, IFormatProvider provider) => conversionType.GetDefaultValue();

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent unsigned 16-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An unsigned 16-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public UInt16 ToUInt16(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent unsigned 32-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An unsigned 32-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public UInt32 ToUInt32(IFormatProvider provider) => default;

        /// <summary>
        /// Converts the value of the current <see cref="Nix" /> to its equivalent unsigned 64-bit integer representation.
        /// </summary>
        /// <param name="provider">
        /// An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// An unsigned 64-bit integer representation of the current <see cref="Nix" />.
        /// </returns>
        public UInt64 ToUInt64(IFormatProvider provider) => default;

        /// <summary>
        /// Represents an ignored value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private readonly Byte Value;

        /// <summary>
        /// Represents a static <see cref="Nix" /> instance.
        /// </summary>
        public static readonly Nix Instance = new(0x00);

        /// <summary>
        /// Represents a static reference to the <see cref="Nix" /> type.
        /// </summary>
        public static readonly Type Type = typeof(Nix);
    }
}