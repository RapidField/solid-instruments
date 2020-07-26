// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents an import facility for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretValueImporter : IAsyncDisposable, IDisposable
    {
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
    }
}