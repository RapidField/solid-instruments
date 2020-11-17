// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Domain;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Performs data access operations against an Entity Framework data model type using a single transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="EntityFrameworkRepository{TIdentifier, TDataAccessModel, TDomainModel, TContext}" /> is the default
    /// implementation of <see cref="IEntityFrameworkRepository{TIdentifier, TDataAccessModel, TDomainModel, TContext}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkRepository<TIdentifier, TDataAccessModel, TDomainModel, TContext> : EntityFrameworkRepository<TIdentifier, TDataAccessModel, TContext>, IEntityFrameworkRepository<TIdentifier, TDataAccessModel, TDomainModel, TContext>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>, new()
        where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EntityFrameworkRepository{TIdentifier, TDataAccessModel, TDomainModel, TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkRepository(TContext context)
            : base(context)
        {
            return;
        }
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework data model type using a single transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="EntityFrameworkRepository{TIdentifier, TDataAccessModel, TContext}" /> is the default implementation of
    /// <see cref="IEntityFrameworkRepository{TIdentifier, TDataAccessModel, TContext}" />.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkRepository<TIdentifier, TDataAccessModel, TContext> : EntityFrameworkRepository<TDataAccessModel, TContext>, IEntityFrameworkRepository<TIdentifier, TDataAccessModel, TContext>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TIdentifier, TDataAccessModel, TContext}" />
        /// class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkRepository(TContext context)
            : base(context)
        {
            return;
        }
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework entity type using a single transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="EntityFrameworkRepository{TEntity, TContext}" /> is the default implementation of
    /// <see cref="IEntityFrameworkRepository{TEntity, TContext}" />.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkRepository<TEntity, TContext> : DataAccessRepository<TEntity>, IEntityFrameworkRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity, TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkRepository(TContext context)
            : base()
        {
            Context = context.RejectIf().IsNull(nameof(context));
            Set = Context.Set<TEntity>();
        }

        /// <summary>
        /// Adds the specified entity to the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Add(TEntity entity, IConcurrencyControlToken controlToken)
        {
            Set.Attach(entity);
            Set.Add(entity);
        }

        /// <summary>
        /// Adds the specified entities to the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to add.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void AddRange(IEnumerable<TEntity> entities, IConcurrencyControlToken controlToken)
        {
            Set.AttachRange(entities);
            Set.AddRange(entities);
        }

        /// <summary>
        /// Returns all entities from the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// All entities within the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </returns>
        protected override IQueryable<TEntity> All(IConcurrencyControlToken controlToken) => ReadQuery;

        /// <summary>
        /// Determines whether or not any entities matching the specified predicate exist in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if any entities matching the specified predicate exist in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />, otherwise <see langword="false" />.
        /// </returns>
        protected override Boolean AnyWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken) => UntrackedQuery.Any(predicate);

        /// <summary>
        /// Configures the specified read query for eager loading.
        /// </summary>
        /// <remarks>
        /// When not overridden by a derived class, no eager loading is performed. Explicit loading must be configured by consuming
        /// classes.
        /// </remarks>
        /// <param name="query">
        /// The query to configure.
        /// </param>
        /// <returns>
        /// The resulting query.
        /// </returns>
        protected virtual IQueryable<TEntity> ConfigureEagerLoading(IQueryable<TEntity> query) => TrackedQuery;

        /// <summary>
        /// Determines whether or not the specified entity exists in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to evaluate.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the specified entity exists in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />, otherwise <see langword="false" />.
        /// </returns>
        protected override Boolean Contains(TEntity entity, IConcurrencyControlToken controlToken)
        {
            var entityType = Context.Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType.FindPrimaryKey();
            var keyValues = new Object[primaryKey.Properties.Count];

            for (var i = 0; i < keyValues.Length; i++)
            {
                keyValues[i] = primaryKey.Properties[i].GetGetter().GetClrValue(entity);
            }

            var trackedEntity = Set.Find(keyValues);

            if (trackedEntity is null)
            {
                return false;
            }

            var trackedEntityEntry = Context.Entry(trackedEntity);

            if (trackedEntityEntry.State == EntityState.Unchanged)
            {
                // Permit consuming code to invoke operations that implicitly attach the entity.
                trackedEntityEntry.State = EntityState.Detached;
            }

            return true;
        }

        /// <summary>
        /// Returns the number of entities in the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The number of entities in the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </returns>
        protected override Int64 Count(IConcurrencyControlToken controlToken) => UntrackedQuery.Count();

        /// <summary>
        /// Returns the number of entities matching the specified predicate in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The number of entities matching the specified predicate in the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </returns>
        protected override Int64 CountWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken) => UntrackedQuery.Count(predicate);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Returns all entities matching the specified predicate from the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="predicate">
        /// An expression to test each entity for a condition.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// All entities matching the specified predicate within the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </returns>
        protected override IQueryable<TEntity> FindWhere(Expression<Func<TEntity, Boolean>> predicate, IConcurrencyControlToken controlToken) => ReadQuery.Where(predicate);

        /// <summary>
        /// Removes the specified entity from the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Remove(TEntity entity, IConcurrencyControlToken controlToken)
        {
            Set.Attach(entity);
            Set.Remove(entity);
        }

        /// <summary>
        /// Removes the specified entities from the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to remove.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void RemoveRange(IEnumerable<TEntity> entities, IConcurrencyControlToken controlToken)
        {
            Set.AttachRange(entities);
            Set.RemoveRange(entities);
        }

        /// <summary>
        /// Updates the specified entity in the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void Update(TEntity entity, IConcurrencyControlToken controlToken)
        {
            Set.Attach(entity);
            Set.Update(entity);
        }

        /// <summary>
        /// Updates the specified entities in the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        /// <param name="entities">
        /// The entities to update.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void UpdateRange(IEnumerable<TEntity> entities, IConcurrencyControlToken controlToken)
        {
            Set.AttachRange(entities);
            Set.UpdateRange(entities);
        }

        /// <summary>
        /// Configures the specified read query for full eager loading.
        /// </summary>
        /// <param name="query">
        /// The query to configure.
        /// </param>
        /// <param name="entityType">
        /// The entity type for which to discover and include navigation properties.
        /// </param>
        /// <param name="parentNavigationPath">
        /// The navigation path for <paramref name="entityType" />.
        /// </param>
        /// <param name="recursionDepth">
        /// </param>
        /// <returns>
        /// The resulting query.
        /// </returns>
        [DebuggerHidden]
        private static IQueryable<TEntity> ConfigureFullEagerLoading(IQueryable<TEntity> query, IEntityType entityType, String parentNavigationPath, Int32 recursionDepth)
        {
            var navigationProperties = entityType.GetDerivedTypesInclusive().SelectMany(type => type.GetNavigations()).Distinct();
            recursionDepth += 1;

            if (recursionDepth > MaximumEagerLoadingRecursionDepth)
            {
                return query;
            }

            foreach (var navigationProperty in navigationProperties)
            {
                var navigationPath = parentNavigationPath.IsNullOrEmpty() ? navigationProperty.Name : $"{parentNavigationPath}.{navigationProperty.Name}";
                query = query.Include(navigationPath);
                query = ConfigureFullEagerLoading(query, navigationProperty.TargetEntityType, navigationPath, recursionDepth);
            }

            return query;
        }

        /// <summary>
        /// Configures the specified read query for full eager loading.
        /// </summary>
        /// <param name="query">
        /// The query to configure.
        /// </param>
        /// <returns>
        /// The resulting query.
        /// </returns>
        [DebuggerHidden]
        private IQueryable<TEntity> ConfigureFullEagerLoading(IQueryable<TEntity> query) => ConfigureFullEagerLoading(query, Context.Model.FindEntityType(EntityType), null, 0).AsTracking();

        /// <summary>
        /// Configures the specified read query.
        /// </summary>
        /// <param name="query">
        /// The query to configure.
        /// </param>
        /// <returns>
        /// The resulting query.
        /// </returns>
        [DebuggerHidden]
        private IQueryable<TEntity> ConfigureReadQuery(IQueryable<TEntity> query) => PerformsConfiguredEagerLoading ? ConfigureEagerLoading(query) : ConfigureFullEagerLoading(query);

        /// <summary>
        /// Gets the database session type of the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        public Type ContextType => typeof(TContext);

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="EntityFrameworkRepository{TEntity, TContext}" /> invokes
        /// <see cref="ConfigureEagerLoading(IQueryable{TEntity})" /> for all read queries. When <see langword="false" />, all
        /// queries are configured for full eager loading. The default value is <see langword="false" />.
        /// </summary>
        protected virtual Boolean PerformsConfiguredEagerLoading => DefaultPerformsConfiguredEagerLoadingValue;

        /// <summary>
        /// Gets a configured query that can be used for read operations.
        /// </summary>
        protected IQueryable<TEntity> ReadQuery => ConfigureReadQuery(TrackedQuery);

        /// <summary>
        /// Gets a tracked query that can be used for read or write operations.
        /// </summary>
        protected IQueryable<TEntity> TrackedQuery => Set.AsTracking();

        /// <summary>
        /// Gets an untracked query that can be used for read operations.
        /// </summary>
        protected IQueryable<TEntity> UntrackedQuery => Set.AsNoTracking();

        /// <summary>
        /// Represents the default value indicating whether or not the current
        /// <see cref="EntityFrameworkRepository{TEntity, TContext}" /> invokes
        /// <see cref="ConfigureEagerLoading(IQueryable{TEntity})" /> for all read queries.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPerformsConfiguredEagerLoadingValue = false;

        /// <summary>
        /// Represents the maximum depth of recursion to honor when configuring queries for full eager loading.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 MaximumEagerLoadingRecursionDepth = 5;

        /// <summary>
        /// Represents the database session for the current <see cref="EntityFrameworkRepository{TEntity, TContext}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TContext Context;

        /// <summary>
        /// Represents an object that is used to perform queries against entities in the database.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DbSet<TEntity> Set;
    }
}