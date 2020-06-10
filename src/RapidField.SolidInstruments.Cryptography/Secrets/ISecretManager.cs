// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a management facility for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretManager : ISecretVaultBasicInformation
    {
        /// <summary>
        /// Removes and safely disposes of all secrets that are stored by the current <see cref="ISecretManager" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Clear();

        /// <summary>
        /// Asynchronously exports the specified secret and encrypts it using the vault's master key.
        /// </summary>
        /// <param name="name">
        /// The textual name of the secret to export.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported encrypted secret.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Task<EncryptedExportedSecret> ExportEncryptedSecretAsync(String name);

        /// <summary>
        /// Asynchronously exports the specified secret and encrypts it using the specified key.
        /// </summary>
        /// <param name="name">
        /// The textual name of the secret to export.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with a key that is used to encrypt the exported secret.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported encrypted secret.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty -or- <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret vault does not contain a secret with the specified name -or- the secret vault does not contain a key with the
        /// specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public Task<EncryptedExportedSecret> ExportEncryptedSecretAsync(String name, String keyName);

        /// <summary>
        /// Asynchronously exports the vault's master key in plaintext form.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported master key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<ExportedSecret> ExportMasterKeyAsync();

        /// <summary>
        /// Asynchronously exports the specified secret in plaintext form.
        /// </summary>
        /// <param name="name">
        /// The textual name of the secret to export.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported plaintext secret.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<ExportedSecret> ExportSecretAsync(String name);

        /// <summary>
        /// Decrypts the specified secret using the master key of the current <see cref="ISecretManager" /> and imports it.
        /// </summary>
        /// <remarks>
        /// When using this method, note that the master key of the current <see cref="ISecretManager" /> must match the key that
        /// was used when exporting the secret. If the exporting vault is not the current <see cref="ISecretManager" />, the master
        /// keys will need to have been synchronized beforehand.
        /// </remarks>
        /// <param name="secret">
        /// The encrypted secret to import.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public void ImportEncryptedSecret(IEncryptedExportedSecret secret);

        /// <summary>
        /// Decrypts the specified secret using the specified key and imports it.
        /// </summary>
        /// <param name="secret">
        /// The encrypted secret to import.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with a key that was used to encrypt the exported secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret vault does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secret" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public void ImportEncryptedSecret(IEncryptedExportedSecret secret, String keyName);

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
        /// The secret vault does not contain a key with the specified name.
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

        /// <summary>
        /// Imports the specified secret in plaintext form.
        /// </summary>
        /// <param name="secret">
        /// The plaintext secret to import.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void ImportSecret(IExportedSecret secret);

        /// <summary>
        /// Attempts to remove a secret with the specified name.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies the target secret.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the secret was removed, otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Boolean TryRemove(String name);
    }
}