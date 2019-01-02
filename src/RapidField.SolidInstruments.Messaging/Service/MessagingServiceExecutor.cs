// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
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
            : base(serviceName)
        {
            LazyHeartbeatSchedule = new Lazy<HeartbeatSchedule>(CreateHeartbeatSchedule, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyHeartbeatTimers = new Lazy<IList<Timer>>(() => new List<Timer>(), LazyThreadSafetyMode.ExecutionAndPublication);
            LazySubscriptionProfile = new Lazy<IMessageSubscriptionProfile>(CreateSubscriptionProfile, LazyThreadSafetyMode.ExecutionAndPublication);
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
        protected virtual void AddSubscriptions(IMessageSubscriptionProfile subscriptionProfile, IConfiguration applicationConfiguration)
        {
            return;
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
        protected virtual void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
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
        /// <param name="executionLifetime">
        /// An object that provides control over execution lifetime.
        /// </param>
        protected sealed override void Execute(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, ServiceExecutionLifetime executionLifetime)
        {
            try
            {
                using (var childScope = dependencyScope.CreateChildScope())
                {
                    OnExecutionStarting(childScope, applicationConfiguration, executionLifetime);
                }

                try
                {
                    AddSubscriptions(SubscriptionProfile, ApplicationConfiguration);
                    StartHeartbeats();
                    executionLifetime.KeepAlive();
                }
                finally
                {
                    StopHeartbeats();

                    using (var childScope = dependencyScope.CreateChildScope())
                    {
                        OnExecutionStopping(childScope, applicationConfiguration);
                    }
                }
            }
            finally
            {
                base.Execute(dependencyScope, applicationConfiguration, executionLifetime);
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
        protected virtual void OnExecutionStarting(IDependencyScope dependencyScope, IConfiguration applicationConfiguration, ServiceExecutionLifetime executionLifetime)
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
            ConfigureHeartbeat(heartbeatSchedule, ApplicationConfiguration);
            return heartbeatSchedule;
        }

        /// <summary>
        /// Creates the subscription profile for the service.
        /// </summary>
        /// <returns>
        /// The subscription profile for the service.
        /// </returns>
        [DebuggerHidden]
        private IMessageSubscriptionProfile CreateSubscriptionProfile()
        {
            var dependencyScope = CreateDependencyScope();
            ReferenceManager.AddObject(dependencyScope);
            return new MessageSubscriptionProfile(dependencyScope);
        }

        /// <summary>
        /// Asynchronously publishes a heartbeat message using the specified schedule item.
        /// </summary>
        /// <param name="scheduleItem">
        /// A schedule item that defines characteristics of the message.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="MessagePublishingException">
        /// An exception was raised while attempting to publish the heartbeat message.
        /// </exception>
        [DebuggerHidden]
        private async Task PublishHeartbeatMessageAsync(IHeartbeatScheduleItem scheduleItem)
        {
            try
            {
                using (var dependencyScope = CreateDependencyScope())
                {
                    var messagePublishingClient = dependencyScope.Resolve<IMessagePublishingClient>();
                    await scheduleItem.PublishHeartbeatMessageAsync(messagePublishingClient).ConfigureAwait(false);
                }
            }
            catch (MessagePublishingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MessagePublishingException(typeof(HeartbeatMessage), exception);
            }
        }

        /// <summary>
        /// Creates a new timer that publishes messages according to the supplied specifications.
        /// </summary>
        /// <param name="heartbeatScheduleItem">
        /// Specifications for the heartbeat.
        /// </param>
        [DebuggerHidden]
        private void ScheduleHeartbeat(IHeartbeatScheduleItem heartbeatScheduleItem)
        {
            var timerCallback = new TimerCallback((state) => Task.Factory.StartNew(async () => await PublishHeartbeatMessageAsync(state as IHeartbeatScheduleItem).ConfigureAwait(false)));
            var timer = new Timer(timerCallback, heartbeatScheduleItem, TimeSpan.Zero, TimeSpan.FromSeconds(heartbeatScheduleItem.IntervalInSeconds));
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
        /// Gets the heartbeat message schedule for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HeartbeatSchedule HeartbeatSchedule => LazyHeartbeatSchedule.Value;

        /// <summary>
        /// Gets a collection of timers that control heartbeat publishing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<Timer> HeartbeatTimers => LazyHeartbeatTimers.Value;

        /// <summary>
        /// Gets the subscription profile for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IMessageSubscriptionProfile SubscriptionProfile => LazySubscriptionProfile.Value;

        /// <summary>
        /// Represents the lazily-initialized heartbeat message schedule for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<HeartbeatSchedule> LazyHeartbeatSchedule;

        /// <summary>
        /// Represents a lazily-initialized collection of timers that control heartbeat publishing.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IList<Timer>> LazyHeartbeatTimers;

        /// <summary>
        /// Represents the lazily-initialized
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IMessageSubscriptionProfile> LazySubscriptionProfile;
    }
}