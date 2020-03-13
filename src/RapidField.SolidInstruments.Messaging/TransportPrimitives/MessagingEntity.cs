// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a messaging entity.
    /// </summary>
    /// <remarks>
    /// <see cref="MessagingEntity" /> is the default implementation of <see cref="IMessagingEntity" />.
    /// </remarks>
    internal abstract class MessagingEntity : Instrument, IMessagingEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        protected MessagingEntity()
            : this(Guid.NewGuid())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected MessagingEntity(Guid identifier)
            : this(identifier, new MessagingEntityPath())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the entity.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        protected MessagingEntity(Guid identifier, IMessagingEntityPath path)
            : this(identifier, path, MessagingEntityOperationalState.Ready)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the entity.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the entity. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="MessagingEntityOperationalState.Unspecified" />.
        /// </exception>
        protected MessagingEntity(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState)
            : this(identifier, path, operationalState, DefaultMessageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the entity.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the entity. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        protected MessagingEntity(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat)
            : this(identifier, path, operationalState, messageBodySerializationFormat, DefaultMessageLockExpirationThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the entity.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the entity. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        protected MessagingEntity(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold)
            : this(identifier, path, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold, DefaultEnqueueTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingEntity" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the entity.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the entity.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the entity. The default value is <see cref="MessagingEntityOperationalState.Ready" />.
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
        protected MessagingEntity(Guid identifier, IMessagingEntityPath path, MessagingEntityOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
            : base()
        {
            EnqueueTimeoutThreshold = enqueueTimeoutThreshold.RejectIf().IsLessThan(EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold));
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            MessageBodySerializationFormat = messageBodySerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageBodySerializationFormat));
            MessageLockExpirationThreshold = messageLockExpirationThreshold.RejectIf().IsLessThan(MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold));
            OperationalState = operationalState.RejectIf().IsEqualToValue(MessagingEntityOperationalState.Unspecified, nameof(operationalState));
            Path = path.RejectIf().IsNull(nameof(path)).TargetArgument;
        }

        /// <summary>
        /// Asynchronously notifies the current <see cref="MessageQueue" /> that a locked message was not processed and can be made
        /// available for processing by other consumers.
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
        public Task ConveyFailureAsync(MessageLockToken lockToken)
        {
            RejectIfDisposed();

            if (TryRemoveLockedMessage(lockToken.RejectIf().IsNull(nameof(lockToken)), out var message))
            {
                return RequeueAsync(message);
            }

            throw new InvalidOperationException("Failure conveyance was not successful. The specified lock token does not reference an existing locked message.");
        }

        /// <summary>
        /// Asynchronously notifies the current <see cref="MessageQueue" /> that a locked message was processed successfully and can
        /// be destroyed permanently.
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
        public Task ConveySuccessAsync(MessageLockToken lockToken)
        {
            RejectIfDisposed();

            if (TryRemoveLockedMessage(lockToken.RejectIf().IsNull(nameof(lockToken)), out _))
            {
                return Task.CompletedTask;
            }

            throw new InvalidOperationException("Success conveyance was not successful. The specified lock token does not reference an existing locked message.");
        }

        /// <summary>
        /// Asynchronously enqueues the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to enqueue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task EnqueueAsync(PrimitiveMessage message)
        {
            RejectIfDisposed();

            if (CurrentStatePermitsEnqueue && TryEnqueue(message.RejectIf().IsNull(nameof(message)).TargetArgument))
            {
                return Task.CompletedTask;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    while (stopwatch.Elapsed < EnqueueTimeoutThreshold)
                    {
                        RejectIfDisposed();
                        Thread.Sleep(EnqueueDelayDuration);

                        if (CurrentStatePermitsEnqueue && TryEnqueue(message))
                        {
                            return;
                        }
                    }
                }
                finally
                {
                    stopwatch.Stop();
                }

                throw new TimeoutException($"The message could not be enqueued. The timeout threshold duration was exceeded while waiting for availability (path: {Path}).");
            });
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessagingEntity" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessagingEntity" />.
        /// </returns>
        public sealed override String ToString() => $"{{ {nameof(Path)}: {Path} }}";

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessageQueue" /> to
        /// <see cref="MessagingEntityOperationalState.EnqueueOnly" />, or to <see cref="MessagingEntityOperationalState.Paused" />
        /// if the previous state was <see cref="MessagingEntityOperationalState.DequeueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryDisableDequeues()
        {
            if (IsDisposedOrDisposing)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                switch (OperationalState)
                {
                    case MessagingEntityOperationalState.DequeueOnly:

                        OperationalState = MessagingEntityOperationalState.Paused;
                        return true;

                    case MessagingEntityOperationalState.EnqueueOnly:

                        return true;

                    case MessagingEntityOperationalState.Paused:

                        return true;

                    case MessagingEntityOperationalState.Ready:

                        OperationalState = MessagingEntityOperationalState.EnqueueOnly;
                        return true;

                    default:

                        return false;
                }
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessageQueue" /> to
        /// <see cref="MessagingEntityOperationalState.DequeueOnly" />, or to <see cref="MessagingEntityOperationalState.Paused" />
        /// if the previous state was <see cref="MessagingEntityOperationalState.EnqueueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryDisableEnqueues()
        {
            if (IsDisposedOrDisposing)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                switch (OperationalState)
                {
                    case MessagingEntityOperationalState.DequeueOnly:

                        return true;

                    case MessagingEntityOperationalState.EnqueueOnly:

                        OperationalState = MessagingEntityOperationalState.Paused;
                        return true;

                    case MessagingEntityOperationalState.Paused:

                        return true;

                    case MessagingEntityOperationalState.Ready:

                        OperationalState = MessagingEntityOperationalState.DequeueOnly;
                        return true;

                    default:

                        return false;
                }
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessageQueue" /> to
        /// <see cref="MessagingEntityOperationalState.Paused" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryPause()
        {
            if (IsDisposedOrDisposing)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                switch (OperationalState)
                {
                    case MessagingEntityOperationalState.DequeueOnly:

                        OperationalState = MessagingEntityOperationalState.Paused;
                        return true;

                    case MessagingEntityOperationalState.EnqueueOnly:

                        OperationalState = MessagingEntityOperationalState.Paused;
                        return true;

                    case MessagingEntityOperationalState.Paused:

                        return true;

                    case MessagingEntityOperationalState.Ready:

                        OperationalState = MessagingEntityOperationalState.Paused;
                        return true;

                    default:

                        return false;
                }
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessageQueue" /> to
        /// <see cref="MessagingEntityOperationalState.Ready" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryResume()
        {
            if (IsDisposedOrDisposing)
            {
                return false;
            }

            using (var controlToken = StateControl.Enter())
            {
                if (IsDisposedOrDisposing)
                {
                    return false;
                }

                switch (OperationalState)
                {
                    case MessagingEntityOperationalState.DequeueOnly:

                        OperationalState = MessagingEntityOperationalState.Ready;
                        return true;

                    case MessagingEntityOperationalState.EnqueueOnly:

                        OperationalState = MessagingEntityOperationalState.Ready;
                        return true;

                    case MessagingEntityOperationalState.Paused:

                        OperationalState = MessagingEntityOperationalState.Ready;
                        return true;

                    case MessagingEntityOperationalState.Ready:

                        return true;

                    default:

                        return false;
                }
            }
        }

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
        protected internal abstract Boolean TryAddLockedMessage(String subscriptionName, PrimitiveMessage message);

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
        protected internal abstract Boolean TryDequeue(String subscriptionName, out PrimitiveMessage message);

        /// <summary>
        /// Attempts to enqueue the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to enqueue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the message was enqueued successfully, otherwise <see langword="false" />.
        /// </returns>
        protected internal abstract Boolean TryEnqueue(PrimitiveMessage message);

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
        protected internal abstract Boolean TryRemoveLockedMessage(MessageLockToken lockToken, out PrimitiveMessage message);

        /// <summary>
        /// Non-destructively returns the next available messages from the current <see cref="MessagingEntity" />, if any, up to the
        /// specified maximum count.
        /// </summary>
        /// <param name="subscriptionName">
        /// The unique name of the subscription, or <see langword="null" /> if no subscription is specified.
        /// </param>
        /// <param name="count">
        /// The maximum number of messages to read from the entity.
        /// </param>
        /// <returns>
        /// The next available messages from the entity, or an empty collection if no messages are available.
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
        protected IEnumerable<PrimitiveMessage> Dequeue(String subscriptionName, Int32 count)
        {
            RejectIfDisposed();

            if (count.RejectIf().IsLessThan(0, nameof(count)) == 0)
            {
                return Array.Empty<PrimitiveMessage>();
            }

            var messageList = new List<PrimitiveMessage>();
            var requeueTasks = new List<Task>();

            while (messageList.Count() < count && CurrentStatePermitsDequeue && TryDequeue(subscriptionName, out var message))
            {
                var lockToken = new MessageLockToken(Guid.NewGuid(), message.Identifier, TimeStamp.Current.Add(MessageLockExpirationThreshold));
                message.LockToken = lockToken;

                if (TryAddLockedMessage(subscriptionName, message))
                {
                    messageList.Add(message);
                    continue;
                }

                requeueTasks.Add(RequeueAsync(message));
            }

            Task.WaitAll(requeueTasks.ToArray());
            return messageList;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessagingEntity" />.
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
                    OperationalState = MessagingEntityOperationalState.Disabled;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Returns a collection of exclusive processing locks for messages contained by the current <see cref="MessagingEntity" />.
        /// </summary>
        /// <returns>
        /// A collection of exclusive processing locks for messages contained by the current <see cref="MessagingEntity" />.
        /// </returns>
        protected abstract IEnumerable<MessageLockToken> GetLockTokens();

        /// <summary>
        /// Gets the number of messages in the current <see cref="MessagingEntity" />.
        /// </summary>
        /// <returns>
        /// The number of messages in the current <see cref="MessagingEntity" />.
        /// </returns>
        protected abstract Int32 GetMessageCount();

        /// <summary>
        /// Asynchronously requeues a previously-enqueued message.
        /// </summary>
        /// <param name="message">
        /// The message to requeue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task RequeueAsync(PrimitiveMessage message)
        {
            RejectIfDisposed();
            var lockToken = new MessageLockToken(Guid.NewGuid(), message.RejectIf().IsNull(nameof(message)).TargetArgument.Identifier);
            message.LockToken = lockToken;
            return EnqueueAsync(message);
        }

        /// <summary>
        /// Gets the maximum length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        public TimeSpan EnqueueTimeoutThreshold
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the messaging entity type of the current <see cref="MessagingEntity" />.
        /// </summary>
        public abstract MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the current <see cref="MessagingEntity" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="MessagingEntity" /> is empty.
        /// </summary>
        public Boolean IsEmpty => MessageCount == 0;

        /// <summary>
        /// Gets a collection of exclusive processing locks for messages contained by the current <see cref="MessagingEntity" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<MessageLockToken> LockTokens
        {
            get
            {
                RejectIfDisposed();
                var lockTokens = GetLockTokens();

                foreach (var lockToken in lockTokens)
                {
                    RejectIfDisposed();
                    yield return lockToken;
                }
            }
        }

        /// <summary>
        /// Gets the format that is used to serialize enqueued message bodies.
        /// </summary>
        public SerializationFormat MessageBodySerializationFormat
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of messages in the current <see cref="MessagingEntity" />.
        /// </summary>
        public Int32 MessageCount => GetMessageCount();

        /// <summary>
        /// Gets the length of time that a locked message is held before abandoning the associated token and making the message
        /// available for processing.
        /// </summary>
        public TimeSpan MessageLockExpirationThreshold
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the operational state of the current <see cref="MessagingEntity" />.
        /// </summary>
        public MessagingEntityOperationalState OperationalState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a unique textual path that identifies the current <see cref="MessagingEntity" />.
        /// </summary>
        public IMessagingEntityPath Path
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current operational state permits dequeue operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean CurrentStatePermitsDequeue => OperationalState == MessagingEntityOperationalState.DequeueOnly || OperationalState == MessagingEntityOperationalState.Ready;

        /// <summary>
        /// Gets a value indicating whether or not the current operational state permits enqueue operations.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean CurrentStatePermitsEnqueue => OperationalState == MessagingEntityOperationalState.EnqueueOnly || OperationalState == MessagingEntityOperationalState.Ready;

        /// <summary>
        /// Represents the default format that is used to serialize enqueued message bodies.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SerializationFormat DefaultMessageBodySerializationFormat = PrimitiveMessage.DefaultBodySerializationFormat;

        /// <summary>
        /// Represents the default length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultEnqueueTimeoutThreshold = TimeSpan.FromSeconds(11);

        /// <summary>
        /// Represents the default length of time that a locked message is held before abandoning the associated token and making
        /// the message available for processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultMessageLockExpirationThreshold = TimeSpan.FromMinutes(3);

        /// <summary>
        /// Represents the length of time that <see cref="EnqueueAsync(PrimitiveMessage)" /> waits between attempts when the entity
        /// is unavailable.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan EnqueueDelayDuration = TimeSpan.FromMilliseconds(3);

        /// <summary>
        /// Represents the minimum permissible length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan EnqueueTimeoutThresholdFloor = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Represents the minimum permissible length of time that a locked message is held before abandoning the associated token
        /// and making the message available for processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan MessageLockExpirationThresholdFloor = TimeSpan.FromSeconds(8);
    }
}