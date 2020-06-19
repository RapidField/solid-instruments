// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a management facility for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretManager : ISecretStore
    {
        /// <summary>
        /// Asynchronously exports the specified secret and encrypts it using the master key for the current
        /// <see cref="ISecretManager" />.
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
        /// The <see cref="ISecretManager" /> does not contain a secret with the specified name.
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
        /// The <see cref="ISecretManager" /> does not contain a secret with the specified name -or- the
        /// <see cref="ISecretManager" /> does not contain a key with the specified name.
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
        /// Asynchronously exports the master key for the <see cref="ISecretManager" /> in plaintext form.
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
        /// The <see cref="ISecretManager" /> does not contain a secret with the specified name.
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
        /// The <see cref="ISecretManager" /> does not contain a key with the specified name.
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
        /// Imports all valid certificates from the current user's personal certificate store.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates();

        /// <summary>
        /// Imports all valid certificates from the specified local certificate store.
        /// </summary>
        /// <param name="storeName">
        /// The name of the store from which the certificates are imported. The default value is <see cref="StoreName.My" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="storeName" /> is not a valid store name.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates(StoreName storeName);

        /// <summary>
        /// Imports all valid certificates from the specified local certificate store.
        /// </summary>
        /// <param name="storeName">
        /// The name of the store from which the certificates are imported. The default value is <see cref="StoreName.My" />.
        /// </param>
        /// <param name="storeLocation">
        /// The location of the store from which the certificates are imported. The default value is
        /// <see cref="StoreLocation.CurrentUser" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="storeName" /> is not a valid store name -or- <paramref name="storeLocation" /> is not a valid store
        /// location.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates(StoreName storeName, StoreLocation storeLocation);

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewCascadingSymmetricKey();

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKeySecret" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The textual name to assign to the new secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretManager" /> already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewCascadingSymmetricKey(String name);

        /// <summary>
        /// Generates a new <see cref="SymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewSymmetricKey();

        /// <summary>
        /// Generates a new <see cref="SymmetricKeySecret" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The textual name to assign to the new secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretManager" /> already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewSymmetricKey(String name);
    }
}