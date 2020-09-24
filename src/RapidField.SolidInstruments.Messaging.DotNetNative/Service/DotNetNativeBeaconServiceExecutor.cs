// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging.Service;
using System;

namespace RapidField.SolidInstruments.Messaging.DotNetNative.Service
{
    /// <summary>
    /// Prepares for and performs execution of a messaging service that publishes periodic <see cref="HeartbeatMessage" /> instances
    /// and responds to <see cref="PingRequestMessage" /> instances using a native .NET dependency package.
    /// </summary>
    /// <remarks>
    /// This service executor can be used to facilitate signaling for repeating and/or scheduled events. It can also be used as a
    /// facility for other services to test message transport availability and latency.
    /// </remarks>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    public abstract class DotNetNativeBeaconServiceExecutor<TDependencyPackage> : BeaconServiceExecutor<TDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine>
        where TDependencyPackage : class, IDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
        /// </summary>
        protected DotNetNativeBeaconServiceExecutor()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
        /// </summary>
        /// <param name="publishesFrequencyAHeartbeats">
        /// A value indicating whether or not the service publishes frequency "A" heartbeat messages every thirteen (13) seconds
        /// [0.22 minutes / 6,646 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        /// <param name="publishesFrequencyBHeartbeats">
        /// A value indicating whether or not the service publishes frequency "B" heartbeat messages every 89 seconds [1.48 minutes
        /// / 0.02 hours / 970 times per calendar day]. The default value is <see langword="false" />.
        /// </param>
        protected DotNetNativeBeaconServiceExecutor(Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats)
            : base(publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeBeaconServiceExecutor(Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats, Boolean publishesFrequencyCHeartbeats, Boolean publishesFrequencyDHeartbeats, Boolean publishesFrequencyEHeartbeats)
            : base(publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats, publishesFrequencyCHeartbeats, publishesFrequencyDHeartbeats, publishesFrequencyEHeartbeats)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeBeaconServiceExecutor(String serviceName)
            : base(serviceName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeBeaconServiceExecutor(String serviceName, Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats)
            : base(serviceName, publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeBeaconServiceExecutor(String serviceName, Boolean publishesFrequencyAHeartbeats, Boolean publishesFrequencyBHeartbeats, Boolean publishesFrequencyCHeartbeats, Boolean publishesFrequencyDHeartbeats, Boolean publishesFrequencyEHeartbeats)
            : base(serviceName, publishesFrequencyAHeartbeats, publishesFrequencyBHeartbeats, publishesFrequencyCHeartbeats, publishesFrequencyDHeartbeats, publishesFrequencyEHeartbeats)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeBeaconServiceExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}