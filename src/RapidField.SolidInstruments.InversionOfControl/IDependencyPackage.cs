// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Encapsulates container configuration for an application.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    /// <typeparam name="TEngine">
    /// The type of the dependency engine that is produced by the package.
    /// </typeparam>
    public interface IDependencyPackage<TConfigurator, TEngine> : IDependencyPackage<TConfigurator>
        where TConfigurator : class, new()
        where TEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Creates a new dependency engine that is configured by the current
        /// <see cref="IDependencyPackage{TConfigurator, TEngine}" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        TEngine CreateEngine(IConfiguration applicationConfiguration);

        /// <summary>
        /// Creates a new dependency engine that is configured by the current
        /// <see cref="IDependencyPackage{TConfigurator, TEngine}" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        TEngine CreateEngine(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors);
    }

    /// <summary>
    /// Encapsulates container configuration for an application.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IDependencyPackage<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Gets a collection of dependency modules for the package.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// A collection of dependency modules.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to create the modules for the package.
        /// </exception>
        IEnumerable<IDependencyModule<TConfigurator>> GetModules(IConfiguration applicationConfiguration);
    }
}