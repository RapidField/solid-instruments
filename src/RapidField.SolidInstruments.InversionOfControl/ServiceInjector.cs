// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Facilitates injection of a collection of service descriptors into a
    /// <see cref="DependencyContainer{TContainer, TConfigurator}" /> configurator.
    /// </summary>
    /// <remarks>
    /// <see cref="ServiceInjector{TConfigurator}" /> is the default implementation of
    /// <see cref="IServiceInjector{TConfigurator}" />.
    /// </remarks>
    /// <typeparam name="TConfigurator">
    /// The type of the object that configures a container.
    /// </typeparam>
    public abstract class ServiceInjector<TConfigurator> : IServiceInjector<TConfigurator>
        where TConfigurator : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInjector{TConfigurator}" /> class.
        /// </summary>
        /// <param name="serviceDescriptors">
        /// A collection of service descriptors that are injected to a configurator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceDescriptors" /> is <see langword="null" />.
        /// </exception>
        protected ServiceInjector(IServiceCollection serviceDescriptors)
        {
            ServiceDescriptors = serviceDescriptors.RejectIf().IsNull(nameof(serviceDescriptors)).TargetArgument;
        }

        /// <summary>
        /// Adds a collection of service descriptors to a <see cref="DependencyContainer{TContainer, TConfigurator}" /> configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurator" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ContainerConfigurationException">
        /// An exception was raised while injecting the service descriptors.
        /// </exception>
        public void Inject(TConfigurator configurator)
        {
            try
            {
                Inject(configurator, ServiceDescriptors);
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
        /// Adds a collection of service descriptors to a <see cref="DependencyContainer{TContainer, TConfigurator}" /> configurator.
        /// </summary>
        /// <param name="configurator">
        /// The object that configures a container.
        /// </param>
        /// <param name="serviceDescriptors">
        /// a collection of service descriptors that are added to the configurator.
        /// </param>
        protected abstract void Inject(TConfigurator configurator, IServiceCollection serviceDescriptors);

        /// <summary>
        /// Represents a collection of service descriptors that are injected into a configurator by the current
        /// <see cref="ServiceInjector{TConfigurator}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IServiceCollection ServiceDescriptors;
    }
}