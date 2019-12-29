// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using RapidField.SolidInstruments.Example.DatabaseModel.Repositories;
using RapidField.SolidInstruments.ObjectComposition;
using System;

namespace RapidField.SolidInstruments.Example.DatabaseModel
{
    /// <summary>
    /// Encapsulates creation of new <see cref="IDataAccessRepository" /> that map to Example database entities.
    /// </summary>
    public sealed class ExampleRepositoryFactory : EntityFrameworkRepositoryFactory<ExampleContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleRepositoryFactory" /> class.
        /// </summary>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> is <see langword="null" />.
        /// </exception>
        public ExampleRepositoryFactory(ExampleContext context)
            : base(context)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleRepositoryFactory" /> class.
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
        public ExampleRepositoryFactory(ExampleContext context, IConfiguration applicationConfiguration)
            : base(context, applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="ExampleRepositoryFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="ExampleRepositoryFactory" />.
        /// </param>
        /// <param name="context">
        /// The database session that is used by the produced repositories.
        /// </param>
        protected override void Configure(ObjectFactoryConfiguration<IDataAccessRepository> configuration, ExampleContext context) => configuration.ProductionFunctions
            .Add(() => new NumberRepository(context))
            .Add(() => new NumberSeriesNumberRepository(context))
            .Add(() => new NumberSeriesRepository(context));
    }
}