// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.Product.DomainModelUpdatedEvent;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.Product
{
    using DomainModelEventMessage = EventMessages.DomainModelUpdatedEventMessage<DomainModel, DomainModelEvent>;

    /// <summary>
    /// Represents a message that provides notification about an update to a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "ProductUpdatedEventMessage")]
    internal sealed class DomainModelUpdatedEventMessage : DomainModelEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelUpdatedEventMessage" /> class.
        /// </summary>
        public DomainModelUpdatedEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelUpdatedEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public DomainModelUpdatedEventMessage(DomainModelEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}