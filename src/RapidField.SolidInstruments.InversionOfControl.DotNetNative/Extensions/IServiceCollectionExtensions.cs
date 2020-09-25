// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the specified <see cref="IConfiguration" /> instance as a singleton as an idempotent, safe operation.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection target, IConfiguration applicationConfiguration)
        {
            target.TryAddSingleton(applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument);
            return target;
        }

        /// <summary>
        /// Registers a command mediator with the current <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandMediator(this IServiceCollection target)
        {
            target.TryAdd(new ServiceDescriptor(CommandMediatorInterfaceType, CreateCommandMediator, ServiceLifetime.Scoped));
            return target;
        }

        /// <summary>
        /// Registers the dependency types defined by the specified native .NET dependency module with the current
        /// <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="dependencyModule">
        /// The module that defines registrations.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dependencyModule" /> is <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddDependencyModule(this IServiceCollection target, IDependencyModule<ServiceCollection> dependencyModule)
        {
            var serviceInjector = new DotNetNativeServiceInjector(target);
            var serviceCollection = new ServiceCollection();
            serviceInjector.Inject(serviceCollection);
            dependencyModule.RejectIf().IsNull(nameof(dependencyModule)).TargetArgument.Configure(serviceCollection);
            target.TryAdd(serviceCollection);
            return target;
        }

        /// <summary>
        /// Registers the dependency types defined by the specified native .NET dependency package with the current
        /// <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="dependencyPackage">
        /// The package that defines registrations.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dependencyPackage" /> is <see langword="null" /> -or- <paramref name="applicationConfiguration" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another dependency package has already been added to the service collection.
        /// </exception>
        public static IServiceCollection AddDependencyPackage(this IServiceCollection target, IDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine> dependencyPackage, IConfiguration applicationConfiguration)
        {
            var dependencyModules = dependencyPackage.RejectIf().IsNull(nameof(dependencyPackage)).TargetArgument.GetModules(applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument);

            foreach (var dependencyModule in dependencyModules)
            {
                target.AddDependencyModule(dependencyModule);
            }

            return target;
        }

        /// <summary>
        /// Registers a native .NET dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another dependency package has already been added to the service collection.
        /// </exception>
        public static IServiceCollection AddDependencyPackage<TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration)
            where TPackage : DotNetNativeDependencyPackage, new() => target.AddDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration);

        /// <summary>
        /// Registers a native .NET dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TPackage">
        /// The type of the package that configures the engine.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceProvider">
        /// The resulting service provider.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another dependency package has already been added to the service collection.
        /// </exception>
        public static IServiceCollection AddDependencyPackage<TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration, out IServiceProvider serviceProvider)
            where TPackage : DotNetNativeDependencyPackage, new() => target.AddDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration, out serviceProvider);

        /// <summary>
        /// Creates a new scoped <see cref="CommandMediator" /> instance.
        /// </summary>
        /// <param name="serviceProvider">
        /// A provider that is used to create a dependency scope for the mediator.
        /// </param>
        /// <returns>
        /// A new scoped <see cref="CommandMediator" /> instance.
        /// </returns>
        [DebuggerHidden]
        private static Object CreateCommandMediator(IServiceProvider serviceProvider)
        {
            var sourceScope = serviceProvider.CreateScope();
            var dependencyScope = new DotNetNativeDependencyScope(sourceScope);
            var commandMediator = new CommandMediator(dependencyScope);
            ReferenceManager.Instance.AddObject(sourceScope);
            ReferenceManager.Instance.AddObject(dependencyScope);
            ReferenceManager.Instance.AddObject(commandMediator);
            return commandMediator;
        }

        /// <summary>
        /// Represents the <see cref="ICommandMediator" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandMediatorInterfaceType = typeof(ICommandMediator);
    }
}