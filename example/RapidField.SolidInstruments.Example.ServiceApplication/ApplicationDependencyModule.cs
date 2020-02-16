// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.AzureServiceBus;
using System;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Example.ServiceApplication
{
    /// <summary>
    /// Encapsulates container configuration for application dependencies.
    /// </summary>
    public class ApplicationDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ApplicationDependencyModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
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
        protected override void Configure(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            // Register the configuration.
            configurator.AddSingleton(applicationConfiguration);

            // Register the service bus connection.
            configurator.AddScoped((serviceProvider) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceBusConnectionString = configuration.GetConnectionString("SolidInstrumentsServiceBusDev");

                if (serviceBusConnectionString.IsNullOrWhiteSpace())
                {
                    throw new ApplicationException("The service bus connection string was not found in configuration.");
                }

                return new ServiceBusConnection(serviceBusConnectionString);
            });

            // Register messaging types.
            configurator.AddScoped<AzureServiceBusMessageAdapter>();
            configurator.AddScoped<IMessageAdapter<AzureServiceBusMessage>, AzureServiceBusMessageAdapter>((serviceProvider) => serviceProvider.GetService<AzureServiceBusMessageAdapter>());
            configurator.AddScoped<AzureServiceBusClientFactory>();
            configurator.AddScoped<IMessagingClientFactory<ISenderClient, IReceiverClient, AzureServiceBusMessage>, AzureServiceBusClientFactory>((serviceProvider) => serviceProvider.GetService<AzureServiceBusClientFactory>());
            configurator.AddScoped<AzureServiceBusTransmittingFacade>();
            configurator.AddScoped<IMessageTransmittingFacade, AzureServiceBusTransmittingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusTransmittingFacade>());
            configurator.AddSingleton<AzureServiceBusListeningFacade>();
            configurator.AddSingleton<IMessageListeningFacade, AzureServiceBusListeningFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusListeningFacade>());
            configurator.AddSingleton<AzureServiceBusRequestingFacade>();
            configurator.AddSingleton<IMessageRequestingFacade, AzureServiceBusRequestingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusRequestingFacade>());
        }
    }
}