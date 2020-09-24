// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Collections.Generic;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Example.Domain.Events.ModelState.UserRole.DomainModelDeletedEvent;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.EventHandlers.ModelState.UserRole
{
    /// <summary>
    /// Processes a single <see cref="DomainModelEvent" />.
    /// </summary>
    public sealed class DomainModelDeletedEventHandler : DomainModelDeletedEventHandler<DomainModel, DomainModelEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEventHandler" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public DomainModelDeletedEventHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainModelDeletedEventHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified domain model.
        /// </summary>
        /// <param name="model">
        /// The model that was created.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
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
        protected override void Process(DomainModel model, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken) => Console.WriteLine($"{DomainModelEvent.DataContractNameVerb} {DomainModel.DataContractName} {model.Identifier}.");
    }
}