// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging.InMemory
{
    /// <summary>
    /// Facilitates conversion of general-format messages to and from in-memory messages.
    /// </summary>
    public sealed class InMemoryMessageAdapter : MessageAdapter<PrimitiveMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryMessageAdapter" /> class.
        /// </summary>
        public InMemoryMessageAdapter()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryMessageAdapter" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public InMemoryMessageAdapter(SerializationFormat messageSerializationFormat)
            : base(messageSerializationFormat)
        {
            return;
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
        /// <param name="serializer">
        /// A serializer that is used to serialize the message.
        /// </param>
        /// <returns>
        /// The implementation-specific message.
        /// </returns>
        protected sealed override PrimitiveMessage ConvertForward<TMessage>(TMessage message, ISerializer<TMessage> serializer) => new PrimitiveMessage(message, new MessageLockToken(Guid.NewGuid(), message.Identifier), MessageSerializationFormat);

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
        protected sealed override TMessage ConvertReverse<TMessage>(PrimitiveMessage message, ISerializer<TMessage> serializer) => message.GetBody<TMessage>();
    }
}