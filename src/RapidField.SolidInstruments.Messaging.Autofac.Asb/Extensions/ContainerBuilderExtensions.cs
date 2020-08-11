// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using RapidField.SolidInstruments.Messaging.AzureServiceBus;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using System;

namespace RapidField.SolidInstruments.Messaging.Autofac.Asb.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support Azure Service Bus messaging.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers a collection of types that establish support for in-memory service bus functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The configuration connection string key name for the Azure Service Bus connection.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForAzureServiceBusMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
        {
            target.RegisterApplicationConfiguration(applicationConfiguration);
            _ = connectionStringConfigurationKeyName.RejectIf().IsNullOrEmpty(nameof(connectionStringConfigurationKeyName));

            target.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var serviceBusConnectionString = configuration.GetConnectionString(connectionStringConfigurationKeyName);

                if (serviceBusConnectionString.IsNullOrWhiteSpace())
                {
                    throw new DependencyResolutionException("The service bus connection string was not found in configuration.");
                }

                return new ServiceBusConnection(serviceBusConnectionString);
            }).IfNotRegistered(typeof(ServiceBusConnection)).AsSelf().InstancePerLifetimeScope();

            target.RegisterType<AzureServiceBusMessageAdapter>().IfNotRegistered(typeof(AzureServiceBusMessageAdapter)).As<IMessageAdapter<PrimitiveMessage>>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<AzureServiceBusClientFactory>().IfNotRegistered(typeof(AzureServiceBusClientFactory)).As<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<AzureServiceBusTransmittingFacade>().IfNotRegistered(typeof(AzureServiceBusTransmittingFacade)).As<IMessageTransmittingFacade>().AsSelf().InstancePerLifetimeScope();
            target.RegisterType<AzureServiceBusListeningFacade>().IfNotRegistered(typeof(AzureServiceBusListeningFacade)).As<IMessageListeningFacade>().AsSelf().SingleInstance();
            target.RegisterType<AzureServiceBusRequestingFacade>().IfNotRegistered(typeof(AzureServiceBusRequestingFacade)).As<IMessageRequestingFacade>().AsSelf().SingleInstance();
        }
    }
}