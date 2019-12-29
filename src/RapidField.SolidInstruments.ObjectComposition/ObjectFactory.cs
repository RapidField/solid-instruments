// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Encapsulates creation of new object instances using explicit types.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectFactory" /> is the default implementation of <see cref="IObjectFactory" />.
    /// </remarks>
    public abstract class ObjectFactory : ObjectFactory<Object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected ObjectFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the current <see cref="ObjectFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="ObjectFactory" />.
        /// </param>
        protected abstract void Configure(ObjectFactoryConfiguration configuration);

        /// <summary>
        /// Configures the current <see cref="ObjectFactory" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="ObjectFactory" />.
        /// </param>
        protected sealed override void Configure(ObjectFactoryConfiguration<Object> configuration) => Configure(new ObjectFactoryConfiguration(configuration));

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Encapsulates creation of new object instances using explicit types.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectFactory{TProductBase}" /> is the default implementation of <see cref="IObjectFactory{TProductBase}" />.
    /// </remarks>
    /// <typeparam name="TProductBase">
    /// The base type from which all produced types derive.
    /// </typeparam>
    public abstract class ObjectFactory<TProductBase> : ConfigurableInstrument<ObjectFactoryConfiguration<TProductBase>>, IObjectFactory<TProductBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory{TProductBase}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected ObjectFactory(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            LazyProductionFunctions = new Lazy<ConcurrentDictionary<Type, ObjectFactoryProductionFunction>>(DefineProductionFunctions, LazyThreadSafetyMode.ExecutionAndPublication);
            ProductBaseType = typeof(TProductBase);
        }

        /// <summary>
        /// Creates a new instance of an object of the specified explicit type.
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
            where TProduct : class, TProductBase => Produce(typeof(TProduct)) as TProduct;

        /// <summary>
        /// Creates a new instance of an object of the specified explicit type.
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
        public Object Produce(Type type)
        {
            try
            {
                if (ProductionFunctions.TryGetValue(type, out var function))
                {
                    return function.Invoke();
                }
            }
            catch (ObjectConfigurationException exception)
            {
                throw new ObjectProductionException(type, exception);
            }

            throw new ArgumentException($"The specified product type, {type.FullName}, is not supported by the object factory type {GetType().FullName}.", nameof(type));
        }

        /// <summary>
        /// Returns a collection of supported product types paired with functions to create them.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the factory.
        /// </exception>
        [DebuggerHidden]
        internal virtual ConcurrentDictionary<Type, ObjectFactoryProductionFunction> DefineProductionFunctions() => Configuration.ProductionFunctions.Dictionary;

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectFactory{TProductBase}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the base type from which all produced types derive.
        /// </summary>
        public Type ProductBaseType
        {
            get;
        }

        /// <summary>
        /// Gets the explicit types that can be produced by the current <see cref="ObjectFactory{TProductBase}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the factory.
        /// </exception>
        public IEnumerable<Type> SupportedProductTypes
        {
            get
            {
                RejectIfDisposed();
                return ProductionFunctions.Values.Select(function => function.ProductType);
            }
        }

        /// <summary>
        /// Gets a collection of type names and functions that produce the associated types.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the factory.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal ConcurrentDictionary<Type, ObjectFactoryProductionFunction> ProductionFunctions => LazyProductionFunctions.Value;

        /// <summary>
        /// Represents a lazily-initialized collection of type names and functions that produce the associated types.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the factory.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<ConcurrentDictionary<Type, ObjectFactoryProductionFunction>> LazyProductionFunctions;
    }
}