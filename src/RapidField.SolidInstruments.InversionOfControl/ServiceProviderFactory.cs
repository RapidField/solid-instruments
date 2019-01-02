// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

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