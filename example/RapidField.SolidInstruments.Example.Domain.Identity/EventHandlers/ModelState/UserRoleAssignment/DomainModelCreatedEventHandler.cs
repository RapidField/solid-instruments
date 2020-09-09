// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.AspNetCore.Identity;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Example.Domain.Events.ModelState.UserRoleAssignment.DomainModelCreatedEvent;

namespace RapidField.SolidInstruments.Example.Domain.Identity.EventHandlers.ModelState.UserRoleAssignment
{
    /// <summary>
    /// Processes a single <see cref="DomainModelEvent" />.
    /// </summary>
    public sealed class DomainModelCreatedEventHandler : DomainModelCreatedEventHandler<DomainModel, DomainModelEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEventHandler" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="databaseContext">
        /// A connection to the Identity database.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="databaseContext" /> is
        /// <see langword="null" />.
        /// </exception>
        public DomainModelCreatedEventHandler(ICommandMediator mediator, DatabaseContext databaseContext)
            : base(mediator)
        {
            DatabaseContext = databaseContext.RejectIf().IsNull(nameof(databaseContext));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainModelCreatedEventHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        protected override void Process(DomainModel model, IEnumerable<String> labels, Guid correlationIdentifier, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            var identityUserRole = new IdentityUserRole<String>()
            {
                RoleId = model.UserRoleIdentifier.ToString(),
                UserId = model.UserIdentifier.ToString()
            };

            if (DatabaseContext.UserRoles.Find(identityUserRole.UserId, identityUserRole.RoleId) is null)
            {
                DatabaseContext.UserRoles.Add(identityUserRole);
                DatabaseContext.SaveChanges();
                Console.WriteLine($"{DomainModelEvent.DataContractNameVerb} {DomainModel.DataContractName} {model.Identifier}.");
            }
        }

        /// <summary>
        /// Represents a connection to the Identity database.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DatabaseContext DatabaseContext;
    }
}