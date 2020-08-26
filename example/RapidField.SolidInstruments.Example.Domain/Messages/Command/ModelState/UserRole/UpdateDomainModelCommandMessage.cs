// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Example.Domain.Commands.ModelState.UserRole.UpdateDomainModelCommand;
using ReportedEvent = RapidField.SolidInstruments.Example.Domain.Events.ModelState.UserRole.DomainModelUpdatedEvent;
using ReportedEventMessage = RapidField.SolidInstruments.Example.Domain.Messages.Event.ModelState.UserRole.DomainModelUpdatedEventMessage;

namespace RapidField.SolidInstruments.Example.Domain.Messages.Command.ModelState.UserRole
{
    using DomainModelCommandMessage = Messaging.CommandMessages.UpdateDomainModelCommandMessage<DomainModel, DomainModelCommand, ReportedEvent, ReportedEventMessage>;

    /// <summary>
    /// Represents a message that contains a command to update a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class UpdateDomainModelCommandMessage : DomainModelCommandMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelCommandMessage" /> class.
        /// </summary>
        public UpdateDomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelCommandMessage" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        public UpdateDomainModelCommandMessage(DomainModelCommand commandObject)
            : base(commandObject)
        {
            return;
        }

        /// <summary>
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DataContractName = DomainModelCommand.DataContractName + DataContractNameSuffix;
    }
}