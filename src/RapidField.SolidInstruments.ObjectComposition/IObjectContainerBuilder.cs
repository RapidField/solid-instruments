// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents an object that configures and initializes new <see cref="IObjectContainer" /> instances.
    /// </summary>
    public interface IObjectContainerBuilder : IObjectBuilder<IObjectContainer>
    {
        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" />.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container.
        /// </typeparam>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public IObjectContainerBuilder Configure<TProduct>()
            where TProduct : class, new();

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" />.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container.
        /// </typeparam>
        /// <param name="productionFunction">
        /// A function that produces the specified type.
        /// </param>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productionFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public IObjectContainerBuilder Configure<TProduct>(Func<TProduct> productionFunction)
            where TProduct : class;

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" /> in response to
        /// a request for <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">
        /// The request type that identifies the registration.
        /// </typeparam>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container as a result of a request for <typeparamref name="TRequest" />.
        /// </typeparam>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public IObjectContainerBuilder Configure<TRequest, TProduct>()
            where TRequest : class
            where TProduct : class, TRequest, new();

        /// <summary>
        /// Configures the <see cref="IObjectContainer" /> to support production of <typeparamref name="TProduct" /> in response to
        /// a request for <typeparamref name="TRequest" />.
        /// </summary>
        /// <typeparam name="TRequest">
        /// The request type that identifies the registration.
        /// </typeparam>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container as a result of a request for <typeparamref name="TRequest" />.
        /// </typeparam>
        /// <param name="productionFunction">
        /// A function that produces the specified type.
        /// </param>
        /// <returns>
        /// The current <see cref="ObjectContainerBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productionFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised while configuring the <see cref="IObjectContainer" />. See inner exception for details.
        /// </exception>
        public IObjectContainerBuilder Configure<TRequest, TProduct>(Func<TProduct> productionFunction)
            where TRequest : class
            where TProduct : class, TRequest;
    }
}