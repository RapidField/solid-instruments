// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using ReportedEvent = RapidField.SolidInstruments.Example.Domain.Events.ModelState.User.DomainModelDeletedEvent;

namespace RapidField.SolidInstruments.Example.Domain.Commands.ModelState.User
{
    /// <summary>
    /// Represents a command to delete a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class DeleteDomainModelCommand : DeleteDomainModelReportableCommand<DomainModel, ReportedEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        public DeleteDomainModelCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DeleteDomainModelCommand(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        public DeleteDomainModelCommand(DomainModel model)
            : base(model)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DeleteDomainModelCommand(DomainModel model, Guid correlationIdentifier)
            : base(model, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public DeleteDomainModelCommand(DomainModel model, IEnumerable<String> labels)
            : base(model, labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommand" /> class.
        /// </summary>
        /// <param name="model">
        /// The desired state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DeleteDomainModelCommand(DomainModel model, IEnumerable<String> labels, Guid correlationIdentifier)
            : base(model, labels, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "DeleteUserCommand";
    }
}