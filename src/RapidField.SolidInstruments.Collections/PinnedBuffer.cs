// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="PinnedBuffer" /> is the default implementation of <see cref="IPinnedBuffer" />.
    /// </remarks>
    public class PinnedBuffer : PinnedBuffer<Byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedBuffer(Int32 length)
            : base(length)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedBuffer(Byte[] field)
            : base(field)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer" /> class.
        /// </summary>
        /// <param name="length">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the buffer with zeros upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedBuffer(Int32 length, Boolean overwriteWithZerosOnDispose)
            : base(length, overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the buffer with zeros upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedBuffer(Byte[] field, Boolean overwriteWithZerosOnDispose)
            : base(field, overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="PinnedBuffer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="PinnedBuffer{T}" /> is the default implementation of <see cref="IPinnedBuffer{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The element type of the buffer.
    /// </typeparam>
    public class PinnedBuffer<T> : ReadOnlyPinnedBuffer<T>, IPinnedBuffer<T>
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedBuffer(Int32 length)
            : this(length, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedBuffer(T[] field)
            : this(field, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the buffer with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedBuffer(Int32 length, Boolean overwriteWithZerosOnDispose)
            : this(new T[length.RejectIf().IsLessThanOrEqualTo(0, nameof(length))], overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the buffer with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedBuffer(T[] field, Boolean overwriteWithZerosOnDispose)
            : base(field)
        {
            OverwriteWithZerosOnDispose = overwriteWithZerosOnDispose;
        }

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
        public new T this[Int32 index]
        {
            get => Field[index];
            set => Field[index] = value;
        }

        /// <summary>
        /// Facilitates implicit <typeparamref name="T" /> array to <see cref="PinnedBuffer{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator PinnedBuffer<T>(T[] target) => target is null ? null : new PinnedBuffer<T>(target);

        /// <summary>
        /// Facilitates implicit <see cref="PinnedBuffer{T}" /> to <see cref="ReadOnlySpan{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlySpan<T>(PinnedBuffer<T> target) => target is null ? ReadOnlySpan<T>.Empty : target.ReadOnlySpan;

        /// <summary>
        /// Facilitates implicit <see cref="PinnedBuffer{T}" /> to <see cref="Span{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Span<T>(PinnedBuffer<T> target) => target is null ? Span<T>.Empty : target.Span;

        /// <summary>
        /// Facilitates implicit <see cref="PinnedBuffer{T}" /> to <typeparamref name="T" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator T[](PinnedBuffer<T> target) => target?.Field;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="PinnedBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="PinnedBuffer{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets the pinned array of elements underlying the current <see cref="PinnedBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// The pinned array of elements underlying the current <see cref="PinnedBuffer{T}" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public T[] GetField()
        {
            RejectIfDisposed();
            return Field;
        }

        /// <summary>
        /// Overwrites the current <see cref="PinnedBuffer{T}" /> with default values.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void OverwriteWithZeros()
        {
            RejectIfDisposed();
            OverwriteWithZeros(this);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="PinnedBuffer{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing && IsDisposed == false && OverwriteWithZerosOnDispose)
                {
                    OverwriteWithZeros(this);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Overwrites the specified <see cref="PinnedBuffer{T}" /> with default values.
        /// </summary>
        /// <typeparam name="TElement">
        /// The element type of the buffer.
        /// </typeparam>
        [DebuggerHidden]
        private static void OverwriteWithZeros<TElement>(PinnedBuffer<TElement> buffer)
            where TElement : struct, IComparable, IComparable<TElement>, IConvertible, IEquatable<TElement>, IFormattable => Array.Clear(buffer.Field, 0, buffer.Length);

        /// <summary>
        /// Gets a <see cref="Span{T}" /> for the current <see cref="PinnedBuffer{T}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Span<T> Span
        {
            get
            {
                RejectIfDisposed();
                return FieldMemory.Span;
            }
        }

        /// <summary>
        /// Represents a value indicating whether or not to overwrite the array with default values upon disposal.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean OverwriteWithZerosOnDispose;
    }
}