// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in bit field and encrypted at rest.
    /// </summary>
    public interface ISecureMemory : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Decrypts the bit field, performs the specified operation against the pinned plaintext and encrypts the bit field as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="action">
        /// The operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Access(Action<PinnedMemory> action);

        /// <summary>
        /// Gets the length of the bit field, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }
    }
}