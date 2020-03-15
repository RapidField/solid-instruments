// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
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
    /// Represents a message topic.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageTopic" /> is the default implementation of <see cref="IMessageTopic" />.
    /// </remarks>
    internal sealed class MessageTopic : MessagingEntity, IMessageTopic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        [DebuggerHidden]
        internal MessageTopic()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier, IMessagingEntityPath path)
            : base(identifier, path)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the topic. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState)
            : base(identifier, path, operationalState)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the topic. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" /> is
        /// equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat)
            : base(identifier, path, operationalState, messageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the topic. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" /> is
        /// equal to <see cref="SerializationFormat.Unspecified" /> -or- <paramref name="messageLockExpirationThreshold" /> is less
        /// than eight seconds.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold)
            : base(identifier, path, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTopic" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the topic.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the topic. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <param name="enqueueTimeoutThreshold">
        /// The maximum length of time to wait for a message to be enqueued before raising an exception. The default value is eight
        /// seconds.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" /> is
        /// equal to <see cref="SerializationFormat.Unspecified" /> -or- <paramref name="messageLockExpirationThreshold" /> is less
        /// than eight seconds -or- <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        [DebuggerHidden]
        internal MessageTopic(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
            : base(identifier, path, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold, enqueueTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Asynchronously creates a new subscription to the current <see cref="MessageTopic" />.
        /// </summary>
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
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A subscription with the specified name already exists.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task CreateSubscriptionAsync(String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (SubscriptionQueues.ContainsKey(subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName))) == false)
            {
                var subscriptionQueue = new MessageQueue(Guid.NewGuid(), Path, OperationalState, MessageBodySerializationFormat, MessageLockExpirationThreshold, EnqueueTimeoutThreshold);

                if (SubscriptionQueues.TryAdd(subscriptionName, subscriptionQueue))
                {
                    return;
                }
            }

            throw new InvalidOperationException($"A subscription with the name \"{subscriptionName}\" already exists.");
        });

        /// <summary>
        /// Asynchronously and non-destructively returns the next available messages from the current <see cref="MessageTopic" />,
        /// if any, up to the specified maximum count.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the next available messages from the topic, or an empty
        /// collection if no messages are available.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="subscriptionName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> DequeueAsync(String subscriptionName, Int32 count) => Task.Factory.StartNew(() =>
        {
            return Dequeue(subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName)), count);
        });

        /// <summary>
        /// Asynchronously destroys the specified subscription to the current <see cref="MessageTopic" />.
        /// </summary>
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
        /// <paramref name="subscriptionName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A subscription with the specified name does not exist.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task DestroySubscriptionAsync(String subscriptionName) => Task.Factory.StartNew(() =>
        {
            RejectIfDisposed();

            if (SubscriptionQueues.ContainsKey(subscriptionName.RejectIf().IsNullOrEmpty(nameof(subscriptionName))))
            {
                if (SubscriptionQueues.TryRemove(subscriptionName, out var subscriptionQueue))
                {
                    if (subscriptionQueue.TryDisableEnqueues())
                    {
                        subscriptionQueue.Dispose();
                    }

                    return;
                }
            }

            throw new InvalidOperationException($"A subscription with the name \"{subscriptionName}\" does not exist.");
        });

        /// <summary>
        /// Attempts to add the specified message to a list of locked messages.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription, or <see langword="null" /> if no subscription is specified.
        /// </param>
        /// <param name="message">
        /// The locked message.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the message was added successfully, otherwise <see langword="false" />.
        /// </returns>
        protected internal sealed override Boolean TryAddLockedMessage(String subscriptionName, PrimitiveMessage message)
        {
            if (subscriptionName.IsNullOrEmpty())
            {
                return false;
            }

            if (message?.LockToken is null)
            {
                return false;
            }

            if (SubscriptionQueues.TryGetValue(subscriptionName, out var queue))
            {
                return queue?.TryAddLockedMessage(subscriptionName, message) ?? false;
            }

            return false;
        }

        /// <summary>
        /// Attempts to remove and return the next message in the topic.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription, or <see langword="null" /> if no subscription is specified.
        /// </param>
        /// <param name="message">
        /// The resulting message.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the message was dequeued successfully, otherwise <see langword="false" />.
        /// </returns>
        protected internal sealed override Boolean TryDequeue(String subscriptionName, out PrimitiveMessage message)
        {
            if (subscriptionName.IsNullOrEmpty())
            {
                message = null;
                return false;
            }

            if (SubscriptionQueues.TryGetValue(subscriptionName, out var queue))
            {
                if (queue is null)
                {
                    message = null;
                    return false;
                }

                if (queue.TryDequeue(subscriptionName, out var dequeuedMessage))
                {
                    message = dequeuedMessage;
                    return true;
                }
            }

            message = null;
            return false;
        }

        /// <summary>
        /// Attempts to enqueue the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to enqueue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the message was enqueued successfully, otherwise <see langword="false" />.
        /// </returns>
        protected internal sealed override Boolean TryEnqueue(PrimitiveMessage message)
        {
            if (message is null)
            {
                return false;
            }

            foreach (var queue in SubscriptionQueues.Values)
            {
                if (queue.TryEnqueue(message) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to remove and return the locked message associated with the specified lock token.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a locked message.
        /// </param>
        /// <param name="message">
        /// The resulting message.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the message was removed successfully, otherwise <see langword="false" />.
        /// </returns>
        protected internal sealed override Boolean TryRemoveLockedMessage(MessageLockToken lockToken, out PrimitiveMessage message)
        {
            if (lockToken is null)
            {
                message = null;
                return false;
            }

            var queue = SubscriptionQueues.Values.FirstOrDefault(queue => queue.LockTokens.Contains(lockToken));

            if (queue is null)
            {
                message = null;
                return false;
            }

            if (queue.TryRemoveLockedMessage(lockToken, out var lockedMessage))
            {
                message = lockedMessage;
                return true;
            }

            message = null;
            return false;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageTopic" />.
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
                    while (SubscriptionCount > 0)
                    {
                        var subscriptionNames = SubscriptionQueues.Keys.ToArray();
                        var subscriptionCount = subscriptionNames.Length;
                        var disposeQueueTasks = new List<Task>(subscriptionCount);

                        for (var i = 0; i < subscriptionCount; i++)
                        {
                            var subscriptionName = subscriptionNames[i];

                            if (SubscriptionQueues.TryRemove(subscriptionName, out var subscriptionQueue))
                            {
                                disposeQueueTasks.Add(Task.Factory.StartNew(() => subscriptionQueue.Dispose()));
                            }
                        }

                        if (disposeQueueTasks.Any())
                        {
                            Task.WaitAll(disposeQueueTasks.ToArray());
                        }
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Returns a collection of exclusive processing locks for messages contained by the current <see cref="MessageTopic" />.
        /// </summary>
        /// <returns>
        /// A collection of exclusive processing locks for messages contained by the current <see cref="MessageTopic" />.
        /// </returns>
        protected override IEnumerable<MessageLockToken> GetLockTokens() => SubscriptionQueues.Values.SelectMany(queue => queue.LockTokens);

        /// <summary>
        /// Gets the number of messages in the current <see cref="MessageTopic" />.
        /// </summary>
        /// <returns>
        /// The number of messages in the current <see cref="MessageTopic" />.
        /// </returns>
        protected sealed override Int32 GetMessageCount() => SubscriptionQueues.Values.Select(queue => queue.MessageCount).Sum();

        /// <summary>
        /// Gets the messaging entity type of the current <see cref="MessageTopic" />.
        /// </summary>
        public override MessagingEntityType EntityType => MessagingEntityType.Topic;

        /// <summary>
        /// Gets the number of subscriptions to the current <see cref="MessageTopic" />.
        /// </summary>
        public Int32 SubscriptionCount => SubscriptionQueues.Count;

        /// <summary>
        /// Gets the unique names of the subscriptions to the current <see cref="MessageTopic" />.
        /// </summary>
        public IEnumerable<String> SubscriptionNames => SubscriptionQueues.Keys;

        /// <summary>
        /// Represents a collection of queues for the subscriptions to the current <see cref="MessageTopic" />, which are keyed by
        /// subscription name.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<String, MessageQueue> SubscriptionQueues = new ConcurrentDictionary<String, MessageQueue>();
    }
}