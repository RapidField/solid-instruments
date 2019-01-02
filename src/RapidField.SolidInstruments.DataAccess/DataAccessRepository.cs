// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs data access operations for a specified entity type.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessRepository{TEntity}" /> is the default implementation of <see cref="IDataAccessRepository{TEntity}" />.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public abstract class DataAccessRepository<TEntity> : Instrument, IDataAccessRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessRepository{TEntity}" /> class.
        /// </summary>
        protected DataAccessRepository()
            : base()
        {
            return;
        }

        /// <summary>
        /// Adds the specified entity to the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void Add(TEntity entity)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Add(entity.RejectIf().IsNull(nameof(entity)).TargetArgument, controlToken);
            }
        }

        /// <summary>
        /// Updates the specified entity in the current <see cref="DataAccessRepository{TEntity}" />, or adds it if it doesn't exist.
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
        public void AddOrUpdate(TEntity entity)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Contains(entity.RejectIf().IsNull(nameof(entity)).TargetArgument, controlToken))
                {
                    Update(entity, controlToken);
                    return;
                }

                Add(entity, controlToken);
            }
        }

        /// <summary>
        /// Updates the specified entities in the current <see cref="DataAccessRepository{TEntity}" />, or adds them if they don't
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
        public void AddOrUpdateRange(IEnumerable<TEntity> entities)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (entities.RejectIf().IsNull(nameof(entities)).TargetArgument.Any(entity => entity is null))
                {
                    throw new ArgumentException("The specified entity collection contains one or more null entities.", nameof(entities));
                }

                foreach (var entity in entities)
                {
                    if (Contains(entity, controlToken))
                    {
                        Update(entity, controlToken);
                        continue;
                    }

                    Add(entity, controlToken);
                }
            }
        }

        /// <summary>
        /// Adds the specified entities to the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void AddRange(IEnumerable<TEntity> entities)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (entities.RejectIf().IsNull(nameof(entities)).TargetArgument.Any(entity => entity is null))
                {
                    throw new ArgumentException("The specified entity collection contains one or more null entities.", nameof(entities));
                }

                AddRange(entities, controlToken);
            }
        }

        /// <summary>
        /// Returns all entities from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// All entities within the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> All()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return All(controlToken);
            }
        }

        /// <summary>
        /// Determines whether or not any entities exist in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if any entities exist in the current <see cref="DataAccessRepository{TEntity}" />, otherwise
        /// <see langword="false" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean Any() => (Count() > 0);

        /// <summary>
        /// Determines whether or not the specified entity exists in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current <see cref="DataAccessRepository{TEntity}" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean Contains(TEntity entity)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return Contains(entity.RejectIf().IsNull(nameof(entity)), controlToken);
            }
        }

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="DataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean ContainsWhere(Expression<Func<TEntity, Boolean>> predicate) => CountWhere(predicate) > 0;

        /// <summary>
        /// Returns the number of entities in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// The number of entities in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int64 Count()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return Count(controlToken);
            }
        }

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return CountWhere(predicate.RejectIf().IsNull(nameof(predicate)), controlToken);
            }
        }

        /// <summary>
        /// Returns all entities matching the specified predicate from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                return FindWhere(predicate.RejectIf().IsNull(nameof(predicate)), controlToken);
            }
        }

        /// <summary>
        /// Removes the specified entity from the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void Remove(TEntity entity)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Remove(entity.RejectIf().IsNull(nameof(entity)).TargetArgument, controlToken);
            }
        }

        /// <summary>
        /// Removes the specified entities from the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (entities.RejectIf().IsNull(nameof(entities)).TargetArgument.Any(entity => entity is null))
                {
                    throw new ArgumentException("The specified entity collection contains one or more null entities.", nameof(entities));
                }

                RemoveRange(entities, controlToken);
            }
        }

        /// <summary>
        /// Removes the entities matching the specified predicate from the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void RemoveWhere(Expression<Func<TEntity, Boolean>> predicate) => RemoveRange(FindWhere(predicate));

        /// <summary>
        /// Updates the specified entity in the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void Update(TEntity entity)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                Update(entity.RejectIf().IsNull(nameof(entity)).TargetArgument, controlToken);
            }
        }

        /// <summary>
        /// Updates the specified entities in the current <see cref="DataAccessRepository{TEntity}" />.
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
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (entities.RejectIf().IsNull(nameof(entities)).TargetArgument.Any(entity => entity is null))
                {
                    throw new ArgumentException("The specified entity collection contains one or more null entities.", nameof(entities));
                }

                UpdateRange(entities, controlToken);
            }
        }

        /// <summary>
        /// Adds the specified entity to the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void Add(TEntity entity, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified entities to the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void AddRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Returns all entities from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// All entities within the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract IQueryable<TEntity> All(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="DataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean AnyWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Determines whether or not the specified entity exists in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current <see cref="DataAccessRepository{TEntity}" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean Contains(TEntity entity, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Returns the number of entities in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The number of entities in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract Int64 Count(ConcurrencyControlToken controlToken);

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="DataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Removes the specified entity from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void Remove(TEntity entity, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Removes the specified entities from the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void RemoveRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Updates the specified entity in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void Update(TEntity entity, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Updates the specified entities in the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that ensures thread safety for the operation.
        /// </param>
        protected abstract void UpdateRange(IEnumerable<TEntity> entities, ConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the entity type of the current <see cref="DataAccessRepository{TEntity}" />.
        /// </summary>
        public Type EntityType => typeof(TEntity);
    }
}