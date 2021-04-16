// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;
using System.ComponentModel;

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
        /// Configures application dependencies.
        /// </summary>
        /// <param name="services">
        /// An object that configures application dependencies.
        /// </param>
        /// <param name="dependencyPackage">
        /// A package that defines the configured dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void ConfigureServices(IServiceCollection services, TDependencyPackage dependencyPackage, IConfiguration applicationConfiguration) => _ = services.AddDependencyPackage(dependencyPackage, applicationConfiguration);

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override DotNetNativeServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new(applicationConfiguration);

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
        /// Configures application dependencies.
        /// </summary>
        /// <param name="services">
        /// An object that configures application dependencies.
        /// </param>
        /// <param name="dependencyPackage">
        /// A package that defines the configured dependencies.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override void ConfigureServices(IServiceCollection services, TDependencyPackage dependencyPackage, IConfiguration applicationConfiguration) => _ = services.AddDependencyPackage(dependencyPackage, applicationConfiguration);

        /// <summary>
        /// Creates the service provider factory.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the web application.
        /// </param>
        /// <returns>
        /// The service provider factory.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override DotNetNativeServiceProviderFactory<TDependencyPackage> CreateServiceProviderFactory(IConfiguration applicationConfiguration) => new(applicationConfiguration);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeWebExecutor{TDependencyPackage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}