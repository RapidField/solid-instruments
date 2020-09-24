// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.DataAccess;
using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Example.Domain.Commands.ModelState.User.FindDomainModelByIdentifierCommand;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.CommandHandlers.ModelState.User
{
    /// <summary>
    /// Processes a single <see cref="DomainModelCommand" />.
    /// </summary>
    public sealed class FindDomainModelByIdentifierCommandHandler : FindDomainModelByIdentifierCommandHandler<Guid, DomainModel, DataAccessModel, DomainModelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindDomainModelByIdentifierCommandHandler" /> class.
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
        /// Releases all resources consumed by the current <see cref="FindDomainModelByIdentifierCommandHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}