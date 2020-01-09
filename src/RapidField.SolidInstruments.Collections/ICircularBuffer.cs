// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a thread-safe, contiguous, generic collection of elements.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the collection.
    /// </typeparam>
    public interface ICircularBuffer<T> : IDisposable, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index of the object to access.
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
        /// Reads the element at the head of the current <see cref="ICircularBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// The element at the head of the buffer.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The buffer is empty.
        /// </exception>
        T Read();

        /// <summary>
        /// Writes an element at the tail of the current <see cref="ICircularBuffer{T}" />.
        /// </summary>
        /// <param name="element">
        /// The element to write to the buffer.
        /// </param>
        void Write(T element);

        /// <summary>
        /// Writes an element at the tail of the current <see cref="ICircularBuffer{T}" />.
        /// </summary>
        /// <param name="element">
        /// The element to write to the buffer.
        /// </param>
        /// <param name="permitOverwrite">
        /// A value indicating whether or not to permit overwriting existing elements. If this argument is <see langword="false" />
        /// an <see cref="InvalidOperationException" /> is raised to prevent overwrite. The default value is
        /// <see langword="true" />.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="permitOverwrite" /> is <see langword="false" /> and the write operation would have caused overwrite.
        /// </exception>
        void Write(T element, Boolean permitOverwrite);

        /// <summary>
        /// Gets the maximum number of elements that the current <see cref="ICircularBuffer{T}" /> can accommodate.
        /// </summary>
        Int32 Capacity
        {
            get;
        }

        /// <summary>
        /// Gets the number of elements contained by the current <see cref="ICircularBuffer{T}" />.
        /// </summary>
        Int32 Length
        {
            get;
        }
    }
}