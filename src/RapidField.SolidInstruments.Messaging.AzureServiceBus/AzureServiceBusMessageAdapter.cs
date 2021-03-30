// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Serialization;
using RapidField.SolidInstruments.Serialization.Extensions;
using System;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Facilitates conversion of general-format messages to and from Azure Service Bus messages.
    /// </summary>
    public sealed class AzureServiceBusMessageAdapter : MessageAdapter<AzureServiceBusMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusMessageAdapter" /> class.
        /// </summary>
        public AzureServiceBusMessageAdapter()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusMessageAdapter" /> class.
        /// </summary>
        /// <param name="messageSerializationFormat">
        /// Gets the format that is used to serialize and deserialize messages. The default value is
        /// <see cref="SerializationFormat.Binary" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageSerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public AzureServiceBusMessageAdapter(SerializationFormat messageSerializationFormat)
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
        protected sealed override AzureServiceBusMessage ConvertForward<TMessage>(TMessage message, ISerializer<TMessage> serializer) => new()
        {
            Body = serializer.Serialize(message),
            ContentType = MessageSerializationFormat.ToMimeMediaType(),
            CorrelationId = message.CorrelationIdentifier.ToSerializedString(),
            Label = typeof(TMessage).ToString(),
            MessageId = message.Identifier.ToSerializedString()
        };

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
        protected sealed override TMessage ConvertReverse<TMessage>(AzureServiceBusMessage message, ISerializer<TMessage> serializer) => serializer.Deserialize(message.Body);
    }
}