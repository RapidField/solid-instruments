// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes messages as a listener.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is listened for.
    /// </typeparam>
    public interface IMessageListener<TMessage> : IMessageListener, IMessageHandler<TMessage>, ICommandHandler<TMessage>
        where TMessage : class, IMessage
    {
    }

    /// <summary>
    /// Processes messages as a listener.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is processed by the listener.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public interface IMessageListener<TRequestMessage, TResponseMessage> : IMessageListener, IMessageHandler<TRequestMessage, TResponseMessage>, ICommandHandler<TRequestMessage>, IRequestListener
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
    }

    /// <summary>
    /// Processes messages as a listener.
    /// </summary>
    public interface IMessageListener : IMessageHandler
    {
        /// <summary>
        /// Gets the type of the message that the current <see cref="IMessageListener" /> processes.
        /// </summary>
        public Type MessageType
        {
            get;
        }
    }
}