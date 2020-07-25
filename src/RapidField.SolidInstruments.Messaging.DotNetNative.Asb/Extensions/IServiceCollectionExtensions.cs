// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using RapidField.SolidInstruments.Messaging.AzureServiceBus;
using System;
using AzureServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace RapidField.SolidInstruments.Messaging.DotNetNative.Asb.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support Azure
    /// Service Bus messaging.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a collection of types that establish support for Azure Service Bus functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The configuration connection string key name for the Azure Service Bus connection.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForAzureServiceBusMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
        {
            target.AddApplicationConfiguration(applicationConfiguration);
            _ = connectionStringConfigurationKeyName.RejectIf().IsNullOrEmpty(nameof(connectionStringConfigurationKeyName));

            target.AddScoped((serviceProvider) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceBusConnectionString = configuration.GetConnectionString(connectionStringConfigurationKeyName)?.Trim();

                if (serviceBusConnectionString.IsNullOrWhiteSpace())
                {
                    throw new DependencyResolutionException("The service bus connection string was not found in configuration.");
                }

                return new ServiceBusConnection(serviceBusConnectionString);
            });

            target.AddScoped<AzureServiceBusMessageAdapter>();
            target.AddScoped<IMessageAdapter<AzureServiceBusMessage>, AzureServiceBusMessageAdapter>((serviceProvider) => serviceProvider.GetService<AzureServiceBusMessageAdapter>());
            target.AddScoped<AzureServiceBusClientFactory>();
            target.AddScoped<IMessagingClientFactory<ISenderClient, IReceiverClient, AzureServiceBusMessage>, AzureServiceBusClientFactory>((serviceProvider) => serviceProvider.GetService<AzureServiceBusClientFactory>());
            target.AddScoped<AzureServiceBusTransmittingFacade>();
            target.AddScoped<IMessageTransmittingFacade, AzureServiceBusTransmittingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusTransmittingFacade>());
            target.AddSingleton<AzureServiceBusListeningFacade>();
            target.AddSingleton<IMessageListeningFacade, AzureServiceBusListeningFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusListeningFacade>());
            target.AddSingleton<AzureServiceBusRequestingFacade>();
            target.AddSingleton<IMessageRequestingFacade, AzureServiceBusRequestingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusRequestingFacade>());
            return target;
        }
    }
}