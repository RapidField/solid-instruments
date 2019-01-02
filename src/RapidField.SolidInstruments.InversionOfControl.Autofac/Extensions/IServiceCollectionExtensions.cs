// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.Extensions;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with Autofac inversion of control features.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers an Autofac dependency engine and provider factory with the current <see cref="IServiceCollection" />.
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
            where TPackage : AutofacDependencyPackage, new() => target.AddDependencyPackage<ContainerBuilder, AutofacDependencyEngine, TPackage>(applicationConfiguration);

        /// <summary>
        /// Registers an Autofac dependency engine and provider factory with the current <see cref="IServiceCollection" />.
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
            where TPackage : AutofacDependencyPackage, new() => target.AddDependencyPackage<ContainerBuilder, AutofacDependencyEngine, TPackage>(applicationConfiguration, out serviceProvider);
    }
}