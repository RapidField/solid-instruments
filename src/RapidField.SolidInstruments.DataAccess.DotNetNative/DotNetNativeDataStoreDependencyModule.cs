// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;

namespace RapidField.SolidInstruments.DataAccess.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for a data store connection and related data access dependencies.
    /// </summary>
    public abstract class DotNetNativeDataStoreDependencyModule : DotNetNativeDependencyModule, IDataStoreDependencyModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDataStoreDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The configuration connection string key name for the data store connection. If the configured connection string value is
        /// equal to "InMemory", the module will register in-memory database components.
        /// </param>
        /// <param name="dataStoreName">
        /// The name of the associated data store.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty -or- <paramref name="dataStoreName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" /> -or-
        /// <paramref name="dataStoreName" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeDataStoreDependencyModule(IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName, String dataStoreName)
            : base(applicationConfiguration)
        {
            ConnectionStringConfigurationKeyName = connectionStringConfigurationKeyName.RejectIf().IsNullOrEmpty(nameof(connectionStringConfigurationKeyName));
            DataStoreName = dataStoreName.RejectIf().IsNullOrEmpty(nameof(dataStoreName));
            RegistersInMemoryDatabaseComponents = applicationConfiguration.GetConnectionString(ConnectionStringConfigurationKeyName)?.ToLower() == InMemoryConnectionStringValue.ToLower();
        }

        /// <summary>
        /// Gets the configuration connection string key name for the data store connection.
        /// </summary>
        public String ConnectionStringConfigurationKeyName
        {
            get;
        }

        /// <summary>
        /// Gets the name of the associated data store.
        /// </summary>
        public String DataStoreName
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the module registers in-memory database components.
        /// </summary>
        /// <remarks>
        /// This property's value is <see langword="true" /> when the configured connection string has a value equal to
        /// <see cref="InMemoryConnectionStringValue" />.
        /// </remarks>
        public Boolean RegistersInMemoryDatabaseComponents
        {
            get;
        }

        /// <summary>
        /// Represents a connection string value that instructs the module to register in-memory data store components.
        /// </summary>
        protected internal const String InMemoryConnectionStringValue = "InMemory";
    }
}