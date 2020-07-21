// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.DeleteDomainModelCommand;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Customer
{
    using DomainModelCommandMessage = CommandMessages.DeleteDomainModelCommandMessage<DomainModel, DomainModelCommand>;

    /// <summary>
    /// Represents a message that contains a command to delete a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "DeleteCustomerCommandMessage")]
    internal sealed class DeleteDomainModelCommandMessage : DomainModelCommandMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommandMessage" /> class.
        /// </summary>
        public DeleteDomainModelCommandMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDomainModelCommandMessage" /> class.
        /// </summary>
        /// <param name="commandObject">
        /// The associated command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandObject" /> is <see langword="null" />.
        /// </exception>
        public DeleteDomainModelCommandMessage(DomainModelCommand commandObject)
            : base(commandObject)
        {
            return;
        }
    }
}