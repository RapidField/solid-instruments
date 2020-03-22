// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.TransportPrimitives
{
    /// <summary>
    /// Supports message exchange for a collection of queues and topics.
    /// </summary>
    public interface IMessageTransport : IInstrument
    {
        /// <summary>
        /// Closes the specified connection as an idempotent operation.
        /// </summary>
        /// <param name="connection">
        /// The connection to close.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connection" /> is <see langword="null" />.
        /// </exception>
        void CloseConnection(IMessageTransportConnection connection);

        /// <summary>
        /// Asynchronously notifies the specified queue that a locked message was not processed and can be made available for
        /// processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing queue.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        Task ConveyFailureToQueueAsync(MessageLockToken lockToken, IMessagingEntityPath path);

        /// <summary>
        /// Asynchronously notifies the specified subscription that a locked message was not processed and can be made available for
        /// processing by other consumers.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was not processed.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing topic.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The operation timed out.
        /// </exception>
        Task ConveyFailureToSubscriptionAsync(MessageLockToken lockToken, IMessagingEntityPath path);

        /// <summary>
        /// Asynchronously notifies the specified queue that a locked message was processed successfully and can be destroyed
        /// permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing queue.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task ConveySuccessToQueueAsync(MessageLockToken lockToken, IMessagingEntityPath path);

        /// <summary>
        /// Asynchronously notifies the specified subscription that a locked message was processed successfully and can be destroyed
        /// permanently.
        /// </summary>
        /// <param name="lockToken">
        /// A lock token corresponding to a message that was processed successfully.
        /// </param>
        /// <param name="path">
        /// A unique textual path that identifies the subscription topic.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockToken" /> is <see langword="null" /> -or- <paramref name="path" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="lockToken" /> does not reference an existing locked message -or- <paramref name="path" /> does not
        /// reference an existing topic.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task ConveySuccessToSubscriptionAsync(MessageLockToken lockToken, IMessagingEntityPath path);

        /// <summary>
        /// Opens and returns a new <see cref="IMessageTransportConnection" /> to the current <see cref="IMessageTransport" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="IMessageTransportConnection" /> to the current <see cref="IMessageTransport" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IMessageTransportConnection CreateConnection();

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
        Task CreateQueueAsync(IMessagingEntityPath path);

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
        Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold);

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
        Task CreateQueueAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold);

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
        Task CreateSubscriptionAsync(IMessagingEntityPath path, String subscriptionName);

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
        Task CreateTopicAsync(IMessagingEntityPath path);

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
        Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold);

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
        Task CreateTopicAsync(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold);

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
        /// The specified entity does not exist.
        /// </exception>
        Task DestroyQueueAsync(IMessagingEntityPath path);

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
        Task DestroySubscriptionAsync(IMessagingEntityPath path, String subscriptionName);

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
        /// The specified entity does not exist.
        /// </exception>
        Task DestroyTopicAsync(IMessagingEntityPath path);

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
        Boolean QueueExists(IMessagingEntityPath path);

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
        Task<IEnumerable<PrimitiveMessage>> ReceiveFromQueueAsync(IMessagingEntityPath path, Int32 count);

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
        Task<IEnumerable<PrimitiveMessage>> ReceiveFromTopicAsync(IMessagingEntityPath path, String subscriptionName, Int32 count);

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
        Task SendToQueueAsync(IMessagingEntityPath path, PrimitiveMessage message);

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
        Task SendToTopicAsync(IMessagingEntityPath path, PrimitiveMessage message);

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
        Boolean SubscriptionExists(IMessagingEntityPath path, String subscriptionName);

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
        Boolean TopicExists(IMessagingEntityPath path);

        /// <summary>
        /// Attempts to create a new queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully created, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryCreateQueue(IMessagingEntityPath path);

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
        Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold);

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
        Boolean TryCreateQueue(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold);

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
        Boolean TryCreateSubscription(IMessagingEntityPath path, String subscriptionName);

        /// <summary>
        /// Attempts to create a new topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the new topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully created, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryCreateTopic(IMessagingEntityPath path);

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
        Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold);

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
        Boolean TryCreateTopic(IMessagingEntityPath path, TimeSpan messageLockExpirationThreshold, TimeSpan enqueueTimeoutThreshold);

        /// <summary>
        /// Attempts to destroy the specified queue.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the queue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the queue was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryDestroyQueue(IMessagingEntityPath path);

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
        Boolean TryDestroySubscription(IMessagingEntityPath path, String subscriptionName);

        /// <summary>
        /// Attempts to destroy the specified topic.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic was successfully destroyed, otherwise <see langword="false" />.
        /// </returns>
        Boolean TryDestroyTopic(IMessagingEntityPath path);

        /// <summary>
        /// Gets the number of active connections to the current <see cref="IMessageTransport" />.
        /// </summary>
        Int32 ConnectionCount
        {
            get;
        }

        /// <summary>
        /// Gets a collection of active connections to the current <see cref="IMessageTransport" />.
        /// </summary>
        IEnumerable<IMessageTransportConnection> Connections
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
        /// Gets the number of queues within the current <see cref="IMessageTransport" />.
        /// </summary>
        Int32 QueueCount
        {
            get;
        }

        /// <summary>
        /// Gets a collection of available queue paths for the current <see cref="IMessageTransport" />.
        /// </summary>
        IEnumerable<IMessagingEntityPath> QueuePaths
        {
            get;
        }

        /// <summary>
        /// Gets the number of topics within the current <see cref="IMessageTransport" />.
        /// </summary>
        Int32 TopicCount
        {
            get;
        }

        /// <summary>
        /// Gets a collection of available topic paths for the current <see cref="IMessageTransport" />.
        /// </summary>
        IEnumerable<IMessagingEntityPath> TopicPaths
        {
            get;
        }
    }
}