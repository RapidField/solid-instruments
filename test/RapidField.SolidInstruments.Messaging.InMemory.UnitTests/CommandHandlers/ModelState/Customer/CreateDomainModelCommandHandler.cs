// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.CreateDomainModelCommand;
using DomainModelEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.Customer.DomainModelCreatedEvent;
using DomainModelEventMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.CommandHandlers.ModelState.Customer
{
    /// <summary>
    /// Processes a single <see cref="DomainModelCommand" />.
    /// </summary>
    internal class CreateDomainModelCommandHandler : CreateDomainModelCommandHandler<DomainModel, DomainModelCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDomainModelCommandHandler" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public CreateDomainModelCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Creates the specified domain model.
        /// </summary>
        /// <param name="model">
        /// The model to create.
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
        protected override void CreateDomainModel(DomainModel model, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken) => mediator.Process(new DomainModelEventMessage(new DomainModelEvent(model, labels, correlationIdentifier)));

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CreateDomainModelCommandHandler{TModel, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}