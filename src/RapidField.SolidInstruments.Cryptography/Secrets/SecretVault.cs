// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a secure container for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    /// <remarks>
    /// <see cref="SecretVault" /> is the default implementation of <see cref="ISecretVault" />.
    /// </remarks>
    public sealed class SecretVault : Instrument, ISecretVault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretVault" /> class.
        /// </summary>
        public SecretVault()
            : this(semanticIdentity: null)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretVault" /> class.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if the parameterless
        /// constructor is used.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="masterPassword" /> does not comply with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="masterPassword" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="masterPassword" /> is disposed.
        /// </exception>
        public SecretVault(IPassword masterPassword)
            : this(masterPassword?.DerivedIdentity.ToSerializedString(), masterPassword)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretVault" /> class.
        /// </summary>
        /// <param name="semanticIdentity">
        /// The unique, lowercase alphanumeric portion of the semantic identifier for the secret vault, or <see langword="null" />
        /// to generate a random identity.
        /// </param>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="semanticIdentity" /> is not null or empty -and- is not a lowercase alphanumeric string.
        /// </exception>
        public SecretVault(String semanticIdentity)
            : base()
        {
            LazyReferenceManager = new Lazy<IReferenceManager>(() => new ReferenceManager(), LazyThreadSafetyMode.ExecutionAndPublication);
            Secrets = new Dictionary<String, IReadOnlySecret>();
            SemanticIdentity = semanticIdentity.IsNullOrEmpty() ? Secret.NewRandomSemanticIdentifier() : semanticIdentity.RejectIf().DoesNotMatchRegularExpression(SemanticIdentityRegularExpression, nameof(semanticIdentity));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretVault" /> class.
        /// </summary>
        /// <param name="semanticIdentity">
        /// The unique, lowercase alphanumeric portion of the semantic identifier for the secret vault, or <see langword="null" />
        /// to generate a random identity.
        /// </param>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if the parameterless
        /// constructor is used.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="masterPassword" /> does not comply with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="masterPassword" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="masterPassword" /> is disposed.
        /// </exception>
        /// <exception cref="StringArgumentPatternException">
        /// <paramref name="semanticIdentity" /> is not null or empty -and- is not a lowercase alphanumeric string.
        /// </exception>
        public SecretVault(String semanticIdentity, IPassword masterPassword)
            : this(semanticIdentity)
        {
            _ = CreateMasterKey(masterPassword);
        }

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, Byte[] secret) => AddOrUpdate(name, Secret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, String secret) => AddOrUpdate(name, StringSecret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, Guid secret) => AddOrUpdate(name, GuidSecret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, Double secret) => AddOrUpdate(name, NumericSecret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, SymmetricKey secret) => AddOrUpdate(name, SymmetricKeySecret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, CascadingSymmetricKey secret) => AddOrUpdate(name, CascadingSymmetricKeySecret.FromValue(name, secret));

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void AddOrUpdate(String name, X509Certificate2 secret) => AddOrUpdate(name, X509CertificateSecret.FromValue(name, secret));

        /// <summary>
        /// Removes and safely disposes of all secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Clear()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                try
                {
                    foreach (var secret in Secrets.Values)
                    {
                        try
                        {
                            secret?.Dispose();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                finally
                {
                    Secrets.Clear();
                }
            }
        }

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
        public Task<EncryptedExportedSecretVault> ExportAsync() => ExportAsync(MasterKeyName);

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
        public async Task<EncryptedExportedSecretVault> ExportAsync(String keyName)
        {
            var names = SecretNames.ToArray();
            var secrets = new List<ExportedSecret>();

            foreach (var name in names)
            {
                var exportedSecret = await ExportSecretAsync(name).ConfigureAwait(false);
                secrets.Add(exportedSecret);
            }

            var exportedSecretVault = new ExportedSecretVault(Identifier, secrets);

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    return await ExportEncryptedSecretVaultAsync(exportedSecretVault, Secrets[keyName]).ConfigureAwait(false);
                }
                else if (keyName == MasterKeyName)
                {
                    return await ExportEncryptedSecretVaultAsync(exportedSecretVault, CreateMasterKey()).ConfigureAwait(false);
                }

                throw new ArgumentException($"The secret vault does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

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
        public Task<EncryptedExportedSecret> ExportEncryptedSecretAsync(String name) => ExportEncryptedSecretAsync(name, MasterKeyName);

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
        public async Task<EncryptedExportedSecret> ExportEncryptedSecretAsync(String name, String keyName)
        {
            RejectIfDisposed();

            if (Secrets.ContainsKey(name.RejectIf().IsNullOrEmpty(nameof(name))))
            {
                var exportedSecret = await ExportSecretAsync(name).ConfigureAwait(false);

                try
                {
                    using (var controlToken = StateControl.Enter())
                    {
                        RejectIfDisposed();

                        if (Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                        {
                            return await ExportEncryptedSecretAsync(exportedSecret, Secrets[keyName]).ConfigureAwait(false);
                        }
                        else if (keyName == MasterKeyName)
                        {
                            return await ExportEncryptedSecretAsync(exportedSecret, CreateMasterKey()).ConfigureAwait(false);
                        }

                        throw new ArgumentException($"The secret vault does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
                    }
                }
                finally
                {
                    exportedSecret.ClearValue();
                }
            }

            throw new ArgumentException($"The secret vault does not contain a secret with the specified name, \"{name}\".", nameof(name));
        }

        /// <summary>
        /// Asynchronously exports the vault's master key in plaintext form.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation and containing the exported master key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Task<ExportedSecret> ExportMasterKeyAsync()
        {
            RejectIfDisposed();

            if (Secrets.ContainsKey(MasterKeyName))
            {
                return ExportSecretAsync(MasterKeyName);
            }

            return Task.Factory.StartNew(() =>
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    return new ExportedSecret(CreateMasterKey());
                }
            });
        }

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
        public Task<ExportedSecret> ExportSecretAsync(String name) => Task.Factory.StartNew(() =>
        {
            return ExportSecret(name);
        });

        /// <summary>
        /// Decrypts the specified secret using the master key of the current <see cref="SecretVault" /> and imports it.
        /// </summary>
        /// <remarks>
        /// When using this method, note that the master key of the current <see cref="SecretVault" /> must match the key that was
        /// used when exporting the secret. If the exporting vault is not the current <see cref="SecretVault" />, the master keys
        /// will need to have been synchronized beforehand.
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
        public void ImportEncryptedSecret(IEncryptedExportedSecret secret) => ImportEncryptedSecret(secret, MasterKeyName);

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
        public void ImportEncryptedSecret(IEncryptedExportedSecret secret, String keyName)
        {
            _ = secret.RejectIf().IsNull(nameof(secret));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var exportedSecret = (IExportedSecret)null;
                    var keySecret = Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                exportedSecret = secret.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                exportedSecret = secret.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecretAccessException($"The specified key, \"{keyName}\", could not be used to import the specified secret. Decryption or deserialization failed.", exception);
                    }

                    ImportSecret(exportedSecret, controlToken);
                    return;
                }
                else if (keyName == MasterKeyName)
                {
                    throw new SecretAccessException("The encrypted secret cannot be imported without specifying an explicit key because the secret vault does not have a master key.");
                }

                throw new ArgumentException($"The secret vault does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

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
        public void ImportEncryptedSecretVault(IEncryptedExportedSecretVault secretVault) => ImportEncryptedSecretVault(secretVault, MasterKeyName);

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
        public void ImportEncryptedSecretVault(IEncryptedExportedSecretVault secretVault, String keyName)
        {
            _ = secretVault.RejectIf().IsNull(nameof(secretVault));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var keySecret = Secrets[keyName];
                    var exportedVault = (IExportedSecretVault)null;

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                exportedVault = secretVault.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                exportedVault = secretVault.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecretAccessException($"The specified key, \"{keyName}\", could not be used to import the specified secret vault. Decryption or deserialization failed.", exception);
                    }

                    var exportedSecrets = exportedVault.GetExportedSecrets();

                    foreach (var exportedSecret in exportedSecrets)
                    {
                        ImportSecret(exportedSecret, controlToken);
                    }

                    return;
                }
                else if (keyName == MasterKeyName)
                {
                    throw new SecretAccessException("The encrypted secret cannot be imported without specifying an explicit key because the secret vault does not have a master key.");
                }

                throw new ArgumentException($"The secret vault does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

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
        public void ImportSecret(IExportedSecret secret)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                ImportSecret(secret, controlToken);
            }
        }

        /// <summary>
        /// Imports all valid certificates from the current user's personal certificate store.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates() => ImportStoreCertificates(StoreName.My);

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
        public void ImportStoreCertificates(StoreName storeName) => ImportStoreCertificates(storeName, StoreLocation.CurrentUser);

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
        public void ImportStoreCertificates(StoreName storeName, StoreLocation storeLocation)
        {
            RejectIfDisposed();
            using var certificateStore = new X509Store(storeName, storeLocation);

            try
            {
                certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            }
            catch (CryptographicException exception)
            {
                throw new SecurityException("Failed to import store certificates. The specified store is unreadable.", exception);
            }
            catch (SecurityException exception)
            {
                throw new SecurityException("Failed to import store certificates. The user does not have permission to access the specified store.", exception);
            }

            var certificates = certificateStore.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true);

            foreach (var certificate in certificates)
            {
                try
                {
                    if (certificate.Verify())
                    {
                        AddOrUpdate(X509CertificateSecret.NewX509CertificateSecretName(certificate.Thumbprint), certificate);
                    }
                }
                catch (CryptographicException exception)
                {
                    throw new SecurityException($"Failed to import store certificates. Certificate \"{certificate.Thumbprint}\" is unreadable.", exception);
                }
            }
        }

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewCascadingSymmetricKey()
        {
            var secret = CascadingSymmetricKeySecret.New();
            AddOrUpdate(secret.Name, secret);
            return secret.Name;
        }

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
        /// The secret vault already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewCascadingSymmetricKey(String name)
        {
            if (SecretNames.Contains(name.RejectIf().IsNullOrEmpty(name)))
            {
                throw new ArgumentException($"Failed to generate a new cascading symmetric key. The secret vault already contains a secret with the name \"{name}\".", nameof(name));
            }

            using var value = CascadingSymmetricKey.New();
            AddOrUpdate(name, CascadingSymmetricKeySecret.FromValue(name, value));
        }

        /// <summary>
        /// Generates a new <see cref="SymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewSymmetricKey()
        {
            var secret = SymmetricKeySecret.New();
            AddOrUpdate(secret.Name, secret);
            return secret.Name;
        }

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
        /// The secret vault already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewSymmetricKey(String name)
        {
            if (SecretNames.Contains(name.RejectIf().IsNullOrEmpty(name)))
            {
                throw new ArgumentException($"Failed to generate a new symmetric key. The secret vault already contains a secret with the name \"{name}\".", nameof(name));
            }

            using var value = SymmetricKey.New();
            AddOrUpdate(name, SymmetricKeySecret.FromValue(name, value));
        }

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<IReadOnlyPinnedMemory<Byte>> readAction) => ReadAsync<IReadOnlyPinnedMemory<Byte>>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<String> readAction) => ReadAsync<String>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<Guid> readAction) => ReadAsync<Guid>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<Double> readAction) => ReadAsync<Double>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<SymmetricKey> readAction) => ReadAsync<SymmetricKey>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<CascadingSymmetricKey> readAction) => ReadAsync<CascadingSymmetricKey>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        public Task ReadAsync(String name, Action<X509Certificate2> readAction) => ReadAsync<X509Certificate2>(name, readAction.RejectIf().IsNull(nameof(readAction)));

        /// <summary>
        /// Converts the value of the current <see cref="SecretVault" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="SecretVault" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(SecretCount)}\": {SecretCount} }}";

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
        public Boolean TryRemove(String name)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(name.RejectIf().IsNullOrEmpty(nameof(name))))
                {
                    if (Secrets.Remove(name, out var secret))
                    {
                        secret?.Dispose();
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Creates and stores a new master key for the current <see cref="SecretVault" />.
        /// </summary>
        /// <returns>
        /// The resulting master key secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        internal IReadOnlySecret CreateMasterKey()
        {
            using var masterPassword = Password.NewStrongPassword();
            return CreateMasterKey(masterPassword);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecretVault" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    try
                    {
                        LazyReferenceManager.Dispose();
                    }
                    finally
                    {
                        Secrets.Clear();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void AddOrUpdate(String name, IReadOnlySecret secret)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();
                AddOrUpdate(name, secret, controlToken);
            }
        }

        /// <summary>
        /// Adds the specified secret using the specified name to the current <see cref="SecretVault" />, or updates it if a secret
        /// with the same name already exists.
        /// </summary>
        /// <param name="name">
        /// A textual name that uniquely identifies <paramref name="secret" />.
        /// </param>
        /// <param name="secret">
        /// The secret value.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" /> -or- <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private void AddOrUpdate(String name, IReadOnlySecret secret, IConcurrencyControlToken controlToken)
        {
            if (Secrets.ContainsKey(name.RejectIf().IsNullOrEmpty(nameof(name))))
            {
                if (Secrets.Remove(name, out var oldSecret))
                {
                    oldSecret?.Dispose();
                }
            }

            ReferenceManager.AddObject(secret);
            Secrets.Add(name, secret.RejectIf().IsNull(nameof(secret)).TargetArgument);
        }

        /// <summary>
        /// Creates and stores a new master key for the current <see cref="SecretVault" />.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key.
        /// </param>
        /// <returns>
        /// The resulting master key secret.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="masterPassword" /> does not comply with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="masterPassword" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private IReadOnlySecret CreateMasterKey(IPassword masterPassword)
        {
            RejectIfDisposed();

            if (masterPassword.MeetsRequirements(MasterKeyPasswordCompositionRequirements))
            {
                using var masterKey = CascadingSymmetricKey.FromPassword(masterPassword);
                var masterKeySecret = CascadingSymmetricKeySecret.FromValue(MasterKeyName, masterKey);
                ReferenceManager.AddObject(masterKeySecret);
                Secrets.Add(masterKeySecret.Name, masterKeySecret);
                return masterKeySecret;
            }

            throw new ArgumentException("The specified password does not comply with the composition requirements for secret vault master keys.", nameof(masterPassword));
        }

        /// <summary>
        /// Asynchronously exports the specified secret and encrypts it using the specified key.
        /// </summary>
        /// <param name="exportedSecret">
        /// The secret to export.
        /// </param>
        /// <param name="keySecret">
        /// The key secret that is used to encrypt the exported secret.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The specified key secret is not a valid key.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        private async Task<EncryptedExportedSecret> ExportEncryptedSecretAsync(ExportedSecret exportedSecret, IReadOnlySecret keySecret)
        {
            var encryptedExportedSecret = (EncryptedExportedSecret)null;
            var keyName = keySecret.Name;

            if (keySecret.ValueType == typeof(SymmetricKey))
            {
                await ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                {
                    encryptedExportedSecret = new EncryptedExportedSecret(exportedSecret, key, keyName);
                }).ConfigureAwait(false);
            }
            else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
            {
                await ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                {
                    encryptedExportedSecret = new EncryptedExportedSecret(exportedSecret, key, keyName);
                }).ConfigureAwait(false);
            }

            return encryptedExportedSecret ?? throw new ArgumentException($"The specified key name, \"{keyName}\", does not reference a valid key.", nameof(keySecret));
        }

        /// <summary>
        /// Asynchronously exports the specified secret vault and encrypts it using the specified key.
        /// </summary>
        /// <param name="exportedSecretVault">
        /// The secrets to export.
        /// </param>
        /// <param name="keySecret">
        /// The key secret that is used to encrypt the exported secrets.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The specified key secret is not a valid key.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        [DebuggerHidden]
        private async Task<EncryptedExportedSecretVault> ExportEncryptedSecretVaultAsync(ExportedSecretVault exportedSecretVault, IReadOnlySecret keySecret)
        {
            var encryptedExportedSecretVault = (EncryptedExportedSecretVault)null;
            var keyName = keySecret.Name;

            if (keySecret.ValueType == typeof(SymmetricKey))
            {
                await ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                {
                    encryptedExportedSecretVault = new EncryptedExportedSecretVault(exportedSecretVault, key, keyName);
                }).ConfigureAwait(false);
            }
            else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
            {
                await ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                {
                    encryptedExportedSecretVault = new EncryptedExportedSecretVault(exportedSecretVault, key, keyName);
                }).ConfigureAwait(false);
            }

            return encryptedExportedSecretVault ?? throw new ArgumentException($"The specified key name, \"{keyName}\", does not reference a valid key.", nameof(keySecret));
        }

        /// <summary>
        /// Exports the specified secret.
        /// </summary>
        /// <param name="name">
        /// The textual name of the secret to export.
        /// </param>
        /// <returns>
        /// The exported plaintext secret.
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
        [DebuggerHidden]
        private ExportedSecret ExportSecret(String name)
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(name.RejectIf().IsNullOrEmpty(nameof(name))))
                {
                    return new ExportedSecret(Secrets[name]);
                }

                throw new ArgumentException($"The secret vault does not contain a secret with the specified name, \"{name}\".", nameof(name));
            }
        }

        /// <summary>
        /// Imports the specified secret in plaintext form.
        /// </summary>
        /// <param name="secret">
        /// The plaintext secret to import.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secret" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        private void ImportSecret(IExportedSecret secret, IConcurrencyControlToken controlToken) => AddOrUpdate(secret.RejectIf().IsNull(nameof(secret)).TargetArgument.Name, secret.ToSecret(), controlToken);

        /// <summary>
        /// Uses a probabilistic method to randomly regenerate and replace the in-memory keys that are used to secure the secrets
        /// stored by the current <see cref="SecretVault" />, with a probability that scales toward 1 as the keys age.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void ProbabilisticallyRegenerateInMemoryKeys(IConcurrencyControlToken controlToken)
        {
            if (Convert.ToDouble(HardenedRandomNumberGenerator.Instance.GetUInt16()) / Convert.ToDouble(UInt16.MaxValue) < InMemoryKeyRegenerationProbability)
            {
                RegenerateInMemoryKeys(controlToken);
            }
        }

        /// <summary>
        /// Decrypts the specified named secret, pins a copy of it in memory, and performs the specified read operation against it
        /// as a thread-safe, atomic operation.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the secret value.
        /// </typeparam>
        /// <param name="name">
        /// A textual name that uniquely identifies the target secret.
        /// </param>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
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
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        [DebuggerHidden]
        private void Read<T>(String name, Action<T> readAction)
        {
            RejectIfDisposed();

            try
            {
                if (Secrets.TryGetValue(name.RejectIf().IsNullOrEmpty(nameof(name)), out var secret))
                {
                    var typedSecret = secret as Secret<T>;

                    if (typedSecret?.HasValue ?? false)
                    {
                        typedSecret.Read(readAction);
                        return;
                    }

                    throw new SecretAccessException($"The secret vault does not contain a valid secret of the specified type, \"{typeof(T).FullName}\".");
                }

                throw new ArgumentException($"The secret vault does not contain a secret with the specified name, \"{name}\".", nameof(name));
            }
            finally
            {
                using (var controlToken = StateControl.Enter())
                {
                    ProbabilisticallyRegenerateInMemoryKeys(controlToken);
                }
            }
        }

        /// <summary>
        /// Asynchronously decrypts the specified named secret, pins a copy of it in memory, and performs the specified read
        /// operation against it as a thread-safe, atomic operation.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the secret value.
        /// </typeparam>
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
        /// The secret vault does not contain a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception -or- the secret vault does not contain a valid secret of the
        /// specified type.
        /// </exception>
        [DebuggerHidden]
        private Task ReadAsync<T>(String name, Action<T> readAction)
        {
            RejectIfDisposed();

            return Task.Factory.StartNew(() =>
            {
                Read(name, readAction);
            });
        }

        /// <summary>
        /// Regenerates and replaces the in-memory keys that are used to secure the secrets stored by the current
        /// <see cref="SecretVault" />.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void RegenerateInMemoryKeys(IConcurrencyControlToken controlToken)
        {
            foreach (var secret in Secrets.Values)
            {
                controlToken.AttachTask(secret.RegenerateInMemoryKey);
            }
        }

        /// <summary>
        /// Gets the unique semantic identifier for the current <see cref="SecretVault" />.
        /// </summary>
        public String Identifier => Secret.GetPrefixedSemanticIdentifier(SecretVaultIdentifierPrefix, SemanticIdentity);

        /// <summary>
        /// Gets the number of secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 SecretCount
        {
            get
            {
                RejectIfDisposed();
                return Secrets.Count;
            }
        }

        /// <summary>
        /// Gets the textual names that uniquely identify the secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> SecretNames
        {
            get
            {
                RejectIfDisposed();
                return Secrets.Keys.ToArray();
            }
        }

        /// <summary>
        /// Gets the number of <see cref="SymmetricKeySecret" /> and <see cref="CascadingSymmetricKeySecret" /> objects that are
        /// stored by the <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 SymmetricKeySecretCount => SymmetricKeySecretNames.Count();

        /// <summary>
        /// Gets the textual names that uniquely identify the <see cref="SymmetricKeySecret" /> and
        /// <see cref="CascadingSymmetricKeySecret" /> objects that are stored by the <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> SymmetricKeySecretNames
        {
            get
            {
                RejectIfDisposed();
                return Secrets.Where(secret => secret.Value.ValueType == typeof(SymmetricKey) || secret.Value.ValueType == typeof(CascadingSymmetricKey)).Select(secret => secret.Key).ToArray();
            }
        }

        /// <summary>
        /// Gets the number of <see cref="X509CertificateSecret" /> objects that are stored by the <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 X509CertificateSecretCount => X509CertificateSecretNames.Count();

        /// <summary>
        /// Gets the textual names that uniquely identify the <see cref="X509CertificateSecret" /> objects that are stored by the
        /// <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> X509CertificateSecretNames
        {
            get
            {
                RejectIfDisposed();
                return Secrets.Where(secret => secret.Value.ValueType == typeof(X509Certificate2)).Select(secret => secret.Key).ToArray();
            }
        }

        /// <summary>
        /// Gets the name of the master key stored within the current <see cref="SecretVault" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal String MasterKeyName => Secret.GetPrefixedSemanticIdentifier(MasterKeyNamePrefix, SemanticIdentity);

        /// <summary>
        /// Gets the length of time since the oldest in-memory key in the current <see cref="SecretVault" /> was generated, or
        /// <see cref="TimeSpan.Zero" /> if the vault does not contain any secrets.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan AgeOfOldestInMemoryKey => Secrets.Any() ? TimeStamp.Current - Secrets.Values.Min(secret => secret?.InMemoryKeyTimeStamp ?? DateTime.MaxValue) : TimeSpan.Zero;

        /// <summary>
        /// Gets a conditional, per-read-operation probability which governs the frequency at which the vault's in-memory keys are
        /// regenerated and replaced.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Double InMemoryKeyRegenerationProbability => Math.Min(AgeOfOldestInMemoryKey.TotalSeconds / InMemoryKeyAgeRegenerationThresholdInSeconds, 1d);

        /// <summary>
        /// Gets a utility that disposes of the secrets that are managed by the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IReferenceManager ReferenceManager => LazyReferenceManager.Value;

        /// <summary>
        /// Represents the textual prefix for the name of the master key stored within every <see cref="SecretVault" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String MasterKeyNamePrefix = "MasterKey";

        /// <summary>
        /// Represents the textual prefix for the semantic identifier of every <see cref="SecretVault" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String SecretVaultIdentifierPrefix = "SecretVault";

        /// <summary>
        /// Represents the composition requirements for password-derived master keys.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly IPasswordCompositionRequirements MasterKeyPasswordCompositionRequirements = PasswordCompositionRequirements.Strict;

        /// <summary>
        /// Represents a collection of secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly IDictionary<String, IReadOnlySecret> Secrets;

        /// <summary>
        /// Represents the unique portion of the semantic identifier for the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly String SemanticIdentity;

        /// <summary>
        /// Gets the length of time, in seconds, for expiration of in-memory keys at which the probability of a key regeneration and
        /// replacement event becomes 1 (100%).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const Int32 InMemoryKeyAgeRegenerationThresholdInSeconds = 180;

        /// <summary>
        /// Represents a regular expression that is used to validate <see cref="SemanticIdentity" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String SemanticIdentityRegularExpression = "^[0-9a-z]*$";

        /// <summary>
        /// Represents the lazily-initialized utility that disposes of the secrets that are managed by the current
        /// <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IReferenceManager> LazyReferenceManager;
    }
}