// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Encapsulates creation of new object instances using explicit types.
    /// </summary>
    /// <typeparam name="ProductTBase">
    /// The base type from which all produced types derive.
    /// </typeparam>
    public interface IObjectFactory<ProductTBase> : IObjectFactory
    {
        /// <summary>
        /// Creates a new instance of an object of the specified type.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The explicit type of the object to create.
        /// </typeparam>
        /// <returns>
        /// A new instance of an object of the specified type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="TProduct" /> is not a supported product type for the factory.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public TProduct Produce<TProduct>()
            where TProduct : class, ProductTBase;

        /// <summary>
        /// Gets the base type from which all produced types derive.
        /// </summary>
        public Type ProductBaseType
        {
            get;
        }
    }

    /// <summary>
    /// Encapsulates creation of new object instances using explicit types.
    /// </summary>
    public interface IObjectFactory : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Creates a new instance of an object of the specified type.
        /// </summary>
        /// <param name="type">
        /// The explicit type of the object to create.
        /// </param>
        /// <returns>
        /// A new instance of an object of the specified type.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="type" /> is not a supported product type for the factory.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public Object Produce(Type type);

        /// <summary>
        /// Gets the types that can be produced by the current <see cref="IObjectFactory{TProductBase}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<Type> SupportedProductTypes
        {
            get;
        }
    }
}