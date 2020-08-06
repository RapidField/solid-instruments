// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
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
        /// Adds support for the specified command message type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command for which support is added.
        /// </typeparam>
        /// <typeparam name="TCommandMessage">
        /// The type of the command message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TCommandMessage" /> was already added.
        /// </exception>
        public void AddCommandListener<TCommand, TCommandMessage>()
            where TCommand : class, ICommand
            where TCommandMessage : class, ICommandMessage<TCommand>;

        /// <summary>
        /// Adds support for the specified event message type.
        /// </summary>
        /// <typeparam name="TEvent">
        /// The type of the event for which support is added.
        /// </typeparam>
        /// <typeparam name="TEventMessage">
        /// The type of the event message for which support is added.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="TEventMessage" /> was already added.
        /// </exception>
        public void AddEventListener<TEvent, TEventMessage>()
            where TEvent : class, IEvent
            where TEventMessage : class, IEventMessage<TEvent>;

        /// <summary>
        /// Adds support for the <see cref="HeartbeatMessage" /> type.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="HeartbeatMessage" /> was already added.
        /// </exception>
        public void AddHeartbeatListener();

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