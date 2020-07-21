// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.EventAuthoring;
using System;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.Messaging.EventMessages
{
    /// <summary>
    /// Represents a message that provides notification about an event.
    /// </summary>
    /// <remarks>
    /// <see cref="EventMessage" /> is the default implementation of <see cref="IEventMessage" />.
    /// </remarks>
    [DataContract]
    public sealed class EventMessage : EventMessage<Event>, IEventMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage" /> class.
        /// </summary>
        public EventMessage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        public EventMessage(Event eventObject)
            : base(eventObject)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a message that provides notification about an event.
    /// </summary>
    /// <remarks>
    /// <see cref="EventMessage{TEvent}" /> is the default implementation of <see cref="IEventMessage{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the associated event.
    /// </typeparam>
    [DataContract]
    public abstract class EventMessage<TEvent> : Message, IEventMessage<TEvent>
        where TEvent : Event, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage{TEvent}" /> class.
        /// </summary>
        protected EventMessage()
            : this(new TEvent())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage{TEvent}" /> class.
        /// </summary>
        /// <param name="eventObject">
        /// The associated event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="eventObject" /> is <see langword="null" />.
        /// </exception>
        protected EventMessage(TEvent eventObject)
            : base(eventObject.CorrelationIdentifier, Guid.NewGuid())
        {
            Event = eventObject.RejectIf().IsNull(nameof(eventObject));
        }

        /// <summary>
        /// Gets or sets the associated event.
        /// </summary>
        [DataMember]
        public TEvent Event
        {
            get;
            set;
        }
    }
}