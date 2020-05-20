// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.CustomerOrder.DomainModel;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.CustomerOrder
{
    /// <summary>
    /// Represents information about the creation of a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = "CustomerOrderCreatedEvent")]
    internal sealed class DomainModelCreatedEvent : DomainModelCreatedEvent<DomainModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEvent" /> class.
        /// </summary>
        public DomainModelCreatedEvent()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEvent" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        public DomainModelCreatedEvent(DomainModel model)
            : base(model)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEvent" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public DomainModelCreatedEvent(DomainModel model, IEnumerable<String> labels)
            : base(model, labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelCreatedEvent" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public DomainModelCreatedEvent(DomainModel model, IEnumerable<String> labels, EventVerbosity verbosity)
            : base(model, labels, verbosity)
        {
            return;
        }
    }
}