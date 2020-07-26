// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Performs read-only data access operations for a specified entity type.
    /// </summary>
    /// <remarks>
    /// <see cref="ReadOnlyDataAccessRepository{TEntity}" /> is the default implementation of
    /// <see cref="IReadOnlyDataAccessRepository{TEntity}" />.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public abstract class ReadOnlyDataAccessRepository<TEntity> : Instrument, IReadOnlyDataAccessRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyDataAccessRepository{TEntity}" /> class.
        /// </summary>
        protected ReadOnlyDataAccessRepository()
            : base()
        {
            return;
        }

        /// <summary>
        /// Returns all entities from the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// All entities within the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
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
        /// Determines whether or not any entities exist in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if any entities exist in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />,
        /// otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean Any() => (Count() > 0);

        /// <summary>
        /// Determines whether or not the specified entity exists in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
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
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean ContainsWhere(Expression<Func<TEntity, Boolean>> predicate) => CountWhere(predicate) > 0;

        /// <summary>
        /// Returns the number of entities in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <returns>
        /// The number of entities in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
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
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
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
        /// Returns all entities matching the specified predicate from the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
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
        /// Returns all entities from the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// All entities within the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract IQueryable<TEntity> All(IConcurrencyControlToken controlToken);

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean AnyWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Determines whether or not the specified entity exists in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />, otherwise <see langword="false" />.
        /// </returns>
        protected abstract Boolean Contains(TEntity entity, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Returns the number of entities in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The number of entities in the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract Int64 Count(IConcurrencyControlToken controlToken);

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current
        /// <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </returns>
        protected abstract IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the entity type of the current <see cref="ReadOnlyDataAccessRepository{TEntity}" />.
        /// </summary>
        public Type EntityType => typeof(TEntity);
    }
}