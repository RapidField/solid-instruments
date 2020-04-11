// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs data access operations for a specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public interface IDataAccessRepository<TEntity> : IDataAccessRepository
        where TEntity : class
    {
        /// <summary>
        /// Adds the specified entity to the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity in the current <see cref="IDataAccessRepository{TEntity}" />, or adds it if it doesn't
        /// exist.
        /// </summary>
        /// <param name="entity">
        /// The entity to add or update.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(TEntity entity);

        /// <summary>
        /// Updates the specified entities in the current <see cref="IDataAccessRepository{TEntity}" />, or adds them if they don't
        /// exist.
        /// </summary>
        /// <param name="entities">
        /// The entities to add or update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="entities" /> contains one or more null references.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entities" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds the specified entities to the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to add.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="entities" /> contains one or more null references.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entities" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Returns all entities from the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// All entities within the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> All();

        /// <summary>
        /// Determines whether or not the specified entity exists in the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current <see cref="IDataAccessRepository{TEntity}" />,
        /// otherwise <see langword="false" />.
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
        /// <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="IDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
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
        /// <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Removes the specified entity from the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Remove(TEntity entity);

        /// <summary>
        /// Removes the specified entities from the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to remove.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="entities" /> contains one or more null references.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entities" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the entities matching the specified predicate from the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void RemoveWhere(Expression<Func<TEntity, Boolean>> predicate);

        /// <summary>
        /// Updates the specified entity in the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Update(TEntity entity);

        /// <summary>
        /// Updates the specified entities in the current <see cref="IDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to update.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="entities" /> contains one or more null references.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entities" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void UpdateRange(IEnumerable<TEntity> entities);
    }

    /// <summary>
    /// Performs data access operations for a specified entity type.
    /// </summary>
    public interface IDataAccessRepository : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Determines whether or not any entities exist in the current <see cref="IDataAccessRepository" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if any entities exist in the current <see cref="IDataAccessRepository" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean Any();

        /// <summary>
        /// Returns the number of entities in the current <see cref="IDataAccessRepository" />.
        /// </summary>
        /// <returns>
        /// The number of entities in the current <see cref="IDataAccessRepository" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int64 Count();

        /// <summary>
        /// Gets the entity type of the current <see cref="IDataAccessRepository" />.
        /// </summary>
        public Type EntityType
        {
            get;
        }
    }
}