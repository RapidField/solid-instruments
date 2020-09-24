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
    /// <remarks>
    /// <see cref="MessageListener{TMessage}" /> is the default implementation of <see cref="IMessageListener{TMessage}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is listened for.
    /// </typeparam>
    public abstract class MessageListener<TMessage> : MessageHandler<TMessage>, IMessageListener<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListener{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="entityType" /> is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessageListener(ICommandMediator mediator, MessagingEntityType entityType)
            : base(mediator, MessageHandlerRole.Listener, entityType)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageListener{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageListener{TMessage}" /> processes.
        /// </summary>
        public Type MessageType => typeof(TMessage);
    }

    /// <summary>
    /// Processes messages as a listener.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageListener{TRequestMessage, TResponseMessage}" /> is the default implementation of
    /// <see cref="IMessageListener{TRequestMessage, TResponseMessage}" />.
    /// </remarks>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is processed by the listener.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public abstract class MessageListener<TRequestMessage, TResponseMessage> : MessageHandler<TRequestMessage, TResponseMessage>, IMessageListener<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListener{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected MessageListener(ICommandMediator mediator)
            : base(mediator, MessageHandlerRole.Listener, Message.RequestEntityType)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageListener{TRequestMessage, TResponseMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageListener{TRequestMessage, TResponseMessage}" />
        /// processes.
        /// </summary>
        public Type MessageType => typeof(TRequestMessage);
    }
}