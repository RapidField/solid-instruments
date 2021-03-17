// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.EventAuthoring;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Prepares for and performs execution of a messaging service.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TDependencyConfigurator">
    /// The type of the object that configures the dependency container.
    /// </typeparam>
    /// <typeparam name="TDependencyEngine">
    /// The type of the dependency engine that is produced by the dependency package.
    /// </typeparam>
    public abstract class MessagingServiceExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine> : ServiceExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine>
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
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
        protected MessagingServiceExecutor(String serviceName)
            : this(serviceName, DefaultRunsContinuouslyValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service.
        /// </param>
        /// <param name="runsContinuously">
        /// A value indicating whether or not the service should schedule heartbeat messages and stay running indefinitely. The
        /// default value is <see langword="true" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected MessagingServiceExecutor(String serviceName, Boolean runsContinuously)
            : this(serviceName, runsContinuously, DefaultPublishesStartAndStopEventsValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service.
        /// </param>
        /// <param name="runsContinuously">
        /// A value indicating whether or not the service should schedule heartbeat messages and stay running indefinitely. The
        /// default value is <see langword="true" />.
        /// </param>
        /// <param name="publishesStartAndStopEvents">
        /// A value indicating whether or not the service should publish event messages when the application starts and stops. The
        /// default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected MessagingServiceExecutor(String serviceName, Boolean runsContinuously, Boolean publishesStartAndStopEvents)
            : base(serviceName)
        {
            LazyHeartbeatSchedule = new(CreateHeartbeatSchedule, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyHeartbeatTimers = new(() => new List<Timer>(), LazyThreadSafetyMode.ExecutionAndPublication);
            LazyListeningProfile = new(CreateListeningProfile, LazyThreadSafetyMode.ExecutionAndPublication);
            PublishesStartAndStopEvents = publishesStartAndStopEvents;
            RunsContinuously = runsContinuously;
        }

        /// <summary>
        /// Unlocks waiting threads and ends execution of the service.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal void EndExecution()
        {
            if (ExecutionLifetime is null)
            {
                return;
            }
            else if (ExecutionLifetime.IsAlive)
            {
                ExecutionLifetime.End();
            }
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
        protected virtual void AddListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            return;
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
        protected virtual void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Begins execution of the service and performs the service operations.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the service application.
        /// </param>
        /// <param name="commandLineArguments">
        /// Command line arguments that are provided at runtime, if any.
        /// </param>
        /// <param name="executionLifetime">
        /// An object that provides control over execution lifetime.
        /// </param>
        protected sealed override void Execute(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, String[] commandLineArguments, IServiceExecutionLifetime executionLifetime)
        {
            try
            {
                AddListeners(ListeningProfile, ApplicationConfiguration, commandLineArguments);
                Thread.Sleep(ExecutionStartDelay);

                try
                {
                    using (var childScope = dependencyScope.CreateChildScope())
                    {
                        OnExecutionStarting(childScope, applicationConfiguration, executionLifetime);

                        if (PublishesStartAndStopEvents)
                        {
                            PublishApplicationStartedEventMessage(childScope);
                        }
                    }

                    try
                    {
                        if (RunsContinuously)
                        {
                            if (SupressStandardConsoleOutput is false)
                            {
                                Console.WriteLine("The service is running in continuous mode.");
                            }

                            Console.CancelKeyPress += (sender, eventArguments) =>
                            {
                                executionLifetime.End();
                                eventArguments.Cancel = true;
                            };

                            try
                            {
                                StartHeartbeats();
                                executionLifetime.KeepAlive();
                            }
                            finally
                            {
                                StopHeartbeats();
                            }
                        }
                    }
                    finally
                    {
                        if (PublishesStartAndStopEvents)
                        {
                            using (var childScope = dependencyScope.CreateChildScope())
                            {
                                PublishApplicationStoppedEventMessage(childScope);
                            }
                        }
                    }
                }
                finally
                {
                    using (var childScope = dependencyScope.CreateChildScope())
                    {
                        OnExecutionStopping(childScope, applicationConfiguration);
                    }
                }
            }
            finally
            {
                base.Execute(dependencyScope, applicationConfiguration, commandLineArguments, executionLifetime);
                Thread.Sleep(ExecutionStopDelay);
            }
        }

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
        protected virtual void OnExecutionStarting(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, IServiceExecutionLifetime executionLifetime)
        {
            return;
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
        protected virtual void OnExecutionStopping(IDependencyScope dependencyScope, IConfiguration applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Creates the heartbeat message schedule for the service.
        /// </summary>
        /// <returns>
        /// The heartbeat message schedule for the service.
        /// </returns>
        [DebuggerHidden]
        private HeartbeatSchedule CreateHeartbeatSchedule()
        {
            var heartbeatSchedule = new HeartbeatSchedule();
            ConfigureHeartbeat(heartbeatSchedule, ApplicationConfiguration, CommandLineArguments);
            return heartbeatSchedule;
        }

        /// <summary>
        /// Creates the listening profile for the service.
        /// </summary>
        /// <returns>
        /// The listening profile for the service.
        /// </returns>
        [DebuggerHidden]
        private IMessageListeningProfile CreateListeningProfile()
        {
            var dependencyScope = CreateDependencyScope();
            ReferenceManager.AddObject(dependencyScope);
            return new MessageListeningProfile(dependencyScope);
        }

        /// <summary>
        /// Publishes an event to notify the system that the application has started.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while publishing the event message.
        /// </exception>
        [DebuggerHidden]
        private void PublishApplicationStartedEventMessage(IDependencyScope dependencyScope)
        {
            var mediator = dependencyScope.Resolve<ICommandMediator>();
            var applicationStartedEvent = new ApplicationStartedEvent(ServiceName);
            var applicationStartedEventMessage = new ApplicationStartedEventMessage(applicationStartedEvent);
            mediator.Process(applicationStartedEventMessage);
        }

        /// <summary>
        /// Publishes an event to notify the system that the application has stopped.
        /// </summary>
        /// <param name="dependencyScope">
        /// A scope that is used to resolve service dependencies.
        /// </param>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while publishing the event message.
        /// </exception>
        [DebuggerHidden]
        private void PublishApplicationStoppedEventMessage(IDependencyScope dependencyScope)
        {
            var mediator = dependencyScope.Resolve<ICommandMediator>();
            var applicationStoppedEvent = new ApplicationStoppedEvent(ServiceName);
            var applicationStoppedEventMessage = new ApplicationStoppedEventMessage(applicationStoppedEvent);
            mediator.Process(applicationStoppedEventMessage);
        }

        /// <summary>
        /// Creates a new timer that transmits messages according to the supplied specifications.
        /// </summary>
        /// <param name="heartbeatScheduleItem">
        /// Specifications for the heartbeat.
        /// </param>
        [DebuggerHidden]
        private void ScheduleHeartbeat(IHeartbeatScheduleItem heartbeatScheduleItem)
        {
            var timerPeriod = TimeSpan.FromSeconds(heartbeatScheduleItem.IntervalInSeconds);
            var timerCallback = new TimerCallback((state) => TransmitHeartbeatMessageAsync(state as IHeartbeatScheduleItem));
            var timer = new Timer(timerCallback, heartbeatScheduleItem, timerPeriod, timerPeriod);
            HeartbeatTimers.Add(timer);
            ReferenceManager.AddObject(timer);
        }

        /// <summary>
        /// Schedules a heartbeat for every item in <see cref="HeartbeatSchedule" />.
        /// </summary>
        [DebuggerHidden]
        private void StartHeartbeats()
        {
            foreach (var heartbeatScheduleItem in HeartbeatSchedule)
            {
                ScheduleHeartbeat(heartbeatScheduleItem);
            }
        }

        /// <summary>
        /// Stops all active heartbeat timers.
        /// </summary>
        [DebuggerHidden]
        private void StopHeartbeats()
        {
            foreach (var timer in HeartbeatTimers)
            {
                timer.Change(0, Timeout.Infinite);
                timer.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously transmits a heartbeat message using the specified schedule item.
        /// </summary>
        /// <param name="scheduleItem">
        /// A schedule item that defines characteristics of the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="MessageTransmissionException">
        /// An exception was raised while attempting to transmit the heartbeat message.
        /// </exception>
        [DebuggerHidden]
        private Task TransmitHeartbeatMessageAsync(IHeartbeatScheduleItem scheduleItem)
        {
            try
            {
                var dependencyScope = CreateDependencyScope();

                try
                {
                    var messageTransmittingFacade = dependencyScope.Resolve<IMessageTransmittingFacade>();

                    return scheduleItem.TransmitHeartbeatMessageAsync(messageTransmittingFacade).ContinueWith(transmitHeartbeatMessageTask =>
                    {
                        dependencyScope.Dispose();
                        transmitHeartbeatMessageTask.Dispose();
                    });
                }
                catch (Exception exception)
                {
                    dependencyScope.Dispose();
                    return Task.FromException(exception);
                }
            }
            catch (MessageTransmissionException exception)
            {
                return Task.FromException(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException(new MessageTransmissionException(typeof(HeartbeatMessage), exception));
            }
        }

        /// <summary>
        /// Gets the heartbeat message schedule for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HeartbeatSchedule HeartbeatSchedule => LazyHeartbeatSchedule.Value;

        /// <summary>
        /// Gets a collection of timers that control heartbeat transmission.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<Timer> HeartbeatTimers => LazyHeartbeatTimers.Value;

        /// <summary>
        /// Gets the listening profile for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IMessageListeningProfile ListeningProfile => LazyListeningProfile.Value;

        /// <summary>
        /// Represents a default value indicating whether or not the service should publish event messages when the application
        /// starts and stops.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesStartAndStopEventsValue = false;

        /// <summary>
        /// Represents a default value indicating whether or not the service should schedule heartbeat messages and stay running
        /// indefinitely. The
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultRunsContinuouslyValue = true;

        /// <summary>
        /// Represents the length of time to delay service execution after adding subscriptions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ExecutionStartDelay = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Represents the length of time to wait after execution has finished.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan ExecutionStopDelay = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Represents the lazily-initialized heartbeat message schedule for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<HeartbeatSchedule> LazyHeartbeatSchedule;

        /// <summary>
        /// Represents a lazily-initialized collection of timers that control heartbeat transmission.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IList<Timer>> LazyHeartbeatTimers;

        /// <summary>
        /// Represents the lazily-initialized listening profile for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IMessageListeningProfile> LazyListeningProfile;

        /// <summary>
        /// Represents value indicating whether or not the service should publish event messages when the application starts and
        /// stops.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesStartAndStopEvents;

        /// <summary>
        /// Represents a value indicating whether or not the service should schedule heartbeat messages and stay running
        /// indefinitely.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean RunsContinuously;
    }
}