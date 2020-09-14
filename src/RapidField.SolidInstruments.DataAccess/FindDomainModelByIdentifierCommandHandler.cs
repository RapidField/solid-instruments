// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Processes a single <see cref="IFindDomainModelByIdentifierCommand{TIdentifier, TModel}" />.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public class FindDomainModelByIdentifierCommandHandler<TIdentifier, TDomainModel, TDataAccessModel, TCommand> : FindDomainModelByIdentifierCommandHandler<TIdentifier, TDomainModel, TCommand>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>, new()
        where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
        where TCommand : class, IFindDomainModelByIdentifierCommand<TIdentifier, TDomainModel>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FindDomainModelByIdentifierCommandHandler{TIdentifier, TDomainModel, TDataAccessModel, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public FindDomainModelByIdentifierCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="FindDomainModelByIdentifierCommandHandler{TIdentifier, TDomainModel, TDataAccessModel, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        protected override TDomainModel FindDomainModel(TIdentifier modelIdentifier, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            var dataAccessModelCommand = new FindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel>(modelIdentifier);
            var dataAccessModel = mediator.Process<TDataAccessModel>(dataAccessModelCommand);
            return dataAccessModel?.ToDomainModel();
        }
    }
}