// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.TextEncoding
{
    /// <summary>
    /// Represents a 128-bit globally unique identifier that is expressed as a 26-character z-base-32 encoded string.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode, Size = 16)]
    public struct EnhancedReadabilityGuid : IComparable<EnhancedReadabilityGuid>, IEquatable<EnhancedReadabilityGuid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedReadabilityGuid" /> structure.
        /// </summary>
        /// <param name="bytes">
        /// An array of sixteen bytes representing the new <see cref="EnhancedReadabilityGuid" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="bytes" /> does not contain exactly sixteen bytes.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes" /> is <see langword="null" />.
        /// </exception>
        public EnhancedReadabilityGuid(Byte[] bytes)
            : this(new Guid(bytes))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedReadabilityGuid" /> structure.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Guid" /> representing the new <see cref="EnhancedReadabilityGuid" />.
        /// </param>
        public EnhancedReadabilityGuid(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Facilitates implicit <see cref="EnhancedReadabilityGuid" /> to <see cref="Guid" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Guid(EnhancedReadabilityGuid target) => target.Value;

        /// <summary>
        /// Determines whether or not two specified <see cref="EnhancedReadabilityGuid" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="EnhancedReadabilityGuid" /> instance is less than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="EnhancedReadabilityGuid" /> instance is less than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="EnhancedReadabilityGuid" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b)
        {
            if ((Object)a is null && (Object)b is null)
            {
                return true;
            }
            else if ((Object)a is null || (Object)b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="EnhancedReadabilityGuid" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="EnhancedReadabilityGuid" /> instance is greater than or equal to
        /// another supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="EnhancedReadabilityGuid" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(EnhancedReadabilityGuid a, EnhancedReadabilityGuid b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its
        /// <see cref="EnhancedReadabilityGuid" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a <see cref="EnhancedReadabilityGuid" />.
        /// </param>
        /// <returns>
        /// A <see cref="EnhancedReadabilityGuid" /> that is equivalent to <paramref name="input" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a <see cref="EnhancedReadabilityGuid" />.
        /// </exception>
        public static EnhancedReadabilityGuid Parse(String input)
        {
            if (Parse(input, out var value, true))
            {
                return new EnhancedReadabilityGuid(value);
            }

            return default;
        }

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its
        /// <see cref="EnhancedReadabilityGuid" /> equivalent. The method returns a value that indicates whether the conversion
        /// succeeded.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a <see cref="EnhancedReadabilityGuid" />.
        /// </param>
        /// <param name="result">
        /// The parsed result if the operation is successful, otherwise the default instance.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the parse operation is successful, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean TryParse(String input, out EnhancedReadabilityGuid result)
        {
            if (Parse(input, out var value, false))
            {
                result = new EnhancedReadabilityGuid(value);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Compares the current <see cref="EnhancedReadabilityGuid" /> to the specified object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="EnhancedReadabilityGuid" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(EnhancedReadabilityGuid other) => Value.CompareTo(other.Value);

        /// <summary>
        /// Determines whether or not the current <see cref="EnhancedReadabilityGuid" /> is equal to the specified
        /// <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is EnhancedReadabilityGuid guid)
            {
                return Equals(guid);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="EnhancedReadabilityGuid" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="EnhancedReadabilityGuid" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(EnhancedReadabilityGuid other) => (Value == other.Value);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedReadabilityGuid" /> structure.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="EnhancedReadabilityGuid" /> structure.
        /// </returns>
        public static EnhancedReadabilityGuid New() => new EnhancedReadabilityGuid(Guid.NewGuid());

        /// <summary>
        /// Converts the current <see cref="EnhancedReadabilityGuid" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="EnhancedReadabilityGuid" />.
        /// </returns>
        public Byte[] ToByteArray() => Value.ToByteArray();

        /// <summary>
        /// Converts the value of the current <see cref="EnhancedReadabilityGuid" /> to its equivalent <see cref="Guid" />
        /// representation.
        /// </summary>
        /// <returns>
        /// A <see cref="Guid" /> representation of the current <see cref="EnhancedReadabilityGuid" />.
        /// </returns>
        public Guid ToGuid() => new Guid(Value.ToByteArray());

        /// <summary>
        /// Converts the value of the current <see cref="EnhancedReadabilityGuid" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="EnhancedReadabilityGuid" />.
        /// </returns>
        public override String ToString() => new String(Encoding.GetChars(Value.ToByteArray()));

        /// <summary>
        /// Converts the specified <see cref="String" /> representation of a time of day value to its
        /// <see cref="EnhancedReadabilityGuid" /> equivalent.
        /// </summary>
        /// <param name="input">
        /// A <see cref="String" /> containing a time of day value to convert.
        /// </param>
        /// <param name="value">
        /// A reference to the <see cref="Guid" /> component of the resulting <see cref="EnhancedReadabilityGuid" />.
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
        /// <paramref name="input" /> does not contain a valid representation of a <see cref="EnhancedReadabilityGuid" />.
        /// </exception>
        [DebuggerHidden]
        private static Boolean Parse(String input, out Guid value, Boolean raiseExceptionOnFail)
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
                value = default;
                return false;
            }

            var lowercaseInput = input.ToLowerInvariant();

            if (raiseExceptionOnFail)
            {
                try
                {
                    value = new Guid(Encoding.GetBytes(lowercaseInput).Take(16).ToArray());
                    return true;
                }
                catch (ArgumentException exception)
                {
                    throw new FormatException($"The specified value, \"{input}\", could not be parsed as a {nameof(EnhancedReadabilityGuid)}.", exception);
                }
            }
            else if (lowercaseInput.Length == EncodedStringLength)
            {
                foreach (var character in lowercaseInput)
                {
                    if (Encoding.Alphabet.Contains(character))
                    {
                        continue;
                    }

                    value = default;
                    return false;
                }

                value = new Guid(Encoding.GetBytes(lowercaseInput).Take(16).ToArray());
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Initializes or reinitializes the instance using the specified serialized representation.
        /// </summary>
        /// <param name="input">
        /// A string representation of a <see cref="EnhancedReadabilityGuid" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="input" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input" /> does not contain a valid representation of a <see cref="EnhancedReadabilityGuid" />.
        /// </exception>
        [DebuggerHidden]
        private void Initialize(String input)
        {
            Parse(input, out var value, true);
            Value = value;
        }

        /// <summary>
        /// Gets or sets a serialized representation of the current <see cref="EnhancedReadabilityGuid" />.
        /// </summary>
        [DataMember(EmitDefaultValue = true, IsRequired = true, Name = "Value")]
        private String SerializationValue
        {
            [DebuggerHidden]
            get => ToString();
            [DebuggerHidden]
            set => Initialize(value);
        }

        /// <summary>
        /// Represents the character length of an encoded <see cref="EnhancedReadabilityGuid" /> string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 EncodedStringLength = 26;

        /// <summary>
        /// Represents the encoding specification used to encode <see cref="EnhancedReadabilityGuid" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Base32Encoding Encoding = Base32Encoding.ZBase32;

        /// <summary>
        /// Represents the <see cref="Guid" /> representation of the current <see cref="EnhancedReadabilityGuid" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        [IgnoreDataMember]
        private Guid Value;
    }
}