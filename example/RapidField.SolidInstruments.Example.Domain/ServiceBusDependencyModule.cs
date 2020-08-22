// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Messaging.DotNetNative.Rmq;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain
{
    /// <summary>
    /// Encapsulates native .NET container configuration for the example service bus connection and related transport dependencies.
    /// </summary>
    public sealed class ServiceBusDependencyModule : DotNetNativeRabbitMqModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ServiceBusDependencyModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration, ServiceBusConnectionStringConfigurationKeyName)
        {
            return;
        }

        /// <summary>
        /// Represents the configuration connection string key name for the service bus connection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ServiceBusConnectionStringConfigurationKeyName = "ExampleServiceBus";
    }
}