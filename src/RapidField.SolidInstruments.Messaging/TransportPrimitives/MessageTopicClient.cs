// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessageTopic" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTopicClient" /> is the default implementation of <see cref="IMessageTopicClient" />.
    /// </remarks>
    internal sealed class MessageTopicClient : MessagingEntityClient, IMessageTopicClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopicClient" /> class.
        /// </summary>
        /// <param name="connection">
        /// The client's connection to the associated entity's transport.
        /// </param>
        /// <param name="path">
        /// The unique textual path for the messaging entity with which the client transacts.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopicClient(IMessageTransportConnection connection, IMessagingEntityPath path)
            : base(connection, path, MessagingEntityType.Topic)
        {
            return;
        }

        /// <summary>
        /// Registers the specified message handler for the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="connection">
        /// The connection with which to register <paramref name="handleMessageAction" />.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        protected override void RegisterMessageHandler(IMessageTransportConnection connection, Action<PrimitiveMessage> handleMessageAction) => throw new NotImplementedException("Message handlers cannot be registered with a topic. Message handlers for topics should be registered using a subscription client.");
    }
}