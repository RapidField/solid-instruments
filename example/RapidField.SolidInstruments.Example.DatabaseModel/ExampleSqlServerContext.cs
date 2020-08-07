// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;

namespace RapidField.SolidInstruments.Example.DatabaseModel
{
    /// <summary>
    /// Represents a connection to a prototypical, SQL Server database.
    /// </summary>
    public sealed class ExampleSqlServerContext : ExampleContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleSqlServerContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ExampleSqlServerContext(IConfiguration applicationConfiguration)
            : base(applicationConfiguration, ContextDatabaseType.SqlServer, QueryTrackingBehavior.TrackAll)
        {
            return;
        }

        /// <summary>
        /// Configures the in-memory database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected sealed override void OnConfiguringInMemory(IConfiguration applicationConfiguration, InMemoryDbContextOptionsBuilder optionsBuilder) => throw new InvalidOperationException();

        /// <summary>
        /// Configures the SQL Server database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected sealed override void OnConfiguringSqlServer(IConfiguration applicationConfiguration, SqlServerDbContextOptionsBuilder optionsBuilder) => base.OnConfiguringSqlServer(applicationConfiguration, optionsBuilder);
    }
}