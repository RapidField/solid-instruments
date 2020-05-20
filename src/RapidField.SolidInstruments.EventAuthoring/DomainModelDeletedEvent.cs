// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about the deletion of an object that models a domain construct.
    /// </summary>
    /// <remarks>
    /// <see cref="DomainModelDeletedEvent{TModel}" /> is the default implementation of
    /// <see cref="IDomainModelDeletedEvent{TModel}" />.
    /// </remarks>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    [DataContract]
    public class DomainModelDeletedEvent<TModel> : DomainModelEvent<TModel>, IDomainModelDeletedEvent<TModel>
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEvent{TModel}" /> class.
        /// </summary>
        public DomainModelDeletedEvent()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">
        /// The resulting state of the associated domain model.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        public DomainModelDeletedEvent(TModel model)
            : this(model, Array.Empty<String>())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEvent{TModel}" /> class.
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
        public DomainModelDeletedEvent(TModel model, IEnumerable<String> labels)
            : this(model, labels, DefaultVerbosity)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelDeletedEvent{TModel}" /> class.
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
        public DomainModelDeletedEvent(TModel model, IEnumerable<String> labels, EventVerbosity verbosity)
            : base(model, DomainModelEventClassification.Deleted, labels, verbosity)
        {
            return;
        }
    }
}