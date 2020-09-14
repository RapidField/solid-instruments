// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents a command to find and return an object that models a domain construct by its primary identifier.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    public interface IFindDomainModelByIdentifierCommand<TIdentifier, TModel> : IFindDomainModelCommand<TModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TModel : class, IDomainModel<TIdentifier>
    {
        /// <summary>
        /// Gets or sets a value that uniquely identifies the associated <see cref="IDomainModel{TIdentifier}" />.
        /// </summary>
        public TIdentifier ModelIdentifier
        {
            get;
            set;
        }
    }
}