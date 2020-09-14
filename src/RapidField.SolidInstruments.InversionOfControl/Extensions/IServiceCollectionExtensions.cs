// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.InversionOfControl.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with inversion of control features.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TConfigurator">
        /// The type of the object that configures containers.
        /// </typeparam>
        /// <typeparam name="TEngine">
        /// The type of the dependency engine that is produced by the package.
        /// </typeparam>
        /// <typeparam name="TPackage">
        /// The package that configures the engine.
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
        [DebuggerHidden]
        internal static IServiceCollection AddDependencyPackage<TConfigurator, TEngine, TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration)
            where TConfigurator : class, new()
            where TEngine : class, IDependencyEngine
            where TPackage : class, IDependencyPackage<TConfigurator, TEngine>, new() => target.AddDependencyPackage<TConfigurator, TEngine, TPackage>(applicationConfiguration, out var serviceProvider);

        /// <summary>
        /// Registers a dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TConfigurator">
        /// The type of the object that configures containers.
        /// </typeparam>
        /// <typeparam name="TEngine">
        /// The type of the dependency engine that is produced by the package.
        /// </typeparam>
        /// <typeparam name="TPackage">
        /// The package that configures the engine.
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
        [DebuggerHidden]
        internal static IServiceCollection AddDependencyPackage<TConfigurator, TEngine, TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration, out IServiceProvider serviceProvider)
            where TConfigurator : class, new()
            where TEngine : class, IDependencyEngine
            where TPackage : class, IDependencyPackage<TConfigurator, TEngine>, new()
        {
            if (target.Any(element => element.ServiceType == typeof(IDependencyEngine)))
            {
                throw new InvalidOperationException("Another dependency package has already been added to the service collection.");
            }

            var engine = DependencyEngine.New<TConfigurator, TEngine, TPackage>(applicationConfiguration, target);
            ReferenceManager.AddObject(engine);
            serviceProvider = engine.Provider;
            return target;
        }

        /// <summary>
        /// Finalizes static members of the <see cref="IServiceCollectionExtensions" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers() => ReferenceManager.Dispose();

        /// <summary>
        /// Represents a reference manager for disposable references created by <see cref="IServiceCollectionExtensions" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IReferenceManager ReferenceManager = new ReferenceManager();

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="IServiceCollectionExtensions" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new StaticMemberFinalizer(FinalizeStaticMembers);
    }
}