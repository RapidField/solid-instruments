// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates messaging operations for a message bus.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagingClient" /> is the default implementation of <see cref="IMessagingClient" />.
    /// </remarks>
    public abstract class MessagingClient : Instrument, IMessagingClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingClient" /> class.
        /// </summary>
        protected MessagingClient()
            : this(DefaultMessageSerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        protected MessagingClient(SerializationFormat messageSerializationFormat)
            : this(messageSerializationFormat, DefaultStateControlMode)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.ProcessorCountSemaphore" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected MessagingClient(SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode)
            : this(messageSerializationFormat, stateControlMode, Timeout.InfiniteTimeSpan)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingClient" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.ProcessorCountSemaphore" />.
        /// </param>
        /// <param name="operationTimeoutThreshold">
        /// The maximum length of time for which an operation may block a thread before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> if indefinite thread blocking is permitted. The default value is
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" /> -or-
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" /> -or-
        /// <paramref name="operationTimeoutThreshold" /> is less than or equal to <see cref="TimeSpan.Zero" /> and is not equal to
        /// <see cref="Timeout.InfiniteTimeSpan" />.
        /// </exception>
        protected MessagingClient(SerializationFormat messageSerializationFormat, ConcurrencyControlMode stateControlMode, TimeSpan operationTimeoutThreshold)
            : base(stateControlMode, operationTimeoutThreshold)
        {
            MessageSerializationFormat = messageSerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageSerializationFormat));
        }

        /// <summary>
        /// Deserializes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// The message to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized result.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="message" /> is invalid or an error occurred during deserialization.
        /// </exception>
        protected TMessage DeserializeMessage<TMessage>(Byte[] message)
            where TMessage : class
        {
            var serializer = new DynamicSerializer<TMessage>(MessageSerializationFormat);
            return serializer.Deserialize(message);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagingClient" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Serializes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// The message to serialize.
        /// </param>
        /// <returns>
        /// The serialized result.
        /// </returns>
        /// <exception cref="SerializationException">
        /// <paramref name="message" /> is invalid or an error occurred during serialization.
        /// </exception>
        protected Byte[] SerializeMessage<TMessage>(TMessage message)
            where TMessage : class
        {
            var serializer = new DynamicSerializer<TMessage>(MessageSerializationFormat);
            return serializer.Serialize(message);
        }

        /// <summary>
        /// Gets the format that is used to serialize and deserialize messages.
        /// </summary>
        public SerializationFormat MessageSerializationFormat
        {
            get;
        }

        /// <summary>
        /// Represents the default format that is used to serialize and deserialize messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SerializationFormat DefaultMessageSerializationFormat = SerializationFormat.Binary;

        /// <summary>
        /// Represents the default concurrency control mode that is used to manage state.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ConcurrencyControlMode DefaultStateControlMode = ConcurrencyControlMode.ProcessorCountSemaphore;
    }
}