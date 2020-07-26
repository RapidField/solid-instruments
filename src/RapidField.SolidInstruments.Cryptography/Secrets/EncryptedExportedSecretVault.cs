// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a symmetrically encrypted <see cref="ExportedSecretVault" />.
    /// </summary>
    /// <remarks>
    /// <see cref="EncryptedExportedSecretVault" /> is the default implementation of <see cref="IEncryptedExportedSecretVault" />.
    /// </remarks>
    [DataContract]
    public sealed class EncryptedExportedSecretVault : EncryptedModel<ExportedSecretVault>, IEncryptedExportedSecretVault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedExportedSecret" /> class.
        /// </summary>
        /// <param name="exportedSecretVault">
        /// The exported secret collection which is encrypted and wrapped by the model.
        /// </param>
        /// <param name="key">
        /// The key that is used to encrypt <paramref name="exportedSecretVault" />.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with <paramref name="key" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exportedSecretVault" /> is <see langword="null" /> -or- <paramref name="key" /> is
        /// <see langword="null" />
        /// -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedExportedSecretVault(ExportedSecretVault exportedSecretVault, ISymmetricKey key, String keyName)
            : this(SymmetricProcessor.ForType<ExportedSecretVault>().Encrypt(exportedSecretVault.RejectIf().IsNull(nameof(exportedSecretVault)), key.RejectIf().IsNull(nameof(key)).TargetArgument), keyName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedExportedSecret" /> class.
        /// </summary>
        /// <param name="exportedSecretVault">
        /// The exported secret collection which is encrypted and wrapped by the model.
        /// </param>
        /// <param name="key">
        /// The key that is used to encrypt <paramref name="exportedSecretVault" />.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with <paramref name="key" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exportedSecretVault" /> is <see langword="null" /> -or- <paramref name="key" /> is
        /// <see langword="null" />
        /// -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedExportedSecretVault(ExportedSecretVault exportedSecretVault, ICascadingSymmetricKey key, String keyName)
            : this(SymmetricProcessor.ForType<ExportedSecretVault>().Encrypt(exportedSecretVault.RejectIf().IsNull(nameof(exportedSecretVault)), key.RejectIf().IsNull(nameof(key)).TargetArgument), keyName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedExportedSecret" /> class.
        /// </summary>
        /// <param name="ciphertext">
        /// The ciphertext of the exported secret.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with the key that was used to produce <paramref name="ciphertext" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" />
        /// -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private EncryptedExportedSecretVault(Byte[] ciphertext, String keyName)
            : base(ciphertext)
        {
            KeyName = keyName.RejectIf().IsNullOrEmpty(nameof(keyName));
        }

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
        public IEnumerable<IReadOnlySecret> ToPlaintextSecrets(ISymmetricKey key) => ToPlaintextModel(key).GetExportedSecrets().Select(exportedSecret => exportedSecret.ToSecret());

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
        public IEnumerable<IReadOnlySecret> ToPlaintextSecrets(ICascadingSymmetricKey key) => ToPlaintextModel(key).GetExportedSecrets().Select(exportedSecret => exportedSecret.ToSecret());

        /// <summary>
        /// Gets or sets the name of the secret associated with the key that was used to encrypt the exported secrets.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// The specified string is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The specified string is <see langword="null" />.
        /// </exception>
        [DataMember]
        public String KeyName
        {
            get => KeyNameReference;
            set => KeyNameReference = value.RejectIf().IsNullOrEmpty(nameof(KeyName));
        }

        /// <summary>
        /// Represents the name of the secret associated with the key that was used to encrypt the exported secrets.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String KeyNameReference;
    }
}