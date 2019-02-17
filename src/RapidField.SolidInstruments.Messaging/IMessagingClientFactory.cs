// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

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
    public interface IMessagingClientFactory<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
        /// <summary>
        /// Gets a shared, managed, implementation-specific message receiver.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message receiver.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        TReceiver GetMessageReceiver<TMessage>(MessagingEntityType entityType)
            where TMessage : class;

        /// <summary>
        /// Gets a shared, managed, implementation-specific message sender.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message that the client handles.
        /// </typeparam>
        /// <param name="entityType">
        /// The type of the entity.
        /// </param>
        /// <returns>
        /// The managed, implementation-specific message sender.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while creating the client.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        TSender GetMessageSender<TMessage>(MessagingEntityType entityType)
            where TMessage : class;
    }
}