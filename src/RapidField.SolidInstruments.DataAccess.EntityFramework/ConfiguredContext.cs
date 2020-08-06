﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Represents an extension of <see cref="DbContext" /> that simplifies configuration for common use cases.
    /// </summary>
    public class ConfiguredContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ConfiguredContext(IConfiguration applicationConfiguration)
            : this(applicationConfiguration, DefaultDatabaseType)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database. The default value is <see cref="ContextDatabaseType.InMemory" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" />.
        /// </exception>
        public ConfiguredContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType)
            : this(applicationConfiguration, databaseType, UseConventionalDatabaseNameIndicator)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database. The default value is <see cref="ContextDatabaseType.InMemory" />.
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
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" />.
        /// </exception>
        public ConfiguredContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, String databaseName)
            : this(applicationConfiguration, databaseType, databaseName ?? UseConventionalDatabaseNameIndicator, DefaultTrackingBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database. The default value is <see cref="ContextDatabaseType.InMemory" />.
        /// </param>
        /// <param name="trackingBehavior">
        /// The query result tracking behavior for the context. The default value is <see cref="QueryTrackingBehavior.TrackAll" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" />.
        /// </exception>
        public ConfiguredContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, QueryTrackingBehavior trackingBehavior)
            : this(applicationConfiguration, databaseType, UseConventionalDatabaseNameIndicator, trackingBehavior)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredContext" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="databaseType">
        /// The database type of the backing database. The default value is <see cref="ContextDatabaseType.InMemory" />.
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
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="databaseType" /> is equal to <see cref="ContextDatabaseType.Unspecified" />.
        /// </exception>
        public ConfiguredContext(IConfiguration applicationConfiguration, ContextDatabaseType databaseType, String databaseName, QueryTrackingBehavior trackingBehavior)
            : base()
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
            DatabaseNameReference = (databaseName ?? UseConventionalDatabaseNameIndicator).RejectIf().IsNullOrEmpty(nameof(databaseName));
            DatabaseType = databaseType.RejectIf().IsEqualToValue(ContextDatabaseType.Unspecified, nameof(databaseType));
            TrackingBehavior = trackingBehavior;
        }

        /// <summary>
        /// Configures the database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder = DatabaseType switch
                {
                    ContextDatabaseType.Cosmos => UseCosmos(optionsBuilder),
                    ContextDatabaseType.InMemory => UseInMemory(optionsBuilder),
                    ContextDatabaseType.SQLServer => UseSqlServer(optionsBuilder),
                    _ => throw new UnsupportedSpecificationException($"The specified database type, {DatabaseType}, is not supported.")
                };

                optionsBuilder.UseQueryTrackingBehavior(TrackingBehavior);
            }
            finally
            {
                base.OnConfiguring(optionsBuilder);
            }
        }

        /// <summary>
        /// Configures the Cosmos DB database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected virtual void OnConfiguringCosmos(IConfiguration applicationConfiguration, CosmosDbContextOptionsBuilder optionsBuilder)
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
        protected virtual void OnConfiguringInMemory(IConfiguration applicationConfiguration, InMemoryDbContextOptionsBuilder optionsBuilder)
        {
            return;
        }

        /// <summary>
        /// Configures the SQL Server database to be used for this context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="optionsBuilder">
        /// A builder that is used to create or modify options for this context.
        /// </param>
        protected virtual void OnConfiguringSqlServer(IConfiguration applicationConfiguration, SqlServerDbContextOptionsBuilder optionsBuilder)
        {
            return;
        }

        /// <summary>
        /// Configures the model that is discovered by convention from entity types exposed via <see cref="DbSet{TEntity}" />
        /// properties on the derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// A builder that is used to configure the model.
        /// </param>
        protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                OnModelCreating(ApplicationConfiguration, modelBuilder);
            }
            finally
            {
                base.OnModelCreating(modelBuilder);
            }
        }

        /// <summary>
        /// Configures the model that is discovered by convention from entity types exposed via <see cref="DbSet{TEntity}" />
        /// properties on the derived context.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="modelBuilder">
        /// A builder that is used to configure the model.
        /// </param>
        protected virtual void OnModelCreating(IConfiguration applicationConfiguration, ModelBuilder modelBuilder)
        {
            return;
        }

        /// <summary>
        /// Returns the conventional, default name of the backing database for the current <see cref="ConfiguredContext" />.
        /// </summary>
        [DebuggerHidden]
        private String GetConventionalDatabaseName()
        {
            var contextTypeName = GetType().Name;

            if (contextTypeName == ContextPostfixStringElement)
            {
                return contextTypeName;
            }
            else if (contextTypeName.EndsWith(ContextPostfixStringElement))
            {
                return contextTypeName.Substring(0, (contextTypeName.Length - ContextPostfixStringElement.Length));
            }

            return contextTypeName;
        }

        /// <summary>
        /// Configures the Cosmos DB database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private void OnConfiguringCosmos(CosmosDbContextOptionsBuilder optionsBuilder) => OnConfiguringCosmos(ApplicationConfiguration, optionsBuilder);

        /// <summary>
        /// Configures the in-memory database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private void OnConfiguringInMemory(InMemoryDbContextOptionsBuilder optionsBuilder) => OnConfiguringInMemory(ApplicationConfiguration, optionsBuilder);

        /// <summary>
        /// Configures the SQL Server database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private void OnConfiguringSqlServer(SqlServerDbContextOptionsBuilder optionsBuilder) => OnConfiguringSqlServer(ApplicationConfiguration, optionsBuilder);

        /// <summary>
        /// Configures the specified options builder for a Cosmos DB database connection.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private DbContextOptionsBuilder UseCosmos(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder();
            var accountEndpoint = (String)null;
            var accountKey = (String)null;

            try
            {
                connectionStringBuilder.ConnectionString = ConnectionString;

                if (connectionStringBuilder.TryGetValue(ConnectionStringKeywordForAccountEndpoint, out var configuredAccountEndpoint))
                {
                    accountEndpoint = configuredAccountEndpoint?.ToString().Trim();
                }

                if (accountEndpoint.IsNullOrEmpty())
                {
                    throw new ConfigurationErrorsException($"A value for the keyword \"{ConnectionStringKeywordForAccountEndpoint}\" was not found within the connection string value for \"{DatabaseName}\".");
                }

                if (connectionStringBuilder.TryGetValue(ConnectionStringKeywordForAccountKey, out var configuredAccountKey))
                {
                    accountKey = configuredAccountKey?.ToString().Trim();
                }

                if (accountKey.IsNullOrEmpty())
                {
                    throw new ConfigurationErrorsException($"A value for the keyword \"{ConnectionStringKeywordForAccountKey}\" was not found within the connection string value for \"{DatabaseName}\".");
                }
            }
            catch (Exception exception)
            {
                throw new ConfigurationErrorsException($"The connection string value for \"{DatabaseName}\" is invalid. See inner exception.", exception);
            };

            return optionsBuilder.UseCosmos(accountEndpoint, accountKey, DatabaseName, OnConfiguringCosmos);
        }

        /// <summary>
        /// Configures the specified options builder for an in-memory database connection.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private DbContextOptionsBuilder UseInMemory(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(DatabaseName, OnConfiguringInMemory).ConfigureWarnings((warningsBuilder) => { warningsBuilder.Ignore(InMemoryEventId.TransactionIgnoredWarning); });

        /// <summary>
        /// Configures the specified options builder for a SQL Server database connection.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        [DebuggerHidden]
        private DbContextOptionsBuilder UseSqlServer(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(ConnectionString, OnConfiguringSqlServer);

        /// <summary>
        /// Gets the connection string for the associated database from <see cref="ApplicationConfiguration" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String ConnectionString => ApplicationConfiguration.GetConnectionString(DatabaseName);

        /// <summary>
        /// Gets the name of the backing database, which matches the associated connection string key in
        /// <see cref="ApplicationConfiguration" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String DatabaseName
        {
            get
            {
                if (DatabaseNameReference == UseConventionalDatabaseNameIndicator)
                {
                    DatabaseNameReference = GetConventionalDatabaseName();
                }

                return DatabaseNameReference;
            }
        }

        /// <summary>
        /// Represents the connection string keyword "AccountEndpoint".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String ConnectionStringKeywordForAccountEndpoint = "AccountEndpoint";

        /// <summary>
        /// Represents the connection string keyword "AccountKey".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String ConnectionStringKeywordForAccountKey = "AccountKey";

        /// <summary>
        /// Represents the word "Context" as a string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ContextPostfixStringElement = "Context";

        /// <summary>
        /// Represents the default database type for backing databases.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const ContextDatabaseType DefaultDatabaseType = ContextDatabaseType.InMemory;

        /// <summary>
        /// Represents the default query result tracking behavior for contexts.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const QueryTrackingBehavior DefaultTrackingBehavior = QueryTrackingBehavior.TrackAll;

        /// <summary>
        /// Represents a value that indicates that the conventional, default database name should be used
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String UseConventionalDatabaseNameIndicator = "__UseConventionalDatabaseName__";

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IConfiguration ApplicationConfiguration;

        /// <summary>
        /// Represents the database type of the backing database.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ContextDatabaseType DatabaseType;

        /// <summary>
        /// Represents the query result tracking behavior for the context.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly QueryTrackingBehavior TrackingBehavior;

        /// <summary>
        /// Represents the name of the backing database, which matches the associated connection string key in
        /// <see cref="ApplicationConfiguration" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String DatabaseNameReference;
    }
}