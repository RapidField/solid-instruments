// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents an exclusive processing lock on a <see cref="PrimitiveMessage" />.
    /// </summary>
    [DataContract]
    public sealed class MessageLockToken : IComparable<MessageLockToken>, IEquatable<MessageLockToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLockToken" /> class.
        /// </summary>
        [DebuggerHidden]
        internal MessageLockToken()
            : this(default, default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLockToken" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the lock token.
        /// </param>
        /// <param name="messageIdentifier">
        /// The unique identifier for the associated, locked message.
        /// </param>
        [DebuggerHidden]
        internal MessageLockToken(Guid identifier, Guid messageIdentifier)
            : this(identifier, messageIdentifier, default)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLockToken" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the lock token.
        /// </param>
        /// <param name="messageIdentifier">
        /// The unique identifier for the associated, locked message.
        /// </param>
        /// <param name="expirationDateTime">
        /// The date and time of expiration for the lock, after which the message will become available for processing.
        /// </param>
        [DebuggerHidden]
        internal MessageLockToken(Guid identifier, Guid messageIdentifier, DateTime expirationDateTime)
            : this(default, identifier, messageIdentifier, expirationDateTime)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLockToken" /> class.
        /// </summary>
        /// <param name="deliveryTag">
        /// A transport-issued tracking identifier, or <see langword="default" /> if the implementation does not utilize delivery
        /// tags.
        /// </param>
        /// <param name="identifier">
        /// A unique identifier for the lock token.
        /// </param>
        /// <param name="messageIdentifier">
        /// The unique identifier for the associated, locked message.
        /// </param>
        /// <param name="expirationDateTime">
        /// The date and time of expiration for the lock, after which the message will become available for processing.
        /// </param>
        [DebuggerHidden]
        internal MessageLockToken(UInt64 deliveryTag, Guid identifier, Guid messageIdentifier, DateTime expirationDateTime)
        {
            DeliveryTag = deliveryTag;
            ExpirationDateTime = expirationDateTime;
            Identifier = identifier;
            MessageIdentifier = messageIdentifier;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="MessageLockToken" /> instances are not equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are not equal.
        /// </returns>
        public static Boolean operator !=(MessageLockToken a, MessageLockToken b) => (a == b) == false;

        /// <summary>
        /// Determines whether or not a specified <see cref="MessageLockToken" /> instance is less than another specified instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator <(MessageLockToken a, MessageLockToken b) => a.CompareTo(b) == -1;

        /// <summary>
        /// Determines whether or not a specified <see cref="MessageLockToken" /> instance is less than or equal to another supplied
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is earlier than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator <=(MessageLockToken a, MessageLockToken b) => a.CompareTo(b) < 1;

        /// <summary>
        /// Determines whether or not two specified <see cref="MessageLockToken" /> instances are equal.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public static Boolean operator ==(MessageLockToken a, MessageLockToken b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether or not a specified <see cref="MessageLockToken" /> instance is greater than another specified
        /// instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than the first object, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean operator >(MessageLockToken a, MessageLockToken b) => a.CompareTo(b) == 1;

        /// <summary>
        /// Determines whether or not a specified <see cref="MessageLockToken" /> instance is greater than or equal to another
        /// supplied instance.
        /// </summary>
        /// <param name="a">
        /// The first <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <param name="b">
        /// The second <see cref="MessageLockToken" /> instance to compare.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the second object is later than or equal to the first object, otherwise
        /// <see langword="false" />.
        /// </returns>
        public static Boolean operator >=(MessageLockToken a, MessageLockToken b) => a.CompareTo(b) > -1;

        /// <summary>
        /// Compares the current <see cref="MessageLockToken" /> to the specified object and returns an indication of their relative
        /// values.
        /// </summary>
        /// <param name="other">
        /// The <see cref="MessageLockToken" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// Negative one if this instance is earlier than the specified instance; one if this instance is later than the supplied
        /// instance; zero if they are equal.
        /// </returns>
        public Int32 CompareTo(MessageLockToken other)
        {
            var identifierComparisonResult = Identifier.CompareTo(other.Identifier);

            if (identifierComparisonResult != 0)
            {
                return identifierComparisonResult;
            }

            var expirationDateTimeComparisonResult = ExpirationDateTime.CompareTo(other.ExpirationDateTime);

            if (expirationDateTimeComparisonResult != 0)
            {
                return expirationDateTimeComparisonResult;
            }

            return MessageIdentifier.CompareTo(other.MessageIdentifier);
        }

        /// <summary>
        /// Determines whether or not the current <see cref="MessageLockToken" /> is equal to the specified <see cref="Object" />.
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
            else if (obj is MessageLockToken token)
            {
                return Equals(token);
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not two specified <see cref="MessageLockToken" /> instances are equal.
        /// </summary>
        /// <param name="other">
        /// The <see cref="MessageLockToken" /> to compare to this instance.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the specified instances are equal.
        /// </returns>
        public Boolean Equals(MessageLockToken other)
        {
            if (ExpirationDateTime != other.ExpirationDateTime)
            {
                return false;
            }
            else if (Identifier != other.Identifier)
            {
                return false;
            }
            else if (DeliveryTag != other.DeliveryTag)
            {
                return false;
            }
            else if (MessageIdentifier != other.MessageIdentifier)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode() => ToByteArray().ComputeThirtyTwoBitHash();

        /// <summary>
        /// Converts the current <see cref="MessageLockToken" /> to an array of bytes.
        /// </summary>
        /// <returns>
        /// An array of bytes representing the current <see cref="MessageLockToken" />.
        /// </returns>
        public Byte[] ToByteArray()
        {
            var bytes = new List<Byte>();
            bytes.AddRange(ExpirationDateTime.ToByteArray());
            bytes.AddRange(Identifier.ToByteArray());
            bytes.AddRange(DeliveryTag.ToByteArray());
            bytes.AddRange(MessageIdentifier.ToByteArray());
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageLockToken" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageLockToken" />.
        /// </returns>
        public sealed override String ToString() => $"{{ {nameof(Identifier)}: {Identifier.ToSerializedString()}, {nameof(ExpirationDateTime)}: {ExpirationDateTime.ToSerializedString()} }}";

        /// <summary>
        /// Represents a transport-issued tracking identifier, or <see langword="default" /> if the implementation does not utilize
        /// delivery tags.
        /// </summary>
        [DataMember]
        internal readonly UInt64 DeliveryTag;

        /// <summary>
        /// Represents the date and time of expiration for the lock, after which the message will become available for processing.
        /// </summary>
        [DataMember]
        internal readonly DateTime ExpirationDateTime;

        /// <summary>
        /// Represents a unique identifier for the lock token.
        /// </summary>
        [DataMember]
        internal readonly Guid Identifier;

        /// <summary>
        /// Represents the unique identifier for the associated, locked message.
        /// </summary>
        [DataMember]
        internal readonly Guid MessageIdentifier;
    }
}