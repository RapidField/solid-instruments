// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Provides an extension point for creating a native .NET container builder and an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="TPackage">
    /// The package that configures the engine.
    /// </typeparam>
    public class DotNetNativeServiceProviderFactory<TPackage> : ServiceProviderFactory<ServiceCollection>
        where TPackage : DotNetNativeDependencyPackage, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeServiceProviderFactory{TPackage}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public DotNetNativeServiceProviderFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Creates an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="configurator">
        /// A source configurator.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// An <see cref="IServiceProvider" />.
        /// </returns>
        protected override IServiceProvider CreateServiceProvider(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            _ = configurator.AddDependencyPackage<TPackage>(applicationConfiguration, out var serviceProvider);
            return serviceProvider;
        }

        /// <summary>
        /// Prepares a newly-created <see cref="ServiceCollection" /> instance.
        /// </summary>
        /// <param name="configurator">
        /// The configurator to prepare.
        /// </param>
        /// <param name="services">
        /// A collection of services.
        /// </param>
        protected override void PrepareConfigurator(ServiceCollection configurator, IServiceCollection services)
        {
            var serviceInjector = new DotNetNativeServiceInjector(services);
            serviceInjector.Inject(configurator);
        }
    }
}