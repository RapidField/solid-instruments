// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a fixed-length bit field that is pinned in memory and encrypted at rest.
    /// </summary>
    public interface ISecureBuffer : IDisposable
    {
        /// <summary>
        /// Decrypts the buffer, performs the specified operation against the pinned plaintext and encrypts the buffer as a
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
        void Access(Action<IPinnedBuffer<Byte>> action);
    }
}