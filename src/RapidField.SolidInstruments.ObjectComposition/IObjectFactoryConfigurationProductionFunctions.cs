// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a collection of configured types and functions that produce them using an <see cref="ObjectFactory" />.
    /// </summary>
    public interface IObjectFactoryConfigurationProductionFunctions : IObjectFactoryConfigurationProductionFunctions<Object>
    {
    }

    /// <summary>
    /// Represents a collection of configured types and functions that produce them using an
    /// <see cref="ObjectFactory{TProductBase}" />.
    /// </summary>
    /// <typeparam name="TProductBase">
    /// The base type from which objects produced by the associated factory derive.
    /// </typeparam>
    public interface IObjectFactoryConfigurationProductionFunctions<TProductBase>
    {
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
            where TProduct : class, TProductBase;
    }
}