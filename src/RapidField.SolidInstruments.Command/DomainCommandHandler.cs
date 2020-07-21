// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes a single <see cref="IDomainCommand" />.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public abstract class DomainCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : class, IDomainCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandHandler{TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainCommandHandler{TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Processes a single <see cref="IDomainCommand{TResult}" />.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a command.
    /// </typeparam>
    public abstract class DomainCommandHandler<TCommand, TResult> : CommandHandler<TCommand, TResult>
        where TCommand : class, IDomainCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCommandHandler{TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainCommandHandler{TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}