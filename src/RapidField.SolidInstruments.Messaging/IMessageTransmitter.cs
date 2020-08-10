// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Transmits messages.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is transmitted by the transmitter.
    /// </typeparam>
    public interface IMessageTransmitter<TMessage> : IMessageTransmitter, IMessageHandler<TMessage>, ICommandHandler<TMessage>
        where TMessage : class, IMessage
    {
    }

    /// <summary>
    /// Transmits messages.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is transmitted by the transmitter.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public interface IMessageTransmitter<TRequestMessage, TResponseMessage> : IMessageTransmitter, IMessageHandler<TRequestMessage, TResponseMessage>, ICommandHandler<TRequestMessage>, IRequestTransmitter
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
    }

    /// <summary>
    /// Transmits messages.
    /// </summary>
    public interface IMessageTransmitter : IMessageHandler
    {
        /// <summary>
        /// Gets the type of the message that the current <see cref="IMessageTransmitter" /> transmits.
        /// </summary>
        public Type MessageType
        {
            get;
        }
    }
}