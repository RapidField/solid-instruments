// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a secure container for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretVault : ISecretManager, ISecretReader, ISecretWriter
    {
        /// <summary>
        /// Asynchronously exports the current <see cref="ISecretVault" /> and encrypts it using the vault's master key.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported encrypted secrets.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Task<EncryptedExportedSecretVault> ExportAsync();

        /// <summary>
        /// Asynchronously exports the current <see cref="ISecretVault" /> and encrypts it using the specified key.
        /// </summary>
        /// <param name="keyName">
        /// The name of the secret associated with a key that is used to encrypt the exported secrets.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported encrypted secrets.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret vault does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Task<EncryptedExportedSecretVault> ExportAsync(String keyName);
    }
}