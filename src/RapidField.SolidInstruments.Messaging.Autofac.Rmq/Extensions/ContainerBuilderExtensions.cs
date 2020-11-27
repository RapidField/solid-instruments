// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using RapidField.SolidInstruments.Messaging.RabbitMq;
using RapidField.SolidInstruments.Messaging.RabbitMq.TransportPrimitives;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.Autofac.Rmq.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support RabbitMQ messaging.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration) => target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport());

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionStringConfigurationKeyName">
        /// The connection URI for the target RabbitMQ instance.
        /// </param>
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
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, String connectionStringConfigurationKeyName)
        {
            var connectionString = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument.GetConnectionString(connectionStringConfigurationKeyName)?.Trim();

            if (connectionString.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("The service bus connection string was not found in configuration.", nameof(connectionStringConfigurationKeyName));
            }

            try
            {
                var connectionUri = new Uri(connectionString);
                target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, connectionUri);
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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="connectionUri">
        /// The connection URI for the target RabbitMQ instance. The default value is "amqp://guest:guest@localhost:5672".
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="connectionUri" /> is not valid for AMQP connections.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="connectionUri" /> is
        /// <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, Uri connectionUri) => target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(connectionUri));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
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
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, Uri connectionUri, SerializationFormat messageBodySerializationFormat) => target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(connectionUri, messageBodySerializationFormat));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
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
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="hostName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="hostName" /> is
        /// <see langword="null" />.
        /// </exception>
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, String hostName, Int32? portNumber, String virtualHost, String userName, String password) => target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(hostName, portNumber, virtualHost, userName, password));

        /// <summary>
        /// Registers a collection of types that establish support for RabbitMQ functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
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
        public static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, String hostName, Int32? portNumber, String virtualHost, String userName, String password, SerializationFormat messageBodySerializationFormat) => target.RegisterSupportingTypesForRabbitMqMessaging(applicationConfiguration, new RabbitMqMessageTransport(hostName, portNumber, virtualHost, userName, password, messageBodySerializationFormat));

        /// <summary>
        /// Registers a collection of types that establish support for in-memory service bus functionality.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="transport">
        /// The transport instance that supports RabbitMQ messaging.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private static void RegisterSupportingTypesForRabbitMqMessaging(this ContainerBuilder target, IConfiguration applicationConfiguration, RabbitMqMessageTransport transport)
        {
            target.RegisterApplicationConfiguration(applicationConfiguration);
            target.RegisterConfigurationCommandHandlers();
            target.RegisterInstance(transport).IfNotRegistered(typeof(RabbitMqMessageTransport)).AsSelf().SingleInstance();
            target.RegisterInstance(transport.CreateConnection()).IfNotRegistered(typeof(IMessageTransportConnection)).AsSelf().SingleInstance();
            target.RegisterType<RabbitMqMessageAdapter>().IfNotRegistered(typeof(RabbitMqMessageAdapter)).As<IMessageAdapter<PrimitiveMessage>>().AsSelf().SingleInstance();
            target.RegisterType<RabbitMqClientFactory>().IfNotRegistered(typeof(RabbitMqClientFactory)).As<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>>().AsSelf().SingleInstance();
            target.RegisterType<RabbitMqTransmittingFacade>().IfNotRegistered(typeof(RabbitMqTransmittingFacade)).As<IMessageTransmittingFacade>().AsSelf().SingleInstance();
            target.RegisterType<RabbitMqListeningFacade>().IfNotRegistered(typeof(RabbitMqListeningFacade)).As<IMessageListeningFacade>().AsSelf().SingleInstance();
            target.RegisterType<RabbitMqRequestingFacade>().IfNotRegistered(typeof(RabbitMqRequestingFacade)).As<IMessageRequestingFacade>().AsSelf().SingleInstance();
        }
    }
}