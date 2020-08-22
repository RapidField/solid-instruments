// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Performs data access operations against an Entity Framework data model type that is identified primarily by a
    /// <see cref="Guid" /> value.
    /// </summary>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkGlobalIdentityRepository<TDataAccessModel, TDomainModel, TContext> : EntityFrameworkRepository<Guid, TDataAccessModel, TDomainModel, TContext>
        where TDomainModel : class, IGlobalIdentityDomainModel, new()
        where TDataAccessModel : class, IGlobalIdentityDataAccessModel<TDomainModel>, new()
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EntityFrameworkGlobalIdentityRepository{TDataAccessModel, TDomainModel, TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkGlobalIdentityRepository(TContext context)
            : base(context)
        {
            return;
        }
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework data model type that is identified primarily by a
    /// <see cref="Guid" /> value.
    /// </summary>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkGlobalIdentityRepository<TDataAccessModel, TContext> : EntityFrameworkRepository<Guid, TDataAccessModel, TContext>
        where TDataAccessModel : class, IGlobalIdentityDataAccessModel
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkGlobalIdentityRepository{TDataAccessModel, TContext}" />
        /// class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkGlobalIdentityRepository(TContext context)
            : base(context)
        {
            return;
        }
    }
}