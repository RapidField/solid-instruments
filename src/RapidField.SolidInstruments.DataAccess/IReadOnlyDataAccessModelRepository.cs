// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs read-only data access operations for a specified data access model type.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    public interface IReadOnlyDataAccessModelRepository<TIdentifier, TDataAccessModel> : IDataAccessModelRepository, IReadOnlyDataAccessRepository<TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Returns the data model matching the specified identifier from the current
        /// <see cref="IReadOnlyDataAccessModelRepository{TIdentifier, TDataAccessModel}" />, or <see langword="null" /> if no model
        /// matching <paramref name="identifier" /> is found.
        /// </summary>
        /// <param name="identifier">
        /// The unique primary identifier for the data model.
        /// </param>
        /// <returns>
        /// The data model matching the specified identifier within the current
        /// <see cref="IReadOnlyDataAccessModelRepository{TIdentifier, TDataAccessModel}" />, or <see langword="null" /> if no model
        /// matching <paramref name="identifier" /> is found.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The result set contains more than one entity.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TDataAccessModel FindByIdentifier(TIdentifier identifier) => FindWhere(entity => entity.Identifier.Equals(identifier)).SingleOrDefault();
    }
}