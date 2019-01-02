// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of configured types and functions that produce them using an
    /// <see cref="ObjectFactory{TProductBase}" />.
    /// </summary>
    /// <typeparam name="TProductBase">
    /// The base type from which objects produced by the associated factory derive.
    /// </typeparam>
    public sealed class ObjectFactoryConfigurationProductionFunctions<TProductBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryConfigurationProductionFunctions{TProductBase}" /> class.
        /// </summary>
        [DebuggerHidden]
        internal ObjectFactoryConfigurationProductionFunctions()
        {
            Functions = new ConcurrentDictionary<Type, ObjectFactoryProductionFunction>();
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
        public ObjectFactoryConfigurationProductionFunctions<TProductBase> Define<TProduct>(Func<TProduct> function)
            where TProduct : class, TProductBase
        {
            var productType = typeof(TProduct);

            if (Functions.TryAdd(productType, new ObjectFactoryProductionFunction<TProduct>(function)))
            {
                return this;
            }

            throw new ArgumentException($"A function is already defined for the specified type, {productType.FullName}.", nameof(function));
        }

        /// <summary>
        /// Represents a collection of functions that produce an output.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ConcurrentDictionary<Type, ObjectFactoryProductionFunction> Functions;
    }
}