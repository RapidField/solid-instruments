// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents a command to update an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> is a derivative of
    /// <see cref="UpdateDomainModelCommand{TModel}" /> which is extended to support <see cref="IReportableCommand{TEvent}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the event that describes the command.
    /// </typeparam>
    [DataContract]
    public class UpdateDomainModelReportableCommand<TModel, TEvent> : UpdateDomainModelCommand<TModel>, IDomainModelReportableCommand<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : DomainModelEvent<TModel>, IDomainModelUpdatedEvent<TModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        public UpdateDomainModelReportableCommand()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public UpdateDomainModelReportableCommand(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        public UpdateDomainModelReportableCommand(TModel model)
            : base(model)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public UpdateDomainModelReportableCommand(TModel model, Guid correlationIdentifier)
            : base(model, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        public UpdateDomainModelReportableCommand(TModel model, IEnumerable<String> labels)
            : base(model, labels)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the command.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public UpdateDomainModelReportableCommand(TModel model, IEnumerable<String> labels, Guid correlationIdentifier)
            : base(model, labels, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Composes an <see cref="IEvent" /> representing information about the current
        /// <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" />.
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
                associatedEvent.Model = Model;
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
        /// <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" />.
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
        /// <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" />.
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
        /// Gets the type of the event that describes the current <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreDataMember]
        public Type EventType => EventTypeReference;

        /// <summary>
        /// Represents the type of the event that describes the current
        /// <see cref="UpdateDomainModelReportableCommand{TModel, TEvent}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EventTypeReference = typeof(TEvent);
    }
}