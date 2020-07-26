// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a symmetrically encrypted <see cref="ExportedSecret" />.
    /// </summary>
    /// <remarks>
    /// <see cref="EncryptedExportedSecret" /> is the default implementation of <see cref="IEncryptedExportedSecret" />.
    /// </remarks>
    [DataContract]
    public sealed class EncryptedExportedSecret : EncryptedModel<ExportedSecret>, IEncryptedExportedSecret
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedExportedSecret" /> class.
        /// </summary>
        /// <param name="exportedSecret">
        /// The exported secret which is encrypted and wrapped by the model.
        /// </param>
        /// <param name="key">
        /// The key that is used to encrypt <paramref name="exportedSecret" />.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with <paramref name="key" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exportedSecret" /> is <see langword="null" /> -or- <paramref name="key" /> is <see langword="null" />
        /// -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedExportedSecret(ExportedSecret exportedSecret, ISymmetricKey key, String keyName)
            : this(SymmetricProcessor.ForType<ExportedSecret>().Encrypt(exportedSecret.RejectIf().IsNull(nameof(exportedSecret)), key.RejectIf().IsNull(nameof(key)).TargetArgument), keyName)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedExportedSecret" /> class.
        /// </summary>
        /// <param name="exportedSecret">
        /// The exported secret which is encrypted and wrapped by the model.
        /// </param>
        /// <param name="key">
        /// The key that is used to encrypt <paramref name="exportedSecret" />.
        /// </param>
        /// <param name="keyName">
        /// The name of the secret associated with <paramref name="key" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exportedSecret" /> is <see langword="null" /> -or- <paramref name="key" /> is <see langword="null" />
        /// -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        internal EncryptedExportedSecret(ExportedSecret exportedSecret, ICascadingSymmetricKey key, String keyName)
            : this(SymmetricProcessor.ForType<ExportedSecret>().Encrypt(exportedSecret.RejectIf().IsNull(nameof(exportedSecret)), key.RejectIf().IsNull(nameof(key)).TargetArgument), keyName)
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
        private EncryptedExportedSecret(Byte[] ciphertext, String keyName)
            : base(ciphertext)
        {
            KeyName = keyName.RejectIf().IsNullOrEmpty(nameof(keyName));
        }

        /// <summary>
        /// Decrypts and reconstitutes the secret.
        /// </summary>
        /// <param name="key">
        /// The key that is used to decrypt the exported secret.
        /// </param>
        /// <returns>
        /// The plaintext secret.
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
        public IReadOnlySecret ToPlaintextSecret(ISymmetricKey key) => ToPlaintextModel(key).ToSecret();

        /// <summary>
        /// Decrypts and reconstitutes the secret.
        /// </summary>
        /// <param name="key">
        /// The key that is used to decrypt the exported secret.
        /// </param>
        /// <returns>
        /// The plaintext secret.
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
        public IReadOnlySecret ToPlaintextSecret(ICascadingSymmetricKey key) => ToPlaintextModel(key).ToSecret();

        /// <summary>
        /// Gets or sets the name of the secret associated with the key that was used to encrypt the exported secret.
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
        /// Represents the name of the secret associated with the key that was used to encrypt the exported secret.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private String KeyNameReference;
    }
}