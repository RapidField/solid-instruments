// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product.DomainModel;
using DomainModelCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Product.UpdateDomainModelCommand;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Product
{
    using DomainModelCommandMessage = CommandMessages.UpdateDomainModelCommandMessage<DomainModel, DomainModelCommand>;

    /// <summary>
    /// Represents a message that contains a command to update a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "UpdateProductCommandMessage")]
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
    }
}