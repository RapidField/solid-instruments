// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Manages the listener types that are supported by a
    /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
    /// </summary>
    public interface IMessageListeningProfile
    {
        /// <summary>
        /// Adds support for the specified queue message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TMessage" /> was already added.
        /// </exception>
        public void AddQueueListener<TMessage>()
            where TMessage : class, IMessage;

        /// <summary>
        /// Adds support for the specified request message type.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message for which support is added.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is associated with <typeparamref name="TRequestMessage" />.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TRequestMessage" /> was already added.
        /// </exception>
        public void AddRequestListener<TRequestMessage, TResponseMessage>()
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage;

        /// <summary>
        /// Adds support for the specified topic message type.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TMessage" /> was already added.
        /// </exception>
        public void AddTopicListener<TMessage>()
            where TMessage : class, IMessage;

        /// <summary>
        /// Gets a collection of message types that are supported by the associated
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        public IEnumerable<Type> SupportedMessageTypes
        {
            get;
        }
    }
}