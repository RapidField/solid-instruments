// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Encapsulates container configuration for a group of related dependencies.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public interface IDependencyModule<TConfigurator> : IDependencyModule
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure the container.
        /// </exception>
        public void Configure(TConfigurator configurator);
    }

    /// <summary>
    /// Encapsulates container configuration for a group of related dependencies.
    /// </summary>
    public interface IDependencyModule
    {
    }
}