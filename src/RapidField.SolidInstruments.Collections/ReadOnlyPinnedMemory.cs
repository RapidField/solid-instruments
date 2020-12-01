// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
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
    /// <see cref="ReadOnlyPinnedMemory" /> is the default implementation of <see cref="IReadOnlyPinnedMemory" />.
    /// </remarks>
    public class ReadOnlyPinnedMemory : ReadOnlyPinnedMemory<Byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedMemory" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than zero.
        /// </exception>
        public ReadOnlyPinnedMemory(Int32 length)
            : base(length)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedMemory" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public ReadOnlyPinnedMemory(Byte[] field)
            : base(field)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReadOnlyPinnedMemory" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a read-only, fixed-length bit field that is pinned in memory.
    /// </summary>
    /// <remarks>
    /// <see cref="ReadOnlyPinnedMemory{T}" /> is the default implementation of <see cref="IReadOnlyPinnedMemory{T}" />.
    /// </remarks>
    /// <typeparam name="T">
    /// The element type of the memory field.
    /// </typeparam>
    public class ReadOnlyPinnedMemory<T> : Instrument, IReadOnlyPinnedMemory<T>
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than zero.
        /// </exception>
        public ReadOnlyPinnedMemory(Int32 length)
            : this(new T[length.RejectIf().IsLessThan(0, nameof(length))])
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPinnedMemory{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The structure collection comprising the memory field.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public ReadOnlyPinnedMemory(T[] field)
            : base()
        {
            Handle = GCHandle.Alloc(field.RejectIf().IsNull(nameof(field)).TargetArgument, GCHandleType.Pinned);
            Field = (T[])Handle.Target;
            HandleIsActive = true;
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
        /// Facilitates implicit <typeparamref name="T" /> array to <see cref="ReadOnlyPinnedMemory{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlyPinnedMemory<T>(T[] target) => target is null ? null : new ReadOnlyPinnedMemory<T>(target);

        /// <summary>
        /// Facilitates implicit <see cref="ReadOnlyPinnedMemory{T}" /> to <see cref="ReadOnlySpan{T}" /> casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator ReadOnlySpan<T>(ReadOnlyPinnedMemory<T> target) => target is null ? ReadOnlySpan<T>.Empty : target.ReadOnlySpan;

        /// <summary>
        /// Facilitates implicit <see cref="ReadOnlyPinnedMemory{T}" /> to <typeparamref name="T" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator T[](ReadOnlyPinnedMemory<T> target) => target?.Field;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return Field[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override Int32 GetHashCode()
        {
            var hashCode = 0x3acf3acf;

            foreach (var element in this)
            {
                hashCode ^= element.GetHashCode() ^ 0x66666666;

                using (var memory = new ReadOnlyPinnedMemory(hashCode.ToByteArray()))
                {
                    hashCode = memory.ComputeThirtyTwoBitHash() ^ 0x33333333;
                }
            }

            return hashCode ^ 0x55555555;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ReadOnlyPinnedMemory{T}" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Length)}\": {Length}, \"{nameof(LengthInBytes)}\": {LengthInBytes} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ReadOnlyPinnedMemory{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (IsDisposed is false)
                {
                    if (HandleIsActive)
                    {
                        HandleIsActive = false;
                        Handle.Free();
                    }

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
        /// Gets a value indicating whether or not the memory field is empty.
        /// </summary>
        public Boolean IsEmpty => Length == 0;

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
        public Int32 LengthInBytes => Length * StructureSize;

        /// <summary>
        /// Gets a <see cref="ReadOnlySpan{T}" /> for the current <see cref="ReadOnlyPinnedMemory{T}" />.
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

        /// <summary>
        /// Represents a value indicating whether or not <see cref="Handle" /> is initialized and active.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean HandleIsActive = false;
    }
}