// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a message queue.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageQueue" /> is the default implementation of <see cref="IMessageQueue" />.
    /// </remarks>
    internal sealed class MessageQueue : MessagingEntity, IMessageQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        [DebuggerHidden]
        internal MessageQueue()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageQueue(Guid identifier)
            : base(identifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageQueue(Guid identifier, IMessagingEntityPath path)
            : base(identifier, path)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal MessageQueue(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState)
            : base(identifier, path, operationalState)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        internal MessageQueue(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat)
            : base(identifier, path, operationalState, messageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        internal MessageQueue(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold)
            : base(identifier, path, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        internal MessageQueue(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
            : base(identifier, path, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold, enqueueTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Asynchronously and non-destructively returns the next available messages from the queue, if any, up to the specified
        /// maximum count.
        /// </summary>
        /// <param name="count">
        /// The maximum number of messages to read from the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the next available messages from the queue, or an empty
        /// collection if no messages are available.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task<IEnumerable<PrimitiveMessage>> DequeueAsync(Int32 count) => Task.Factory.StartNew(() =>
        {
            return Dequeue(null, count);
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
        protected internal sealed override Boolean TryAddLockedMessage(String subscriptionName, PrimitiveMessage message) => message?.LockToken is null ? false : LockedMessages.TryAdd(message.LockToken, message);

        /// <summary>
        /// Attempts to remove and return the next message in the queue.
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
            if (Messages.TryDequeue(out var dequeuedMessage))
            {
                message = dequeuedMessage;
                return true;
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

            Messages.Enqueue(message);
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

            if (LockedMessages.TryRemove(lockToken, out var lockedMessage))
            {
                message = lockedMessage;
                return true;
            }

            message = null;
            return false;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageQueue" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                var disposalAttemptCount = 0;

                while (MessageCount > 0 && disposalAttemptCount < MaximumDisposalAttemptCount)
                {
                    Thread.Sleep(EnqueueTimeoutThreshold);
                    disposalAttemptCount++;
                }

                LockedMessages?.Clear();
                Messages?.Clear();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Returns a collection of exclusive processing locks for messages contained by the current <see cref="MessageQueue" />.
        /// </summary>
        /// <returns>
        /// A collection of exclusive processing locks for messages contained by the current <see cref="MessageQueue" />.
        /// </returns>
        protected override IEnumerable<MessageLockToken> GetLockTokens() => LockedMessages.Keys;

        /// <summary>
        /// Gets the number of messages in the current <see cref="MessageQueue" />.
        /// </summary>
        /// <returns>
        /// The number of messages in the current <see cref="MessageQueue" />.
        /// </returns>
        protected sealed override Int32 GetMessageCount() => Messages.Count + LockedMessages.Count;

        /// <summary>
        /// Gets the messaging entity type of the current <see cref="MessageQueue" />.
        /// </summary>
        public override MessagingEntityType EntityType => MessagingEntityType.Queue;

        /// <summary>
        /// Represents the maximum number of times that <see cref="MessageQueue" /> instances will attempt to wait for natural
        /// clearance before discarding outstanding messages during disposal.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumDisposalAttemptCount = 3;

        /// <summary>
        /// Represents the underlying first-in first-out collection that contains messages that are locked for processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<MessageLockToken, PrimitiveMessage> LockedMessages = new();

        /// <summary>
        /// Represents the underlying first-in first-out collection that contains enqueued messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentQueue<PrimitiveMessage> Messages = new();
    }
}