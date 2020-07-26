// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs data access operations for a specified data access model type.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    public interface IDataAccessModelRepository<TIdentifier, TDataAccessModel> : IDataAccessRepository<TDataAccessModel>, IReadOnlyDataAccessModelRepository<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
        /// <summary>
        /// Removes the data model matching the specified identifier from the current
        /// <see cref="IDomainModelRepository{TIdentifier, TDataAccessModel, TDomainModel}" />.
        /// </summary>
        /// <param name="identifier">
        /// The unique primary identifier for the data model.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveByIdentifier(TIdentifier identifier) => RemoveWhere(entity => entity.Identifier.Equals(identifier));
    }

    /// <summary>
    /// Performs data access operations for a specified data access model type.
    /// </summary>
    public interface IDataAccessModelRepository : IDataAccessRepository
    {
    }
}