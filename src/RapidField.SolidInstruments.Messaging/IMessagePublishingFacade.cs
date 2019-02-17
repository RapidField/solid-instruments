// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific publishing operations for a message bus.
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
    public interface IMessagePublishingFacade<TSender, TReceiver, TAdaptedMessage> : IMessagePublishingFacade, IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific publishing operations for a message bus.
    /// </summary>
    public interface IMessagePublishingFacade : IMessagingFacade
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
            where TMessage : class, IMessageBase;
    }
}