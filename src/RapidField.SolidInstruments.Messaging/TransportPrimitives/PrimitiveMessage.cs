// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Serialization;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a serializable message.
    /// </summary>
    [DataContract]
    public sealed class PrimitiveMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveMessage" /> class.
        /// </summary>
        [DebuggerHidden]
        internal PrimitiveMessage()
            : this(null, null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveMessage" /> class.
        /// </summary>
        /// <param name="body">
        /// The message body.
        /// </param>
        /// <param name="lockToken">
        /// An exclusive lock key for the current message, which can be used to complete or abandon processing.
        /// </param>
        [DebuggerHidden]
        internal PrimitiveMessage(IMessageBase body, MessageLockToken lockToken)
            : this(body, lockToken, DefaultBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveMessage" /> class.
        /// </summary>
        /// <param name="body">
        /// The message body.
        /// </param>
        /// <param name="lockToken">
        /// An exclusive lock token for the current message, which can be used to complete or abandon processing.
        /// </param>
        /// <param name="bodySerializationFormat">
        /// The format that is used to serialize and deserialize the message body. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        [DebuggerHidden]
        internal PrimitiveMessage(IMessageBase body, MessageLockToken lockToken, SerializationFormat bodySerializationFormat)
        {
            Body = body;
            BodyTypeName = body?.GetType().ToString();
            BodySerializationFormat = bodySerializationFormat;
            LockToken = lockToken;
        }

        /// <summary>
        /// Converts the value of the current <see cref="PrimitiveMessage" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="PrimitiveMessage" />.
        /// </returns>
        public sealed override String ToString() => BodyTypeName.IsNullOrEmpty() ? $"{Identifier.ToSerializedString()}" : $"{Identifier.ToSerializedString()} | {BodyTypeName}";

        /// <summary>
        /// Returns the typed message body.
        /// </summary>
        /// <typeparam name="TBody">
        /// The type of the message body.
        /// </typeparam>
        /// <returns>
        /// The typed message body, or <see langword="null" /> if the body is <see langword="null" />.
        /// </returns>
        [DebuggerHidden]
        internal TBody GetBody<TBody>()
            where TBody : class, IMessageBase => Body as TBody;

        /// <summary>
        /// Deserializes the specified message body.
        /// </summary>
        /// <param name="serializedBody">
        /// A serialized, encoded message body.
        /// </param>
        /// <exception cref="SerializationException">
        /// An error occurred during deserialization.
        /// </exception>
        [DebuggerHidden]
        private void DeserializeBody(String serializedBody)
        {
            if (serializedBody.IsNullOrEmpty())
            {
                Body = null;
                return;
            }

            try
            {
                var buffer = SerializedBodyEncoding.GetBytes(serializedBody);
                Body = DynamicSerializer.Deserialize(buffer, BodyType, BodySerializationFormat) as IMessageBase;
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during deserialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Serializes and encodes the message body.
        /// </summary>
        /// <returns>
        /// The serialized, encoded message body -or- <see cref="String.Empty" /> if the body is null.
        /// </returns>
        /// <exception cref="SerializationException">
        /// An error occurred during serialization.
        /// </exception>
        [DebuggerHidden]
        private String SerializeBody()
        {
            if (Body is null)
            {
                return String.Empty;
            }

            try
            {
                var buffer = DynamicSerializer.Serialize(Body, BodyType, BodySerializationFormat);
                return SerializedBodyEncoding.GetString(buffer);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SerializationException("An error occurred during serialization. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Gets or sets the format that is used to serialize and deserialize the message body.
        /// </summary>
        [DataMember(Order = 2)]
        public SerializationFormat BodySerializationFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the message body type.
        /// </summary>
        [DataMember(Order = 1)]
        public String BodyTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a unique identifier that is assigned to related messages.
        /// </summary>
        [IgnoreDataMember]
        public Guid CorrelationIdentifier => Body.CorrelationIdentifier;

        /// <summary>
        /// Gets a unique identifier for the message.
        /// </summary>
        [IgnoreDataMember]
        public Guid Identifier => Body.Identifier;

        /// <summary>
        /// Gets instructions and contextual information relating to processing for the current <see cref="PrimitiveMessage" />.
        /// </summary>
        [IgnoreDataMember]
        public IMessageProcessingInformation ProcessingInformation => Body.ProcessingInformation;

        /// <summary>
        /// Gets or sets an exclusive lock token for the current message, which can be used to complete or abandon processing, or
        /// <see langword="null" /> if the message is not locked.
        /// </summary>
        [DataMember(Order = 4)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal MessageLockToken LockToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the serialized message body.
        /// </summary>
        /// <exception cref="SerializationException">
        /// An error occurred during serialization or deserialization.
        /// </exception>
        [DataMember(Name = SerializedBodyDataMemberName, Order = 3)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String SerializedBody
        {
            [DebuggerHidden]
            get => SerializeBody();
            [DebuggerHidden]
            set => DeserializeBody(value);
        }

        /// <summary>
        /// Gets the type of <see cref="Body" /> as specified by <see cref="BodyTypeName" />.
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <see cref="BodyTypeName" /> names an invalid or inaccessible type.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private Type BodyType
        {
            [DebuggerHidden]
            get
            {
                if (BodyTypeName.IsNullOrEmpty())
                {
                    return null;
                }

                try
                {
                    return AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(BodyTypeName)).First(type => type is not null);
                }
                catch (Exception exception)
                {
                    throw new TypeAccessException($"The type {BodyTypeName} is invalid or could not be accessed. See inner exception.", exception);
                }
            }
        }

        /// <summary>
        /// Represents the default format to use for message body serialization and deserialization.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const SerializationFormat DefaultBodySerializationFormat = SerializationFormat.CompressedJson;

        /// <summary>
        /// Represents the name of <see cref="SerializedBody" /> in serialization contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SerializedBodyDataMemberName = "Body";

        /// <summary>
        /// Represents the text encoding that is used to encode and decode <see cref="SerializedBody" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Encoding SerializedBodyEncoding = Base32Encoding.Default;

        /// <summary>
        /// Represents the message body.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private IMessageBase Body;
    }
}