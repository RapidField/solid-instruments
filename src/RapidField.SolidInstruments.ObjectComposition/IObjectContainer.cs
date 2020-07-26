// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Manages object creation, storage, resolution and disposal for a related group of object instances.
    /// </summary>
    public interface IObjectContainer : IInstrument
    {
        /// <summary>
        /// Returns the instance of specified type that is managed by the current <see cref="IObjectContainer" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the instance to return.
        /// </typeparam>
        /// <returns>
        /// The instance of specified type that is managed by the current <see cref="IObjectContainer" />.
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
        public T Get<T>()
            where T : class;

        /// <summary>
        /// Returns a new instance of specified type that is managed by the current <see cref="IObjectContainer" />.
        /// </summary>
        /// <remarks>
        /// <see cref="GetNew{T}" /> differs from <see cref="Get{T}" /> in that it returns a distinct instance for every subsequent
        /// call.
        /// </remarks>
        /// <typeparam name="T">
        /// The type of the instance to return.
        /// </typeparam>
        /// <returns>
        /// The instance of specified type that is managed by the current <see cref="IObjectContainer" />.
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
        public T GetNew<T>()
            where T : class;

        /// <summary>
        /// Gets the types of the instances that are managed by the current <see cref="IObjectContainer" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<Type> InstanceTypes
        {
            get;
        }
    }
}