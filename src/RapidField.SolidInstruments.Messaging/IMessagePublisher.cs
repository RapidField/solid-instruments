// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Publishes messages.
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Gets the type of the message that the current <see cref="IMessagePublisher" /> publishes.
        /// </summary>
        Type MessageType
        {
            get;
        }
    }

    /// <summary>
    /// Publishes messages.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is published by the publisher.
    /// </typeparam>
    public interface IMessagePublisher<TMessage> : IMessagePublisher, IMessageHandler<TMessage>, ICommandHandler<TMessage>
        where TMessage : class, IMessage
    {
    }

    /// <summary>
    /// Publishes messages.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is published by the publisher.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is published in response to the request.
    /// </typeparam>
    public interface IMessagePublisher<TRequestMessage, TResponseMessage> : IMessagePublisher, IMessageHandler<TRequestMessage, TResponseMessage>, ICommandHandler<TRequestMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
    }
}