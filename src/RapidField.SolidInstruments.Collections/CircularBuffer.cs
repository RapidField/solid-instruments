// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a thread-safe, contiguous, generic collection of elements.
    /// </summary>
    /// <remarks>
    /// <see cref="CircularBuffer{T}" /> is the default implementation of <see cref="ICircularBuffer{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The element type of the collection.
    /// </typeparam>
    public class CircularBuffer<T> : Instrument, ICircularBuffer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularBuffer{T}" /> class.
        /// </summary>
        /// <param name="capacity">
        /// The maximum number of elements that the buffer can accommodate.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity" /> is less than one.
        /// </exception>
        public CircularBuffer(Int32 capacity)
            : base(ConcurrencyControlMode.SingleThreadSpinLock)
        {
            Capacity = capacity.RejectIf().IsLessThan(1, nameof(capacity));
            ElementArray = new T[Capacity];
            Length = 0;
        }

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
        public T this[Int32 index]
        {
            get
            {
                using (var controlToken = StateControl.Enter())
                {
                    var adjustedIndex = ReadIndex + index;
                    return (adjustedIndex < Capacity) ? ElementArray[adjustedIndex] : ElementArray[adjustedIndex - Capacity];
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="CircularBuffer{T}" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="CircularBuffer{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Reads the element at the head of the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// The element at the head of the buffer.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The buffer is empty.
        /// </exception>
        public T Read()
        {
            using (var controlToken = StateControl.Enter())
            {
                if (--Length < 0)
                {
                    Length = 0;
                    throw new InvalidOperationException("A read operation failed because the buffer is empty.");
                }

                var element = ElementArray[ReadIndex];

                if (++ReadIndex == Capacity)
                {
                    ReadIndex = 0;
                }

                return element;
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="CircularBuffer{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="CircularBuffer{T}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Capacity)}\": {Capacity}, \"{nameof(Length)}\": {Length} }}";

        /// <summary>
        /// Writes an element at the tail of the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        /// <param name="element">
        /// The element to write to the buffer.
        /// </param>
        public void Write(T element)
        {
            using (var controlToken = StateControl.Enter())
            {
                ElementArray[WriteIndex] = element;

                if (Length < Capacity)
                {
                    Length++;
                }

                if (++WriteIndex == Capacity)
                {
                    WriteIndex = 0;
                }
            }
        }

        /// <summary>
        /// Writes an element at the tail of the current <see cref="CircularBuffer{T}" />.
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
        public void Write(T element, Boolean permitOverwrite)
        {
            if (permitOverwrite == false && Length == Capacity)
            {
                throw new InvalidOperationException("A write operation failed because it would have overwritten an unread element in the buffer.");
            }

            Write(element);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the maximum number of elements that the current <see cref="CircularBuffer{T}" /> can accommodate.
        /// </summary>
        public Int32 Capacity
        {
            get;
        }

        /// <summary>
        /// Gets the number of elements contained by the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        public Int32 Length
        {
            get;
            private set;
        }

        /// <summary>
        /// Represents the underlying collection for the current <see cref="CircularBuffer{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T[] ElementArray;

        /// <summary>
        /// Represents the buffer read position as an index of <see cref="ElementArray" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 ReadIndex = 0;

        /// <summary>
        /// Represents the buffer write position as an index of <see cref="ElementArray" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 WriteIndex = 0;
    }
}