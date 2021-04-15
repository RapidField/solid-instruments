// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

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
    public abstract class ObjectBuilder<TResult> : ObjectBuilder, IObjectBuilder<TResult>
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TResult ToResult() => WithStateControl(controlToken =>
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
        });

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectBuilder{TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
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

        /// <summary>
        /// Gets the type of the object that the builder composes and configures.
        /// </summary>
        public sealed override Type ResultType => ResultTypeValue;

        /// <summary>
        /// Represents the type of the object that the builder composes and configures.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeValue = typeof(TResult);
    }

    /// <summary>
    /// Represents an object that configures and produces new object instances.
    /// </summary>
    /// <remarks>
    /// <see cref="ObjectBuilder" /> is the default implementation of <see cref="IObjectBuilder" />.
    /// </remarks>
    public abstract class ObjectBuilder : Instrument, IObjectBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder" /> class.
        /// </summary>
        protected ObjectBuilder()
            : base()
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ObjectBuilder" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the type of the object that the builder composes and configures.
        /// </summary>
        public abstract Type ResultType
        {
            get;
        }
    }
}