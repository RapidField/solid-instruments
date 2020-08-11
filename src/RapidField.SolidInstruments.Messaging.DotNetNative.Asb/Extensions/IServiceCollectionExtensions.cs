// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            target.TryAddScoped((serviceProvider) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceBusConnectionString = configuration.GetConnectionString(connectionStringConfigurationKeyName)?.Trim();

                if (serviceBusConnectionString.IsNullOrWhiteSpace())
                {
                    throw new DependencyResolutionException("The service bus connection string was not found in configuration.");
                }

                return new ServiceBusConnection(serviceBusConnectionString);
            });

            target.TryAddScoped<AzureServiceBusMessageAdapter>();
            target.TryAddScoped<IMessageAdapter<AzureServiceBusMessage>>((serviceProvider) => serviceProvider.GetService<AzureServiceBusMessageAdapter>());
            target.TryAddScoped<AzureServiceBusClientFactory>();
            target.TryAddScoped<IMessagingClientFactory<ISenderClient, IReceiverClient, AzureServiceBusMessage>>((serviceProvider) => serviceProvider.GetService<AzureServiceBusClientFactory>());
            target.TryAddScoped<AzureServiceBusTransmittingFacade>();
            target.TryAddScoped<IMessageTransmittingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusTransmittingFacade>());
            target.TryAddSingleton<AzureServiceBusListeningFacade>();
            target.TryAddSingleton<IMessageListeningFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusListeningFacade>());
            target.TryAddSingleton<AzureServiceBusRequestingFacade>();
            target.TryAddSingleton<IMessageRequestingFacade>((serviceProvider) => serviceProvider.GetService<AzureServiceBusRequestingFacade>());
            return target;
        }
    }
}