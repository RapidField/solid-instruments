// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a client that interacts with an <see cref="IMessagingEntity" />.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagingEntityClient" /> is the default implementation of <see cref="IMessagingEntityClient" />.
    /// </remarks>
    internal abstract class MessagingEntityClient : IMessagingEntityReceiveClient, IMessagingEntitySendClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntityClient" /> class.
        /// </summary>
        /// <param name="connection">
        /// The client's connection to the associated entity's transport.
        /// </param>
        /// <param name="path">
        /// The unique textual path for the messaging entity with which the client transacts.
        /// </param>
        /// <param name="entityType">
        /// The entity type of the associated entity.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessagingEntityClient(IMessageTransportConnection connection, IMessagingEntityPath path, MessagingEntityType entityType)
        {
            Connection = connection.RejectIf().IsNull(nameof(connection)).TargetArgument;
            EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
            Path = path.RejectIf().IsNull(nameof(path)).TargetArgument;
        }

        /// <summary>
        /// Asynchronously notifies the associated <see cref="IMessagingEntity" /> that a locked message was not processed and can
        /// be made available for processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task ConveyFailureAsync(MessageLockToken lockToken) => EntityType switch
        {
            MessagingEntityType.Queue => Connection.Transport.ConveyFailureToQueueAsync(lockToken, Path),
            MessagingEntityType.Topic => Connection.Transport.ConveyFailureToSubscriptionAsync(lockToken, Path),
            _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {EntityType}, is not supported.")
        };

        /// <summary>
        /// Asynchronously notifies the associated <see cref="IMessagingEntity" /> that a locked message was processed successfully
        /// and can be destroyed permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task ConveySuccessAsync(MessageLockToken lockToken) => EntityType switch
        {
            MessagingEntityType.Queue => Connection.Transport.ConveySuccessToQueueAsync(lockToken, Path),
            MessagingEntityType.Topic => Connection.Transport.ConveySuccessToSubscriptionAsync(lockToken, Path),
            _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {EntityType}, is not supported.")
        };

        /// <summary>
        /// Registers the specified message handler for the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handleMessageAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public void RegisterMessageHandler(Action<PrimitiveMessage> handleMessageAction)
        {
            try
            {
                RegisterMessageHandler(Connection, handleMessageAction.RejectIf().IsNull(nameof(handleMessageAction)));
            }
            catch (ObjectDisposedException exception)
            {
                throw new MessageTransportConnectionClosedException("Failed to register message handler. The connection is closed.", exception);
            }
        }

        /// <summary>
        /// Asynchronously sends the specified message to the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The connection is closed.
        /// </exception>
        public Task SendAsync(PrimitiveMessage message) => EntityType switch
        {
            MessagingEntityType.Queue => EnsureQueueExistanceAsync(Path).ContinueWith(async ensureQueueExistenceTask =>
            {
                await Connection.Transport.SendToQueueAsync(Path, message).ConfigureAwait(false);
            }),
            MessagingEntityType.Topic => EnsureTopicExistanceAsync(Path).ContinueWith(async ensureTopicExistenceTask =>
            {
                await Connection.Transport.SendToTopicAsync(Path, message).ConfigureAwait(false);
            }),
            _ => throw new UnsupportedSpecificationException($"The specified messaging entity type, {EntityType}, is not supported.")
        };

        /// <summary>
        /// Asynchronously creates the specified queue if it does not exist.
        /// </summary>
        /// <param name="queuePath">
        /// The queue entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queuePath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        protected Task EnsureQueueExistanceAsync(IMessagingEntityPath queuePath) => Task.Factory.StartNew(() =>
        {
            lock (EnsureQueueExistenceSyncRoot)
            {
                try
                {
                    if (Connection.Transport.QueueExists(queuePath))
                    {
                        return;
                    }

                    Connection.Transport.CreateQueueAsync(queuePath).Wait();
                }
                catch (ObjectDisposedException exception)
                {
                    throw new MessageTransportConnectionClosedException("Failed to ensure queue existence. The connection is closed.", exception);
                }
            }
        });

        /// <summary>
        /// Asynchronously creates the specified subscription if it does not exist.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicPath" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        protected Task EnsureSubscriptionExistanceAsync(IMessagingEntityPath topicPath, String subscriptionName) => EnsureTopicExistanceAsync(topicPath).ContinueWith(ensureTopicExistenceTask =>
        {
            lock (EnsureSubscriptionExistenceSyncRoot)
            {
                try
                {
                    if (Connection.Transport.SubscriptionExists(topicPath, subscriptionName))
                    {
                        return;
                    }

                    Connection.Transport.CreateSubscriptionAsync(topicPath, subscriptionName).Wait();
                }
                catch (ObjectDisposedException exception)
                {
                    throw new MessageTransportConnectionClosedException("Failed to ensure subscription existence. The connection is closed.", exception);
                }
            }
        });

        /// <summary>
        /// Asynchronously creates the specified topic if it does not exist.
        /// </summary>
        /// <param name="topicPath">
        /// The topic entity path.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="topicPath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransportConnectionClosedException">
        /// The transport connection is closed.
        /// </exception>
        protected Task EnsureTopicExistanceAsync(IMessagingEntityPath topicPath) => Task.Factory.StartNew(() =>
        {
            lock (EnsureTopicExistenceSyncRoot)
            {
                try
                {
                    if (Connection.Transport.TopicExists(topicPath))
                    {
                        return;
                    }

                    Connection.Transport.CreateTopicAsync(topicPath).Wait();
                }
                catch (ObjectDisposedException exception)
                {
                    throw new MessageTransportConnectionClosedException("Failed to ensure topic existence. The connection is closed.", exception);
                }
            }
        });

        /// <summary>
        /// Registers the specified message handler for the associated <see cref="IMessagingEntity" />.
        /// </summary>
        /// <param name="connection">
        /// The connection with which to register <paramref name="handleMessageAction" />.
        /// </param>
        /// <param name="handleMessageAction">
        /// An action to perform upon message receipt.
        /// </param>
        protected abstract void RegisterMessageHandler(IMessageTransportConnection connection, Action<PrimitiveMessage> handleMessageAction);

        /// <summary>
        /// Gets the client's connection to the associated entity's <see cref="IMessageTransport" />.
        /// </summary>
        public IMessageTransportConnection Connection
        {
            get;
        }

        /// <summary>
        /// Gets the entity type of the associated <see cref="IMessagingEntity" />.
        /// </summary>
        public MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the unique textual path for the messaging entity with which the client transacts.
        /// </summary>
        public IMessagingEntityPath Path
        {
            get;
        }

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureQueueExistanceAsync(IMessagingEntityPath)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Object EnsureQueueExistenceSyncRoot = new Object();

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureSubscriptionExistanceAsync(IMessagingEntityPath, String)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Object EnsureSubscriptionExistenceSyncRoot = new Object();

        /// <summary>
        /// Represents an object that is used to synchronize access to
        /// <see cref="EnsureTopicExistanceAsync(IMessagingEntityPath)" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Object EnsureTopicExistenceSyncRoot = new Object();
    }
}