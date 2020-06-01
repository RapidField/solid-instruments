// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        {
            LazyReferenceManager = new Lazy<IReferenceManager>(() => new ReferenceManager(), LazyThreadSafetyMode.ExecutionAndPublication);
            Secrets = new Dictionary<String, IReadOnlySecret>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretVault" /> class.
        /// </summary>
        /// <param name="password">
        /// A 13+ character password from which a master key is derived, which is used as the default encryption key for exported
        /// secrets. A random master key is generated on demand if the parameterless constructor is used.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="password" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="password" /> is shorter than thirteen characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="password" /> is <see langword="null" />.
        /// </exception>
        public SecretVault(String password)
            : this()
        {
            using (var masterKey = CascadingSymmetricKey.FromPassword(password))
            {
                var masterKeySecret = CascadingSymmetricKeySecret.FromValue(MasterKeyName, masterKey);
                ReferenceManager.AddObject(masterKeySecret);
                Secrets.Add(masterKeySecret.Name, masterKeySecret);
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
                            using (var masterKey = CascadingSymmetricKey.New())
                            {
                                var masterKeySecret = CascadingSymmetricKeySecret.FromValue(keyName, masterKey);
                                AddOrUpdate(MasterKeyName, masterKeySecret, controlToken);
                                return await ExportEncryptedSecretAsync(exportedSecret, masterKeySecret).ConfigureAwait(false);
                            }
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
        /// Decrypts the specified secret using the vault's master key and imports it.
        /// </summary>
        /// <remarks>
        /// When using this method, note that the master key of the current <see cref="ISecretVault" /> must match the key that was
        /// used when exporting the secret. If the exporting vault is not the current vault, the master keys will need to have been
        /// synchronized beforehand.
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
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var keySecret = Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                ImportSecret(secret.ToPlaintextModel(key), controlToken);
                            }).Wait();
                            return;
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                ImportSecret(secret.ToPlaintextModel(key), controlToken);
                            }).Wait();
                            return;
                        }

                        throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecretAccessException($"The specified key, \"{keyName}\", could not be used to import the specified secret. Decryption or deserialization failed.", exception);
                    }
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
        public override String ToString() => $"{{ \"{nameof(Count)}\": {Count} }}";

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
                        using (var controlToken = StateControl.Enter())
                        {
                            LazyReferenceManager.Dispose();
                        }
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

            return encryptedExportedSecret ?? throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret));
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
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

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
        /// Gets the number of secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 Count
        {
            get
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    return Secrets.Count;
                }
            }
        }

        /// <summary>
        /// Gets the textual names that uniquely identify the secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> Names
        {
            get
            {
                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();

                    foreach (var name in Secrets.Keys)
                    {
                        yield return name;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a utility that disposes of the secrets that are managed by the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IReferenceManager ReferenceManager => LazyReferenceManager.Value;

        /// <summary>
        /// Represents the name of the master key stored within every <see cref="SecretVault" /> instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const String MasterKeyName = "__MasterKey";

        /// <summary>
        /// Represents the lazily-initialized utility that disposes of the secrets that are managed by the current
        /// <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IReferenceManager> LazyReferenceManager;

        /// <summary>
        /// Represents a collection of secrets that are stored by the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDictionary<String, IReadOnlySecret> Secrets;
    }
}