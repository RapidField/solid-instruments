// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Manages object creation, storage, resolution and disposal for a related group of factory-produced instances.
    /// </summary>
    public interface IFactoryProducedInstanceGroup : IDisposable
    {
        /// <summary>
        /// Returns the instance of specified type that is managed by the current <see cref="IFactoryProducedInstanceGroup" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the instance to return.
        /// </typeparam>
        /// <returns>
        /// The instance of specified type that is managed by the current <see cref="IFactoryProducedInstanceGroup" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T" /> is not a supported type for the group.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        T Get<T>()
            where T : class;

        /// <summary>
        /// Returns the lazily-initialized instance of specified type that is managed by the current
        /// <see cref="IFactoryProducedInstanceGroup" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the lazily-initialized instance to return.
        /// </typeparam>
        /// <returns>
        /// The lazily-initialized instance of specified type that is managed by the current
        /// <see cref="IFactoryProducedInstanceGroup" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Lazy<T> GetLazy<T>()
            where T : class;

        /// <summary>
        /// Gets the types of the instances that are managed by the current <see cref="IFactoryProducedInstanceGroup" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        IEnumerable<Type> InstanceTypes
        {
            get;
        }
    }
}