// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of <see cref="ObjectFactory" /> instances that function as a single <see cref="IObjectFactory" />.
    /// </summary>
    public class CompositeObjectFactory : CompositeObjectFactory<Object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory" /> class.
        /// </summary>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factories" /> is <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(params ObjectFactory[] factories)
            : base(factories)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory" /> class.
        /// </summary>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factories" /> is <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IList<ObjectFactory> factories)
            : this(DefaultConfiguration, factories)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="factories" /> is
        /// <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IConfiguration applicationConfiguration, params ObjectFactory[] factories)
            : base(applicationConfiguration, factories)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="factories" /> is
        /// <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IConfiguration applicationConfiguration, IList<ObjectFactory> factories)
            : base(applicationConfiguration, new List<ObjectFactory<Object>>(factories))
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CompositeObjectFactory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a collection of <see cref="ObjectFactory{TProductBase}" /> instances that function as a single
    /// <see cref="IObjectFactory{TProductBase}" />.
    /// </summary>
    /// <typeparam name="TProductBase">
    /// The base type from which all produced types derive.
    /// </typeparam>
    public class CompositeObjectFactory<TProductBase> : ObjectFactory<TProductBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory{TProductBase}" /> class.
        /// </summary>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factories" /> is <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(params ObjectFactory<TProductBase>[] factories)
            : this(new List<ObjectFactory<TProductBase>>(factories))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory{TProductBase}" /> class.
        /// </summary>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factories" /> is <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IList<ObjectFactory<TProductBase>> factories)
            : this(DefaultConfiguration, factories)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory{TProductBase}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="factories" /> is
        /// <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IConfiguration applicationConfiguration, params ObjectFactory<TProductBase>[] factories)
            : this(applicationConfiguration, new List<ObjectFactory<TProductBase>>(factories))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeObjectFactory{TProductBase}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="factories">
        /// A collection of factories comprising the new composite factory.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="factories" /> is an empty collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="factories" /> contains a null element.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="factories" /> is
        /// <see langword="null" />.
        /// </exception>
        public CompositeObjectFactory(IConfiguration applicationConfiguration, IList<ObjectFactory<TProductBase>> factories)
            : base(applicationConfiguration)
        {
            Factories = factories.RejectIf().IsNullOrEmpty(nameof(factories)).OrIf(argument => argument.Any(factory => factory is null)).TargetArgument;
        }

        /// <summary>
        /// Returns a collection of supported product types paired with functions to create them.
        /// </summary>
        /// <exception cref="ObjectConfigurationException">
        /// An exception was raised during configuration of the factory.
        /// </exception>
        [DebuggerHidden]
        internal sealed override ConcurrentDictionary<Type, IObjectFactoryProductionFunction> DefineProductionFunctions()
        {
            var dictionary = new ConcurrentDictionary<Type, IObjectFactoryProductionFunction>();

            foreach (var factory in Factories)
            {
                foreach (var function in factory.ProductionFunctions)
                {
                    dictionary.AddOrUpdate(function.Key, function.Value, (key, oldValue) => function.Value);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Configures the current <see cref="CompositeObjectFactory{TProductBase}" />.
        /// </summary>
        /// <param name="configuration">
        /// Configuration information for the current <see cref="CompositeObjectFactory{TProductBase}" />.
        /// </param>
        protected sealed override void Configure(ObjectFactoryConfiguration<TProductBase> configuration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CompositeObjectFactory{TProductBase}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the collection of <see cref="IObjectFactory" /> instances comprising the current
        /// <see cref="CompositeObjectFactory{TProductBase}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEnumerable<ObjectFactory<TProductBase>> Factories;
    }
}