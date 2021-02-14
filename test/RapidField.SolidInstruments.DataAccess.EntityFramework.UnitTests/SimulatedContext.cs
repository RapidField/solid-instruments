// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Entities;
using System;
using System.Configuration;
using NumberEntity = RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests.Entities.Number;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    /// <summary>
    /// Represents a connection to a prototypical database.
    /// </summary>
    public class SimulatedContext : ConfiguredContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// The constructor was unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public SimulatedContext(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database, or <see cref="ContextDatabaseType.Unspecified" /> to determine the connection
        /// type dynamically based on the format of the connection string.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" /> and the constructor was
        /// unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public SimulatedContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType)
            : base(applicationConfiguration, databaseType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database, or <see cref="ContextDatabaseType.Unspecified" /> to determine the connection
        /// type dynamically based on the format of the connection string.
        /// </param>
        /// <param name="databaseName">
        /// The name of the backing database, which matches the associated connection string key in
        /// <paramref name="applicationConfiguration" />. The default value is equal to the context's type name with "Context"
        /// trimmed from the end, if found.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="databaseName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="databaseName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" /> and the constructor was
        /// unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public SimulatedContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, String databaseName)
            : base(applicationConfiguration, databaseType, databaseName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database, or <see cref="ContextDatabaseType.Unspecified" /> to determine the connection
        /// type dynamically based on the format of the connection string.
        /// </param>
        /// <param name="trackingBehavior">
        /// The query result tracking behavior for the context. The default value is <see cref="QueryTrackingBehavior.TrackAll" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" /> and the constructor was
        /// unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public SimulatedContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, QueryTrackingBehavior trackingBehavior)
            : base(applicationConfiguration, databaseType, trackingBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database, or <see cref="ContextDatabaseType.Unspecified" /> to determine the connection
        /// type dynamically based on the format of the connection string.
        /// </param>
        /// <param name="databaseName">
        /// The name of the backing database, which matches the associated connection string key in
        /// <paramref name="applicationConfiguration" />. The default value is equal to the context's type name with "Context"
        /// trimmed from the end, if found.
        /// </param>
        /// <param name="trackingBehavior">
        /// The query result tracking behavior for the context. The default value is <see cref="QueryTrackingBehavior.TrackAll" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="databaseName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="databaseName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" /> and the constructor was
        /// unable to determine the appropriate connection type by evaluating the connection string.
        /// </exception>
        public SimulatedContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, String databaseName, QueryTrackingBehavior trackingBehavior)
            : base(applicationConfiguration, databaseType, databaseName, trackingBehavior)
        {
            return;
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="NumberEntity" /> records.
        /// </summary>
        public DbSet<NumberEntity> Numbers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="NumberSeriesNumber" /> records.
        /// </summary>
        public DbSet<NumberSeriesNumber> NumberSeriesNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a persistent collection of <see cref="NumberSeries" /> records.
        /// </summary>
        public DbSet<NumberSeries> NumerSeries
        {
            get;
            set;
        }
    }
}