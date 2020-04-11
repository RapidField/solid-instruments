// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Represents an appliance that creates manages implementation-specific messaging clients.
    /// </summary>
    /// <typeparam name="TSender">
    /// The type of the implementation-specific send client.
    /// </typeparam>
    /// <typeparam name="TReceiver">
    /// The type of the implementation-specific receive client.
    /// </typeparam>
    /// <typeparam name="TAdaptedMessage">
    /// The type of implementation-specific adapted messages.
    /// </typeparam>
    public interface IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage> : IAsyncDisposable, IDisposable
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Returns a queue entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A queue entity path for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        public IMessagingEntityPath GetQueuePath<TMessage>(IEnumerable<String> pathLabels)
             where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetQueueReceiver<TMessage>()
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetQueueReceiver<TMessage>(IEnumerable<String> pathLabels)
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetQueueSender<TMessage>()
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetQueueSender<TMessage>(IEnumerable<String> pathLabels)
            where TMessage : class;

        /// <summary>
        /// Returns a subscription name for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver, or <see langword="null" /> if the receiver is unspecified.
        /// </param>
        /// <param name="entityPath">
        /// The unique path for the entity.
        /// </param>
        /// <returns>
        /// A subscription name for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="receiverIdentifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="receiverIdentifier" /> is <see langword="null" /> -or- <paramref name="entityPath" /> is
        /// <see langword="null" />.
        /// </exception>
        public String GetSubscriptionName<TMessage>(String receiverIdentifier, IMessagingEntityPath entityPath)
            where TMessage : class;

        /// <summary>
        /// Returns a topic entity path for the specified message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A topic entity path for the specified message type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        public IMessagingEntityPath GetTopicPath<TMessage>(IEnumerable<String> pathLabels)
             where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="receiverIdentifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="receiverIdentifier" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetTopicReceiver<TMessage>(String receiverIdentifier)
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="receiverIdentifier">
        /// A unique textual identifier for the message receiver.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="receiverIdentifier" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="receiverIdentifier" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageListeningException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TReceiver GetTopicReceiver<TMessage>(String receiverIdentifier, IEnumerable<String> pathLabels)
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetTopicSender<TMessage>()
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender for a type-defined topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TSender GetTopicSender<TMessage>(IEnumerable<String> pathLabels)
            where TMessage : class;
    }
}