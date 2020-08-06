// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Service;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    /// <summary>
    /// Prepares for and performs execution of the test messaging service.
    /// </summary>
    internal sealed class SimulatedMessagingServiceExecutor : MessagingServiceExecutor<SimulatedDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedMessagingServiceExecutor" /> class.
        /// </summary>
        /// <param name="onStartingAction">
        /// An action that is performed when the service is starting.
        /// </param>
        /// <param name="onStoppingAction">
        /// An action that is performed when the service is stopping.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="onStartingAction" /> is <see langword="null" /> -or- <paramref name="onStoppingAction" /> is
        /// <see langword="null" />.
        /// </exception>
        public SimulatedMessagingServiceExecutor(Action<ICommandMediator> onStartingAction, Action<ICommandMediator> onStoppingAction)
            : base("Simulated Messaging Service", false)
        {
            OnStartingAction = onStartingAction.RejectIf().IsNull(nameof(onStartingAction));
            OnStoppingAction = onStoppingAction.RejectIf().IsNull(nameof(onStoppingAction));
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
        protected override void AddListeners(IMessageListeningProfile subscriptionProfile, IConfiguration applicationConfiguration)
        {
            try
            {
                // Add queue listeners.
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>();
                subscriptionProfile.AddQueueListener<Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>();

                // Add topic listeners.
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>();
                subscriptionProfile.AddTopicListener<Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>();

                // Add request listeners.
                subscriptionProfile.AddRequestListener<Messages.RequestResponse.Ping.RequestMessage, Messages.RequestResponse.Ping.ResponseMessage>();
            }
            finally
            {
                base.AddListeners(subscriptionProfile, applicationConfiguration);
            }
        }

        /// <summary>
        /// Builds the application configuration for the service.
        /// </summary>
        /// <param name="configurationBuilder">
        /// An object that is used to build the configuration.
        /// </param>
        protected override void BuildConfiguration(IConfigurationBuilder configurationBuilder) => base.BuildConfiguration(configurationBuilder);

        /// <summary>
        /// Configures the service to transmit heartbeat messages.
        /// </summary>
        /// <param name="heartbeatSchedule">
        /// An object that defines how the service transmits heartbeat messages.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        protected override void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration) => base.ConfigureHeartbeat(heartbeatSchedule, applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedMessagingServiceExecutor" />.
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
        protected override void OnExecutionStarting(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, IServiceExecutionLifetime executionLifetime)
        {
            try
            {
                var mediator = dependencyScope.Resolve<ICommandMediator>();
                OnStartingAction(mediator);
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
                OnStoppingAction(mediator);
            }
            finally
            {
                base.OnExecutionStopping(dependencyScope, applicationConfiguration);
            }
        }

        /// <summary>
        /// Represents an action that is performed when the service is starting.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<ICommandMediator> OnStartingAction;

        /// <summary>
        /// Represents an action that is performed when the service is stopping.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Action<ICommandMediator> OnStoppingAction;
    }
}