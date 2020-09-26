// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using System;

namespace RapidField.SolidInstruments.Web.Autofac
{
    /// <summary>
    /// Prepares for and performs execution of a web application using Autofac IoC tooling.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TStartup">
    /// The type of the user-defined startup class that configures the web host.
    /// </typeparam>
    public abstract class AutofacWebExecutor<TDependencyPackage, TStartup> : WebExecutor<TDependencyPackage, ContainerBuilder, AutofacDependencyEngine, AutofacServiceProviderFactory<TDependencyPackage>, TStartup>
        where TDependencyPackage : AutofacDependencyPackage, new()
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacWebExecutor{TDependencyPackage, TStartup}" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the web application.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        protected AutofacWebExecutor(String applicationName)
            : base(applicationName)
        {
            return;
        }

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        protected sealed override AutofacServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new AutofacServiceProviderFactory<TDependencyPackage>(applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AutofacWebExecutor{TDependencyPackage, TStartup}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Prepares for and performs execution of a web application using Autofac IoC tooling.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    public abstract class AutofacWebExecutor<TDependencyPackage> : WebExecutor<TDependencyPackage, ContainerBuilder, AutofacDependencyEngine, AutofacServiceProviderFactory<TDependencyPackage>>
        where TDependencyPackage : AutofacDependencyPackage, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacWebExecutor{TDependencyPackage}" /> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the web application.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="applicationName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationName" /> is <see langword="null" />.
        /// </exception>
        protected AutofacWebExecutor(String applicationName)
            : base(applicationName)
        {
            return;
        }

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        protected sealed override AutofacServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new AutofacServiceProviderFactory<TDependencyPackage>(applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AutofacWebExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}