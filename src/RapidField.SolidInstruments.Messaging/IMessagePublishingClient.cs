// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates publishing operations for a message bus.
    /// </summary>
    public interface IMessagePublishingClient : IMessagingClient
    {
        /// <summary>
        /// Asynchronously publishes the specified message to a bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to publish.
        /// </typeparam>
        /// <param name="message">
        /// The message to publish.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="message" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task PublishAsync<TMessage>(TMessage message, MessagingEntityType entityType)
            where TMessage : class, IMessage;

        /// <summary>
        /// Asynchronously publishes the specified request message to a bus and waits for the correlated response message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to publish.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessage" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish <paramref name="requestMessage" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;
    }
}