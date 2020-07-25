// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessageTopic" /> subscription.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageSubscriptionClient" /> is the default implementation of <see cref="IMessageSubscriptionClient" />.
    /// </remarks>
    internal sealed class MessageSubscriptionClient : MessagingEntityClient, IMessageSubscriptionClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriptionClient" /> class.
        /// </summary>
        /// <param name="connection">
        /// The client's connection to the associated entity's transport.
        /// </param>
        /// <param name="path">
        /// The unique textual path for the messaging entity with which the client transacts.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the associated subscription.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" /> -or-
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageSubscriptionClient(IMessageTransportConnection connection, IMessagingEntityPath path, String subscriptionName)
            : base(connection, path, MessagingEntityType.Topic)
        {
            SubscriptionName = subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName));
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
        protected override void RegisterMessageHandler(IMessageTransportConnection connection, Action<PrimitiveMessage> handleMessageAction) => EnsureSubscriptionExistanceAsync(Path, SubscriptionName).ContinueWith(ensureSubscriptionExistenceTask =>
        {
            connection.RegisterSubscriptionHandler(Path, SubscriptionName, handleMessageAction);
        }).Wait();

        /// <summary>
        /// Gets the unique name of the associated subscription.
        /// </summary>
        public String SubscriptionName
        {
            get;
        }
    }
}