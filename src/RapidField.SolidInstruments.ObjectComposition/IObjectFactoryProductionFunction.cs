// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a function that produces an object of a specified type.
    /// </summary>
    internal interface IObjectFactoryProductionFunction
    {
        /// <summary>
        /// Invokes the function and returns an output object.
        /// </summary>
        /// <returns>
        /// The function output.
        /// </returns>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public Object Invoke();

        /// <summary>
        /// Gets the type of the object produced by the function.
        /// </summary>
        public Type ProductType
        {
            get;
        }
    }
}