// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Encapsulates creation of new Entity Framework <see cref="IDataAccessRepository" /> instances that map to database entities.
    /// </summary>
    /// <remarks>
    /// <see cref="EntityFrameworkRepositoryFactory{TContext}" /> is the default implementation of
    /// <see cref="IEntityFrameworkRepositoryFactory{TContext}" />.
    /// </remarks>
    /// <typeparam name="TContext">
    /// The type of the database session that is used by the produced repositories.
    /// </typeparam>
    public abstract class EntityFrameworkRepositoryFactory<TContext> : DataAccessRepositoryFactory, IEntityFrameworkRepositoryFactory<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepositoryFactory{TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        protected EntityFrameworkRepositoryFactory(TContext context)
            : base()
        {
            Context = context.RejectIf().IsNull(nameof(context));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepositoryFactory{TContext}" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" /> -or- <paramref name="applicationConfiguration" /> is
        /// <see langword="null" />.
        /// </exception>
        protected EntityFrameworkRepositoryFactory(TContext context, IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            Context = context.RejectIf().IsNull(nameof(context));
        }

        /// <summary>
        /// Configures the current <see cref="EntityFrameworkRepositoryFactory{TContext}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="EntityFrameworkRepositoryFactory{TContext}" />.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration) => Configure(configuration, Context);

        /// <summary>
        /// Configures the current <see cref="EntityFrameworkRepositoryFactory{TContext}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="EntityFrameworkRepositoryFactory{TContext}" />.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        protected abstract void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration, TContext context);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EntityFrameworkRepositoryFactory{TContext}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the database session that is used by the produced repositories.
        /// </summary>
        public TContext Context
        {
            get;
        }

        /// <summary>
        /// Gets the type of the database session that is used by the produced repositories.
        /// </summary>
        public Type ContextType => typeof(TContext);
    }
}