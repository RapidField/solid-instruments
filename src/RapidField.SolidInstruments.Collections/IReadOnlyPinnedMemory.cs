﻿// =================================================================================================================================
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
    /// The element type of the memory field.
    /// </typeparam>
    public interface IReadOnlyPinnedMemory<T> : IEnumerable<T>, IReadOnlyPinnedMemory
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
        public T this[Int32 index]
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="ReadOnlySpan{T}" /> for the current <see cref="IReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ReadOnlySpan<T> ReadOnlySpan
        {
            get;
        }
    }

    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    public interface IReadOnlyPinnedMemory : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether or not the memory field is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get;
        }

        /// <summary>
        /// Gets the number of elements comprising the memory field.
        /// </summary>
        public Int32 Length
        {
            get;
        }

        /// <summary>
        /// Gets the length of the memory field, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }
    }
}