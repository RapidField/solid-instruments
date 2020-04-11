// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event related to an object that models a domain construct.
    /// </summary>
    public interface IDomainModelEvent<TModel> : IDomainEvent
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Gets a classification that describes the effect of a the current <see cref="IDomainModelEvent{TModel}" /> upon
        /// <see cref="Model" />.
        /// </summary>
        public DomainModelEventClassification Classification
        {
            get;
        }

        /// <summary>
        /// Gets the resulting state of the associated domain model.
        /// </summary>
        public TModel Model
        {
            get;
        }
    }
}