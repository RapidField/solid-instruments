// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using RapidField.SolidInstruments.Core;
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
        protected sealed override IReceiverClient CreateMessageReceiver<TMessage>(ServiceBusConnection connection, MessagingEntityType entityType, IMessagingEntityPath entityPath, String subscriptionName) => entityType switch
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
        protected sealed override ISenderClient CreateMessageSender<TMessage>(ServiceBusConnection connection, MessagingEntityType entityType, IMessagingEntityPath entityPath) => entityType switch
        {
            MessagingEntityType.Queue => CreateQueueClient<TMessage>(connection, entityPath),
            MessagingEntityType.Topic => CreateTopicClient<TMessage>(connection, entityPath),
            _ => throw new UnsupportedSpecificationException($"The specified entity type, {entityType}, is not supported.")
        };

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
        /// <param name="queuePath">
        /// The unique path for the queue.
        /// </param>
        /// <returns>
        /// A new <see cref="IQueueClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IQueueClient CreateQueueClient<TMessage>(ServiceBusConnection connection, IMessagingEntityPath queuePath)
            where TMessage : class
        {
            EnsureQueueExistanceAsync(queuePath).Wait();
            return new QueueClient(connection, queuePath.ToString(), ReceiveBehavior, RetryBehavior);
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
        /// <param name="topicPath">
        /// A unique path for the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// A name for the subscription.
        /// </param>
        /// <returns>
        /// A new <see cref="ISubscriptionClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ISubscriptionClient CreateSubscriptionClient<TMessage>(ServiceBusConnection connection, IMessagingEntityPath topicPath, String subscriptionName)
            where TMessage : class
        {
            EnsureSubscriptionExistanceAsync(topicPath, subscriptionName).Wait();
            return new SubscriptionClient(connection, topicPath.ToString(), subscriptionName, ReceiveBehavior, RetryBehavior);
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
        /// <param name="topicPath">
        /// A unique path for the topic.
        /// </param>
        /// <returns>
        /// A new <see cref="ITopicClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ITopicClient CreateTopicClient<TMessage>(ServiceBusConnection connection, IMessagingEntityPath topicPath)
            where TMessage : class
        {
            EnsureTopicExistanceAsync(topicPath).Wait();
            return new TopicClient(connection, topicPath.ToString(), RetryBehavior);
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
        private Task EnsureQueueExistanceAsync(IMessagingEntityPath queuePath) => Task.Factory.StartNew(() =>
        {
            lock (EnsureQueueExistenceSyncRoot)
            {
                ManagementClient.QueueExistsAsync(queuePath.ToString()).ContinueWith(queueExistsTask =>
                {
                    var queueExists = queueExistsTask.Result;

                    if (queueExists)
                    {
                        return;
                    }

                    ManagementClient.CreateQueueAsync(queuePath.ToString()).Wait();
                }).Wait();
            }
        });

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
        private Task EnsureSubscriptionExistanceAsync(IMessagingEntityPath topicPath, String subscriptionName) => Task.Factory.StartNew(() =>
        {
            EnsureTopicExistanceAsync(topicPath).Wait();

            lock (EnsureSubscriptionExistenceSyncRoot)
            {
                ManagementClient.SubscriptionExistsAsync(topicPath.ToString(), subscriptionName).ContinueWith(subscriptionExistsTask =>
                {
                    var subscriptionExists = subscriptionExistsTask.Result;

                    if (subscriptionExists)
                    {
                        return;
                    }

                    ManagementClient.CreateSubscriptionAsync(topicPath.ToString(), subscriptionName).Wait();
                }).Wait();
            }
        });

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
        private Task EnsureTopicExistanceAsync(IMessagingEntityPath topicPath) => Task.Factory.StartNew(() =>
        {
            lock (EnsureTopicExistenceSyncRoot)
            {
                ManagementClient.TopicExistsAsync(topicPath.ToString()).ContinueWith(topicExistsTask =>
                {
                    var topicExists = topicExistsTask.Result;

                    if (topicExists)
                    {
                        return;
                    }

                    ManagementClient.CreateTopicAsync(topicPath.ToString()).Wait();
                }).Wait();
            }
        });

        /// <summary>
        /// Represents the behavior used by clients when receiving messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ReceiveMode ReceiveBehavior = ReceiveMode.PeekLock;

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureQueueExistanceAsync(IMessagingEntityPath)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Object EnsureQueueExistenceSyncRoot = new Object();

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureSubscriptionExistanceAsync(IMessagingEntityPath, String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Object EnsureSubscriptionExistenceSyncRoot = new Object();

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureTopicExistanceAsync(IMessagingEntityPath)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Object EnsureTopicExistenceSyncRoot = new Object();

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