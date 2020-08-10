// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Prepares for and performs execution of a messaging service that publishes periodic <see cref="HeartbeatMessage" /> instances
    /// and responds to <see cref="PingRequestMessage" /> instances.
    /// </summary>
    /// <remarks>
    /// This service executor can be used to facilitate signaling for repeating and/or scheduled events. It can also be used as a
    /// facility for other services to test message transport availability and latency.
    /// </remarks>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TDependencyConfigurator">
    /// The type of the object that configures the dependency container.
    /// </typeparam>
    /// <typeparam name="TDependencyEngine">
    /// The type of the dependency engine that is produced by the dependency package.
    /// </typeparam>
    public abstract class BeaconServiceExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine> : MessagingServiceExecutor<TDependencyPackage, TDependencyConfigurator, TDependencyEngine>
        where TDependencyPackage : class, IDependencyPackage<TDependencyConfigurator, TDependencyEngine>, new()
        where TDependencyConfigurator : class, new()
        where TDependencyEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        protected BeaconServiceExecutor()
            : this(DefaultServiceName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="publishesFrequencyAHeartbeats">
        /// A value indicating whether or not the service publishes frequency "A" heartbeat messages every thirteen (13) seconds
        /// [0.22 minutes / 6,646 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyBHeartbeats">
        /// A value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds [1.48 minutes
        /// / 0.02 hours / 970 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        protected BeaconServiceExecutor(Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats)
            : this(publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats, DefaultPublishesFrequencyCHeartbeatsValue, DefaultPublishesFrequencyDHeartbeatsValue, DefaultPublishesFrequencyEHeartbeatsValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="publishesFrequencyAHeartbeats">
        /// A value indicating whether or not the service publishes frequency "A" heartbeat messages every thirteen (13) seconds
        /// [0.22 minutes / 6,646 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyBHeartbeats">
        /// A value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds [1.48 minutes
        /// / 0.02 hours / 970 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyCHeartbeats">
        /// A value indicating whether or not the service publishes frequency "C" heartbeat messages every 233 seconds [3.88 minutes
        /// / 0.06 hours / 370 times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        /// <param name="publishesFrequencyDHeartbeats">
        /// A value indicating whether or not the service publishes frequency "D" heartbeat messages every 1,597 seconds [26.62
        /// minutes / 0.44 hours / 54 times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        /// <param name="publishesFrequencyEHeartbeats">
        /// A value indicating whether or not the service publishes frequency "E" heartbeat messages every 28,657 seconds [477.62
        /// minutes / 7.96 hours / three (3) times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        protected BeaconServiceExecutor(Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats, Boolean publishesFrequencyCHeartbeats, Boolean publishesFrequencyDHeartbeats, Boolean publishesFrequencyEHeartbeats)
            : this(DefaultServiceName, publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats, publishesFrequencyCHeartbeats, publishesFrequencyDHeartbeats, publishesFrequencyEHeartbeats)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service. The default value is "Beacon Service".
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected BeaconServiceExecutor(String serviceName)
            : this(serviceName, DefaultPublishesFrequencyAHeartbeatsValue, DefaultPublishesFrequencyBHeartbeatsValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service. The default value is "Beacon Service".
        /// </param>
        /// <param name="publishesFrequencyAHeartbeats">
        /// A value indicating whether or not the service publishes frequency "A" heartbeat messages every thirteen (13) seconds
        /// [0.22 minutes / 6,646 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyBHeartbeats">
        /// A value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds [1.48 minutes
        /// / 0.02 hours / 970 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected BeaconServiceExecutor(String serviceName, Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats)
            : this(serviceName, publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats, DefaultPublishesFrequencyCHeartbeatsValue, DefaultPublishesFrequencyDHeartbeatsValue, DefaultPublishesFrequencyEHeartbeatsValue)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> class.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service. The default value is "Beacon Service".
        /// </param>
        /// <param name="publishesFrequencyAHeartbeats">
        /// A value indicating whether or not the service publishes frequency "A" heartbeat messages every thirteen (13) seconds
        /// [0.22 minutes / 6,646 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyBHeartbeats">
        /// A value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds [1.48 minutes
        /// / 0.02 hours / 970 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyCHeartbeats">
        /// A value indicating whether or not the service publishes frequency "C" heartbeat messages every 233 seconds [3.88 minutes
        /// / 0.06 hours / 370 times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        /// <param name="publishesFrequencyDHeartbeats">
        /// A value indicating whether or not the service publishes frequency "D" heartbeat messages every 1,597 seconds [26.62
        /// minutes / 0.44 hours / 54 times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        /// <param name="publishesFrequencyEHeartbeats">
        /// A value indicating whether or not the service publishes frequency "E" heartbeat messages every 28,657 seconds [477.62
        /// minutes / 7.96 hours / three (3) times per calendar day]. The default value is <see langword="true" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="serviceName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is <see langword="null" />.
        /// </exception>
        protected BeaconServiceExecutor(String serviceName, Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats, Boolean publishesFrequencyCHeartbeats, Boolean publishesFrequencyDHeartbeats, Boolean publishesFrequencyEHeartbeats)
            : base(serviceName)
        {
            PublishesFrequencyAHeartbeats = publishesFrequencyAHeartbeats;
            PublishesFrequencyBHeartbeats = publishesFrequencyBHeartbeats;
            PublishesFrequencyCHeartbeats = publishesFrequencyCHeartbeats;
            PublishesFrequencyDHeartbeats = publishesFrequencyDHeartbeats;
            PublishesFrequencyEHeartbeats = publishesFrequencyEHeartbeats;
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
        protected sealed override void AddListeners(IMessageListeningProfile listeningProfile, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            try
            {
                listeningProfile.AddPingRequestListener();
            }
            finally
            {
                base.AddListeners(listeningProfile, applicationConfiguration, commandLineArguments);
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
        protected sealed override void ConfigureHeartbeat(HeartbeatSchedule heartbeatSchedule, IConfiguration applicationConfiguration, String[] commandLineArguments)
        {
            try
            {
                if (PublishesFrequencyAHeartbeats)
                {
                    heartbeatSchedule.AddItem(HeartbeatMessage.FrequencyAIntervalInSeconds, HeartbeatMessage.DefaultEntityType, HeartbeatMessage.FrequencyALabel);
                }

                if (PublishesFrequencyBHeartbeats)
                {
                    heartbeatSchedule.AddItem(HeartbeatMessage.FrequencyBIntervalInSeconds, HeartbeatMessage.DefaultEntityType, HeartbeatMessage.FrequencyBLabel);
                }

                if (PublishesFrequencyCHeartbeats)
                {
                    heartbeatSchedule.AddItem(HeartbeatMessage.FrequencyCIntervalInSeconds, HeartbeatMessage.DefaultEntityType, HeartbeatMessage.FrequencyCLabel);
                }

                if (PublishesFrequencyDHeartbeats)
                {
                    heartbeatSchedule.AddItem(HeartbeatMessage.FrequencyDIntervalInSeconds, HeartbeatMessage.DefaultEntityType, HeartbeatMessage.FrequencyDLabel);
                }

                if (PublishesFrequencyEHeartbeats)
                {
                    heartbeatSchedule.AddItem(HeartbeatMessage.FrequencyEIntervalInSeconds, HeartbeatMessage.DefaultEntityType, HeartbeatMessage.FrequencyELabel);
                }
            }
            finally
            {
                base.ConfigureHeartbeat(heartbeatSchedule, applicationConfiguration, commandLineArguments);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "A" heartbeat messages every
        /// thirteen (13) seconds [0.22 minutes / 6,646 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesFrequencyAHeartbeatsValue = false;

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "B" heartbeat messages every 89
        /// seconds [1.48 minutes / 0.02 hours / 970 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesFrequencyBHeartbeatsValue = false;

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "C" heartbeat messages every 233
        /// seconds [3.88 minutes / 0.06 hours / 370 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesFrequencyCHeartbeatsValue = true;

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "D" heartbeat messages every 1,597
        /// seconds [26.62 minutes / 0.44 hours / 54 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesFrequencyDHeartbeatsValue = true;

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "E" heartbeat messages every 28,657
        /// seconds [477.62 minutes / 7.96 hours / three (3) times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Boolean DefaultPublishesFrequencyEHeartbeatsValue = true;

        /// <summary>
        /// Represents a default value name for the service.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String DefaultServiceName = "Beacon Service";

        /// <summary>
        /// Represents a default value indicating whether or not the service publishes frequency "A" heartbeat messages every
        /// thirteen (13) seconds [0.22 minutes / 6,646 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesFrequencyAHeartbeats;

        /// <summary>
        /// Represents a value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds
        /// [1.48 minutes / 0.02 hours / 970 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesFrequencyBHeartbeats;

        /// <summary>
        /// Represents a value indicating whether or not the service publishes frequency "C" heartbeat messages every 233 seconds
        /// [3.88 minutes / 0.06 hours / 370 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesFrequencyCHeartbeats;

        /// <summary>
        /// Represents a value indicating whether or not the service publishes frequency "D" heartbeat messages every 1,597 seconds
        /// [26.62 minutes / 0.44 hours / 54 times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesFrequencyDHeartbeats;

        /// <summary>
        /// Represents a value indicating whether or not the service publishes frequency "E" heartbeat messages every 28,657 seconds
        /// [477.62 minutes / 7.96 hours / three (3) times per calendar day].
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean PublishesFrequencyEHeartbeats;
    }
}