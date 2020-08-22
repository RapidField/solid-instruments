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
    /// <see cref="String" /> value.
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
    public class EntityFrameworkSemanticIdentityRepository<TDataAccessModel, TDomainModel, TContext> : EntityFrameworkRepository<String, TDataAccessModel, TDomainModel, TContext>
        where TDomainModel : class, ISemanticIdentityDomainModel, new()
        where TDataAccessModel : class, ISemanticIdentityDataAccessModel<TDomainModel>, new()
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EntityFrameworkSemanticIdentityRepository{TDataAccessModel, TDomainModel, TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkSemanticIdentityRepository(TContext context)
            : base(context)
        {
            return;
        }
    }

    /// <summary>
    /// Performs data access operations against an Entity Framework data model type that is identified primarily by a
    /// <see cref="String" /> value.
    /// </summary>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data model.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class EntityFrameworkSemanticIdentityRepository<TDataAccessModel, TContext> : EntityFrameworkRepository<String, TDataAccessModel, TContext>
        where TDataAccessModel : class, ISemanticIdentityDataAccessModel
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSemanticIdentityRepository{TDataAccessModel, TContext}" />
        /// class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public EntityFrameworkSemanticIdentityRepository(TContext context)
            : base(context)
        {
            return;
        }
    }
}