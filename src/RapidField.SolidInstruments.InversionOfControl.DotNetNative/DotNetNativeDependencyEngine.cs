// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative
{
    /// <summary>
    /// Represents a configurable dependency resolution system that abstracts the native .NET classes.
    /// </summary>
    public sealed class DotNetNativeDependencyEngine : DependencyEngine<ServiceCollection, DotNetNativeServiceInjector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDependencyEngine" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="package">
        /// A package that configures dependencies for the engine.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="package" /> is
        /// <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is <see langword="null" /> .
        /// </exception>
        [DebuggerHidden]
        internal DotNetNativeDependencyEngine(IConfiguration applicationConfiguration, IDependencyPackage<ServiceCollection> package, IServiceCollection serviceDescriptors)
            : base(applicationConfiguration, package, serviceDescriptors)
        {
            return;
        }

        /// <summary>
        /// Creates a new dependency engine.
        /// </summary>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// A new dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        public static DotNetNativeDependencyEngine New<TPackage>(IConfiguration applicationConfiguration)
            where TPackage : DotNetNativeDependencyPackage, new() => New<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration);

        /// <summary>
        /// Creates a new dependency engine.
        /// </summary>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <returns>
        /// A new dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        [DebuggerHidden]
        internal static DotNetNativeDependencyEngine New<TPackage>(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors)
            where TPackage : DotNetNativeDependencyPackage, new() => New<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration, serviceDescriptors);

        /// <summary>
        /// Creates a new dependency container.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceInjector">
        /// An object that injects service descriptors into a container configurator.
        /// </param>
        /// <param name="configureAction">
        /// An action that configures the container.
        /// </param>
        /// <returns>
        /// A new dependency container.
        /// </returns>
        protected override IDependencyContainer CreateContainer(IConfiguration applicationConfiguration, IServiceInjector<ServiceCollection> serviceInjector, Action<IConfiguration, ServiceCollection> configureAction) => new DotNetNativeDependencyContainer(applicationConfiguration, serviceInjector, configureAction);

        /// <summary>
        /// Creates a new service injector.
        /// </summary>
        /// <returns>
        /// A new service injector.
        /// </returns>
        protected override DotNetNativeServiceInjector CreateServiceInjector(IServiceCollection serviceDescriptors) => new(serviceDescriptors);
    }
}