// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Messaging.Autofac.Asb.Extensions;
using RapidField.SolidInstruments.Messaging.Autofac.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging.Autofac.Asb
{
    /// <summary>
    /// Encapsulates Autofac container configuration for an Azure Service Bus connection and related transport dependencies.
    /// </summary>
    public class AutofacAzureServiceBusModule : AutofacTransportDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacAzureServiceBusModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The configuration connection string key name for the service bus connection.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" />.
        /// </exception>
        public AutofacAzureServiceBusModule(IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
            : base(applicationConfiguration, connectionStringConfigurationKeyName)
        {
            return;
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected override void Configure(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            if (RegistersInMemoryServiceBusComponents)
            {
                configurator.RegisterSupportingTypesForInMemoryMessaging();
                return;
            }

            configurator.RegisterSupportingTypesForAzureServiceBusMessaging(applicationConfiguration, ConnectionStringConfigurationKeyName);
        }
    }
}