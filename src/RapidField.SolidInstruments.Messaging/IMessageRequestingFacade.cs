// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Facilitates implementation-specific requesting operations for a message bus.
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
    /// <typeparam name="TTransmittingFacade">
    /// The type of the implementation-specific messaging facade that is used to transmit request messages.
    /// </typeparam>
    /// <typeparam name="TListeningFacade">
    /// The type of the implementation-specific messaging facade that listens for request messages.
    /// </typeparam>
    public interface IMessageRequestingFacade<TSender, TReceiver, TAdaptedMessage, TTransmittingFacade, TListeningFacade> : IMessageRequestingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
        where TTransmittingFacade : MessageTransmittingFacade<TSender, TReceiver, TAdaptedMessage>
        where TListeningFacade : MessageListeningFacade<TSender, TReceiver, TAdaptedMessage>
    {
    }

    /// <summary>
    /// Facilitates implementation-specific requesting operations for a message bus.
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
    public interface IMessageRequestingFacade<TSender, TReceiver, TAdaptedMessage> : IMessageRequestingFacade, IMessagingFacade<TSender, TReceiver, TAdaptedMessage>
        where TAdaptedMessage : class
    {
    }

    /// <summary>
    /// Facilitates implementation-specific requesting operations for a message bus.
    /// </summary>
    public interface IMessageRequestingFacade : IMessagingFacade
    {
        /// <summary>
        /// Asynchronously transmits the specified request message to a bus and waits for the correlated response message.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message.
        /// </typeparam>
        /// <param name="requestMessage">
        /// The request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the correlated response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="requestMessage" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="MessageRequestingException">
        /// An exception was raised while attempting to process <paramref name="requestMessage" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Task<TResponseMessage> RequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage requestMessage)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;
    }
}