// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents a command to perform a domain action which can be described by an <see cref="IEvent" />.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainReportableCommand{TEvent}" /> is the default implementation of
    /// <see cref="IDomainReportableCommand{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    [DataContract]
    public class DomainReportableCommand<TEvent> : DomainCommand, IDomainReportableCommand<TEvent>
        where TEvent : class, IDomainEvent, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainReportableCommand{TEvent}" /> class.
        /// </summary>
        public DomainReportableCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainReportableCommand{TEvent}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainReportableCommand(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainReportableCommand{TEvent}" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public DomainReportableCommand(IEnumerable<String> labels)
            : base(labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainReportableCommand{TEvent}" /> class.
        /// </summary>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainReportableCommand(IEnumerable<String> labels, Guid correlationIdentifier)
            : base(labels, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Composes an <see cref="IEvent" /> representing information about the current
        /// <see cref="DomainReportableCommand{TEvent}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEvent" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        public TEvent ComposeEvent()
        {
            try
            {
                var associatedEvent = new TEvent();

                foreach (var label in Labels)
                {
                    associatedEvent.Labels.Add(label);
                }

                associatedEvent.CorrelationIdentifier = CorrelationIdentifier;
                EnrichEvent(associatedEvent);
                return associatedEvent;
            }
            catch (Exception exception)
            {
                throw new EventAuthoringException(typeof(TEvent), exception);
            }
        }

        /// <summary>
        /// Composes an <see cref="IEvent" /> representing information about the current
        /// <see cref="DomainReportableCommand{TEvent}" />.
        /// </summary>
        /// <returns>
        /// An <see cref="IEvent" /> representing information about the current object.
        /// </returns>
        /// <exception cref="EventAuthoringException">
        /// An exception was raised while composing the event.
        /// </exception>
        IEvent IReportable.ComposeEvent() => ComposeEvent();

        /// <summary>
        /// Hydrates and enriches the specified event with details that are pertinent to the current
        /// <see cref="DomainReportableCommand{TEvent}" />.
        /// </summary>
        /// <remarks>
        /// When overridden by a derived class, this method is invoked by <see cref="ComposeEvent" /> to enrich event objects with
        /// additional details.
        /// </remarks>
        /// <param name="associatedEvent">
        /// The event to enrich.
        /// </param>
        protected virtual void EnrichEvent(TEvent associatedEvent)
        {
            return;
        }

        /// <summary>
        /// Gets the type of the event that describes the current <see cref="DomainReportableCommand{TEvent}" />.
        /// </summary>
        [IgnoreDataMember]
        public Type EventType => EventTypeReference;

        /// <summary>
        /// Represents the type of the event that describes the current <see cref="DomainReportableCommand{TEvent}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventTypeReference = typeof(TEvent);
    }
}