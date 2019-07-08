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
    /// <remarks>
    /// <see cref="MessageSubscriber{TMessage}" /> is the default implementation of <see cref="IMessageSubscriber{TMessage}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is subscribed to.
    /// </typeparam>
    public abstract class MessageSubscriber<TMessage> : MessageHandler<TMessage>, IMessageSubscriber<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriber{TMessage}" /> class.
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
        protected MessageSubscriber(ICommandMediator mediator, MessagingEntityType entityType)
            : base(mediator, MessageHandlerRole.Subscriber, entityType)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageSubscriber{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageSubscriber{TMessage}" /> processes.
        /// </summary>
        public Type MessageType => typeof(TMessage);
    }

    /// <summary>
    /// Processes messages as a subscriber.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageSubscriber{TRequestMessage, TResponseMessage}" /> is the default implementation of
    /// <see cref="IMessageSubscriber{TRequestMessage, TResponseMessage}" />.
    /// </remarks>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is processed by the subscriber.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is published in response to the request.
    /// </typeparam>
    public abstract class MessageSubscriber<TRequestMessage, TResponseMessage> : MessageHandler<TRequestMessage, TResponseMessage>, IMessageSubscriber<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSubscriber{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected MessageSubscriber(ICommandMediator mediator)
            : base(mediator, MessageHandlerRole.Subscriber, Message.RequestEntityType)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageSubscriber{TRequestMessage, TResponseMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the type of the message that the current <see cref="MessageSubscriber{TRequestMessage, TResponseMessage}" />
        /// processes.
        /// </summary>
        public Type MessageType => typeof(TRequestMessage);
    }
}