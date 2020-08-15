// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Performs data access operations against an Entity Framework data model type using a single transaction.
    /// </summary>
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
    public interface IEntityFrameworkRepository<TIdentifier, TDataAccessModel, TDomainModel, TContext> : IEntityFrameworkRepository<TIdentifier, TDataAccessModel, TContext>, IDomainModelRepository<TIdentifier, TDataAccessModel, TDomainModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>, new()
        where TDataAccessModel : class, IDataAccessModel<TIdentifier, TDomainModel>, new()
        where TContext : DbContext
    {
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework data model type using a single transaction.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public interface IEntityFrameworkRepository<TIdentifier, TDataAccessModel, TContext> : IEntityFrameworkRepository<TDataAccessModel, TContext>, IDataAccessModelRepository<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
        where TContext : DbContext
    {
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework entity type using a single transaction.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public interface IEntityFrameworkRepository<TEntity, TContext> : IDataAccessRepository<TEntity>, IEntityFrameworkRepository<TContext>
        where TEntity : class
        where TContext : DbContext
    {
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework entity type using a single transaction.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public interface IEntityFrameworkRepository<TContext> : IEntityFrameworkRepository
        where TContext : DbContext
    {
        /// <summary>
        /// Gets the database session type of the current <see cref="IEntityFrameworkRepository{TContext}" />.
        /// </summary>
        public Type ContextType
        {
            get;
        }
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework entity type using a single transaction.
    /// </summary>
    public interface IEntityFrameworkRepository : IDataAccessRepository
    {
    }
}