// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.CustomerOrder.CreateDomainModelCommand;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.CustomerOrder
{
    using DomainModelCommandMessage = CommandMessages.CreateDomainModelCommandMessage<DomainModel, DomainModelCommand>;

    /// <summary>
    /// Represents a message that contains a command to create a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "CreateCustomerOrderCommandMessage")]
    internal sealed class CreateDomainModelCommandMessage : DomainModelCommandMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDomainModelCommandMessage" /> class.
        /// </summary>
        public CreateDomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDomainModelCommandMessage" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        public CreateDomainModelCommandMessage(DomainModelCommand commandObject)
            : base(commandObject)
        {
            return;
        }
    }
}