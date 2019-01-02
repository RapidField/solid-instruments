// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Collections
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory.
    /// </summary>
    public class PinnedBuffer : PinnedStructureArray<Byte>
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
        /// The pinned values.
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
        /// The length of the array.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the array with zeros upon disposal. The default value is
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
        /// The pinned values.
        /// </param>
        /// <param name="overwriteWithZerosOnDispose">
        /// A value indicating whether or not to overwrite the array with zeros upon disposal. The default value is
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
}