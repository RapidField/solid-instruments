// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of definitions that control the behavior of an <see cref="ObjectContainer" /> when resolving
    /// requested objects.
    /// </summary>
    public interface IObjectContainerConfigurationDefinitions
    {
        /// <summary>
        /// Registers the specified product type with the associated <see cref="IObjectContainer" />.
        /// </summary>
        /// <typeparam name="TRequest">
        /// The request type that identifies the registration.
        /// </typeparam>
        /// <typeparam name="TProduct">
        /// The type that is produced by the container as a result of a request for <typeparamref name="TRequest" />.
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// A definition already exists for <typeparamref name="TRequest" />.
        /// </exception>
        public IObjectContainerConfigurationDefinitions Add<TRequest, TProduct>()
            where TRequest : class
            where TProduct : class, TRequest;

        /// <summary>
        /// Registers the specified product type with the associated <see cref="IObjectContainer" />.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type that is registered for production by the container.
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// A definition already exists for <typeparamref name="TProduct" />.
        /// </exception>
        public IObjectContainerConfigurationDefinitions Add<TProduct>()
            where TProduct : class;
    }
}