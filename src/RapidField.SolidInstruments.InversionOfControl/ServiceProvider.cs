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
    /// Resolves service objects.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider, ISupportRequiredService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProvider" /> class.
        /// </summary>
        /// <param name="container">
        /// A dependency container that resolves services for the provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal ServiceProvider(IDependencyContainer container)
        {
            Container = container.RejectIf().IsNull(nameof(container)).TargetArgument;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Object GetRequiredService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (DependencyResolutionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new DependencyResolutionException(serviceType, exception);
            }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" /> -or- <see langword="null" /> if there is no service object of
        /// type <paramref name="serviceType" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Object GetService(Type serviceType)
        {
            try
            {
                return GetRequiredService(serviceType);
            }
            catch (DependencyResolutionException)
            {
                return null;
            }
        }

        /// <summary>
        /// Represents a dependency container that resolves services for the provider.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IDependencyContainer Container;
    }
}