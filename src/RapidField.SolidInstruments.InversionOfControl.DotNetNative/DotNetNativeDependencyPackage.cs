// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for an application.
    /// </summary>
    public abstract class DotNetNativeDependencyPackage : DependencyPackage<ServiceCollection, DotNetNativeDependencyEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDependencyPackage" /> class.
        /// </summary>
        protected DotNetNativeDependencyPackage()
        {
            return;
        }

        /// <summary>
        /// Creates a new dependency engine that is configured by the current <see cref="DotNetNativeDependencyPackage" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <param name="package">
        /// The current dependency package.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="package" /> is
        /// <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is <see langword="null" /> .
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override DotNetNativeDependencyEngine CreateEngine(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors, IDependencyPackage<ServiceCollection> package) => new(applicationConfiguration, package, serviceDescriptors);
    }
}