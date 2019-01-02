// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.Extensions;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers an DotNetNative dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
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
        public static IServiceCollection AddDependencyPackage<TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration)
            where TPackage : DotNetNativeDependencyPackage, new() => target.AddDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration);

        /// <summary>
        /// Registers an DotNetNative dependency engine and provider factory with the current <see cref="IServiceCollection" />.
        /// </summary>
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
        public static IServiceCollection AddDependencyPackage<TPackage>(this IServiceCollection target, IConfiguration applicationConfiguration, out IServiceProvider serviceProvider)
            where TPackage : DotNetNativeDependencyPackage, new() => target.AddDependencyPackage<ServiceCollection, DotNetNativeDependencyEngine, TPackage>(applicationConfiguration, out serviceProvider);
    }
}