// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Provides an extension point for creating a container specific builder and an <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public abstract class ServiceProviderFactory<TConfigurator> : IServiceProviderFactory<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderFactory{TConfigurator}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected ServiceProviderFactory(IConfiguration applicationConfiguration)
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
        }

        /// <summary>
        /// Returns a new <typeparamref name="TConfigurator" /> instance.
        /// </summary>
        /// <param name="services">
        /// The collection of services.
        /// </param>
        /// <returns>
        /// A new <typeparamref name="TConfigurator" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services" /> is <see langword="null" />.
        /// </exception>
        public TConfigurator CreateBuilder(IServiceCollection services)
        {
            var configurator = new TConfigurator();
            PrepareConfigurator(configurator, services.RejectIf().IsNull(nameof(services)).TargetArgument);
            return configurator;
        }

        /// <summary>
        /// Creates an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="containerBuilder">
        /// A source configurator.
        /// </param>
        /// <returns>
        /// An <see cref="IServiceProvider" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="containerBuilder" /> is <see langword="null" />.
        /// </exception>
        public IServiceProvider CreateServiceProvider(TConfigurator containerBuilder) => CreateServiceProvider(containerBuilder.RejectIf().IsNull(nameof(containerBuilder)), ApplicationConfiguration);

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
        protected abstract IServiceProvider CreateServiceProvider(TConfigurator configurator, IConfiguration applicationConfiguration);

        /// <summary>
        /// Prepares a newly-created <typeparamref name="TConfigurator" /> instance.
        /// </summary>
        /// <param name="configurator">
        /// The configurator to prepare.
        /// </param>
        /// <param name="services">
        /// A collection of services.
        /// </param>
        protected abstract void PrepareConfigurator(TConfigurator configurator, IServiceCollection services);

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        protected readonly IConfiguration ApplicationConfiguration;
    }

    /// <summary>
    /// Provides an extension point for creating a container specific builder and an <see cref="IServiceProvider" />.
    /// </summary>
    public sealed class ServiceProviderFactory : IServiceProviderFactory<Nix>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderFactory" /> class.
        /// </summary>
        /// <param name="dependencyContainer">
        /// The container that resolves dependencies for the provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dependencyContainer" /> is <see langword="null" />.
        /// </exception>
        public ServiceProviderFactory(IDependencyContainer dependencyContainer)
        {
            DependencyContainer = dependencyContainer.RejectIf().IsNull(nameof(dependencyContainer)).TargetArgument;
        }

        /// <summary>
        /// Returns an ignored <see cref="Nix" /> instance.
        /// </summary>
        /// <param name="services">
        /// The collection of services.
        /// </param>
        /// <returns>
        /// An ignored <see cref="Nix" /> instance.
        /// </returns>
        public Nix CreateBuilder(IServiceCollection services) => Nix.Instance;

        /// <summary>
        /// Creates an <see cref="IServiceProvider" />.
        /// </summary>
        /// <param name="containerBuilder">
        /// An ignored argument.
        /// </param>
        /// <returns>
        /// An <see cref="IServiceProvider" />.
        /// </returns>
        public IServiceProvider CreateServiceProvider(Nix containerBuilder) => new ServiceProvider(DependencyContainer);

        /// <summary>
        /// Represents the container that resolves dependencies for the provider.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyContainer DependencyContainer;
    }
}