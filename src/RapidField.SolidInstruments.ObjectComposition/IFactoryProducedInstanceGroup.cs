// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Manages object creation, storage, resolution and disposal for a related group of factory-produced object instances.
    /// </summary>
    public interface IFactoryProducedInstanceGroup : IObjectContainer
    {
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
        public Lazy<T> GetLazy<T>()
            where T : class;
    }
}