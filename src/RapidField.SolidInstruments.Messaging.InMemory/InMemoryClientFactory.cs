// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.InMemory
{
    /// <summary>
    /// Represents an appliance that manages in-memory messaging clients.
    /// </summary>
    public sealed class InMemoryClientFactory : MessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage, IMessageTransportConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryClientFactory" /> class.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        public InMemoryClientFactory(IMessageTransportConnection connection)
            : base(connection)
        {
            Transport = connection.Transport;
        }

        /// <summary>
        /// Creates a new implementation-specific client that facilitates receive operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="entityPath">
        /// The unique path for the entity.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates receive operations.
        /// </returns>
        protected sealed override IMessagingEntityReceiveClient CreateMessageReceiver<TMessage>(IMessageTransportConnection connection, MessagingEntityType entityType, IMessagingEntityPath entityPath, String subscriptionName) => entityType switch
        {
            MessagingEntityType.Queue => CreateQueueClient<TMessage>(connection, entityPath),
            MessagingEntityType.Topic => CreateSubscriptionClient<TMessage>(connection, entityPath, subscriptionName),
            _ => throw new UnsupportedSpecificationException($"The specified entity type, {entityType}, is not supported.")
        };

        /// <summary>
        /// Creates a new implementation-specific client that facilitates send operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="entityPath">
        /// The unique path for the entity.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates send operations.
        /// </returns>
        protected sealed override IMessagingEntitySendClient CreateMessageSender<TMessage>(IMessageTransportConnection connection, MessagingEntityType entityType, IMessagingEntityPath entityPath) => entityType switch
        {
            MessagingEntityType.Queue => CreateQueueClient<TMessage>(connection, entityPath),
            MessagingEntityType.Topic => CreateTopicClient<TMessage>(connection, entityPath),
            _ => throw new UnsupportedSpecificationException($"The specified entity type, {entityType}, is not supported.")
        };

        /// <summary>
        /// Releases all resources consumed by the current <see cref="InMemoryClientFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a new <see cref="IMessageQueueClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="queuePath">
        /// The unique path for the queue.
        /// </param>
        /// <returns>
        /// A new <see cref="IMessageQueueClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IMessageQueueClient CreateQueueClient<TMessage>(IMessageTransportConnection connection, IMessagingEntityPath queuePath)
            where TMessage : class
        {
            EnsureQueueExistanceAsync(queuePath).Wait();
            return new MessageQueueClient(connection, queuePath);
        }

        /// <summary>
        /// Creates a new <see cref="IMessageSubscriptionClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="topicPath">
        /// A unique path for the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// A name for the subscription.
        /// </param>
        /// <returns>
        /// A new <see cref="IMessageSubscriptionClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IMessageSubscriptionClient CreateSubscriptionClient<TMessage>(IMessageTransportConnection connection, IMessagingEntityPath topicPath, String subscriptionName)
            where TMessage : class
        {
            EnsureSubscriptionExistanceAsync(topicPath, subscriptionName).Wait();
            return new MessageSubscriptionClient(connection, topicPath, subscriptionName);
        }

        /// <summary>
        /// Creates a new <see cref="IMessageTopicClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <param name="topicPath">
        /// A unique path for the topic.
        /// </param>
        /// <returns>
        /// A new <see cref="IMessageTopicClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IMessageTopicClient CreateTopicClient<TMessage>(IMessageTransportConnection connection, IMessagingEntityPath topicPath)
            where TMessage : class
        {
            EnsureTopicExistanceAsync(topicPath).Wait();
            return new MessageTopicClient(connection, topicPath);
        }

        /// <summary>
        /// Asynchronously creates the specified in-memory queue if it does not exist.
        /// </summary>
        /// <param name="queuePath">
        /// The queue entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task EnsureQueueExistanceAsync(IMessagingEntityPath queuePath) => Task.Factory.StartNew(async () =>
        {
            if (Transport.QueueExists(queuePath))
            {
                return;
            }

            await Transport.CreateQueueAsync(queuePath).ConfigureAwait(false);
        });

        /// <summary>
        /// Asynchronously creates the specified in-memory subscription if it does not exist.
        /// </summary>
        /// <param name="topicPath">
        /// The topic entity path for the subscription.
        /// </param>
        /// <param name="subscriptionName">
        /// The name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task EnsureSubscriptionExistanceAsync(IMessagingEntityPath topicPath, String subscriptionName) => Task.Factory.StartNew(async () =>
        {
            await EnsureTopicExistanceAsync(topicPath).ConfigureAwait(false);

            if (Transport.SubscriptionExists(topicPath, subscriptionName))
            {
                return;
            }

            await Transport.CreateSubscriptionAsync(topicPath, subscriptionName).ConfigureAwait(false);
        });

        /// <summary>
        /// Asynchronously creates the specified in-memory topic if it does not exist.
        /// </summary>
        /// <param name="topicPath">
        /// The topic entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private Task EnsureTopicExistanceAsync(IMessagingEntityPath topicPath) => Task.Factory.StartNew(async () =>
        {
            if (Transport.TopicExists(topicPath))
            {
                return;
            }

            await Transport.CreateTopicAsync(topicPath).ConfigureAwait(false);
        });

        /// <summary>
        /// Represents the transport for which the current <see cref="InMemoryClientFactory" /> creates clients.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IMessageTransport Transport;
    }
}