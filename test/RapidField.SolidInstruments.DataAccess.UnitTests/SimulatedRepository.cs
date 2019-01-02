// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DataAccessRepository{TEntity}" /> derivative that is used for testing.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type of the repository.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The value type of the entity.
    /// </typeparam>
    internal abstract class SimulatedRepository<TEntity, TValue> : DataAccessRepository<TEntity>
        where TEntity : SimulatedEntity<TValue>
        where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRepository" /> class.
        /// </summary>
        /// <param name="dataStore">
        /// The data store for the repository.
        /// </param>
        public SimulatedRepository(SimulatedDataStore<TEntity, TValue> dataStore)
            : base()
        {
            DataStore = dataStore;
        }

        /// <summary>
        /// Adds the specified entity to the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void Add(TEntity entity, ConcurrencyControlToken controlToken) => DataStore.Add(entity.Identifier, entity.Value);

        /// <summary>
        /// Adds the specified entities to the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void AddRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken)
        {
            foreach (var entity in entities)
            {
                DataStore.Add(entity.Identifier, entity.Value);
            }
        }

        /// <summary>
        /// Returns all entities from the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// All entities within the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </returns>
        protected override IQueryable<TEntity> All(ConcurrencyControlToken controlToken) => DataStore.GetAllEntities();

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />, otherwise <see langword="false" />.
        /// </returns>
        protected override Boolean AnyWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken) => CountWhere(predicate, controlToken) > 0;

        /// <summary>
        /// Determines whether or not the specified entity exists in the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />, otherwise <see langword="false" />.
        /// </returns>
        protected override Boolean Contains(TEntity entity, ConcurrencyControlToken controlToken) => DataStore.ContainsKey(entity.Identifier);

        /// <summary>
        /// Returns the number of entities in the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The number of entities in the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </returns>
        protected override Int64 Count(ConcurrencyControlToken controlToken) => DataStore.Count;

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </returns>
        protected override Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken) => FindWhere(predicate, controlToken).Count();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current
        /// <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </returns>
        protected override IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken) => DataStore.GetEntitiesWhere(predicate);

        /// <summary>
        /// Removes the specified entity from the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void Remove(TEntity entity, ConcurrencyControlToken controlToken) => DataStore.Remove(entity.Identifier);

        /// <summary>
        /// Removes the specified entities from the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void RemoveRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken)
        {
            foreach (var entity in entities)
            {
                DataStore.Remove(entity.Identifier);
            }
        }

        /// <summary>
        /// Updates the specified entity in the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void Update(TEntity entity, ConcurrencyControlToken controlToken)
        {
            Remove(entity, controlToken);
            Add(entity, controlToken);
        }

        /// <summary>
        /// Updates the specified entities in the current <see cref="SimulatedRepository{TEntity, TValue}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected override void UpdateRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken)
        {
            RemoveRange(entities, controlToken);
            AddRange(entities, controlToken);
        }

        /// <summary>
        /// Represents the data store for the repository.
        /// </summary>
        public readonly SimulatedDataStore<TEntity, TValue> DataStore;
    }
}