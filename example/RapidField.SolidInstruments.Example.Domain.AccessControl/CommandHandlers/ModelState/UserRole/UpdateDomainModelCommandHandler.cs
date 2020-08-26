﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.DataAccess;
using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Example.Domain.Commands.ModelState.UserRole.UpdateDomainModelCommand;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.CommandHandlers.ModelState.UserRole
{
    /// <summary>
    /// Processes a single <see cref="DomainModelCommand" />.
    /// </summary>
    public sealed class UpdateDomainModelCommandHandler : UpdateDomainModelCommandHandler<Guid, DomainModel, DataAccessModel, DomainModelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelCommandHandler" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public UpdateDomainModelCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="UpdateDomainModelCommandHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}