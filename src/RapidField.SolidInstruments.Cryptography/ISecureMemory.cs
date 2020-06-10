// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;
using System.Threading.Tasks;

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
        /// Asynchronously decrypts the bit field, performs the specified operation against the pinned plaintext and encrypts the
        /// bit field as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="action">
        /// The operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task AccessAsync(Action<PinnedMemory> action);

        /// <summary>
        /// Regenerates and replaces the source bytes for the private key that is used to secure the current
        /// <see cref="ISecureMemory" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        internal void RegeneratePrivateKey();

        /// <summary>
        /// Gets the length of the bit field, in bytes.
        /// </summary>
        public Int32 LengthInBytes
        {
            get;
        }

        /// <summary>
        /// Gets the current zero-based ordinal version of the bits comprising the private key that is used to secure the current
        /// <see cref="ISecureMemory" />.
        /// </summary>
        internal UInt32 PrivateKeyVersion
        {
            get;
        }
    }
}