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
using System.Threading;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl.Service
{
    /// <summary>
    /// Prepares for and performs execution of the <see cref="AccessControl" /> domain service.
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
                HydrateNamedDomainEntities(dependencyScope);
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
            listeningProfile.AddCommandListener<Commands.ModelState.User.CreateDomainModelCommand, Messages.Command.ModelState.User.CreateDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.User.DeleteDomainModelCommand, Messages.Command.ModelState.User.DeleteDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.User.UpdateDomainModelCommand, Messages.Command.ModelState.User.UpdateDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRole.CreateDomainModelCommand, Messages.Command.ModelState.UserRole.CreateDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRole.DeleteDomainModelCommand, Messages.Command.ModelState.UserRole.DeleteDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRole.UpdateDomainModelCommand, Messages.Command.ModelState.UserRole.UpdateDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRoleAssignment.CreateDomainModelCommand, Messages.Command.ModelState.UserRoleAssignment.CreateDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRoleAssignment.DeleteDomainModelCommand, Messages.Command.ModelState.UserRoleAssignment.DeleteDomainModelCommandMessage>();
            listeningProfile.AddCommandListener<Commands.ModelState.UserRoleAssignment.UpdateDomainModelCommand, Messages.Command.ModelState.UserRoleAssignment.UpdateDomainModelCommandMessage>();
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
        private static void AddStandardListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments) => listeningProfile.AddHeartbeatListener();

        /// <summary>
        /// Creates or updates all named domain models.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        [DebuggerHidden]
        private static void HydrateNamedDomainEntities(IDependencyScope dependencyScope)
        {
            var mediator = dependencyScope.Resolve<ICommandMediator>();
            HydrateNamedUsers(mediator);
            HydrateNamedUserRoles(mediator);
            HydrateNamedUserRoleAssignments(mediator);
            Console.WriteLine("Database entities created/updated.");
        }

        /// <summary>
        /// Creates or updates all named <see cref="Models.UserRoleAssignment.DomainModel" /> instances.
        /// </summary>
        /// <param name="mediator">
        /// A mediator that is used to issue commands.
        /// </param>
        [DebuggerHidden]
        private static void HydrateNamedUserRoleAssignments(ICommandMediator mediator)
        {
            foreach (var domainModel in Models.UserRoleAssignment.DomainModel.Named.All())
            {
                mediator.Process(new Messages.Command.ModelState.UserRoleAssignment.CreateDomainModelCommandMessage(new Commands.ModelState.UserRoleAssignment.CreateDomainModelCommand(domainModel)));
            }

            Pause();
        }

        /// <summary>
        /// Creates or updates all named <see cref="Models.UserRole.DomainModel" /> instances.
        /// </summary>
        /// <param name="mediator">
        /// A mediator that is used to issue commands.
        /// </param>
        [DebuggerHidden]
        private static void HydrateNamedUserRoles(ICommandMediator mediator)
        {
            foreach (var domainModel in Models.UserRole.DomainModel.Named.All())
            {
                mediator.Process(new Messages.Command.ModelState.UserRole.CreateDomainModelCommandMessage(new Commands.ModelState.UserRole.CreateDomainModelCommand(domainModel)));
            }

            Pause();
        }

        /// <summary>
        /// Creates or updates all named <see cref="Models.User.DomainModel" /> instances.
        /// </summary>
        /// <param name="mediator">
        /// A mediator that is used to issue commands.
        /// </param>
        [DebuggerHidden]
        private static void HydrateNamedUsers(ICommandMediator mediator)
        {
            foreach (var domainModel in Models.User.DomainModel.Named.All())
            {
                mediator.Process(new Messages.Command.ModelState.User.CreateDomainModelCommandMessage(new Commands.ModelState.User.CreateDomainModelCommand(domainModel)));
            }

            Pause();
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