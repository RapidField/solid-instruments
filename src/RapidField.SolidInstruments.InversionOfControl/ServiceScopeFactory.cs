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
    /// Represents a factory that produces <see cref="IServiceScope" /> instances.
    /// </summary>
    public sealed class ServiceScopeFactory : IServiceScopeFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceScopeFactory" /> class.
        /// </summary>
        /// <param name="dependencyScope">
        /// The scope that is abstracted by the service scope.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dependencyScope" /> is <see langword="null" />.
        /// </exception>
        public ServiceScopeFactory(IDependencyScope dependencyScope)
        {
            DependencyScope = dependencyScope.RejectIf().IsNull(nameof(dependencyScope)).TargetArgument;
        }

        /// <summary>
        /// Creates a new service scope.
        /// </summary>
        /// <returns>
        /// A new service scope.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        public IServiceScope CreateScope()
        {
            try
            {
                return new ServiceScope(DependencyScope.CreateChildScope());
            }
            catch (ObjectDisposedException exception)
            {
                throw new CreateDependencyScopeException(exception);
            }
        }

        /// <summary>
        /// Represents the parent scope that produces the derivative scopes.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyScope DependencyScope;
    }
}