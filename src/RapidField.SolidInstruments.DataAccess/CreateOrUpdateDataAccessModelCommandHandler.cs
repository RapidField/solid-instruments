// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Processes a single <see cref="CreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" />.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model type.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model that is associated with the command.
    /// </typeparam>
    /// <typeparam name="TRepository">
    /// The type of the data access model repository that is used to process the command.
    /// </typeparam>
    public class CreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository> : DataAccessModelCommandHandler<TIdentifier, TDataAccessModel, CreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel>, TRepository>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
        where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel>
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CreateOrUpdateDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TRepository}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="transaction" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" /> -or- <paramref name="transaction" /> is <see langword="null" />.
        /// </exception>
        public CreateOrUpdateDataAccessModelCommandHandler(ICommandMediator mediator, IDataAccessRepositoryFactory repositoryFactory, IDataAccessTransaction transaction)
            : base(mediator, repositoryFactory, transaction)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="CreateOrUpdateDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TRepository}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
        /// <param name="repository">
        /// The data access repository that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Process(CreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel> command, ICommandMediator mediator, TRepository repository, IConcurrencyControlToken controlToken) => repository.AddOrUpdate(command.Model);
    }
}