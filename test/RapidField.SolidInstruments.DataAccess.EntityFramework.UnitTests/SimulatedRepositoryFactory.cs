// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Repositories;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> that map to Simulated database entities.
    /// </summary>
    public sealed class SimulatedRepositoryFactory : EntityFrameworkRepositoryFactory<SimulatedContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRepositoryFactory" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public SimulatedRepositoryFactory(SimulatedContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedRepositoryFactory" /> class.
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
        public SimulatedRepositoryFactory(SimulatedContext context, IConfiguration applicationConfiguration)
            : base(context, applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="SimulatedRepositoryFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="SimulatedRepositoryFactory" />.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration, SimulatedContext context) => configuration.ProductionFunctions
            .Add(() => new NumberRepository(context))
            .Add(() => new NumberSeriesNumberRepository(context))
            .Add(() => new NumberSeriesRepository(context));
    }
}