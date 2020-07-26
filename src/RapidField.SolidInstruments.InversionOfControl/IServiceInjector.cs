// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Facilitates injection of a collection of service descriptors into a
    /// <see cref="DependencyContainer{TContainer, TConfigurator}" /> configurator.
    /// </summary>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures a container.
    /// </typeparam>
    public interface IServiceInjector<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Adds a collection of service descriptors to a <see cref="DependencyContainer{TContainer, TConfigurator}" />
        /// configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while injecting the service descriptors.
        /// </exception>
        public void Inject(TConfigurator configurator);
    }
}