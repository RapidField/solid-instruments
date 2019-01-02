// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.ObjectComposition;
using RapidField.SolidInstruments.Prototype.DatabaseModel.Repositories;
using System;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> that map to Prototype database entities.
    /// </summary>
    public sealed class PrototypeRepositoryFactory : EntityFrameworkRepositoryFactory<PrototypeContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrototypeRepositoryFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="context" /> is
        /// <see langword="null" />.
        /// </exception>
        public PrototypeRepositoryFactory(IConfiguration applicationConfiguration, PrototypeContext context)
            : base(applicationConfiguration, context)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="PrototypeRepositoryFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="PrototypeRepositoryFactory" />.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration, PrototypeContext context) => configuration.ProductionFunctions
            .Define(() => new NumberRepository(context))
            .Define(() => new NumberSeriesNumberRepository(context))
            .Define(() => new NumberSeriesRepository(context));
    }
}