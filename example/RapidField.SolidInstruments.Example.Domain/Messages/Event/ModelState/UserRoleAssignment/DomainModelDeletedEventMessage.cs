﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;
using DomainModelEvent = RapidField.SolidInstruments.Example.Domain.Events.ModelState.UserRoleAssignment.DomainModelDeletedEvent;

namespace RapidField.SolidInstruments.Example.Domain.Messages.Event.ModelState.UserRoleAssignment
{
    using DomainModelEventMessage = Messaging.EventMessages.DomainModelDeletedEventMessage<DomainModel, DomainModelEvent>;

    /// <summary>
    /// Represents a message that provides notification about the deletion of a <see cref="DomainModel" />.
    /// </summary>
    [DataContract(Name = DataContractName)]
    public sealed class DomainModelDeletedEventMessage : DomainModelEventMessage
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
        /// Represents the name that is used when representing this type in serialization and transport contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String DataContractName = DomainModelEvent.DataContractName + DataContractNameSuffix;
    }
}