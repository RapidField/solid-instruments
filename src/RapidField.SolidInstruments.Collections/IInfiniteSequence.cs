// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a thread-safe, infinite sequence of calculated values.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the sequence.
    /// </typeparam>
    public interface IInfiniteSequence<T> : ICalculatedSequence<T>, IInfiniteSequence
    {
        /// <summary>
        /// Gets the term at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the term to get.
        /// </param>
        /// <returns>
        /// The term at the specified index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index" /> is less than zero.
        /// </exception>
        public T this[Int32 index]
        {
            get;
        }
    }

    /// <summary>
    /// Represents a thread-safe, infinite sequence of calculated values.
    /// </summary>
    public interface IInfiniteSequence : ICalculatedSequence
    {
        /// <summary>
        /// Clears the terms in the current <see cref="IInfiniteSequence" />, leaving in place the seed terms.
        /// </summary>
        public void Reset();
    }
}