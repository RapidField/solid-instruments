// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Represents a durable message queue.
    /// </summary>
    public interface IDurableMessageQueue : IAsyncDisposable, IDisposable
    {
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
        Task ConveyFailureAsync(DurableMessageLockToken lockToken);

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
        Task ConveySuccessAsync(DurableMessageLockToken lockToken);

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
        Task<IEnumerable<DurableMessage>> DequeueAsync(Int32 count);

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
        Task EnqueueAsync(IMessageBase message);

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.EnqueueOnly" />, or to
        /// <see cref="DurableMessageQueueOperationalState.Paused" /> if the previous state was
        /// <see cref="DurableMessageQueueOperationalState.DequeueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryDisableDequeues();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.DequeueOnly" />, or to
        /// <see cref="DurableMessageQueueOperationalState.Paused" /> if the previous state was
        /// <see cref="DurableMessageQueueOperationalState.EnqueueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryDisableEnqueues();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.Paused" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryPause();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IDurableMessageQueue" /> to
        /// <see cref="DurableMessageQueueOperationalState.Ready" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryResume();

        /// <summary>
        /// Gets the number of messages in the current <see cref="IDurableMessageQueue" />.
        /// </summary>
        Int32 Depth
        {
            get;
        }

        /// <summary>
        /// Gets the maximum length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        TimeSpan EnqueueTimeoutThreshold
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the current <see cref="IDurableMessageQueue" />.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IDurableMessageQueue" /> is empty.
        /// </summary>
        Boolean IsEmpty
        {
            get;
        }

        /// <summary>
        /// Gets the format that is used to serialize enqueued message bodies.
        /// </summary>
        SerializationFormat MessageBodySerializationFormat
        {
            get;
        }

        /// <summary>
        /// Gets the length of time that a locked message is held before abandoning the associated token and making the message
        /// available for processing.
        /// </summary>
        TimeSpan MessageLockExpirationThreshold
        {
            get;
        }

        /// <summary>
        /// Gets the operational state of the current <see cref="IDurableMessageQueue" />.
        /// </summary>
        DurableMessageQueueOperationalState OperationalState
        {
            get;
        }

        /// <summary>
        /// Gets the unique textual path that identifies the current <see cref="IDurableMessageQueue" />.
        /// </summary>
        String Path
        {
            get;
        }
    }
}