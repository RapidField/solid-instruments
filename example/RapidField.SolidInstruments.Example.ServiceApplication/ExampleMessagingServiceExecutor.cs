// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.Service;
using System;
using System.IO;

namespace RapidField.SolidInstruments.Example.ServiceApplication
{
    /// <summary>
    /// Prepares for and performs execution of the Example messaging service.
    /// </summary>
    internal sealed class ExampleMessagingServiceExecutor : MessagingServiceExecutor<ApplicationDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleMessagingServiceExecutor" /> class.
        /// </summary>
        public ExampleMessagingServiceExecutor()
            : base("Example Messaging Service")
        {
            return;
        }

        /// <summary>
        /// Adds message listeners to the service.
        /// </summary>
        /// <param name="listeningProfile">
        /// An object that is used to add listeners.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        protected override void AddListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            try
            {
                // Add event listeners.
                listeningProfile.AddHeartbeatListener();
                listeningProfile.AddEventListener<ApplicationStartedEvent, ApplicationStartedEventMessage>();
                listeningProfile.AddEventListener<ApplicationStoppedEvent, ApplicationStoppedEventMessage>();
                listeningProfile.AddEventListener<ExceptionRaisedEvent, ExceptionRaisedEventMessage>();

                // Add request listeners.
                listeningProfile.AddRequestListener<PingRequestMessage, PingResponseMessage>();
            }
            finally
            {
                base.AddListeners(listeningProfile, applicationConfiguration, commandLineArguments);
            }
        }

        /// <summary>
        /// Builds the application configuration for the service.
        /// </summary>
        /// <param name="configurationBuilder">
        /// An object that is used to build the configuration.
        /// </param>
        protected override void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            try
            {
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            }
            finally
            {
                base.BuildConfiguration(configurationBuilder);
            }
        }

        /// <summary>
        /// Configures the service to transmit heartbeat messages.
        /// </summary>
        /// <param name="heartbeatSchedule">
        /// An object that defines how the service transmits heartbeat messages.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        protected override void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            try
            {
                heartbeatSchedule.AddItem(3, MessagingEntityType.Topic, "ThreeSecondHeartbeat");
            }
            finally
            {
                base.ConfigureHeartbeat(heartbeatSchedule, applicationConfiguration, commandLineArguments);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ExampleMessagingServiceExecutor" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// When overridden by a derived class, gets a copyright notice which is written to the console at the start of service
        /// execution.
        /// </summary>
        protected override sealed String CopyrightNotice => "Copyright (c) RapidField LLC. All rights reserved.";

        /// <summary>
        /// When overridden by a derived class, gets a product name associated with the service which is written to the console at
        /// the start of service execution.
        /// </summary>
        protected override sealed String ProductName => "Solid Instruments";
    }
}