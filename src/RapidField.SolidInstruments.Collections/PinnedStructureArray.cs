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

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a fixed-length array of structures that is pinned in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The element type of the structure array.
    /// </typeparam>
    public class PinnedStructureArray<T> : Instrument, IEnumerable<T>
        where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedStructureArray{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedStructureArray(Int32 length)
            : this(length, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedStructureArray{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The pinned values as an array of <typeparamref name="T" />
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedStructureArray(T[] field)
            : this(field, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedStructureArray{T}" /> class.
        /// </summary>
        /// <param name="length">
        /// The length of the array.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the array with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length" /> is less than or equal to zero.
        /// </exception>
        public PinnedStructureArray(Int32 length, Boolean overwriteWithZerosOnDispose)
            : this(new T[length.RejectIf().IsLessThanOrEqualTo(0, nameof(length))], overwriteWithZerosOnDispose)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedStructureArray{T}" /> class.
        /// </summary>
        /// <param name="field">
        /// The pinned values as an array of <typeparamref name="T" />
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the array with default values upon disposal. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field" /> is <see langword="null" />.
        /// </exception>
        public PinnedStructureArray(T[] field, Boolean overwriteWithZerosOnDispose)
            : base(ConcurrencyControlMode.SingleThreadLock)
        {
            OverwriteWithZerosOnDispose = overwriteWithZerosOnDispose;
            Handle = GCHandle.Alloc(field.RejectIf().IsNull(nameof(field)).TargetArgument, GCHandleType.Pinned);
            Field = (T[])Handle.Target;
            Length = field.Length;
            Pointer = Handle.AddrOfPinnedObject();
        }

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
            get => Field[index];
            set => Field[index] = value;
        }

        /// <summary>
        /// Facilitates implicit <see cref="PinnedStructureArray{T}" /> to <typeparamref name="T" /> array casting.
        /// </summary>
        /// <param name="target">
        /// The object to cast from.
        /// </param>
        public static implicit operator T[](PinnedStructureArray<T> target) => target.Field;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="PinnedStructureArray{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="PinnedStructureArray{T}" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return Field[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="PinnedStructureArray{T}" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="PinnedStructureArray{T}" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Overwrites the current <see cref="PinnedStructureArray{T}" /> with default values.
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
        /// Releases all resources consumed by the current <see cref="PinnedStructureArray{T}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    if (OverwriteWithZerosOnDispose)
                    {
                        OverwriteWithZeros(this);
                    }

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
        /// Overwrites the specified <see cref="PinnedStructureArray{T}" /> with default values.
        /// </summary>
        /// <typeparam name="TValue">
        /// The element type of the structure array.
        /// </typeparam>
        [DebuggerHidden]
        private static void OverwriteWithZeros<TValue>(PinnedStructureArray<TValue> array)
            where TValue : struct => Array.Clear(array, 0, array.Length);

        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        public Int32 Length
        {
            get;
        }

        /// <summary>
        /// Gets the pinned values as an array of <typeparamref name="T" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T[] Field
        {
            get;
        }

        /// <summary>
        /// Represents a handle for the array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly GCHandle Handle;

        /// <summary>
        /// Represents a value indicating whether or not to overwrite the array with default values upon disposal.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean OverwriteWithZerosOnDispose;

        /// <summary>
        /// Represents a pointer for the array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IntPtr Pointer;
    }
}