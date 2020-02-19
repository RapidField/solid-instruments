// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.ObjectComposition
{
    /// <summary>
    /// Represents a function that produces an object of a specified type.
    /// </summary>
    /// <typeparam name="TProduct">
    /// The type of the object produced by the function.
    /// </typeparam>
    internal sealed class ObjectFactoryProductionFunction<TProduct> : ObjectFactoryProductionFunction
        where TProduct : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryProductionFunction{TProduct}" /> class.
        /// </summary>
        /// <param name="function">
        /// A function that produces an output.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="function" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        public ObjectFactoryProductionFunction(Func<TProduct> function)
            : base()
        {
            ProductType = typeof(TProduct);
            Function = function.RejectIf().IsNull(nameof(function));
        }

        /// <summary>
        /// Invokes the function and returns an output.
        /// </summary>
        /// <returns>
        /// The function output.
        /// </returns>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        [DebuggerHidden]
        public sealed override Object Invoke()
        {
            try
            {
                return Function();
            }
            catch (Exception exception)
            {
                throw new ObjectProductionException(ProductType, exception);
            }
        }

        /// <summary>
        /// Gets the type of the object produced by the function.
        /// </summary>
        public sealed override Type ProductType
        {
            get;
        }

        /// <summary>
        /// Represents a function that produces an output.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<TProduct> Function;
    }

    /// <summary>
    /// Represents a function that produces an object of a specified type.
    /// </summary>
    internal abstract class ObjectFactoryProductionFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryProductionFunction" /> class.
        /// </summary>
        [DebuggerHidden]
        protected ObjectFactoryProductionFunction()
        {
            return;
        }

        /// <summary>
        /// Invokes the function and returns an output object.
        /// </summary>
        /// <returns>
        /// The function output.
        /// </returns>
        /// <exception cref="ObjectProductionException">
        /// An exception was raised during object production.
        /// </exception>
        public abstract Object Invoke();

        /// <summary>
        /// Converts the value of the current <see cref="ObjectFactoryProductionFunction" /> to its equivalent string
        /// representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ObjectFactoryProductionFunction" />.
        /// </returns>
        public override String ToString() => $"{{ {nameof(ProductType)}: {ProductType.FullName} }}";

        /// <summary>
        /// Gets the type of the object produced by the function.
        /// </summary>
        public abstract Type ProductType
        {
            get;
        }
    }
}