// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.TextEncoding;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.AzureServiceBus
{
    /// <summary>
    /// Produces Azure Service Bus clients using a shared connection.
    /// </summary>
    internal sealed class AzureServiceBusClientFactory : Instrument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusClientFactory" /> class.
        /// </summary>
        /// <param name="connection">
        /// A connection that governs interaction with Azure Service Bus entities.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal AzureServiceBusClientFactory(ServiceBusConnection connection)
            : base()
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            ManagementClient = new ManagementClient(Connection.Endpoint.ToString(), Connection.TokenProvider);
        }

        /// <summary>
        /// Creates a new Azure Service Bus client that facilitates receive operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// A new Azure Service Bus client that facilitates receive operations.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while creating the client.
        /// </exception>
        public IReceiverClient CreateReceiveClient<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            switch (entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)).TargetArgument)
            {
                case MessagingEntityType.Queue:

                    try
                    {
                        return CreateQueueClient<TMessage>();
                    }
                    catch (Exception exception)
                    {
                        throw new MessageSubscriptionException(typeof(TMessage), exception);
                    }

                case MessagingEntityType.Topic:

                    try
                    {
                        return CreateSubscriptionClient<TMessage>();
                    }
                    catch (Exception exception)
                    {
                        throw new MessageSubscriptionException(typeof(TMessage), exception);
                    }

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Creates a new Azure Service Bus client that facilitates send operations.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// A new Azure Service Bus client that facilitates send operations.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        public ISenderClient CreateSendClient<TMessage>(MessagingEntityType entityType)
            where TMessage : class
        {
            switch (entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType)).TargetArgument)
            {
                case MessagingEntityType.Queue:

                    try
                    {
                        return CreateQueueClient<TMessage>();
                    }
                    catch (Exception exception)
                    {
                        throw new MessagePublishingException(typeof(TMessage), exception);
                    }

                case MessagingEntityType.Topic:

                    try
                    {
                        return CreateTopicClient<TMessage>();
                    }
                    catch (Exception exception)
                    {
                        throw new MessagePublishingException(typeof(TMessage), exception);
                    }

                default:

                    throw new InvalidOperationException($"The specified entity type, {entityType}, is not supported.");
            }
        }

        /// <summary>
        /// Returns a queue entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A queue entity path for the specified message type.
        /// </returns>
        [DebuggerHidden]
        internal static String GetQueuePath<TMessage>()
             where TMessage : class => $"Queue-{typeof(TMessage).Name}";

        /// <summary>
        /// Returns a subscription name for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A subscription name for the specified message type.
        /// </returns>
        [DebuggerHidden]
        internal static String GetSubscriptionName<TMessage>()
             where TMessage : class => $"Subscription-{EnhancedReadabilityGuid.New().ToString()}";

        /// <summary>
        /// Returns a topic entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <returns>
        /// A topic entity path for the specified message type.
        /// </returns>
        [DebuggerHidden]
        internal static String GetTopicPath<TMessage>()
             where TMessage : class => $"Topic-{typeof(TMessage).Name}";

        /// <summary>
        /// Destroys the specified Azure Service Bus subscription if it exists.
        /// </summary>
        /// <param name="topicPath">
        /// The topic entity path for the subscription.
        /// </param>
        /// <param name="subscriptionName">
        /// The name of the subscription.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="topicPath" /> is empty -or- <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicPath" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal void DestroySubscription(String topicPath, String subscriptionName)
        {
            using (var controlToken = StateControl.Enter())
            {
                controlToken.AttachTask(DestroySubscriptionAsync(topicPath.RejectIf().IsNullOrEmpty(nameof(topicPath)), subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName))));
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AzureServiceBusClientFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a new <see cref="IQueueClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="IQueueClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private IQueueClient CreateQueueClient<TMessage>()
            where TMessage : class
        {
            var queuePath = GetQueuePath<TMessage>();

            using (var controlToken = StateControl.Enter())
            {
                controlToken.AttachTask(EnsureQueueExistanceAsync(queuePath));
            }

            return new QueueClient(Connection, queuePath, ReceiveBehavior, RetryBehavior);
        }

        /// <summary>
        /// Creates a new <see cref="ISubscriptionClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="ISubscriptionClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ISubscriptionClient CreateSubscriptionClient<TMessage>()
            where TMessage : class
        {
            var topicPath = GetTopicPath<TMessage>();
            var subscriptionName = GetSubscriptionName<TMessage>();

            using (var controlToken = StateControl.Enter())
            {
                controlToken.AttachTask(EnsureSubscriptionExistanceAsync(topicPath, subscriptionName));
            }

            return new SubscriptionClient(Connection, topicPath, subscriptionName, ReceiveBehavior, RetryBehavior);
        }

        /// <summary>
        /// Creates a new <see cref="ITopicClient" /> for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="ITopicClient" />.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception was raised while creating the client.
        /// </exception>
        [DebuggerHidden]
        private ITopicClient CreateTopicClient<TMessage>()
            where TMessage : class
        {
            var topicPath = GetTopicPath<TMessage>();

            using (var controlToken = StateControl.Enter())
            {
                controlToken.AttachTask(EnsureTopicExistanceAsync(topicPath));
            }

            return new TopicClient(Connection, topicPath, RetryBehavior);
        }

        /// <summary>
        /// Asynchronously destroys the specified Azure Service Bus subscription if it exists.
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
        private async Task DestroySubscriptionAsync(String topicPath, String subscriptionName)
        {
            var subscriptionExists = await ManagementClient.SubscriptionExistsAsync(topicPath, subscriptionName).ConfigureAwait(false);

            if (subscriptionExists)
            {
                await ManagementClient.DeleteSubscriptionAsync(topicPath, subscriptionName).ConfigureAwait(false);
            }
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
        /// Represents a connection that governs interaction with Azure Service Bus entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ServiceBusConnection Connection;

        /// <summary>
        /// Represents a client that manages Azure Service Bus entities.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ManagementClient ManagementClient;
    }
}