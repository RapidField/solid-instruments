// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.InversionOfControl
{
    /// <summary>
    /// Represents a shared initialization and disposal scope for container-resolved objects.
    /// </summary>
    public interface IDependencyScope : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Creates a new child initialization and disposal scope for the current <see cref="IDependencyScope" />.
        /// </summary>
        /// <returns>
        /// A new child initialization and disposal scope for the current <see cref="IDependencyScope" />.
        /// </returns>
        /// <exception cref="CreateDependencyScopeException">
        /// An exception was raised while attempting to create the scope.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IDependencyScope CreateChildScope();

        /// <summary>
        /// Requests an object of specified type from the associated container for the current <see cref="IDependencyScope" />.
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
        Object Resolve(Type type);

        /// <summary>
        /// Requests an object of specified type from the associated container for the current <see cref="IDependencyScope" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to resolve.
        /// </typeparam>
        /// <returns>
        /// An object of specified type from the associated container.
        /// </returns>
        /// <exception cref="DependencyResolutionException">
        /// An exception was raised while attempting to resolve the dependency.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        T Resolve<T>()
            where T : class;
    }
}