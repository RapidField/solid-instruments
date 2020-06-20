// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Secrets;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a centralized facility for safeguarding digital secrets and performing cryptographic operations.
    /// </summary>
    /// <remarks>
    /// <see cref="SoftwareSecurityModule" /> is the default implementation of <see cref="ISoftwareSecurityModule" />.
    /// </remarks>
    public sealed class SoftwareSecurityModule : SoftwareSecurityModule<SecretStoreFilePersistenceVehicle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if a passwordless
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
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        public SoftwareSecurityModule(IPassword masterPassword)
            : this(masterPassword, SecretStoreFilePersistenceVehicle.DefaultDeleteStateFileUponDisposal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="filePath">
        /// The full or relative path of the persisted state file. The default value is a pre-defined local path incorporating the
        /// semantic identity of the in-memory store.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="filePath" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath" /> is invalid, or the caller does not have access to the path.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        public SoftwareSecurityModule(String filePath)
            : this(SecretStoreFilePersistenceVehicle.DefaultDeleteStateFileUponDisposal, filePath)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if a passwordless
        /// constructor is used.
        /// </param>
        /// <param name="filePath">
        /// The full or relative path of the persisted state file. The default value is a pre-defined local path incorporating the
        /// semantic identity of the in-memory store.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="filePath" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath" /> is invalid, or the caller does not have access to the path -or-
        /// <paramref name="masterPassword" /> does not comply with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="masterPassword" /> is <see langword="null" /> -or- <paramref name="filePath" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="masterPassword" /> is disposed.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        public SoftwareSecurityModule(IPassword masterPassword, String filePath)
            : this(masterPassword, SecretStoreFilePersistenceVehicle.DefaultDeleteStateFileUponDisposal, filePath)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if a passwordless
        /// constructor is used.
        /// </param>
        /// <param name="deleteStateFileUponDisposal">
        /// A value indicating whether or not the persisted state file should be deleted when the persistence vehicle is disposed.
        /// The default value is <see langword="false" />.
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
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        [DebuggerHidden]
        internal SoftwareSecurityModule(IPassword masterPassword, Boolean deleteStateFileUponDisposal)
            : this(new SecretStoreFilePersistenceVehicle(new SecretVault(masterPassword), deleteStateFileUponDisposal))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="deleteStateFileUponDisposal">
        /// A value indicating whether or not the persisted state file should be deleted when the persistence vehicle is disposed.
        /// The default value is <see langword="false" />.
        /// </param>
        /// <param name="filePath">
        /// The full or relative path of the persisted state file. The default value is a pre-defined local path incorporating the
        /// semantic identity of the in-memory store.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="filePath" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath" /> is invalid, or the caller does not have access to the path.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        [DebuggerHidden]
        internal SoftwareSecurityModule(Boolean deleteStateFileUponDisposal, String filePath)
            : this(new SecretStoreFilePersistenceVehicle(new SecretVault(), deleteStateFileUponDisposal, filePath))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="masterPassword">
        /// A <see cref="PasswordCompositionRequirements.Strict" /> compliant password from which to derive the master key, which is
        /// used as the default encryption key for exported secrets. A master key is generated on demand if a passwordless
        /// constructor is used.
        /// </param>
        /// <param name="deleteStateFileUponDisposal">
        /// A value indicating whether or not the persisted state file should be deleted when the persistence vehicle is disposed.
        /// The default value is <see langword="false" />.
        /// </param>
        /// <param name="filePath">
        /// The full or relative path of the persisted state file. The default value is a pre-defined local path incorporating the
        /// semantic identity of the in-memory store.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="filePath" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath" /> is invalid, or the caller does not have access to the path -or-
        /// <paramref name="masterPassword" /> does not comply with <see cref="PasswordCompositionRequirements.Strict" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="masterPassword" /> is <see langword="null" /> -or- <paramref name="filePath" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="masterPassword" /> is disposed.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        [DebuggerHidden]
        internal SoftwareSecurityModule(IPassword masterPassword, Boolean deleteStateFileUponDisposal, String filePath)
            : this(new SecretStoreFilePersistenceVehicle(new SecretVault(masterPassword), deleteStateFileUponDisposal, filePath))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule" /> class.
        /// </summary>
        /// <param name="persistenceVehicle">
        /// A provider that facilitates persistence of the in-memory secret store.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="persistenceVehicle" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="persistenceVehicle" /> is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// The file path specified by <paramref name="persistenceVehicle" /> references an existing file -and- an exception was
        /// raised while loading persisted state.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        [DebuggerHidden]
        private SoftwareSecurityModule(SecretStoreFilePersistenceVehicle persistenceVehicle)
            : base(persistenceVehicle, String.IsNullOrEmpty(persistenceVehicle?.FilePath) ? false : File.Exists(persistenceVehicle.FilePath))
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SoftwareSecurityModule" />.
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
                    PersistenceVehicle?.InMemoryStore.Dispose();
                    PersistenceVehicle?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }

    /// <summary>
    /// Represents a centralized facility for safeguarding digital secrets and performing cryptographic operations.
    /// </summary>
    /// <remarks>
    /// <see cref="SoftwareSecurityModule{TPersistenceVehicle}" /> is the default implementation of
    /// <see cref="ISoftwareSecurityModule{TPersistenceVehicle}" />.
    /// </remarks>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public abstract class SoftwareSecurityModule<TPersistenceVehicle> : PersistentSecretStore<TPersistenceVehicle>, ISoftwareSecurityModule<TPersistenceVehicle>
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule{TPersistenceVehicle}" /> class.
        /// </summary>
        /// <param name="persistenceVehicle">
        /// A provider that facilitates persistence of the in-memory secret store.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="persistenceVehicle" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="persistenceVehicle" /> is disposed.
        /// </exception>
        protected SoftwareSecurityModule(TPersistenceVehicle persistenceVehicle)
            : base(persistenceVehicle)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareSecurityModule{TPersistenceVehicle}" /> class.
        /// </summary>
        /// <param name="persistenceVehicle">
        /// A provider that facilitates persistence of the in-memory secret store.
        /// </param>
        /// <param name="loadPersistedState">
        /// A value indicating whether or not to load persisted state during construction. The default value is
        /// <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="persistenceVehicle" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="persistenceVehicle" /> is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// <paramref name="loadPersistedState" /> is <see langword="true" /> -and- an exception was raised while loading persisted
        /// state.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        protected SoftwareSecurityModule(TPersistenceVehicle persistenceVehicle, Boolean loadPersistedState)
            : base(persistenceVehicle, loadPersistedState)
        {
            return;
        }

        /// <summary>
        /// Decrypts the specified model using the master key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to decrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to decrypt.
        /// </param>
        /// <returns>
        /// The decrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel Decrypt<TModel>(IEncryptedModel<TModel> model)
            where TModel : class, IModel
        {
            EnsureExistenceOfMasterKey();
            return Decrypt(model, InMemoryStore.MasterKeyName);
        }

        /// <summary>
        /// Decrypts the specified model using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to decrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to encrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when encrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The decrypted model.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption or deserialization.
        /// </exception>
        public TModel Decrypt<TModel>(IEncryptedModel<TModel> model, String keyName)
            where TModel : class, IModel
        {
            _ = model.RejectIf().IsNull(nameof(model));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (InMemoryStore.Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var plaintextModel = (TModel)null;
                    var keySecret = InMemoryStore.Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                plaintextModel = model.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                plaintextModel = model.ToPlaintextModel(key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecurityException($"The specified key, \"{keyName}\", could not be used to decrypt the specified model. Decryption or deserialization failed.", exception);
                    }

                    return plaintextModel;
                }
                else if (keyName == InMemoryStore.MasterKeyName)
                {
                    throw new SecurityException("The model cannot be decrypted without specifying an explicit key because the secret store does not have a master key.");
                }

                throw new ArgumentException($"The secret store does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

        /// <summary>
        /// Decrypts the specified Base-64 encoded ciphertext string using the master key.
        /// </summary>
        /// <param name="ciphertext">
        /// The Base-64 encoded ciphertext string to decrypt.
        /// </param>
        /// <returns>
        /// The resulting plaintext string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption.
        /// </exception>
        public String Decrypt(String ciphertext)
        {
            EnsureExistenceOfMasterKey();
            return Decrypt(ciphertext, InMemoryStore.MasterKeyName);
        }

        /// <summary>
        /// Decrypts the specified Base-64 encoded ciphertext string using the specified key.
        /// </summary>
        /// <param name="ciphertext">
        /// The Base-64 encoded ciphertext string to decrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when decrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The resulting plaintext string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="ciphertext" /> is empty -or- <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ciphertext" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during decryption.
        /// </exception>
        public String Decrypt(String ciphertext, String keyName)
        {
            _ = ciphertext.RejectIf().IsNullOrEmpty(nameof(ciphertext));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (InMemoryStore.Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var plaintext = (String)null;
                    var keySecret = InMemoryStore.Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                plaintext = SymmetricStringProcessor.Instance.DecryptFromBase64String(ciphertext, key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                plaintext = SymmetricStringProcessor.Instance.DecryptFromBase64String(ciphertext, key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecurityException($"The specified key, \"{keyName}\", could not be used to decrypt the specified string. Decryption failed.", exception);
                    }

                    return plaintext;
                }
                else if (keyName == InMemoryStore.MasterKeyName)
                {
                    throw new SecurityException("The string cannot be decrypted without specifying an explicit key because the secret store does not have a master key.");
                }

                throw new ArgumentException($"The secret store does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

        /// <summary>
        /// Encrypts the specified model using the master key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to encrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to encrypt.
        /// </param>
        /// <returns>
        /// The encrypted model.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public IEncryptedModel<TModel> Encrypt<TModel>(TModel model)
            where TModel : class, IModel
        {
            EnsureExistenceOfMasterKey();
            return Encrypt(model, InMemoryStore.MasterKeyName);
        }

        /// <summary>
        /// Encrypts the specified model using the specified key.
        /// </summary>
        /// <typeparam name="TModel">
        /// The type of the model to encrypt.
        /// </typeparam>
        /// <param name="model">
        /// The model to encrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when encrypting the model, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The encrypted model.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption or serialization.
        /// </exception>
        public IEncryptedModel<TModel> Encrypt<TModel>(TModel model, String keyName)
            where TModel : class, IModel
        {
            _ = model.RejectIf().IsNull(nameof(model));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (InMemoryStore.Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var encryptedModel = (IEncryptedModel<TModel>)null;
                    var keySecret = InMemoryStore.Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                encryptedModel = model.Encrypt(key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                encryptedModel = model.Encrypt(key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecurityException($"The specified key, \"{keyName}\", could not be used to encrypt the specified model. Encryption or serialization failed.", exception);
                    }

                    return encryptedModel;
                }
                else if (keyName == InMemoryStore.MasterKeyName)
                {
                    throw new SecurityException("The model cannot be encrypted without specifying an explicit key because the secret store does not have a master key.");
                }

                throw new ArgumentException($"The secret store does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext string using the master key.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext string to encrypt.
        /// </param>
        /// <returns>
        /// The resulting Base-64 encoded, encrypted string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption.
        /// </exception>
        public String Encrypt(String plaintext)
        {
            EnsureExistenceOfMasterKey();
            return Encrypt(plaintext, InMemoryStore.MasterKeyName);
        }

        /// <summary>
        /// Encrypts the specified plaintext string using the specified key.
        /// </summary>
        /// <param name="plaintext">
        /// The plaintext string to encrypt.
        /// </param>
        /// <param name="keyName">
        /// The name of the key to use when encrypting the plaintext string, or <see langword="null" /> to use the master key.
        /// </param>
        /// <returns>
        /// The resulting Base-64 encoded, encrypted string.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="plaintext" /> is empty -or- <paramref name="keyName" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The secret store does not contain a key with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="plaintext" /> is <see langword="null" /> -or- <paramref name="keyName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised during encryption.
        /// </exception>
        public String Encrypt(String plaintext, String keyName)
        {
            _ = plaintext.RejectIf().IsNullOrEmpty(nameof(plaintext));

            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (InMemoryStore.Secrets.ContainsKey(keyName.RejectIf().IsNullOrEmpty(nameof(keyName))))
                {
                    var ciphertext = (String)null;
                    var keySecret = InMemoryStore.Secrets[keyName];

                    try
                    {
                        if (keySecret.ValueType == typeof(SymmetricKey))
                        {
                            ((SymmetricKeySecret)keySecret).ReadAsync((SymmetricKey key) =>
                            {
                                ciphertext = SymmetricStringProcessor.Instance.EncryptToBase64String(plaintext, key);
                            }).Wait();
                        }
                        else if (keySecret.ValueType == typeof(CascadingSymmetricKey))
                        {
                            ((CascadingSymmetricKeySecret)keySecret).ReadAsync((CascadingSymmetricKey key) =>
                            {
                                ciphertext = SymmetricStringProcessor.Instance.EncryptToBase64String(plaintext, key);
                            }).Wait();
                        }
                        else
                        {
                            throw new ArgumentException($"The specified key name, \"{keyName}\" does not reference a valid key.", nameof(keySecret.Name));
                        }
                    }
                    catch (AggregateException exception)
                    {
                        throw new SecurityException($"The specified key, \"{keyName}\", could not be used to encrypt the specified string. Encryption failed.", exception);
                    }

                    return ciphertext;
                }
                else if (keyName == InMemoryStore.MasterKeyName)
                {
                    throw new SecurityException("The string cannot be encrypted without specifying an explicit key because the secret store does not have a master key.");
                }

                throw new ArgumentException($"The secret store does not contain a key with the specified name, \"{keyName}\".", nameof(keyName));
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
        public void ImportStoreCertificates()
        {
            RejectIfDisposed();
            InMemoryStore.ImportStoreCertificates();
            PersistInMemoryStore();
        }

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
        public void ImportStoreCertificates(StoreName storeName)
        {
            RejectIfDisposed();
            InMemoryStore.ImportStoreCertificates(storeName);
            PersistInMemoryStore();
        }

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
            InMemoryStore.ImportStoreCertificates(storeName, storeLocation);
            PersistInMemoryStore();
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
            RejectIfDisposed();
            var keyName = InMemoryStore.NewCascadingSymmetricKey();
            PersistInMemoryStore();
            return keyName;
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
        /// The software security module already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewCascadingSymmetricKey(String name)
        {
            RejectIfDisposed();
            InMemoryStore.NewCascadingSymmetricKey(name);
            PersistInMemoryStore();
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
            RejectIfDisposed();
            var keyName = InMemoryStore.NewSymmetricKey();
            PersistInMemoryStore();
            return keyName;
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
        /// The software security module already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewSymmetricKey(String name)
        {
            RejectIfDisposed();
            InMemoryStore.NewSymmetricKey(name);
            PersistInMemoryStore();
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SoftwareSecurityModule{TPersistenceVehicle}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Creates a master key for the in-memory store and persists it if one does not exist.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        [DebuggerHidden]
        private void EnsureExistenceOfMasterKey()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                if (InMemoryStore.Secrets.ContainsKey(InMemoryStore.MasterKeyName) == false)
                {
                    _ = InMemoryStore.CreateMasterKey();
                    PersistInMemoryStore();
                }
            }
        }

        /// <summary>
        /// Gets the secret reading facility for the current <see cref="SoftwareSecurityModule{TPersistenceVehicle}" />.
        /// </summary>
        public ISecretReader SecretReader => InMemoryStore;
    }
}