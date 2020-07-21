// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.CustomerOrder.DomainModelCreatedEvent;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.CustomerOrder
{
    using DomainModelEventMessage = EventMessages.DomainModelCreatedEventMessage<DomainModel, DomainModelEvent>;

    /// <summary>
    /// Represents a message that provides notification about the creation of a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "CustomerOrderCreatedEventMessage")]
    internal sealed class DomainModelCreatedEventMessage : DomainModelEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEventMessage" /> class.
        /// </summary>
        public DomainModelCreatedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public DomainModelCreatedEventMessage(DomainModelEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}