// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Represents an appliance that manages Azure Service Bus messaging clients.
    /// </summary>
    public sealed class AzureServiceBusClientFactory : MessagingClientFactory<ISenderClient, IReceiverClient, AzureServiceBusMessage, ServiceBusConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusClientFactory" /> class.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        public AzureServiceBusClientFactory(ServiceBusConnection connection)
            : base(connection)
        {
            ManagementClient = new ManagementClient(connection.Endpoint.ToString(), connection.TokenProvider);
        }

        /// <summary>
        /// Creates a new implementation-specific client that facilitates receive operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates receive operations.
        /// </returns>
        protected sealed override IReceiverClient CreateMessageReceiver<TMessage>(MessagingEntityType entityType, ServiceBusConnection connection)
        {
            switch (entityType)
            {
                case MessagingEntityType.Queue:

                    return CreateQueueClient<TMessage>(connection);

                case MessagingEntityType.Topic:

                    return CreateSubscriptionClient<TMessage>(connection);

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Creates a new implementation-specific client that facilitates send operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new implementation-specific client that facilitates send operations.
        /// </returns>
        protected sealed override ISenderClient CreateMessageSender<TMessage>(MessagingEntityType entityType, ServiceBusConnection connection)
        {
            switch (entityType)
            {
                case MessagingEntityType.Queue:

                    return CreateQueueClient<TMessage>(connection);

                case MessagingEntityType.Topic:

                    return CreateTopicClient<TMessage>(connection);

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusClientFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a new <see cref="IQueueClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new <see cref="IQueueClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IQueueClient CreateQueueClient<TMessage>(ServiceBusConnection connection)
            where TMessage : class
        {
            var queuePath = GetQueuePath<TMessage>();
            EnsureQueueExistanceAsync(queuePath).Wait();
            return new QueueClient(connection, queuePath, ReceiveBehavior, RetryBehavior);
        }

        /// <summary>
        /// Creates a new <see cref="ISubscriptionClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new <see cref="ISubscriptionClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ISubscriptionClient CreateSubscriptionClient<TMessage>(ServiceBusConnection connection)
            where TMessage : class
        {
            var topicPath = GetTopicPath<TMessage>();
            var subscriptionName = GetSubscriptionName<TMessage>();
            EnsureSubscriptionExistanceAsync(topicPath, subscriptionName).Wait();
            return new SubscriptionClient(connection, topicPath, subscriptionName, ReceiveBehavior, RetryBehavior);
        }

        /// <summary>
        /// Creates a new <see cref="ITopicClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="connection">
        /// A connection that governs interaction with messaging entities.
        /// </param>
        /// <returns>
        /// A new <see cref="ITopicClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ITopicClient CreateTopicClient<TMessage>(ServiceBusConnection connection)
            where TMessage : class
        {
            var topicPath = GetTopicPath<TMessage>();
            EnsureTopicExistanceAsync(topicPath).Wait();
            return new TopicClient(connection, topicPath, RetryBehavior);
        }

        /// <summary>
        /// Asynchronously creates the specified Azure Service Bus queue if it does not exist.
        /// </summary>
        /// <param name="queuePath">
        /// The queue entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private async Task EnsureQueueExistanceAsync(String queuePath)
        {
            var queueExists = await ManagementClient.QueueExistsAsync(queuePath).ConfigureAwait(false);

            if (queueExists)
            {
                return;
            }

            await ManagementClient.CreateQueueAsync(queuePath).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously creates the specified Azure Service Bus subscription if it does not exist.
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
        private async Task EnsureSubscriptionExistanceAsync(String topicPath, String subscriptionName)
        {
            await EnsureTopicExistanceAsync(topicPath).ConfigureAwait(false);
            var subscriptionExists = await ManagementClient.SubscriptionExistsAsync(topicPath, subscriptionName).ConfigureAwait(false);

            if (subscriptionExists)
            {
                return;
            }

            await ManagementClient.CreateSubscriptionAsync(topicPath, subscriptionName);
        }

        /// <summary>
        /// Asynchronously creates the specified Azure Service Bus topic if it does not exist.
        /// </summary>
        /// <param name="topicPath">
        /// The topic entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        [DebuggerHidden]
        private async Task EnsureTopicExistanceAsync(String topicPath)
        {
            var topicExists = await ManagementClient.TopicExistsAsync(topicPath).ConfigureAwait(false);

            if (topicExists)
            {
                return;
            }

            await ManagementClient.CreateTopicAsync(topicPath).ConfigureAwait(false);
        }

        /// <summary>
        /// Represents the behavior used by clients when receiving messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ReceiveMode ReceiveBehavior = ReceiveMode.PeekLock;

        /// <summary>
        /// Represents the behavior used by clients when retrying an operation.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly RetryPolicy RetryBehavior = RetryPolicy.Default;

        /// <summary>
        /// Represents a client that manages Azure Service Bus entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ManagementClient ManagementClient;
    }
}