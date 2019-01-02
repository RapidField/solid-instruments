// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes messages as a subscriber.
    /// </summary>
    public interface IMessageSubscriber
    {
        /// <summary>
        /// Gets the type of the message that the current <see cref="IMessageSubscriber" /> processes.
        /// </summary>
        Type MessageType
        {
            get;
        }
    }

    /// <summary>
    /// Processes messages as a subscriber.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is subscribed to.
    /// </typeparam>
    public interface IMessageSubscriber<TMessage> : IMessageSubscriber, IMessageHandler<TMessage>, ICommandHandler<TMessage>
        where TMessage : class, IMessage
    {
    }

    /// <summary>
    /// Processes messages as a subscriber.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is processed by the subscriber.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is published in response to the request.
    /// </typeparam>
    public interface IMessageSubscriber<TRequestMessage, TResponseMessage> : IMessageSubscriber, IMessageHandler<TRequestMessage, TResponseMessage>, ICommandHandler<TRequestMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
    }
}