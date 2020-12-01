// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    /// <summary>
    /// Represents a <see cref="DependencyContainer{TContainer, TConfigurator}" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedDependecyContainer : DependencyContainer<SimulatedSourceContainer, SimulatedSourceConfigurator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDependecyContainer" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="configureAction" /> is
        /// <see langword="null" />.
        /// </exception>
        public SimulatedDependecyContainer(IConfiguration applicationConfiguration, Action<IConfiguration, SimulatedSourceConfigurator> configureAction)
            : base(applicationConfiguration, configureAction)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDependecyContainer" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <param name="serviceInjector">
        /// An object that adds service descriptors to the container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceInjector" /> is
        /// <see langword="null" /> -or- <paramref name="configureAction" /> is <see langword="null" />.
        /// </exception>
        public SimulatedDependecyContainer(IConfiguration applicationConfiguration, IServiceInjector<SimulatedSourceConfigurator> serviceInjector, Action<IConfiguration, SimulatedSourceConfigurator> configureAction)
            : base(applicationConfiguration, serviceInjector, configureAction)
        {
            return;
        }

        /// <summary>
        /// Builds the container that is abstracted by the current <see cref="SimulatedDependecyContainer" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        protected sealed override SimulatedSourceContainer BuildSourceContainer(SimulatedSourceConfigurator configurator) => configurator.Build();

        /// <summary>
        /// Creates a new initialization and disposal scope for the current <see cref="SimulatedDependecyContainer" />.
        /// </summary>
        /// <param name="sourceContainer">
        /// The container.
        /// </param>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="SimulatedDependecyContainer" />.
        /// </returns>
        protected sealed override IDependencyScope CreateScope(SimulatedSourceContainer sourceContainer) => new SimulatedDependencyScope(sourceContainer.CreateNewScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedDependecyContainer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Conditionally registers important dependency types if they are missing following user-defined registrations.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected override void RegisterFallbackTypes(SimulatedSourceConfigurator configurator)
        {
            return;
        }

        /// <summary>
        /// Registers a command mediator with the configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected sealed override void RegisterMediator(SimulatedSourceConfigurator configurator)
        {
            return;
        }
    }
}