// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific subscription operations for a message bus.
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
    /// <typeparam name="TPublishingFacade">
    /// The type of the implementation-specific messaging facade that is used to publish request messages.
    /// </typeparam>
    public interface IMessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage, TPublishingFacade> : IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TPublishingFacade : MessagePublishingFacade<TSender, TReceiver, TAdaptedMessage>
    {
    }

    /// <summary>
    /// Facilitates implementation-specific subscription operations for a message bus.
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
    public interface IMessageSubscriptionFacade<TSender, TReceiver, TAdaptedMessage> : IMessageSubscriptionFacade, IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific subscription operations for a message bus.
    /// </summary>
    public interface IMessageSubscriptionFacade : IMessagingFacade
    {
        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message.
        /// </typeparam>
        /// <param name="messageHandler">
        /// An action that handles a message.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while attempting to register <paramref name="messageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void RegisterHandler<TMessage>(Action<TMessage> messageHandler, MessagingEntityType entityType)
            where TMessage : class, IMessage;

        /// <summary>
        /// Registers the specified message handler with the bus.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessageHandler">
        /// A function that handles a request message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessageHandler" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageSubscriptionException">
        /// An exception was raised while attempting to register <paramref name="requestMessageHandler" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void RegisterHandler<TRequestMessage, TResponseMessage>(Func<TRequestMessage, TResponseMessage> requestMessageHandler)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Gets the collection of message types for which the current <see cref="IMessageSubscriptionFacade" /> has one or more
        /// registered handlers.
        /// </summary>
        IEnumerable<Type> SubscribedMessageTypes
        {
            get;
        }
    }
}