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
using System.Runtime.InteropServices;
using System.Threading;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="ReadOnlyPinnedBuffer" /> is the default implementation of <see cref="IReadOnlyPinnedBuffer" />.
    /// </remarks>
    public class ReadOnlyPinnedBuffer : ReadOnlyPinnedBuffer<Byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedBuffer" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public ReadOnlyPinnedBuffer(Int32 length)
            : base(length)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedBuffer" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public ReadOnlyPinnedBuffer(Byte[] field)
            : base(field)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReadOnlyPinnedBuffer" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="ReadOnlyPinnedBuffer{T}" /> is the default implementation of <see cref="IReadOnlyPinnedBuffer{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The element type of the buffer.
    /// </typeparam>
    public class ReadOnlyPinnedBuffer<T> : Instrument, IReadOnlyPinnedBuffer<T>
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public ReadOnlyPinnedBuffer(Int32 length)
            : this(new T[length.RejectIf().IsLessThanOrEqualTo(0, nameof(length))])
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedBuffer{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the buffer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public ReadOnlyPinnedBuffer(T[] field)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            Handle = GCHandle.Alloc(field.RejectIf().IsNull(nameof(field)).TargetArgument, GCHandleType.Pinned);
            Field = (T[])Handle.Target;
            LazyFieldMemory = new Lazy<Memory<T>>(InitializeFieldMemory, LazyThreadSafetyMode.ExecutionAndPublication);
            Length = field.Length;
            Pointer = Handle.AddrOfPinnedObject();
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
        public T this[Int32 index] => Field[index];

        /// <summary>
        /// Facilitates implicit <typeparamref name="T" /> array to <see cref="ReadOnlyPinnedBuffer{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlyPinnedBuffer<T>(T[] target) => target is null ? null : new ReadOnlyPinnedBuffer<T>(target);

        /// <summary>
        /// Facilitates implicit <see cref="ReadOnlyPinnedBuffer{T}" /> to <see cref="ReadOnlySpan{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlySpan<T>(ReadOnlyPinnedBuffer<T> target) => target is null ? ReadOnlySpan<T>.Empty : target.ReadOnlySpan;

        /// <summary>
        /// Facilitates implicit <see cref="ReadOnlyPinnedBuffer{T}" /> to <typeparamref name="T" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator T[](ReadOnlyPinnedBuffer<T> target) => target?.Field;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return Field[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing && IsDisposed == false)
                {
                    Handle.Free();
                    Pointer = IntPtr.Zero;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Initializes the lazily-initialized structure collection as a <see cref="Memory{T}" />.
        /// </summary>
        [DebuggerHidden]
        private Memory<T> InitializeFieldMemory() => new Memory<T>(Field);

        /// <summary>
        /// Gets a value indicating whether or not the buffer is empty.
        /// </summary>
        public Boolean IsEmpty => Length == 0;

        /// <summary>
        /// Gets the number of elements comprising the buffer.
        /// </summary>
        public Int32 Length
        {
            get;
        }

        /// <summary>
        /// Gets the length of the buffer, in bytes.
        /// </summary>
        public Int32 LengthInBytes => (Length * StructureSize);

        /// <summary>
        /// Gets a <see cref="ReadOnlySpan{T}" /> for the current <see cref="ReadOnlyPinnedBuffer{T}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ReadOnlySpan<T> ReadOnlySpan
        {
            get
            {
                RejectIfDisposed();
                return FieldMemory.Span;
            }
        }

        /// <summary>
        /// Gets the structure collection as a <see cref="Memory{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Memory<T> FieldMemory => LazyFieldMemory.Value;

        /// <summary>
        /// Represents the structure collection as an array of <typeparamref name="T" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly T[] Field;

        /// <summary>
        /// Represents a pointer for <see cref="Field" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal IntPtr Pointer;

        /// <summary>
        /// Represents the byte length of the element type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Int32 StructureSize = Marshal.SizeOf<T>();

        /// <summary>
        /// Represents a handle for the array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly GCHandle Handle;

        /// <summary>
        /// Represents the lazily-initialized structure collection as a <see cref="Memory{T}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Memory<T>> LazyFieldMemory;
    }
}