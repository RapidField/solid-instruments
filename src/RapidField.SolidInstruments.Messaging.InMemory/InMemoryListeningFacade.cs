// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.InMemory
{
    /// <summary>
    /// Facilitates listening operations for in-memory queues.
    /// </summary>
    public sealed class InMemoryListeningFacade : MessageListeningFacade<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage, InMemoryTransmittingFacade>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryListeningFacade" /> class.
        /// </summary>
        /// <param name="transmittingFacade">
        /// An implementation-specific messaging facade that is used to transmit response messages.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="transmittingFacade" /> is <see langword="null" />.
        /// </exception>
        public InMemoryListeningFacade(InMemoryTransmittingFacade transmittingFacade)
            : base(transmittingFacade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InMemoryListeningFacade" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="receiveClient">
        /// An implementation-specific receive client.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void RegisterMessageHandler(Action<PrimitiveMessage> messageHandler, IMessagingEntityReceiveClient receiveClient, IConcurrencyControlToken controlToken)
        {
            var messageHandlerAction = new Action<PrimitiveMessage>((message) =>
            {
                var lockToken = (MessageLockToken)null;

                try
                {
                    lockToken = message.LockToken;

                    if (lockToken is null)
                    {
                        throw new MessageListeningException("The message cannot be processed because the lock token is invalid.");
                    }
                    else if (receiveClient.Connection.State == MessageTransportConnectionState.Closed)
                    {
                        throw new MessageListeningException("The message cannot be processed because the receive client is unavailable.");
                    }

                    messageHandler(message);
                    receiveClient.ConveySuccessAsync(lockToken).Wait();
                }
                catch
                {
                    receiveClient.ConveyFailureAsync(lockToken).Wait();
                }
            });

            receiveClient.RegisterMessageHandler(messageHandlerAction);
        }

        /// <summary>
        /// Asynchronously sends the specified message to a dead letter queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="adaptedMessage">
        /// The adapted message.
        /// </param>
        /// <param name="message">
        /// The message to route to a dead letter queue.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task RouteToDeadLetterQueueAsync<TMessage>(PrimitiveMessage adaptedMessage, TMessage message, IEnumerable<String> pathLabels, MessagingEntityType entityType)
        {
            var deadLetterQueueSender = ClientFactory.GetQueueSender<TMessage>(AppendDeadLetterQueueLabel(pathLabels));
            return deadLetterQueueSender.SendAsync(adaptedMessage);
        }
    }
}