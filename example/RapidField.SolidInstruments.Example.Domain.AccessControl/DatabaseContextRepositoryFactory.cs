// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.Example.Domain.AccessControl.Repositories;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> instances that map to <see cref="AccessControl" />
    /// database entities.
    /// </summary>
    public sealed class DatabaseContextRepositoryFactory : EntityFrameworkRepositoryFactory<DatabaseContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContextRepositoryFactory" /> class.
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
        public DatabaseContextRepositoryFactory(DatabaseContext context, IConfiguration applicationConfiguration)
            : base(context, applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="DatabaseContextRepositoryFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="DatabaseContextRepositoryFactory" />.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration, DatabaseContext context) => configuration.ProductionFunctions
            .Add(() => new UserRepository(context))
            .Add(() => new UserRoleAssignmentRepository(context))
            .Add(() => new UserRoleRepository(context));

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DatabaseContextRepositoryFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}