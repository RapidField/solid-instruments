// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.Messaging.DotNetNative.Service;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Service;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RapidField.SolidInstruments.Example.Domain.Identity.Service
{
    /// <summary>
    /// Prepares for and performs execution of the <see cref="Identity" /> domain service.
    /// </summary>
    public sealed class ApplicationServiceExecutor : DotNetNativeMessagingServiceExecutor<ApplicationDependencyPackage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationServiceExecutor" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        public ApplicationServiceExecutor(String serviceName)
            : base(serviceName)
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
                AddStandardListeners(listeningProfile, applicationConfiguration, commandLineArguments);
                AddCommandListeners(listeningProfile, applicationConfiguration, commandLineArguments);
                AddEventListeners(listeningProfile, applicationConfiguration, commandLineArguments);
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
        /// Releases all resources consumed by the current <see cref="ApplicationServiceExecutor" />.
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
                TestServiceBusConnectivity(dependencyScope);
                MigrateDatabaseSchema(dependencyScope);
            }
            finally
            {
                base.OnExecutionStarting(dependencyScope, applicationConfiguration, executionLifetime);
            }
        }

        /// <summary>
        /// Adds command message listeners to the service.
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
        [DebuggerHidden]
        private static void AddCommandListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            return;
        }

        /// <summary>
        /// Adds event message listeners to the service.
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
        [DebuggerHidden]
        private static void AddEventListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            listeningProfile.AddEventListener<Events.ModelState.User.DomainModelCreatedEvent, Messages.Event.ModelState.User.DomainModelCreatedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.User.DomainModelDeletedEvent, Messages.Event.ModelState.User.DomainModelDeletedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.User.DomainModelUpdatedEvent, Messages.Event.ModelState.User.DomainModelUpdatedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRole.DomainModelCreatedEvent, Messages.Event.ModelState.UserRole.DomainModelCreatedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRole.DomainModelDeletedEvent, Messages.Event.ModelState.UserRole.DomainModelDeletedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRole.DomainModelUpdatedEvent, Messages.Event.ModelState.UserRole.DomainModelUpdatedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRoleAssignment.DomainModelCreatedEvent, Messages.Event.ModelState.UserRoleAssignment.DomainModelCreatedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRoleAssignment.DomainModelDeletedEvent, Messages.Event.ModelState.UserRoleAssignment.DomainModelDeletedEventMessage>();
            listeningProfile.AddEventListener<Events.ModelState.UserRoleAssignment.DomainModelUpdatedEvent, Messages.Event.ModelState.UserRoleAssignment.DomainModelUpdatedEventMessage>();
        }

        /// <summary>
        /// Adds standard message listeners to the service.
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
        [DebuggerHidden]
        private static void AddStandardListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            return;
        }

        /// <summary>
        /// Creates or updates the database schema.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        [DebuggerHidden]
        private static void MigrateDatabaseSchema(IDependencyScope dependencyScope)
        {
            var databaseContext = dependencyScope.Resolve<DatabaseContext>();
            databaseContext.Database.EnsureCreated();
            Pause();
            Console.WriteLine("Database schema created/updated.");
        }

        /// <summary>
        /// Blocks the current thread for a short period to ensure synchronous execution of startup events.
        /// </summary>
        [DebuggerHidden]
        private static void Pause() => Thread.Sleep(PauseDurationInMilliseconds);

        /// <summary>
        /// Raises an exception if a service bus connection is unavailable.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <exception cref="ServiceExectuionException">
        /// The service was unable to verify service bus connectivity.
        /// </exception>
        [DebuggerHidden]
        private static void TestServiceBusConnectivity(IDependencyScope dependencyScope)
        {
            try
            {
                var mediator = dependencyScope.Resolve<ICommandMediator>();
                var pingRequestCorrelationIdentifier = Guid.NewGuid();
                var pingResponseMessage = mediator.Process<PingResponseMessage>(new PingRequestMessage(pingRequestCorrelationIdentifier));

                if (pingResponseMessage is null || pingResponseMessage.CorrelationIdentifier != pingRequestCorrelationIdentifier)
                {
                    throw new ServiceExectuionException("The service was unable to verify service bus connectivity.");
                }
            }
            catch (CommandHandlingException exception)
            {
                throw new ServiceExectuionException("The service was unable to verify service bus connectivity. See inner exception.", exception);
            }

            Console.WriteLine("Service bus connectivity verified.");
        }

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

        /// <summary>
        /// Represents the number of milliseconds to wait when invoking <see cref="Pause" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 PauseDurationInMilliseconds = 1597;
    }
}