// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for an application.
    /// </summary>
    public abstract class AutofacDependencyPackage : DependencyPackage<ContainerBuilder, AutofacDependencyEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyPackage" /> class.
        /// </summary>
        protected AutofacDependencyPackage()
        {
            return;
        }

        /// <summary>
        /// Creates a new dependency engine that is configured by the current <see cref="AutofacDependencyPackage" />.
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
        protected sealed override AutofacDependencyEngine CreateEngine(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors, IDependencyPackage<ContainerBuilder> package) => new AutofacDependencyEngine(applicationConfiguration, package, serviceDescriptors);
    }
}