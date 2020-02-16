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
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
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
        Task TransmitToQueueAsync<TMessage>(TMessage message, IEnumerable<String> pathTokens)
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
        Task TransmitToQueueAsync<TMessage>(TMessage message)
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
        /// <param name="pathTokens">
        /// An ordered collection of non-null, non-empty alphanumeric string tokens from which to construct the path, or
        /// <see langword="null" /> to omit path tokens. The default value is <see langword="null" />.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="pathTokens" /> contains one or more null or empty tokens and/or tokens with non-alphanumeric characters.
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
        Task TransmitToTopicAsync<TMessage>(TMessage message, IEnumerable<String> pathTokens)
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
        Task TransmitToTopicAsync<TMessage>(TMessage message)
            where TMessage : class, IMessageBase;
    }
}