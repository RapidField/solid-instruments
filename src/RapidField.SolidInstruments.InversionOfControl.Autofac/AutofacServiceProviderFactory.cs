// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac
{
    /// <summary>
    /// Provides an extension point for creating an Autofac container builder and an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="TPackage">
    /// The package that configures the engine.
    /// </typeparam>
    public class AutofacServiceProviderFactory<TPackage> : ServiceProviderFactory<ContainerBuilder>
        where TPackage : AutofacDependencyPackage, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProviderFactory{TPackage}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public AutofacServiceProviderFactory(IConfiguration applicationConfiguration)
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
        protected override IServiceProvider CreateServiceProvider(ContainerBuilder configurator, IConfiguration applicationConfiguration)
        {
            var container = configurator.Build();
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// Prepares a newly-created <see cref="ContainerBuilder" /> instance.
        /// </summary>
        /// <param name="configurator">
        /// The configurator to prepare.
        /// </param>
        /// <param name="services">
        /// A collection of services.
        /// </param>
        protected override void PrepareConfigurator(ContainerBuilder configurator, IServiceCollection services)
        {
            services = services.AddDependencyPackage<TPackage>(ApplicationConfiguration);
            var serviceInjector = new AutofacServiceInjector(services);
            serviceInjector.Inject(configurator);
        }
    }
}