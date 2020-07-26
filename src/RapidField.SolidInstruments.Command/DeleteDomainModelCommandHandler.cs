// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes a single <see cref="IDeleteDomainModelCommand{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public abstract class DeleteDomainModelCommandHandler<TModel, TCommand> : DomainModelCommandHandler<TModel, TCommand>
        where TModel : class, IDomainModel
        where TCommand : class, IDeleteDomainModelCommand<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommandHandler{TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DeleteDomainModelCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Deletes the specified domain model.
        /// </summary>
        /// <param name="model">
        /// The model to delete.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier to assign to sub-commands.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected abstract void DeleteDomainModel(TModel model, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DeleteDomainModelCommandHandler{TModel, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => DeleteDomainModel(command.Model, command.Labels, command.CorrelationIdentifier, mediator, controlToken);
    }
}