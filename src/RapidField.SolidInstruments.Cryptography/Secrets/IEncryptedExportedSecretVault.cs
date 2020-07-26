// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a symmetrically encrypted <see cref="ExportedSecretVault" />.
    /// </summary>
    public interface IEncryptedExportedSecretVault : IEncryptedModel<ExportedSecretVault>
    {
        /// <summary>
        /// Decrypts and reconstitutes the exported secrets.
        /// </summary>
        /// <param name="key">
        /// The key that is used to decrypt the exported secrets.
        /// </param>
        /// <returns>
        /// The plaintext secrets.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public IEnumerable<IReadOnlySecret> ToPlaintextSecrets(ISymmetricKey key);

        /// <summary>
        /// Decrypts and reconstitutes the exported secrets.
        /// </summary>
        /// <param name="key">
        /// The key that is used to decrypt the exported secrets.
        /// </param>
        /// <returns>
        /// The plaintext secrets.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="BadImageFormatException">
        /// The type assembly or one of its dependencies is invalid.
        /// </exception>
        /// <exception cref="FileLoadException">
        /// The type assembly could not be loaded.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public IEnumerable<IReadOnlySecret> ToPlaintextSecrets(ICascadingSymmetricKey key);

        /// <summary>
        /// Gets the name of the secret associated with the key that was used to encrypt the exported secrets.
        /// </summary>
        public String KeyName
        {
            get;
        }
    }
}