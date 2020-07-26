// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <typeparamref name="TResult" /> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectBuilder{TResult}" /> is the default implementation of <see cref="IObjectBuilder{TResult}" />.
    /// </remarks>
    /// <typeparam name="TResult">
    /// The output type that results from the invocation of <see cref="ObjectBuilder{TResult}.ToResult()" />.
    /// </typeparam>
    public abstract class ObjectBuilder<TResult> : Instrument, IObjectBuilder<TResult>
        where TResult : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{TResult}" /> class.
        /// </summary>
        protected ObjectBuilder()
            : base()
        {
            return;
        }

        /// <summary>
        /// Produces the configured <typeparamref name="TResult" /> instance.
        /// </summary>
        /// <returns>
        /// The configured <typeparamref name="TResult" /> instance.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised during finalization of the builder.
        /// </exception>
        public TResult ToResult()
        {
            using (var controlToken = StateControl.Enter())
            {
                try
                {
                    return ToResult(controlToken);
                }
                catch (ObjectBuilderException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new ObjectBuilderException(GetType(), exception);
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectBuilder{TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Produces the configured <typeparamref name="TResult" /> instance.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The configured <typeparamref name="TResult" /> instance.
        /// </returns>
        protected abstract TResult ToResult(IConcurrencyControlToken controlToken);
    }
}