// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that configures and produces new <typeparamref name="TResult" /> instances.
    /// </summary>
    /// <typeparam name="TResult">
    /// The output type that results from the invocation of <see cref="IObjectBuilder{TResult}.ToResult" />.
    /// </typeparam>
    public interface IObjectBuilder<TResult> : IAsyncDisposable, IDisposable
        where TResult : class
    {
        /// <summary>
        /// Produces the configured <typeparamref name="TResult" /> instance.
        /// </summary>
        /// <returns>
        /// The configured <typeparamref name="TResult" /> instance.
        /// </returns>
        /// <exception cref="ObjectBuilderException">
        /// An exception was raised during finalization of the builder.
        /// </exception>
        public TResult ToResult();
    }
}