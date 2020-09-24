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
    /// Processes a single <see cref="IFindDomainModelByIdentifierCommand{TIdentifier, TModel}" />.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public abstract class FindDomainModelByIdentifierCommandHandler<TIdentifier, TModel, TCommand> : FindDomainModelCommandHandler<TModel, TCommand>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TModel : class, IDomainModel<TIdentifier>
        where TCommand : class, IFindDomainModelByIdentifierCommand<TIdentifier, TModel>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FindDomainModelByIdentifierCommandHandler{TIdentifier, TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected FindDomainModelByIdentifierCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="FindDomainModelByIdentifierCommandHandler{TIdentifier, TModel, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Searches for and returns the specified domain model.
        /// </summary>
        /// <param name="modelIdentifier">
        /// The unique primary identifier for the model.
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
        /// <returns>
        /// The specified model, or <see langword="null" /> if no matching model is found.
        /// </returns>
        protected abstract TModel FindDomainModel(TIdentifier modelIdentifier, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <note type="note">
        /// Do not process <paramref name="command" /> using <paramref name="mediator" />, as doing so will generally result in
        /// infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected sealed override TModel Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => FindDomainModel(command.ModelIdentifier, command.Labels, command.CorrelationIdentifier, mediator, controlToken);
    }
}