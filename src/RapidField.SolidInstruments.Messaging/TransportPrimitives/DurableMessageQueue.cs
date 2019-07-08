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
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a durable message queue.
    /// </summary>
    /// <remarks>
    /// <see cref="DurableMessageQueue" /> is the default implementation of <see cref="IDurableMessageQueue" />.
    /// </remarks>
    public sealed class DurableMessageQueue : Instrument, IDurableMessageQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(IDurableMessageQueuePersistenceProxy persistenceProxy)
            : this(Guid.NewGuid(), persistenceProxy)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, IDurableMessageQueuePersistenceProxy persistenceProxy)
            : this(identifier, $"{identifier.ToSerializedString()}", persistenceProxy)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy)
            : this(identifier, path, persistenceProxy, DurableMessageQueueOperationalState.Ready)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="DurableMessageQueueOperationalState.Ready" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="DurableMessageQueueOperationalState.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy, DurableMessageQueueOperationalState operationalState)
            : this(identifier, path, persistenceProxy, operationalState, DefaultMessageBodySerializationFormat)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="DurableMessageQueueOperationalState.Ready" />.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="DurableMessageQueueOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" />
        /// is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy, DurableMessageQueueOperationalState operationalState, SerializationFormat messageBodySerializationFormat)
            : this(identifier, path, persistenceProxy, operationalState, messageBodySerializationFormat, DefaultMessageLockExpirationThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="DurableMessageQueueOperationalState.Ready" />.
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <param name="messageLockExpirationThreshold">
        /// The length of time that a locked message is held before abandoning the associated token and making the message available
        /// for processing. The default value is three minutes.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="DurableMessageQueueOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" />
        /// is equal to <see cref="SerializationFormat.Unspecified" /> -or- <paramref name="messageLockExpirationThreshold" /> is
        /// less than eight seconds.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy, DurableMessageQueueOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold)
            : this(identifier, path, persistenceProxy, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold, DefaultEnqueueTimeoutThreshold)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="DurableMessageQueueOperationalState.Ready" />.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="DurableMessageQueueOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" />
        /// is equal to <see cref="SerializationFormat.Unspecified" /> -or- <paramref name="messageLockExpirationThreshold" /> is
        /// less than eight seconds -or- <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        [DebuggerHidden]
        internal DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy, DurableMessageQueueOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold)
            : this(identifier, path, persistenceProxy, operationalState, messageBodySerializationFormat, messageLockExpirationThreshold, enqueueTimeoutThreshold, Array.Empty<DurableMessage>(), new Dictionary<DurableMessageLockToken, DurableMessage>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurableMessageQueue" /> class.
        /// </summary>
        /// <param name="identifier">
        /// A unique identifier for the queue.
        /// </param>
        /// <param name="path">
        /// The unique textual path that identifies the queue.
        /// </param>
        /// <param name="persistenceProxy">
        /// An object that manages thread-safe persistence for the queue.
        /// </param>
        /// <param name="operationalState">
        /// The operational state of the queue. The default value is <see cref="DurableMessageQueueOperationalState.Ready" />.
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
        /// <param name="messages">
        /// The underlying first-in first-out collection that contains enqueued messages.
        /// </param>
        /// <param name="lockedMessages">
        /// The underlying first-in first-out collection that contains messages that are locked for processing.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="path" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> is <see langword="null" /> -or- <paramref name="persistenceProxy" /> is <see langword="null" />
        /// -or- <paramref name="messages" /> is <see langword="null" /> -or- <paramref name="lockedMessages" /> is
        ///  <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="operationalState" /> is equal
        /// to <see cref="DurableMessageQueueOperationalState.Unspecified" /> -or- <paramref name="messageBodySerializationFormat" />
        /// is equal to <see cref="SerializationFormat.Unspecified" /> -or- <paramref name="messageLockExpirationThreshold" /> is
        /// less than eight seconds -or- <paramref name="enqueueTimeoutThreshold" /> is less than two seconds.
        /// </exception>
        [DebuggerHidden]
        private DurableMessageQueue(Guid identifier, String path, IDurableMessageQueuePersistenceProxy persistenceProxy, DurableMessageQueueOperationalState operationalState, SerializationFormat messageBodySerializationFormat, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold, IEnumerable<DurableMessage> messages, IDictionary<DurableMessageLockToken, DurableMessage> lockedMessages)
            : base()
        {
            EnqueueTimeoutThreshold = enqueueTimeoutThreshold.RejectIf().IsLessThan(EnqueueTimeoutThresholdFloor, nameof(enqueueTimeoutThreshold));
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            LockedMessages = new ConcurrentDictionary<DurableMessageLockToken, DurableMessage>(lockedMessages.RejectIf().IsNull(nameof(lockedMessages)).TargetArgument);
            Messages = new ConcurrentQueue<DurableMessage>(messages.RejectIf().IsNull(nameof(messages)).TargetArgument);
            MessageBodySerializationFormat = messageBodySerializationFormat.RejectIf().IsEqualToValue(SerializationFormat.Unspecified, nameof(messageBodySerializationFormat));
            MessageLockExpirationThreshold = messageLockExpirationThreshold.RejectIf().IsLessThan(MessageLockExpirationThresholdFloor, nameof(messageLockExpirationThreshold));
            OperationalState = operationalState.RejectIf().IsEqualToValue(DurableMessageQueueOperationalState.Unspecified, nameof(operationalState));
            Path = path.RejectIf().IsNullOrEmpty(nameof(path));
            PersistenceProxy = persistenceProxy.RejectIf().IsNull(nameof(persistenceProxy)).TargetArgument;
        }

        /// <summary>
        /// Asynchronously notifies the queue that a locked message was not processed and can be made available for processing by
        /// other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task ConveyFailureAsync(DurableMessageLockToken lockToken)
        {
            RejectIfDisposed();

            if (LockedMessages.TryRemove(lockToken.RejectIf().IsNull(nameof(lockToken)), out var message))
            {
                return RequeueAsync(message);
            }

            throw new ArgumentException("The specified lock token does not reference an existing locked message in the queue.", nameof(lockToken));
        }

        /// <summary>
        /// Asynchronously notifies the queue that a locked message was processed successfully and can be destroyed permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="lockToken" /> does not reference an existing locked message.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task ConveySuccessAsync(DurableMessageLockToken lockToken)
        {
            RejectIfDisposed();

            if (LockedMessages.TryRemove(lockToken.RejectIf().IsNull(nameof(lockToken)), out _))
            {
                return PersistSnapshotAsync();
            }

            throw new ArgumentException("The specified lock token does not reference an existing locked message in the queue.", nameof(lockToken));
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
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public async Task<IEnumerable<DurableMessage>> DequeueAsync(Int32 count)
        {
            RejectIfDisposed();

            if (count.RejectIf().IsLessThan(0, nameof(count)) == 0)
            {
                return Array.Empty<DurableMessage>();
            }

            var messageList = new List<DurableMessage>();

            while (messageList.Count() < count && TryDequeue(out var message))
            {
                var lockToken = new DurableMessageLockToken(Guid.NewGuid(), message.Identifier, TimeStamp.Current.Add(MessageLockExpirationThreshold));
                message.LockToken = lockToken;

                if (LockedMessages.TryAdd(lockToken, message))
                {
                    messageList.Add(message);
                    continue;
                }

                await RequeueAsync(message).ConfigureAwait(false);
            }

            if (messageList.Any())
            {
                await PersistSnapshotAsync().ConfigureAwait(false);
            }

            return messageList;
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
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        public Task EnqueueAsync(IMessageBase message)
        {
            var lockToken = new DurableMessageLockToken(Guid.NewGuid(), message.RejectIf().IsNull(nameof(message)).TargetArgument.Identifier);
            var durableMessage = new DurableMessage(message, lockToken, MessageBodySerializationFormat);
            return EnqueueAsync(durableMessage);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DurableMessageQueue" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DurableMessageQueue" />.
        /// </returns>
        public sealed override String ToString() => Path;

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.EnqueueOnly" />, or to
        /// <see cref="DurableMessageQueueOperationalState.Paused" /> if the previous state was
        /// <see cref="DurableMessageQueueOperationalState.DequeueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryDisableDequeues()
        {
            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.DequeueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Paused;
                    return true;

                case DurableMessageQueueOperationalState.EnqueueOnly:

                    return true;

                case DurableMessageQueueOperationalState.Paused:

                    return true;

                case DurableMessageQueueOperationalState.Ready:

                    OperationalState = DurableMessageQueueOperationalState.EnqueueOnly;
                    return true;

                default:

                    return false;
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.DequeueOnly" />, or to
        /// <see cref="DurableMessageQueueOperationalState.Paused" /> if the previous state was
        /// <see cref="DurableMessageQueueOperationalState.EnqueueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryDisableEnqueues()
        {
            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.DequeueOnly:

                    return true;

                case DurableMessageQueueOperationalState.EnqueueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Paused;
                    return true;

                case DurableMessageQueueOperationalState.Paused:

                    return true;

                case DurableMessageQueueOperationalState.Ready:

                    OperationalState = DurableMessageQueueOperationalState.DequeueOnly;
                    return true;

                default:

                    return false;
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.Paused" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryPause()
        {
            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.DequeueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Paused;
                    return true;

                case DurableMessageQueueOperationalState.EnqueueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Paused;
                    return true;

                case DurableMessageQueueOperationalState.Paused:

                    return true;

                case DurableMessageQueueOperationalState.Ready:

                    OperationalState = DurableMessageQueueOperationalState.Paused;
                    return true;

                default:

                    return false;
            }
        }

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.Ready" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        public Boolean TryResume()
        {
            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.DequeueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Ready;
                    return true;

                case DurableMessageQueueOperationalState.EnqueueOnly:

                    OperationalState = DurableMessageQueueOperationalState.Ready;
                    return true;

                case DurableMessageQueueOperationalState.Paused:

                    OperationalState = DurableMessageQueueOperationalState.Ready;
                    return true;

                case DurableMessageQueueOperationalState.Ready:

                    return true;

                default:

                    return false;
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DurableMessageQueue" />.
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
                    OperationalState = DurableMessageQueueOperationalState.Disabled;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Produces a serializable persistence snapshot of the current <see cref="DurableMessageQueue" /> in a thread-safe manner.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A serializable persistence snapshot of the current <see cref="DurableMessageQueue" />.
        /// </returns>
        [DebuggerHidden]
        private DurableMessageQueueSnapshot CaptureSnapshot(ConcurrencyControlToken controlToken) => new DurableMessageQueueSnapshot(this);

        /// <summary>
        /// Asynchronously produces a serializable persistence snapshot of the current <see cref="IDurableMessageQueue" /> in a
        /// thread-safe manner.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation and containing a serializable persistence snapshot of the current
        /// <see cref="IDurableMessageQueue" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private Task<DurableMessageQueueSnapshot> CaptureSnapshotAsync()
        {
            RejectIfDisposed();

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                var snapshot = CaptureSnapshot(controlToken);
                return Task.FromResult(snapshot);
            }
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
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task EnqueueAsync(DurableMessage message)
        {
            if (TryEnqueue(message))
            {
                return PersistSnapshotAsync();
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                while (stopwatch.Elapsed < EnqueueTimeoutThreshold)
                {
                    RejectIfDisposed();
                    Thread.Sleep(EnqueueDelayDuration);

                    if (TryEnqueue(message))
                    {
                        return PersistSnapshotAsync();
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            throw new TimeoutException($"The timeout threshold duration was exceeded while waiting for queue availability (path: {Path}).");
        }

        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the current <see cref="DurableMessageQueue" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private async Task PersistSnapshotAsync()
        {
            RejectIfDisposed();

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                await PersistSnapshotAsync(controlToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Asynchronously persists a thread-safe snapshot of the current <see cref="DurableMessageQueue" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        [DebuggerHidden]
        private Task PersistSnapshotAsync(ConcurrencyControlToken controlToken)
        {
            var snapshot = CaptureSnapshot(controlToken);
            return PersistenceProxy.PersistSnapshotAsync(snapshot);
        }

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
        /// <exception cref="DurableMessageQueuePersistenceException">
        /// An exception was raised while attempting to persist the snapshot.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        [DebuggerHidden]
        private Task RequeueAsync(DurableMessage message)
        {
            var lockToken = new DurableMessageLockToken(Guid.NewGuid(), message.RejectIf().IsNull(nameof(message)).TargetArgument.Identifier);
            message.LockToken = lockToken;
            return EnqueueAsync(message);
        }

        /// <summary>
        /// Attempts to remove and return the next message in the queue.
        /// </summary>
        /// <param name="message">
        /// The resulting message.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the operation was successful.
        /// </returns>
        [DebuggerHidden]
        private Boolean TryDequeue(out DurableMessage message)
        {
            if (IsDisposedOrDisposing)
            {
                message = null;
                return false;
            }

            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.DequeueOnly:

                    return Messages.TryDequeue(out message);

                case DurableMessageQueueOperationalState.Ready:

                    return Messages.TryDequeue(out message);

                default:

                    message = null;
                    return false;
            }
        }

        /// <summary>
        /// Attempts to enqueue the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to enqueue.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the operation was successful.
        /// </returns>
        [DebuggerHidden]
        private Boolean TryEnqueue(DurableMessage message)
        {
            if (IsDisposedOrDisposing)
            {
                return false;
            }
            else if (message is null)
            {
                return false;
            }

            switch (OperationalState)
            {
                case DurableMessageQueueOperationalState.EnqueueOnly:

                    Messages.Enqueue(message);
                    return true;

                case DurableMessageQueueOperationalState.Ready:

                    Messages.Enqueue(message);
                    return true;

                default:

                    return false;
            }
        }

        /// <summary>
        /// Gets the number of messages in the current <see cref="DurableMessageQueue" />.
        /// </summary>
        public Int32 Depth => Messages.Count + LockedMessages.Count;

        /// <summary>
        /// Gets the maximum length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        public TimeSpan EnqueueTimeoutThreshold
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a unique identifier for the current <see cref="DurableMessageQueue" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="DurableMessageQueue" /> is empty.
        /// </summary>
        public Boolean IsEmpty => Depth == 0;

        /// <summary>
        /// Gets the format that is used to serialize enqueued message bodies.
        /// </summary>
        public SerializationFormat MessageBodySerializationFormat
        {
            get;
            private set;
        }

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
        /// Gets the operational state of the current <see cref="DurableMessageQueue" />.
        /// </summary>
        public DurableMessageQueueOperationalState OperationalState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique textual path that identifies the current <see cref="DurableMessageQueue" />.
        /// </summary>
        public String Path
        {
            get;
            internal set;
        }

        /// <summary>
        /// Represents the underlying first-in first-out collection that contains messages that are locked for processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ConcurrentDictionary<DurableMessageLockToken, DurableMessage> LockedMessages;

        /// <summary>
        /// Represents the underlying first-in first-out collection that contains enqueued messages.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ConcurrentQueue<DurableMessage> Messages;

        /// <summary>
        /// Represents the default format that is used to serialize enqueued message bodies.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const SerializationFormat DefaultMessageBodySerializationFormat = DurableMessage.DefaultBodySerializationFormat;

        /// <summary>
        /// Represents the default length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultEnqueueTimeoutThreshold = TimeSpan.FromSeconds(11);

        /// <summary>
        /// Represents the default length of time that a locked message is held before abandoning the associated token and making the
        /// message available for processing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultMessageLockExpirationThreshold = TimeSpan.FromMinutes(3);

        /// <summary>
        /// Represents the length of time that <see cref="EnqueueAsync(IMessageBase)" /> waits between attempts when the queue is
        /// unavailable.
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

        /// <summary>
        /// Represents an object that manages thread-safe persistence for the queue.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDurableMessageQueuePersistenceProxy PersistenceProxy;
    }
}