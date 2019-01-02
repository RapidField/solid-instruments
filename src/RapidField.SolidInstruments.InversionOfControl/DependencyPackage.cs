// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Encapsulates container configuration for an application.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyPackage{TConfigurator, TEngine}" /> is the default implementation of
    /// <see cref="IDependencyPackage{TConfigurator, TEngine}" />.
    /// </remarks>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    /// <typeparam name="TEngine">
    /// The type of the dependency engine that is produced by the package.
    /// </typeparam>
    public abstract class DependencyPackage<TConfigurator, TEngine> : DependencyPackage<TConfigurator>, IDependencyPackage<TConfigurator, TEngine>
        where TConfigurator : class, new()
        where TEngine : class, IDependencyEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPackage{TConfigurator, TEngine}" /> class.
        /// </summary>
        protected DependencyPackage()
            : base()
        {
            return;
        }

        /// <summary>
        /// Creates a new dependency engine that is configured by the current
        /// <see cref="DependencyPackage{TConfigurator, TEngine}" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        public TEngine CreateEngine(IConfiguration applicationConfiguration) => CreateEngine(applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument, DependencyEngine.EmptyServiceCollection, this);

        /// <summary>
        /// Creates a new dependency engine that is configured by the current
        /// <see cref="DependencyPackage{TConfigurator, TEngine}" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="serviceDescriptors" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure a container.
        /// </exception>
        public TEngine CreateEngine(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors) => CreateEngine(applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument, serviceDescriptors.RejectIf().IsNull(nameof(serviceDescriptors)).TargetArgument, this);

        /// <summary>
        /// Creates a new dependency engine that is configured by the current
        /// <see cref="DependencyPackage{TConfigurator, TEngine}" />.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors to which the engine will supply dependencies.
        /// </param>
        /// <param name="package">
        /// The current dependency package.
        /// </param>
        /// <returns>
        /// The resulting dependency engine.
        /// </returns>
        protected abstract TEngine CreateEngine(IConfiguration applicationConfiguration, IServiceCollection serviceDescriptors, IDependencyPackage<TConfigurator> package);
    }

    /// <summary>
    /// Encapsulates container configuration for an application.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyPackage{TConfigurator}" /> is the default implementation of
    /// <see cref="IDependencyPackage{TConfigurator}" />.
    /// </remarks>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public abstract class DependencyPackage<TConfigurator> : IDependencyPackage<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPackage{TConfigurator}" /> class.
        /// </summary>
        protected DependencyPackage()
        {
            return;
        }

        /// <summary>
        /// Gets the collection of dependency modules for the package.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The package's dependency modules.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to create the modules for the package.
        /// </exception>
        public IEnumerable<IDependencyModule<TConfigurator>> GetModules(IConfiguration applicationConfiguration)
        {
            applicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;

            if (Modules is null)
            {
                try
                {
                    Modules = CreateModules(applicationConfiguration);
                }
                catch (ContainerConfigurationException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new ContainerConfigurationException(exception);
                }
            }

            return Modules;
        }

        /// <summary>
        /// Creates a new collection of dependency modules for the package.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <returns>
        /// The package's dependency modules.
        /// </returns>
        protected abstract IEnumerable<IDependencyModule<TConfigurator>> CreateModules(IConfiguration applicationConfiguration);

        /// <summary>
        /// Represents a collection of dependency modules for the package, or <see langword="null" /> if the modules have not yet
        /// been created.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IEnumerable<IDependencyModule<TConfigurator>> Modules = null;
    }
}