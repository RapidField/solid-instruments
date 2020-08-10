// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.UpdateDomainModelCommand;
using ReportedEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.Customer.DomainModelUpdatedEvent;
using ReportedEventMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Customer
{
    using DomainModelCommandMessage = CommandMessages.UpdateDomainModelCommandMessage<DomainModel, DomainModelCommand, ReportedEvent, ReportedEventMessage>;

    /// <summary>
    /// Represents a message that contains a command to update a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = DataContractName)]
    internal sealed class UpdateDomainModelCommandMessage : DomainModelCommandMessage
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
        /// Represents the name that is used when representing this current type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "UpdateCustomerCommandMessage";
    }
}