// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the buffer.
    /// </typeparam>
    public interface IReadOnlyPinnedBuffer<T> : IEnumerable<T>, IReadOnlyPinnedBuffer
        where T : struct, IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Gets the element at the specified index.
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
        T this[Int32 index]
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="ReadOnlySpan{T}" /> for the current <see cref="IReadOnlyPinnedBuffer{T}" />.
        /// </summary>
        ReadOnlySpan<T> ReadOnlySpan
        {
            get;
        }
    }

    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    public interface IReadOnlyPinnedBuffer : IDisposable
    {
        /// <summary>
        /// Gets the number of elements comprising the buffer.
        /// </summary>
        Int32 Length
        {
            get;
        }

        /// <summary>
        /// Gets the length of the buffer, in bytes.
        /// </summary>
        Int32 LengthInBytes
        {
            get;
        }
    }
}