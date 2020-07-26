// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs read-only data access operations for a specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public interface IReadOnlyDataAccessRepository<TEntity> : IReadOnlyDataAccessRepository
        where TEntity : class
    {
        /// <summary>
        /// Returns all entities from the current <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// All entities within the current <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> All();

        /// <summary>
        /// Determines whether or not the specified entity exists in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean Contains(TEntity entity);

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean ContainsWhere(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current
        /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate);
    }

    /// <summary>
    /// Performs read-only data access operations for a specified entity type.
    /// </summary>
    public interface IReadOnlyDataAccessRepository : IDataAccessRepository
    {
    }
}