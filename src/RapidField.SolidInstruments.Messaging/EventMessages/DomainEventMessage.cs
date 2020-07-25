// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about domain activity.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainEventMessage" /> is the default implementation of <see cref="IDomainEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class DomainEventMessage : DomainEventMessage<DomainEvent>, IDomainEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventMessage" /> class.
        /// </summary>
        public DomainEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public DomainEventMessage(DomainEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about domain activity.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainEventMessage{TEvent}" /> is the default implementation of <see cref="IDomainEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class DomainEventMessage<TEvent> : EventMessage<TEvent>, IDomainEventMessage<TEvent>
        where TEvent : DomainEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventMessage{TEvent}" /> class.
        /// </summary>
        protected DomainEventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected DomainEventMessage(TEvent eventObject)
            : base(eventObject)
        {
            return;
        }
    }
}