// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using RapidField.SolidInstruments.Messaging.Service;
using System;

namespace RapidField.SolidInstruments.Messaging.Autofac.Service
{
    /// <summary>
    /// Prepares for and performs execution of a messaging service using an Autofac dependency package.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    public abstract class AutofacMessagingServiceExecutor<TDependencyPackage> : MessagingServiceExecutor<TDependencyPackage, ContainerBuilder, AutofacDependencyEngine>
        where TDependencyPackage : class, IDependencyPackage<ContainerBuilder, AutofacDependencyEngine>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessagingServiceExecutor{TDependencyPackage}" /> class.
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
        protected AutofacMessagingServiceExecutor(String serviceName)
            : base(serviceName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessagingServiceExecutor{TDependencyPackage}" /> class.
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
        protected AutofacMessagingServiceExecutor(String serviceName, Boolean runsContinuously)
            : base(serviceName, runsContinuously)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessagingServiceExecutor{TDependencyPackage}" /> class.
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
        protected AutofacMessagingServiceExecutor(String serviceName, Boolean runsContinuously, Boolean publishesStartAndStopEvents)
            : base(serviceName, runsContinuously, publishesStartAndStopEvents)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AutofacMessagingServiceExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}