// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event related to an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelEvent{TModel}" /> is the default implementation of <see cref="IDomainModelEvent{TModel}" />.
    /// </remarks>
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
            : this(model, classification, labels, verbosity, null)
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
    }
}