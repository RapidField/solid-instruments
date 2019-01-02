// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using System;
using System.Diagnostics;
using DotNetServiceProvider = Microsoft.Extensions.DependencyInjection.ServiceProvider;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Represents an abstraction for a utility that creates, destroys, and manages scoping for registered dependencies.
    /// </summary>
    public sealed class DotNetNativeDependencyContainer : DependencyContainer<DotNetServiceProvider, ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDependencyContainer" /> class.
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
        [DebuggerHidden]
        internal DotNetNativeDependencyContainer(IConfiguration applicationConfiguration, Action<IConfiguration, ServiceCollection> configureAction)
            : base(applicationConfiguration, configureAction)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDependencyContainer" /> class.
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
        [DebuggerHidden]
        internal DotNetNativeDependencyContainer(IConfiguration applicationConfiguration, IServiceInjector<ServiceCollection> serviceInjector, Action<IConfiguration, ServiceCollection> configureAction)
            : base(applicationConfiguration, serviceInjector, configureAction)
        {
            return;
        }

        /// <summary>
        /// Builds the container that is abstracted by the current <see cref="DotNetNativeDependencyContainer" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        protected override DotNetServiceProvider BuildSourceContainer(ServiceCollection configurator) => configurator.BuildServiceProvider();

        /// <summary>
        /// Creates a new initialization and disposal scope for the current <see cref="DotNetNativeDependencyContainer" />.
        /// </summary>
        /// <param name="sourceContainer">
        /// The container.
        /// </param>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="DotNetNativeDependencyContainer" />.
        /// </returns>
        protected override IDependencyScope CreateScope(DotNetServiceProvider sourceContainer) => new DotNetNativeDependencyScope(sourceContainer.CreateScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DotNetNativeDependencyContainer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers a command mediator with the configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected sealed override void RegisterMediator(ServiceCollection configurator) => configurator.AddTransient<ICommandMediator, CommandMediator>((provider) => CreateMediator(provider));

        /// <summary>
        /// Creates a new command mediator using the specified provider.
        /// </summary>
        /// <param name="provider">
        /// The provider from which to derive the mediator's scope.
        /// </param>
        /// <returns>
        /// The resulting command mediator.
        /// </returns>
        [DebuggerHidden]
        private CommandMediator CreateMediator(IServiceProvider provider)
        {
            var sourceScope = provider.CreateScope();
            var dependencyScope = new DotNetNativeDependencyScope(sourceScope);
            ReferenceManager.AddObject(dependencyScope);
            return new CommandMediator(dependencyScope);
        }
    }
}