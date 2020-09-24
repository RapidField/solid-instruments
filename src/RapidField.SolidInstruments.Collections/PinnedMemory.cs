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
    /// <see cref="PinnedMemory" /> is the default implementation of <see cref="IPinnedMemory" />.
    /// </remarks>
    public class PinnedMemory : PinnedMemory<Byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than zero.
        /// </exception>
        public PinnedMemory(Int32 length)
            : base(length)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedMemory(Byte[] field)
            : base(field)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory" /> class.
        /// </summary>
        /// <param name="length">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the memory field with zeros upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than zero.
        /// </exception>
        public PinnedMemory(Int32 length, Boolean overwriteWithZerosOnDispose)
            : base(length, overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the memory field with zeros upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedMemory(Byte[] field, Boolean overwriteWithZerosOnDispose)
            : base(field, overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="PinnedMemory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="PinnedMemory{T}" /> is the default implementation of <see cref="IPinnedMemory{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The element type of the memory field.
    /// </typeparam>
    public class PinnedMemory<T> : ReadOnlyPinnedMemory<T>, IPinnedMemory<T>
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedMemory(Int32 length)
            : this(length, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedMemory(T[] field)
            : this(field, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the memory field with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedMemory(Int32 length, Boolean overwriteWithZerosOnDispose)
            : this(new T[length.RejectIf().IsLessThanOrEqualTo(0, nameof(length))], overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the memory field with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedMemory(T[] field, Boolean overwriteWithZerosOnDispose)
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
        /// Facilitates implicit <typeparamref name="T" /> array to <see cref="PinnedMemory{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator PinnedMemory<T>(T[] target) => target is null ? null : new PinnedMemory<T>(target);

        /// <summary>
        /// Facilitates implicit <see cref="PinnedMemory{T}" /> to <see cref="ReadOnlySpan{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlySpan<T>(PinnedMemory<T> target) => target is null ? ReadOnlySpan<T>.Empty : target.ReadOnlySpan;

        /// <summary>
        /// Facilitates implicit <see cref="PinnedMemory{T}" /> to <see cref="Span{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator Span<T>(PinnedMemory<T> target) => target is null ? Span<T>.Empty : target.Span;

        /// <summary>
        /// Facilitates implicit <see cref="PinnedMemory{T}" /> to <typeparamref name="T" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator T[](PinnedMemory<T> target) => target?.Field;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="PinnedMemory{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="PinnedMemory{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Overwrites the current <see cref="PinnedMemory{T}" /> with default values.
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
        /// Releases all resources consumed by the current <see cref="PinnedMemory{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (IsDisposed == false && OverwriteWithZerosOnDispose)
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
        /// Overwrites the specified <see cref="PinnedMemory{T}" /> with default values.
        /// </summary>
        /// <typeparam name="TElement">
        /// The element type of the memory field.
        /// </typeparam>
        /// <param name="memory">
        /// The memory field to overwrite.
        /// </param>
        [DebuggerHidden]
        private static void OverwriteWithZeros<TElement>(PinnedMemory<TElement> memory)
            where TElement : struct, IComparable, IComparable<TElement>, IConvertible, IEquatable<TElement>, IFormattable => Array.Clear(memory.Field, 0, memory.Length);

        /// <summary>
        /// Gets a <see cref="Span{T}" /> for the current <see cref="PinnedMemory{T}" />.
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