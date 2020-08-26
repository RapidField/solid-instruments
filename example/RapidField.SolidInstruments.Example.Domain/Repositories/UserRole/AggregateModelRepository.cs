// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;
using DataAccessModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;
using DomainModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Repositories.UserRole
{
    /// <summary>
    /// Performs data access operations for the <see cref="DataAccessModel" /> type.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the repository.
    /// </typeparam>
    public class AggregateModelRepository<TContext> : EntityFrameworkGlobalIdentityRepository<DataAccessModel, DomainModel, TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateModelRepository{TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session for the repository.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public AggregateModelRepository(TContext context)
            : base(context)
        {
            return;
        }
    }
}