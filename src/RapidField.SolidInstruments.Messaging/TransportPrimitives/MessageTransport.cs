// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Supports message exchange for a collection of queues and topics.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTransport" /> is the default implementation of <see cref="IMessageTransport" />.
    /// </remarks>
    internal sealed class MessageTransport : Instrument, IMessageTransport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransport" /> class.
        /// </summary>
        [DebuggerHidden]
        internal MessageTransport()
            : this(PrimitiveMessage.DefaultBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransport" /> class.
        /// </summary>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTransport(SerializationFormat messageBodySerializationFormat)
            : base()
        {
            MessageBodySerializationFormat = messageBodySerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageBodySerializationFormat));
        }

        /// <summary>
        /// Closes the specified connection as an idempotent operation.
        /// </summary>
        /// <param name="connection">
        /// The connection to close.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        public void CloseConnection(IMessageTransportConnection connection)
        {
            if (ConnectionDictionary.ContainsKey(connection.RejectIf().IsNull(nameof(connection)).TargetArgument.Identifier))
            {
                if (ConnectionDictionary.TryRemove(connection.Identifier, out _))
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Opens and returns a new <see cref="IMessageTransportConnection" /> to the current <see cref="MessageTransport" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="IMessageTransportConnection" /> to the current <see cref="MessageTransport" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IMessageTransportConnection CreateConnection()
        {
            RejectIfDisposed();
            var connection = new MessageTransportConnection(this);

            if (ConnectionDictionary.TryAdd(connection.Identifier, connection))
            {
                return connection;
            }

            return connection;
        }

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path) => CreateQueueAsync(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => CreateQueueAsync(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Asynchronously creates a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds -or-
        /// <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateQueue(path.RejectIf().IsNull(nameof(path)).TargetArgument, messageLockExpirationThreshold.RejectIf().IsLessThan(MessagingEntity.MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold)), enqueueTimeoutThreshold.RejectIf().IsLessThan(MessagingEntity.EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold))))
            {
                return;
            }

            throw new InvalidOperationException($"The specified queue, \"{path}\", already exists");
        });

        /// <summary>
        /// Asynchronously creates a new subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription already exists.
        /// </exception>
        public Task CreateSubscriptionAsync(IMessagingEntityPath path, String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateSubscription(path.RejectIf().IsNull(nameof(path)).TargetArgument, subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName))))
            {
                return;
            }

            throw new InvalidOperationException($"The specified subscription, \"{subscriptionName}\", already exists");
        });

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path) => CreateTopicAsync(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => CreateTopicAsync(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Asynchronously creates a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageLockExpirationThreshold" /> is less than eight seconds -or-
        /// <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// An entity with the specified path already exists.
        /// </exception>
        public Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryCreateTopic(path.RejectIf().IsNull(nameof(path)).TargetArgument, messageLockExpirationThreshold.RejectIf().IsLessThan(MessagingEntity.MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold)), enqueueTimeoutThreshold.RejectIf().IsLessThan(MessagingEntity.EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold))))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to create topic. The specified topic, \"{path}\", already exists");
        });

        /// <summary>
        /// Asynchronously destroys the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        public Task DestroyQueueAsync(IMessagingEntityPath path) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroyQueue(path.RejectIf().IsNull(nameof(path)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy queue. The specified queue, \"{path}\", does not exist.");
        });

        /// <summary>
        /// Asynchronously destroys the specified subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified subscription does not exist.
        /// </exception>
        public Task DestroySubscriptionAsync(IMessagingEntityPath path, String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroySubscription(path.RejectIf().IsNull(nameof(path)).TargetArgument, subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName))))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy subscription. The specified subscription, \"{subscriptionName}\", does not exist.");
        });

        /// <summary>
        /// Asynchronously destroys the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        public Task DestroyTopicAsync(IMessagingEntityPath path) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (TryDestroyTopic(path.RejectIf().IsNull(nameof(path)).TargetArgument))
            {
                return;
            }

            throw new InvalidOperationException($"Failed to destroy topic. The specified topic, \"{path}\", does not exist.");
        });

        /// <summary>
        /// Returns a value indicating whether or not the specified queue exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean QueueExists(IMessagingEntityPath path)
        {
            RejectIfDisposed();
            return QueuePaths.Any(queuePath => queuePath == path.RejectIf().IsNull(nameof(path)).TargetArgument);
        }

        /// <summary>
        /// Asynchronously requests the specified number of messages from the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the dequeued messages, if any.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> ReceiveFromQueueAsync(IMessagingEntityPath path, Int32 count)
        {
            if (QueueDictionary.TryGetValue(path.RejectIf().IsNull(nameof(path)).TargetArgument, out var queue))
            {
                return queue.DequeueAsync(count);
            }

            throw new InvalidOperationException($"Failed to receive message(s). The specified queue, \"{path}\", does not exist.");
        }

        /// <summary>
        /// Asynchronously requests the specified number of messages from the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the dequeued messages, if any.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> ReceiveFromTopicAsync(IMessagingEntityPath path, String subscriptionName, Int32 count)
        {
            if (TopicDictionary.TryGetValue(path.RejectIf().IsNull(nameof(path)).TargetArgument, out var topic))
            {
                return topic.DequeueAsync(subscriptionName, count);
            }

            throw new InvalidOperationException($"Failed to receive message(s). The specified topic, \"{path}\", does not exist.");
        }

        /// <summary>
        /// Asynchronously sends the specified message to the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified queue does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task SendToQueueAsync(IMessagingEntityPath path, PrimitiveMessage message)
        {
            if (QueueDictionary.TryGetValue(path.RejectIf().IsNull(nameof(path)).TargetArgument, out var queue))
            {
                return queue.EnqueueAsync(message);
            }

            throw new InvalidOperationException($"Failed to send message. The specified queue, \"{path}\", does not exist.");
        }

        /// <summary>
        /// Asynchronously sends the specified message to the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified topic does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task SendToTopicAsync(IMessagingEntityPath path, PrimitiveMessage message)
        {
            if (TopicDictionary.TryGetValue(path.RejectIf().IsNull(nameof(path)).TargetArgument, out var topic))
            {
                return topic.EnqueueAsync(message);
            }

            throw new InvalidOperationException($"Failed to send message. The specified topic, \"{path}\", does not exist.");
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified subscription exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="subscriptionName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean SubscriptionExists(IMessagingEntityPath path, String subscriptionName)
        {
            RejectIfDisposed();

            if (TopicDictionary.TryGetValue(path.RejectIf().IsNull(nameof(path)).TargetArgument, out var topic))
            {
                return topic.SubscriptionNames.Contains(subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName)));
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified topic exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic exists, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean TopicExists(IMessagingEntityPath path)
        {
            RejectIfDisposed();
            return TopicPaths.Any(topciPath => topciPath == path.RejectIf().IsNull(nameof(path)).TargetArgument);
        }

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path) => TryCreateQueue(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => TryCreateQueue(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
        {
            if (IsDisposedOrDisposing || path is null || messageLockExpirationThreshold < MessagingEntity.MessageLockExpirationThresholdFloor || enqueueTimeoutThreshold < MessagingEntity.EnqueueTimeoutThresholdFloor)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                var queue = new MessageQueue(Guid.NewGuid(), path, MessagingEntityOperationalState.Ready, MessageBodySerializationFormat, messageLockExpirationThreshold, enqueueTimeoutThreshold);

                if (QueueDictionary.TryAdd(path, queue))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to create a new subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateSubscription(IMessagingEntityPath path, String subscriptionName)
        {
            if (IsDisposedOrDisposing || path is null || subscriptionName is null)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }
                else if (TopicExists(path) == false)
                {
                    if (TryCreateTopic(path) == false)
                    {
                        return false;
                    }
                }

                if (TopicDictionary.TryGetValue(path, out var topic))
                {
                    return topic.TryCreateSubscription(subscriptionName);
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path) => TryCreateTopic(path, MessagingEntity.DefaultMessageLockExpirationThreshold);

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold) => TryCreateTopic(path, messageLockExpirationThreshold, MessagingEntity.DefaultEnqueueTimeoutThreshold);

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
        {
            if (IsDisposedOrDisposing || path is null || messageLockExpirationThreshold < MessagingEntity.MessageLockExpirationThresholdFloor || enqueueTimeoutThreshold < MessagingEntity.EnqueueTimeoutThresholdFloor)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                var topic = new MessageTopic(Guid.NewGuid(), path, MessagingEntityOperationalState.Ready, MessageBodySerializationFormat, messageLockExpirationThreshold, enqueueTimeoutThreshold);

                if (TopicDictionary.TryAdd(path, topic))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to destroy the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroyQueue(IMessagingEntityPath path)
        {
            if (IsDisposedOrDisposing || path is null)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                if (QueueDictionary.TryRemove(path, out var queue))
                {
                    queue?.Dispose();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to destroy the specified subscription.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the subscription was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroySubscription(IMessagingEntityPath path, String subscriptionName)
        {
            if (IsDisposedOrDisposing || path is null || subscriptionName is null)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                if (TopicDictionary.TryGetValue(path, out var topic))
                {
                    return topic.TryDestroySubscription(subscriptionName);
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to destroy the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        public Boolean TryDestroyTopic(IMessagingEntityPath path)
        {
            if (IsDisposedOrDisposing || path is null)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                if (TopicDictionary.TryRemove(path, out var topic))
                {
                    topic?.Dispose();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTransport" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    while (TopicCount > 0 || QueueCount > 0 || ConnectionCount > 0)
                    {
                        DestroyAllTopics();
                        DestroyAllQueues();
                        DestroyAllConnections();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Closes and disposes of all connections to the current <see cref="MessageTransport" />.
        /// </summary>
        [DebuggerHidden]
        private void DestroyAllConnections()
        {
            var destroyConnectionTasks = new List<Task>();

            while (ConnectionDictionary.Any())
            {
                var connectionIdentifier = ConnectionDictionary.Keys.FirstOrDefault();

                if (connectionIdentifier == default)
                {
                    continue;
                }
                else if (ConnectionDictionary.TryRemove(connectionIdentifier, out var connection))
                {
                    destroyConnectionTasks.Add(Task.Factory.StartNew(() =>
                    {
                        connection.Dispose();
                    }));
                }
            }

            Task.WaitAll(destroyConnectionTasks.ToArray());
        }

        /// <summary>
        /// Removes and disposes of all queues within the current <see cref="MessageTransport" />.
        /// </summary>
        [DebuggerHidden]
        private void DestroyAllQueues()
        {
            var destroyQueueTasks = new List<Task>();

            while (QueueDictionary.Any())
            {
                var queuePath = QueueDictionary.Keys.First();

                if (queuePath is null)
                {
                    continue;
                }
                else if (QueueDictionary.TryRemove(queuePath, out var queue))
                {
                    destroyQueueTasks.Add(Task.Factory.StartNew(() =>
                    {
                        queue.Dispose();
                    }));
                }
            }

            Task.WaitAll(destroyQueueTasks.ToArray());
        }

        /// <summary>
        /// Removes and disposes of all topics within the current <see cref="MessageTransport" />.
        /// </summary>
        [DebuggerHidden]
        private void DestroyAllTopics()
        {
            var destroyTopicTasks = new List<Task>();

            while (TopicDictionary.Any())
            {
                var topicPath = TopicDictionary.Keys.First();

                if (topicPath is null)
                {
                    continue;
                }
                else if (TopicDictionary.TryRemove(topicPath, out var topic))
                {
                    destroyTopicTasks.Add(Task.Factory.StartNew(() =>
                    {
                        topic.Dispose();
                    }));
                }
            }

            Task.WaitAll(destroyTopicTasks.ToArray());
        }

        /// <summary>
        /// Gets the number of active connections to the current <see cref="MessageTransport" />.
        /// </summary>
        public Int32 ConnectionCount => Connections.Count();

        /// <summary>
        /// Gets a collection of active connections to the current <see cref="MessageTransport" />.
        /// </summary>
        public IEnumerable<IMessageTransportConnection> Connections => ConnectionDictionary.Values;

        /// <summary>
        /// Gets the format that is used to serialize enqueued message bodies.
        /// </summary>
        public SerializationFormat MessageBodySerializationFormat
        {
            get;
        }

        /// <summary>
        /// Gets the number of queues within the current <see cref="MessageTransport" />.
        /// </summary>
        public Int32 QueueCount => QueuePaths.Count();

        /// <summary>
        /// Gets a collection of available queue paths for the current <see cref="MessageTransport" />.
        /// </summary>
        public IEnumerable<IMessagingEntityPath> QueuePaths => QueueDictionary.Keys;

        /// <summary>
        /// Gets the number of topics within the current <see cref="MessageTransport" />.
        /// </summary>
        public Int32 TopicCount => TopicPaths.Count();

        /// <summary>
        /// Gets a collection of available topic paths for the current <see cref="MessageTransport" />.
        /// </summary>
        public IEnumerable<IMessagingEntityPath> TopicPaths => TopicDictionary.Keys;

        /// <summary>
        /// Represents a collection of active connections to the current <see cref="MessageTransport" />, which are keyed by
        /// identifier.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<Guid, IMessageTransportConnection> ConnectionDictionary = new ConcurrentDictionary<Guid, IMessageTransportConnection>();

        /// <summary>
        /// Represents a collection of available queues for the current <see cref="MessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<IMessagingEntityPath, IMessageQueue> QueueDictionary = new ConcurrentDictionary<IMessagingEntityPath, IMessageQueue>();

        /// <summary>
        /// Represents a collection of available topics for the current <see cref="MessageTransport" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<IMessagingEntityPath, IMessageTopic> TopicDictionary = new ConcurrentDictionary<IMessagingEntityPath, IMessageTopic>();
    }
}