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
    /// Represents a messaging entity.
    /// </summary>
    internal interface IMessagingEntity : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Asynchronously notifies the current <see cref="IMessagingEntity" /> that a locked message was not processed and can be
        /// made available for processing by other consumers.
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
        Task ConveyFailureAsync(MessageLockToken lockToken);

        /// <summary>
        /// Asynchronously notifies the current <see cref="IMessagingEntity" /> that a locked message was processed successfully and
        /// can be destroyed permanently.
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
        Task ConveySuccessAsync(MessageLockToken lockToken);

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
        Task EnqueueAsync(PrimitiveMessage message);

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessagingEntity" /> to
        /// <see cref="MessagingEntityOperationalState.EnqueueOnly" />, or to <see cref="MessagingEntityOperationalState.Paused" />
        /// if the previous state was <see cref="MessagingEntityOperationalState.DequeueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryDisableDequeues();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessagingEntity" /> to
        /// <see cref="MessagingEntityOperationalState.DequeueOnly" />, or to <see cref="MessagingEntityOperationalState.Paused" />
        /// if the previous state was <see cref="MessagingEntityOperationalState.EnqueueOnly" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryDisableEnqueues();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessagingEntity" /> to
        /// <see cref="MessagingEntityOperationalState.Paused" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryPause();

        /// <summary>
        /// Attempts to set the operational state of the current <see cref="IMessagingEntity" /> to
        /// <see cref="MessagingEntityOperationalState.Ready" />.
        /// </summary>
        /// <returns>
        /// True if the operational state was successfully set, otherwise false.
        /// </returns>
        Boolean TryResume();

        /// <summary>
        /// Gets the maximum length of time to wait for a message to be enqueued before raising an exception.
        /// </summary>
        TimeSpan EnqueueTimeoutThreshold
        {
            get;
        }

        /// <summary>
        /// Gets the messaging entity type of the current <see cref="IMessagingEntity" />.
        /// </summary>
        MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the current <see cref="IMessagingEntity" />.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IMessagingEntity" /> is empty.
        /// </summary>
        Boolean IsEmpty
        {
            get;
        }

        /// <summary>
        /// Gets a collection of exclusive processing locks for messages contained by the current <see cref="IMessagingEntity" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IEnumerable<MessageLockToken> LockTokens
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
        /// Gets the number of messages in the current <see cref="IMessagingEntity" />.
        /// </summary>
        Int32 MessageCount
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
        /// Gets the operational state of the current <see cref="IMessagingEntity" />.
        /// </summary>
        MessagingEntityOperationalState OperationalState
        {
            get;
        }

        /// <summary>
        /// Gets a unique textual path that identifies the current <see cref="IMessagingEntity" />.
        /// </summary>
        IMessagingEntityPath Path
        {
            get;
        }
    }
}