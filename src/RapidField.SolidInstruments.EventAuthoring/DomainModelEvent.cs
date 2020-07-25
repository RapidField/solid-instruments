// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelEvent{TModel}" /> is the default implementation of <see cref="IDomainModelEvent{TModel}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    [DataContract]
    public class DomainModelEvent<TModel> : DomainEvent, IDomainModelEvent<TModel>
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        public DomainModelEvent()
            : base()
        {
            Model = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelEvent(Guid correlationIdentifier)
            : base(correlationIdentifier)
        {
            Model = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification)
            : this(model, classification, Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, Guid correlationIdentifier)
            : this(model, classification, Array.Empty<String>(), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels)
            : this(model, classification, labels, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels, Guid correlationIdentifier)
            : this(model, classification, labels, DefaultVerbosity, correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
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
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels, EventVerbosity verbosity)
            : this(model, classification, labels, verbosity, GetDescription(model, classification))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels, EventVerbosity verbosity, Guid correlationIdentifier)
            : this(model, classification, labels, verbosity, GetDescription(model, classification), correlationIdentifier)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels, EventVerbosity verbosity, String description)
            : base(labels, verbosity, description)
        {
            Classification = classification.RejectIf().IsEqualToValue(DomainModelEventClassification.Unspecified, nameof(classification));
            Model = model.RejectIf().IsNull(nameof(model));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <param name="labels">
        /// A collection of textual labels that provide categorical and/or contextual information about the event.
        /// </param>
        /// <param name="verbosity">
        /// The verbosity level of the event. The default value is <see cref="EventVerbosity.Normal" />
        /// </param>
        /// <param name="description">
        /// A textual description of the event. This argument can be <see langword="null" />.
        /// </param>
        /// <param name="correlationIdentifier">
        /// A unique identifier that is assigned to related events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="labels" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="classification" /> is equal to <see cref="DomainModelEventClassification.Unspecified" /> -or-
        /// <paramref name="verbosity" /> is equal to <see cref="EventVerbosity.Unspecified" /> -or-
        /// <paramref name="correlationIdentifier" /> is equal to <see cref="Guid.Empty" />.
        /// </exception>
        public DomainModelEvent(TModel model, DomainModelEventClassification classification, IEnumerable<String> labels, EventVerbosity verbosity, String description, Guid correlationIdentifier)
            : base(labels, verbosity, description, correlationIdentifier)
        {
            Classification = classification.RejectIf().IsEqualToValue(DomainModelEventClassification.Unspecified, nameof(classification));
            Model = model.RejectIf().IsNull(nameof(model));
        }

        /// <summary>
        /// Returns a description for the current <see cref="DomainModelEvent{TModel}" />.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <param name="classification">
        /// A classification that describes the effect of a the event upon <paramref name="model" />.
        /// </param>
        /// <returns>
        /// A description for the current <see cref="DomainModelEvent{TModel}" />.
        /// </returns>
        [DebuggerHidden]
        private static String GetDescription(IDomainModel model, DomainModelEventClassification classification)
        {
            if (model is null)
            {
                return null;
            }

            var modelTypeName = model.GetType().FullName;

            return classification switch
            {
                DomainModelEventClassification.Associated => $"An event occurred that was associated with a model of type {modelTypeName}.",
                DomainModelEventClassification.Created => $"A model of type {modelTypeName} was created.",
                DomainModelEventClassification.Deleted => $"A model of type {modelTypeName} was deleted.",
                DomainModelEventClassification.Updated => $"A model of type {modelTypeName} was updated.",
                _ => null,
            };
        }

        /// <summary>
        /// Gets or sets a classification that describes the effect of a the current <see cref="DomainModelEvent{TModel}" /> upon
        /// <see cref="Model" />.
        /// </summary>
        [DataMember]
        public DomainModelEventClassification Classification
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resulting state of the associated domain model.
        /// </summary>
        [DataMember]
        public TModel Model
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the associated domain model.
        /// </summary>
        [IgnoreDataMember]
        public Type ModelType => typeof(TModel);
    }
}