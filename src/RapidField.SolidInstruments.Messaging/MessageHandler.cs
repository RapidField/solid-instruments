// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageHandler{TMessage}" /> is the default implementation of <see cref="IMessageHandler{TMessage}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is processed by the handler.
    /// </typeparam>
    public abstract class MessageHandler<TMessage> : CommandHandler<TMessage>, IMessageHandler<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="role">
        /// The role of the handler.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="role" /> is equal to <see cref="MessageHandlerRole.Unspecified" /> -or- <paramref name="entityType" />
        /// is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessageHandler(ICommandMediator mediator, MessageHandlerRole role, MessagingEntityType entityType)
            : base(mediator)
        {
            EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
            Role = role.RejectIf().IsEqualToValue(MessageHandlerRole.Unspecified, nameof(role));
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageHandler{TMessage}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageHandler{TMessage}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(EntityType)}\": {EntityType}, \"{nameof(Role)}\": \"{Role}\" }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageHandler{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the targeted entity type for the current <see cref="MessageHandler{TMessage}" />.
        /// </summary>
        public MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the role of the current <see cref="MessageHandler{TMessage}" />.
        /// </summary>
        public MessageHandlerRole Role
        {
            get;
        }
    }

    /// <summary>
    /// Processes messages.
    /// </summary>
    /// <remarks>
    /// <see cref="MessageHandler{TMessage, TResult}" /> is the default implementation of
    /// <see cref="IMessageHandler{TMessage, TResult}" />.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a message.
    /// </typeparam>
    public abstract class MessageHandler<TMessage, TResult> : CommandHandler<TMessage, TResult>, IMessageHandler<TMessage, TResult>
        where TMessage : class, IMessage<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler{TMessage, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="role">
        /// The role of the handler.
        /// </param>
        /// <param name="entityType">
        /// The targeted entity type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="role" /> is equal to <see cref="MessageHandlerRole.Unspecified" /> -or- <paramref name="entityType" />
        /// is equal to <see cref="MessagingEntityType.Unspecified" />.
        /// </exception>
        protected MessageHandler(ICommandMediator mediator, MessageHandlerRole role, MessagingEntityType entityType)
            : base(mediator)
        {
            EntityType = entityType.RejectIf().IsEqualToValue(MessagingEntityType.Unspecified, nameof(entityType));
            Role = role.RejectIf().IsEqualToValue(MessageHandlerRole.Unspecified, nameof(role));
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageHandler{TMessage, TResult}" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="MessageHandler{TMessage, TResult}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(EntityType)}\": {EntityType}, \"{nameof(Role)}\": \"{Role}\" }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="MessageHandler{TMessage, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the targeted entity type for the current <see cref="MessageHandler{TMessage, TResult}" />.
        /// </summary>
        public MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the role of the current <see cref="MessageHandler{TMessage, TResult}" />.
        /// </summary>
        public MessageHandlerRole Role
        {
            get;
        }
    }
}