// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific transmission operations for a message bus.
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
    public interface IMessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage> : IMessageTransmittingFacade, IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific transmission operations for a message bus.
    /// </summary>
    public interface IMessageTransmittingFacade : IMessagingFacade
    {
        /// <summary>
        /// Asynchronously transmits the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToQueueAsync<TMessage>(TMessage message, IEnumerable<String> pathLabels)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Asynchronously transmits the specified message to a queue.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToQueueAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Asynchronously transmits the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <param name="pathLabels">
        /// An ordered collection of as many as three non-null, non-empty alphanumeric textual labels to append to the path, or
        /// <see langword="null" /> to omit path labels. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathLabels" /> contains one or more null or empty labels and/or labels with non-alphanumeric characters,
        /// or contains more than three elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToTopicAsync<TMessage>(TMessage message, IEnumerable<String> pathLabels)
            where TMessage : class, IMessageBase;

        /// <summary>
        /// Asynchronously transmits the specified message to a topic.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <param name="message">
        /// The message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task TransmitToTopicAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase;
    }
}