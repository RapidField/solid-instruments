// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the buffer.
    /// </typeparam>
    public interface IPinnedBuffer<T> : IEnumerable<T>, IPinnedBuffer, IReadOnlyPinnedBuffer<T>
        where T : struct, IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index of the element to access.
        /// </param>
        /// <returns>
        /// The element at the specified index if one exists.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// The specified index is out of range.
        /// </exception>
        new T this[Int32 index]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a <see cref="Span{T}" /> for the current <see cref="IPinnedBuffer{T}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        Span<T> Span
        {
            get;
        }
    }

    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    public interface IPinnedBuffer : IReadOnlyPinnedBuffer
    {
        /// <summary>
        /// Overwrites the current <see cref="IPinnedBuffer" /> with default values.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        void OverwriteWithZeros();
    }
}