// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Encapsulates container configuration for a group of related dependencies.
    /// </summary>
    /// <remarks>
    /// <see cref="DependencyModule{TConfigurator}" /> is the default implementation of
    /// <see cref="IDependencyModule{TConfigurator}" />.
    /// </remarks>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures containers.
    /// </typeparam>
    public abstract class DependencyModule<TConfigurator> : IDependencyModule<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyModule{TConfigurator}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DependencyModule(IConfiguration applicationConfiguration)
        {
            ApplicationConfiguration = applicationConfiguration.RejectIf().IsNull(nameof(applicationConfiguration)).TargetArgument;
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while attempting to configure the container.
        /// </exception>
        public void Configure(TConfigurator configurator)
        {
            configurator = configurator.RejectIf().IsNull(nameof(configurator));

            try
            {
                Configure(configurator, ApplicationConfiguration);
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

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected abstract void Configure(TConfigurator configurator, IConfiguration applicationConfiguration);

        /// <summary>
        /// Represents configuration information for the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IConfiguration ApplicationConfiguration;
    }
}