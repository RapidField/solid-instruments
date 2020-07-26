// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes messages.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IMessageHandler{TMessage}" /> as a registration target for inversion of control tools. Use
    /// <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is processed by the handler.
    /// </typeparam>
    public interface IMessageHandler<in TMessage> : IMessageHandler<TMessage, Nix>, ICommandHandler<TMessage>
        where TMessage : class, IMessage
    {
    }

    /// <summary>
    /// Processes messages.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IMessageHandler{TMessage, TResult}" /> as a registration target for inversion of control tools. Use
    /// <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TMessage">
    /// The type of the message that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a message.
    /// </typeparam>
    public interface IMessageHandler<in TMessage, TResult> : ICommandHandler<TMessage, TResult>
        where TMessage : class, IMessage<TResult>
    {
        /// <summary>
        /// Gets the targeted entity type for the current <see cref="IMessageHandler{TMessage, TResult}" />.
        /// </summary>
        public MessagingEntityType EntityType
        {
            get;
        }

        /// <summary>
        /// Gets the role of the current <see cref="IMessageHandler{TMessage, TResult}" />.
        /// </summary>
        public MessageHandlerRole Role
        {
            get;
        }
    }
}