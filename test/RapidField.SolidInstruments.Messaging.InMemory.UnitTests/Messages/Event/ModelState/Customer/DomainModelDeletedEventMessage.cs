// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.Customer.DomainModelDeletedEvent;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.Customer
{
    using DomainModelEventMessage = EventMessages.DomainModelDeletedEventMessage<DomainModel, DomainModelEvent>;

    /// <summary>
    /// Represents a message that provides notification about the deletion of a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "CustomerDeletedEventMessage")]
    internal sealed class DomainModelDeletedEventMessage : DomainModelEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEventMessage" /> class.
        /// </summary>
        public DomainModelDeletedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public DomainModelDeletedEventMessage(DomainModelEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}