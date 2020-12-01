// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of definitions that control the behavior of an <see cref="ObjectContainer" /> when resolving
    /// requested objects.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectContainerConfigurationDefinitions" /> is the default implementation of
    /// <see cref="IObjectContainerConfigurationDefinitions" />.
    /// </remarks>
    public sealed class ObjectContainerConfigurationDefinitions : IObjectContainerConfigurationDefinitions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerConfigurationDefinitions" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ObjectContainerConfigurationDefinitions()
        {
            Registrations = new();
        }

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
            where TProduct : class, TRequest
        {
            var requestType = typeof(TRequest);
            var productType = typeof(TProduct);

            if (Registrations.TryAdd(requestType, new ObjectContainerDefinition(requestType, productType)))
            {
                return this;
            }

            throw new ArgumentException($"A definition already exists for the specified request type, {requestType.FullName}.", nameof(TRequest));
        }

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
            where TProduct : class => Add<TProduct, TProduct>();

        /// <summary>
        /// Represents a collection of request-product type pairs that constitute the definitions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ConcurrentDictionary<Type, IObjectContainerDefinition> Registrations;
    }
}