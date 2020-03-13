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
    internal interface IMessageTransport : IInstrument
    {
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Boolean QueueExists(IMessagingEntityPath path);

        /// <summary>
        /// Returns a value indicating whether or not the specified topic exists.
        /// </summary>
        /// <param name="path">
        /// A unique textual path that identifies the topic.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the topic exists, otherwise <see langword="false" />.
        /// </returns>
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
        /// Gets a collection of active connections to the current <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
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
        /// Gets a collection of available queue paths for the current <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IEnumerable<IMessagingEntityPath> QueuePaths
        {
            get;
        }

        /// <summary>
        /// Gets a collection of available topic paths for the current <see cref="IMessageTransport" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IEnumerable<IMessagingEntityPath> TopicPaths
        {
            get;
        }
    }
}