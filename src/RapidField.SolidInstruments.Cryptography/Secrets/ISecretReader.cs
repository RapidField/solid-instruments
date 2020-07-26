// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a read facility for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretReader : ISecretScalarValueReader, ISecretSymmetricKeyReader, ISecretX509CertificateReader
    {
        /// <summary>
        /// Asynchronously decrypts the specified named secret, pins a copy of it in memory, and performs the specified read
        /// operation against it as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the target secret.
        /// </param>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretStore" /> does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the <see cref="ISecretStore" /> does not contain a valid secret
        /// of the specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<IReadOnlyPinnedMemory<Byte>> readAction);

        /// <summary>
        /// Asynchronously decrypts the specified named secret, pins a copy of it in memory, and performs the specified read
        /// operation against it as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the target secret.
        /// </param>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretStore" /> does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the <see cref="ISecretStore" /> does not contain a valid secret
        /// of the specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<String> readAction);
    }
}