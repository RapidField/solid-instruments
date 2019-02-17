// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates conversion of general-format messages to and from implementation-specific messages.
    /// </summary>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    /// <remarks>
    /// <see cref="MessageAdapter{TAdaptedMessage}" /> is the default implementation of
    /// <see cref="IMessageAdapter{TAdaptedMessage}" />.
    /// </remarks>
    public abstract class MessageAdapter<TAdaptedMessage> : IMessageAdapter<TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAdapter{TAdaptedMessage}" /> class.
        /// </summary>
        protected MessageAdapter()
            : this(DefaultMessageSerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAdapter{TAdaptedMessage}" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        protected MessageAdapter(SerializationFormat messageSerializationFormat)
        {
            MessageSerializationFormat = messageSerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageSerializationFormat));
        }

        /// <summary>
        /// Converts the specified general-format message to an implementation-specific message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// A general-format message to convert.
        /// </param>
        /// <returns>
        /// The implementation-specific message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        public TAdaptedMessage ConvertForward<TMessage>(TMessage message)
            where TMessage : class, IMessageBase => ConvertForward(message.RejectIf().IsNull(nameof(message)), new DynamicSerializer<TMessage>(MessageSerializationFormat));

        /// <summary>
        /// Converts the specified implementation-specific message to a general-format message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// An implementation-specific message to convert.
        /// </param>
        /// <returns>
        /// The general-format message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        public TMessage ConvertReverse<TMessage>(TAdaptedMessage message)
            where TMessage : class, IMessageBase => ConvertReverse(message.RejectIf().IsNull(nameof(message)), new DynamicSerializer<TMessage>(MessageSerializationFormat));

        /// <summary>
        /// Converts the specified general-format message to an implementation-specific message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// A general-format message to convert.
        /// </param>
        /// <param name="serializer">
        /// A serializer that is used to serialize the message.
        /// </param>
        /// <returns>
        /// The implementation-specific message.
        /// </returns>
        protected abstract TAdaptedMessage ConvertForward<TMessage>(TMessage message, ISerializer<TMessage> serializer)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Converts the specified implementation-specific message to a general-format message.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="message">
        /// An implementation-specific message to convert.
        /// </param>
        /// <param name="serializer">
        /// A serializer that is used to serialize the message.
        /// </param>
        /// <returns>
        /// The general-format message.
        /// </returns>
        protected abstract TMessage ConvertReverse<TMessage>(TAdaptedMessage message, ISerializer<TMessage> serializer)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Gets the type of implementation-specific adapted messages.
        /// </summary>
        public Type AdaptedMessageType => typeof(TAdaptedMessage);

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