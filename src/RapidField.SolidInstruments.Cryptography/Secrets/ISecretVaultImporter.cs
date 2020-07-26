// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents an import facility for exported secret vaults.
    /// </summary>
    public interface ISecretVaultImporter : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Decrypts the specified secret vault using the master key of the current <see cref="ISecretManager" /> and imports it.
        /// </summary>
        /// <remarks>
        /// When using this method, note that the master key of the current <see cref="ISecretManager" /> must match the key that
        /// was used when exporting the secret vault. If the exporting vault is not the current <see cref="ISecretManager" />, the
        /// master keys will need to have been synchronized beforehand.
        /// </remarks>
        /// <param name="secretVault">
        /// The encrypted secret vault to import.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secretVault" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public void ImportEncryptedSecretVault(IEncryptedExportedSecretVault secretVault);

        /// <summary>
        /// Decrypts the specified secret vault using the specified key and imports it.
        /// </summary>
        /// <param name="secretVault">
        /// The encrypted secret vault to import.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with a key that was used to encrypt the exported secret vault.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretManager" /> does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secretVault" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public void ImportEncryptedSecretVault(IEncryptedExportedSecretVault secretVault, String keyName);
    }
}