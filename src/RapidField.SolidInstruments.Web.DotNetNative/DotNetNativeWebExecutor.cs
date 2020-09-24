// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Web.DotNetNative
{
    /// <summary>
    /// Prepares for and performs execution of a web application using native .NET IoC tooling.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    /// <typeparam name="TStartup">
    /// The type of the user-defined startup class that configures the web host.
    /// </typeparam>
    public abstract class DotNetNativeWebExecutor<TDependencyPackage, TStartup> : WebExecutor<TDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine, DotNetNativeServiceProviderFactory<TDependencyPackage>, TStartup>
        where TDependencyPackage : DotNetNativeDependencyPackage, new()
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeWebExecutor{TDependencyPackage, TStartup}" /> class.
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
        protected DotNetNativeWebExecutor(String applicationName)
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
        protected override DotNetNativeServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new DotNetNativeServiceProviderFactory<TDependencyPackage>(applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeWebExecutor{TDependencyPackage, TStartup}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Prepares for and performs execution of a web application using native .NET IoC tooling.
    /// </summary>
    /// <typeparam name="TDependencyPackage">
    /// The type of the package that configures the dependency engine.
    /// </typeparam>
    public abstract class DotNetNativeWebExecutor<TDependencyPackage> : WebExecutor<TDependencyPackage, ServiceCollection, DotNetNativeDependencyEngine, DotNetNativeServiceProviderFactory<TDependencyPackage>>
        where TDependencyPackage : DotNetNativeDependencyPackage, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeWebExecutor{TDependencyPackage}" /> class.
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
        protected DotNetNativeWebExecutor(String applicationName)
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
        protected override DotNetNativeServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new DotNetNativeServiceProviderFactory<TDependencyPackage>(applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeWebExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}