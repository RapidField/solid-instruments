// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using System;

namespace RapidField.SolidInstruments.Messaging.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for a service bus connection and related transport dependencies.
    /// </summary>
    public abstract class AutofacTransportDependencyModule : AutofacDependencyModule, ITransportDependencyModule<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacTransportDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The configuration connection string key name for the service bus connection. If the configured connection string value
        /// is equal to "InMemory", the module will register in-memory service bus components.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" />.
        /// </exception>
        protected AutofacTransportDependencyModule(IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
            : base(applicationConfiguration)
        {
            ConnectionStringConfigurationKeyName = connectionStringConfigurationKeyName.RejectIf().IsNullOrEmpty(nameof(connectionStringConfigurationKeyName));
            RegistersInMemoryServiceBusComponents = applicationConfiguration.GetConnectionString(ConnectionStringConfigurationKeyName)?.ToLower() == InMemoryConnectionStringValue.ToLower();
        }

        /// <summary>
        /// Gets the configuration connection string key name for the service bus connection.
        /// </summary>
        public String ConnectionStringConfigurationKeyName
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the module registers in-memory service bus components.
        /// </summary>
        /// <remarks>
        /// This property's value is <see langword="true" /> when the configured connection string has a value equal to
        /// <see cref="InMemoryConnectionStringValue" />.
        /// </remarks>
        public Boolean RegistersInMemoryServiceBusComponents
        {
            get;
        }

        /// <summary>
        /// Represents a connection string value that instructs the module to register in-memory service bus components.
        /// </summary>
        protected internal static readonly String InMemoryConnectionStringValue = DependencyEngine.InMemoryConnectionStringValue;
    }
}