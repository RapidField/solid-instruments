// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using RapidField.SolidInstruments.Messaging.RabbitMq;
using RapidField.SolidInstruments.Messaging.RabbitMq.TransportPrimitives;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.DotNetNative.Rmq.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support RabbitMQ
    /// messaging.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration) => target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport());

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The connection URI for the target RabbitMQ instance.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="connectionStringConfigurationKeyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The connection URI defined by <paramref name="connectionStringConfigurationKeyName" /> is not valid for AMQP
        /// connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or-
        /// <paramref name="connectionStringConfigurationKeyName" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
        {
            var connectionString = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument.GetConnectionString(connectionStringConfigurationKeyName)?.Trim();

            if (connectionString.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("The service bus connection string was not found in configuration.", nameof(connectionStringConfigurationKeyName));
            }

            try
            {
                var connectionUri = new Uri(connectionString);
                return target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, connectionUri);
            }
            catch (Exception exception)
            {
                throw new ArgumentException("The specified connection string does not contain a valid AMQP connection URI.", nameof(connectionStringConfigurationKeyName), exception);
            }
        }

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance. The default value is "amqp://guest:guest@localhost:5672".
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="connectionUri" /> is
        /// <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, Uri connectionUri) => target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(connectionUri));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance. The default value is "amqp://guest:guest@localhost:5672".
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="connectionUri" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, Uri connectionUri, SerializationFormat messageBodySerializationFormat) => target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(connectionUri, messageBodySerializationFormat));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="hostName">
        /// The name of the host to connect to. The default value is "localhost".
        /// </param>
        /// <param name="portNumber">
        /// The port number to connect to, or <see langword="null" /> to use the default port number (5672).
        /// </param>
        /// <param name="virtualHost">
        /// The name of the virtual host to connect to, or <see langword="null" /> to omit a virtual host.
        /// </param>
        /// <param name="userName">
        /// The user name for the connection, or <see langword="null" /> to use the default user name ("guest").
        /// </param>
        /// <param name="password">
        /// The password for the connection, or <see langword="null" /> to use the default password ("guest").
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="hostName" /> is
        /// <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, String hostName, Int32? portNumber, String virtualHost, String userName, String password) => target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(hostName, portNumber, virtualHost, userName, password));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="hostName">
        /// The name of the host to connect to. The default value is "localhost".
        /// </param>
        /// <param name="portNumber">
        /// The port number to connect to, or <see langword="null" /> to use the default port number (5672).
        /// </param>
        /// <param name="virtualHost">
        /// The name of the virtual host to connect to, or <see langword="null" /> to omit a virtual host.
        /// </param>
        /// <param name="userName">
        /// The user name for the connection, or <see langword="null" /> to use the default user name ("guest").
        /// </param>
        /// <param name="password">
        /// The password for the connection, or <see langword="null" /> to use the default password ("guest").
        /// </param>
        /// <param name="messageBodySerializationFormat">
        /// The format that is used to serialize enqueued message bodies. The default value is
        /// <see cref="SerializationFormat.CompressedJson" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="hostName" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="messageBodySerializationFormat" /> is equal to <see cref="SerializationFormat.Unspecified" />.
        /// </exception>
        public static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, String hostName, Int32? portNumber, String virtualHost, String userName, String password, SerializationFormat messageBodySerializationFormat) => target.AddSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(hostName, portNumber, virtualHost, userName, password, messageBodySerializationFormat));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="transport">
        /// The transport instance that supports RabbitMQ messaging.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static IServiceCollection AddSupportingTypesForRabbitMqMessaging(this IServiceCollection target, IConfiguration applicationConfiguration, RabbitMqMessageTransport transport)
        {
            target.AddApplicationConfiguration(applicationConfiguration);
            target.TryAddSingleton(transport);
            target.TryAddSingleton(transport.CreateConnection());
            target.TryAddSingleton<RabbitMqMessageAdapter>();
            target.TryAddSingleton<IMessageAdapter<PrimitiveMessage>>((serviceProvider) => serviceProvider.GetService<RabbitMqMessageAdapter>());
            target.TryAddSingleton<RabbitMqClientFactory>();
            target.TryAddSingleton<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>>((serviceProvider) => serviceProvider.GetService<RabbitMqClientFactory>());
            target.TryAddSingleton<RabbitMqTransmittingFacade>();
            target.TryAddSingleton<IMessageTransmittingFacade>((serviceProvider) => serviceProvider.GetService<RabbitMqTransmittingFacade>());
            target.TryAddSingleton<RabbitMqListeningFacade>();
            target.TryAddSingleton<IMessageListeningFacade>((serviceProvider) => serviceProvider.GetService<RabbitMqListeningFacade>());
            target.TryAddSingleton<RabbitMqRequestingFacade>();
            target.TryAddSingleton<IMessageRequestingFacade>((serviceProvider) => serviceProvider.GetService<RabbitMqRequestingFacade>());
            return target;
        }
    }
}