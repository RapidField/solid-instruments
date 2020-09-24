// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac
{
    /// <summary>
    /// Represents an abstraction for a utility that creates, destroys, and manages scoping for Autofac-registered dependencies.
    /// </summary>
    public sealed class AutofacDependencyContainer : DependencyContainer<IContainer, ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyContainer" /> class.
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
        internal AutofacDependencyContainer(IConfiguration applicationConfiguration, Action<IConfiguration, ContainerBuilder> configureAction)
            : base(applicationConfiguration, configureAction)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyContainer" /> class.
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
        internal AutofacDependencyContainer(IConfiguration applicationConfiguration, IServiceInjector<ContainerBuilder> serviceInjector, Action<IConfiguration, ContainerBuilder> configureAction)
            : base(applicationConfiguration, serviceInjector, configureAction)
        {
            return;
        }

        /// <summary>
        /// Builds the container that is abstracted by the current <see cref="AutofacDependencyContainer" />.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        /// <returns>
        /// The container.
        /// </returns>
        protected override IContainer BuildSourceContainer(ContainerBuilder configurator) => configurator.Build();

        /// <summary>
        /// Creates a new initialization and disposal scope for the current <see cref="AutofacDependencyContainer" />.
        /// </summary>
        /// <param name="sourceContainer">
        /// The container.
        /// </param>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="AutofacDependencyContainer" />.
        /// </returns>
        protected override IDependencyScope CreateScope(IContainer sourceContainer) => new AutofacDependencyScope(sourceContainer.BeginLifetimeScope());

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AutofacDependencyContainer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Registers a command mediator with the configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures the container.
        /// </param>
        protected sealed override void RegisterMediator(ContainerBuilder configurator) => configurator.Register(provider => CreateMediator(provider)).As<ICommandMediator>().InstancePerDependency();

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
        private CommandMediator CreateMediator(IComponentContext provider)
        {
            var lifetimeScope = provider.Resolve<ILifetimeScope>();
            var dependencyScope = new AutofacDependencyScope(lifetimeScope);
            ReferenceManager.AddObject(dependencyScope);
            return new CommandMediator(dependencyScope);
        }
    }
}