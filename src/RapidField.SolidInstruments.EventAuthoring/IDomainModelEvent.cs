// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an event related to an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    public interface IDomainModelEvent<TModel> : IDomainModelEvent
        where TModel : class, IDomainModel
    {
        /// <summary>
        /// Gets the resulting state of the associated domain model.
        /// </summary>
        public TModel Model
        {
            get;
        }
    }

    /// <summary>
    /// Represents information about an event related to an object that models a domain construct.
    /// </summary>
    public interface IDomainModelEvent : IDomainEvent
    {
        /// <summary>
        /// Gets a classification that describes the effect of a the current <see cref="IDomainModelEvent" /> upon the associated
        /// model.
        /// </summary>
        public DomainModelEventClassification Classification
        {
            get;
        }

        /// <summary>
        /// Gets the type of the associated domain model.
        /// </summary>
        public Type ModelType
        {
            get;
        }
    }
}