﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Product.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Events.ModelState.Product.DomainModelDeletedEvent;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Event.ModelState.Product
{
    using DomainModelEventMessage = EventMessages.DomainModelDeletedEventMessage<DomainModel, DomainModelEvent>;

    /// <summary>
    /// Represents a message that provides notification about the deletion of a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = DataContractName)]
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

        /// <summary>
        /// Represents the name that is used when representing this current type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DataContractName = "ProductDeletedEventMessage";
    }
}