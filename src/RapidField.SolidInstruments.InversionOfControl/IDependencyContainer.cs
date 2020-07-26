// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents an abstraction for a utility that creates, destroys, and manages scoping for dependencies.
    /// </summary>
    public interface IDependencyContainer : IInstrument
    {
        /// <summary>
        /// Creates a new initialization and disposal scope for the current <see cref="IDependencyContainer" />.
        /// </summary>
        /// <returns>
        /// A new initialization and disposal scope for the current <see cref="IDependencyContainer" />.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IDependencyScope CreateScope();

        /// <summary>
        /// Requests an object of specified type from the current <see cref="IDependencyContainer" />.
        /// </summary>
        /// <param name="type">
        /// The type of the object to resolve.
        /// </param>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Object Resolve(Type type);
    }
}