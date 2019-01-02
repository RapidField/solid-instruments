// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    /// <summary>
    /// Represents an entity data store that is used for testing.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the value for the data store's entities.
    /// </typeparam>
    internal abstract class SimulatedDataStore<TEntity, TValue> : Dictionary<Guid, TValue>
        where TEntity : SimulatedEntity<TValue>
        where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDataStore{TValue}" /> class.
        /// </summary>
        /// <param name="entities">
        /// A collection of values comprising the data store's entities.
        /// </param>
        protected SimulatedDataStore(IEnumerable<TValue> values)
            : base(values.Select(value => new KeyValuePair<Guid, TValue>(Guid.NewGuid(), value)))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDataStore{TValue}" /> class.
        /// </summary>
        /// <param name="entities">
        /// A collection of key-value pairs comprising the data store's entities.
        /// </param>
        protected SimulatedDataStore(IEnumerable<TEntity> entities)
            : base(entities.Select(entity => new KeyValuePair<Guid, TValue>(entity.Identifier, entity.Value)))
        {
            return;
        }

        /// <summary>
        /// Returns all entities that exist in the data store.
        /// </summary>
        /// <returns>
        /// All entities that exist in the data store.
        /// </returns>
        public abstract IQueryable<TEntity> GetAllEntities();

        /// <summary>
        /// Returns all entities that match the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// A predicate to evaluate.
        /// </param>
        /// <returns>
        /// All entities that match the specified predicate.
        /// </returns>
        public IQueryable<TEntity> GetEntitiesWhere(Expression<Func<TEntity, Boolean>> predicate) => GetAllEntities().Where(predicate.Compile()).AsQueryable();

        /// <summary>
        /// Returns the entity associated with the specified identifier, or <see langword="null" /> if the entity doesn't exist in
        /// the data store.
        /// </summary>
        /// <param name="identifier">
        /// An identifier for the entity to retrieve.
        /// </param>
        /// <returns>
        /// The entity associated with the specified identifier, or <see langword="null" /> if the entity doesn't exist in the data
        /// store.
        /// </returns>
        public TEntity GetEntityByIdentifier(Guid identifier) => GetAllEntities().Where(entity => entity.Identifier == identifier).SingleOrDefault();
    }
}