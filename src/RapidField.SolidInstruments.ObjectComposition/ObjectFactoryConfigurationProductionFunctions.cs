// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of configured types and functions that produce them using an <see cref="ObjectFactory" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectFactoryConfigurationProductionFunctions" /> is the default implementation of
    /// <see cref="IObjectFactoryConfigurationProductionFunctions" />.
    /// </remarks>
    public sealed class ObjectFactoryConfigurationProductionFunctions : ObjectFactoryConfigurationProductionFunctions<Object>, IObjectFactoryConfigurationProductionFunctions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfigurationProductionFunctions" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ObjectFactoryConfigurationProductionFunctions()
            : base()
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfiguration" /> class.
        /// </summary>
        /// <param name="functions">
        /// Configured types that are wrapped by the new functions.
        /// </param>
        [DebuggerHidden]
        internal ObjectFactoryConfigurationProductionFunctions(IObjectFactoryConfigurationProductionFunctions<Object> functions)
            : base(((ObjectFactoryConfigurationProductionFunctions<Object>)functions).Dictionary)
        {
            return;
        }
    }

    /// <summary>
    /// Represents a collection of configured types and functions that produce them using an
    /// <see cref="ObjectFactory{TProductBase}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectFactoryConfigurationProductionFunctions{TProductBase}" /> is the default implementation of
    /// <see cref="IObjectFactoryConfigurationProductionFunctions{TProductBase}" />.
    /// </remarks>
    /// <typeparam name="TProductBase">
    /// The base type from which objects produced by the associated factory derive.
    /// </typeparam>
    public class ObjectFactoryConfigurationProductionFunctions<TProductBase> : IObjectFactoryConfigurationProductionFunctions<TProductBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfigurationProductionFunctions{TProductBase}" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ObjectFactoryConfigurationProductionFunctions()
            : this(new())
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfigurationProductionFunctions{TProductBase}" /> class.
        /// </summary>
        /// <param name="dictionary">
        /// A collection of functions that produce an output.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal ObjectFactoryConfigurationProductionFunctions(ConcurrentDictionary<Type, IObjectFactoryProductionFunction> dictionary)
        {
            Dictionary = dictionary.RejectIf().IsNull(nameof(dictionary));
        }

        /// <summary>
        /// Defines the function that produces the specified type.
        /// </summary>
        /// <typeparam name="TProduct">
        /// The type of the object produced by the function.
        /// </typeparam>
        /// <param name="function">
        /// A function that produces an output of type <typeparamref name="TProduct" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// A function is already defined for the specified type, <typeparamref name="TProduct" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        public IObjectFactoryConfigurationProductionFunctions<TProductBase> Add<TProduct>(Func<TProduct> function)
            where TProduct : class, TProductBase
        {
            var productType = typeof(TProduct);

            if (Dictionary.TryAdd(productType, new ObjectFactoryProductionFunction<TProduct>(function)))
            {
                return this;
            }

            throw new ArgumentException($"A function is already defined for the specified type, {productType.FullName}.", nameof(function));
        }

        /// <summary>
        /// Represents a collection of functions that produce an output.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ConcurrentDictionary<Type, IObjectFactoryProductionFunction> Dictionary;
    }
}