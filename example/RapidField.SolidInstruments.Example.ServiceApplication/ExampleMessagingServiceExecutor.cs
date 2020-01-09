// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Service;
using System;
using System.IO;
using System.Threading;

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
        /// Adds message subscriptions to the service.
        /// </summary>
        /// <param name="subscriptionProfile">
        /// An object that is used to add subscriptions.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        protected override void AddSubscriptions(IMessageSubscriptionProfile subscriptionProfile, IConfiguration applicationConfiguration)
        {
            try
            {
                // Add queue subscribers.
                subscriptionProfile.AddQueueSubscriber<ApplicationStartedEventMessage>();
                subscriptionProfile.AddQueueSubscriber<ApplicationStoppedEventMessage>();
                subscriptionProfile.AddQueueSubscriber<ExceptionRaisedEventMessage>();

                // Add topic subscribers.
                subscriptionProfile.AddTopicSubscriber<HeartbeatMessage>();

                // Add request subscribers.
                subscriptionProfile.AddRequestSubscriber<PingRequestMessage, PingResponseMessage>();
            }
            finally
            {
                base.AddSubscriptions(subscriptionProfile, applicationConfiguration);
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
        /// Configures the service to publish heartbeat messages.
        /// </summary>
        /// <param name="heartbeatSchedule">
        /// An object that defines how the service publishes heartbeat messages.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        protected override void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration)
        {
            try
            {
                heartbeatSchedule.AddItem(2, MessagingEntityType.Topic, "TwoSecondHeartbeat");
            }
            finally
            {
                base.ConfigureHeartbeat(heartbeatSchedule, applicationConfiguration);
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
        /// Performs startup operations for the service.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        /// <param name="executionLifetime">
        /// An object that provides control over execution lifetime.
        /// </param>
        protected override void OnExecutionStarting(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, ServiceExecutionLifetime executionLifetime)
        {
            Console.WriteLine($"Solid Instruments | {ServiceName}");
            Console.WriteLine($"Copyright (c) RapidField LLC. All rights reserved.{Environment.NewLine}");

            try
            {
                Console.CancelKeyPress += (sender, eventArguments) =>
                {
                    executionLifetime.End();
                    eventArguments.Cancel = true;
                };

                var mediator = dependencyScope.Resolve<ICommandMediator>();
                var applicationStartedEvent = new ApplicationStartedEvent(ServiceName);
                var applicationStartedEventMessage = new ApplicationStartedEventMessage(applicationStartedEvent);
                mediator.Process(applicationStartedEventMessage);
                Thread.Sleep(1600);
            }
            finally
            {
                base.OnExecutionStarting(dependencyScope, applicationConfiguration, executionLifetime);
            }
        }

        /// <summary>
        /// Performs shutdown operations for the service.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        protected override void OnExecutionStopping(IDependencyScope dependencyScope, IConfiguration applicationConfiguration)
        {
            try
            {
                var mediator = dependencyScope.Resolve<ICommandMediator>();
                var applicationStoppedEvent = new ApplicationStoppedEvent(ServiceName);
                var applicationStoppedEventMessage = new ApplicationStoppedEventMessage(applicationStoppedEvent);
                mediator.Process(applicationStoppedEventMessage);
                Thread.Sleep(3200);
            }
            finally
            {
                base.OnExecutionStopping(dependencyScope, applicationConfiguration);
            }
        }
    }
}