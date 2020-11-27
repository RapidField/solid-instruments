// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a utility that provides file-based state persistence for an in-memory <see cref="ISecretStore" />.
    /// </summary>
    public sealed class SecretStoreFilePersistenceVehicle : SecretStorePersistenceVehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceVehicle" /> class.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted by the persistence vehicle.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inMemoryStore" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal SecretStoreFilePersistenceVehicle(SecretVault inMemoryStore)
            : this(inMemoryStore, DefaultDeleteStateFileUponDisposal)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceVehicle" /> class.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted by the persistence vehicle.
        /// </param>
        /// <param name="deleteStateFileUponDisposal">
        /// A value indicating whether or not the persisted state file should be deleted when the persistence vehicle is disposed.
        /// The default value is <see langword="false" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inMemoryStore" /> is <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal SecretStoreFilePersistenceVehicle(SecretVault inMemoryStore, Boolean deleteStateFileUponDisposal)
            : this(inMemoryStore, deleteStateFileUponDisposal, String.IsNullOrEmpty(inMemoryStore?.SemanticIdentity) ? null : $"__{inMemoryStore.SemanticIdentity}.pss")
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceVehicle" /> class.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted by the persistence vehicle.
        /// </param>
        /// <param name="deleteStateFileUponDisposal">
        /// A value indicating whether or not the persisted state file should be deleted when the persistence vehicle is disposed.
        /// The default value is <see langword="false" />.
        /// </param>
        /// <param name="filePath">
        /// The full or relative path of the persisted state file. The default value is a pre-defined local path incorporating the
        /// semantic identity of <paramref name="inMemoryStore" />.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="filePath" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath" /> is invalid, or the caller does not have access to the path.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inMemoryStore" /> is <see langword="null" /> -or- <paramref name="filePath" /> is
        /// <see langword="null" />.
        /// </exception>
        [DebuggerHidden]
        internal SecretStoreFilePersistenceVehicle(SecretVault inMemoryStore, Boolean deleteStateFileUponDisposal, String filePath)
            : base(inMemoryStore)
        {
            _ = filePath.RejectIf().IsNullOrEmpty(nameof(filePath));
            DeleteStateFileUponDisposal = deleteStateFileUponDisposal;

            try
            {
                FilePath = Path.IsPathRooted(filePath) ? filePath : Path.GetFullPath(filePath);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"The specified path, \"{filePath}\", is invalid, or the caller does not have access to the path. See inner exception.", nameof(filePath), exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecretStoreFilePersistenceVehicle" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (DeleteStateFileUponDisposal && FilePath is not null && File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Asynchronously requests and returns the persisted state.
        /// </summary>
        /// <param name="semanticIdentity">
        /// The unique portion of the semantic identifier for the in-memory store.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the obscured, Base-64-encoded persisted state.
        /// </returns>
        protected sealed override Task<String> LoadPersistedStateAsync(String semanticIdentity, IConcurrencyControlToken controlToken)
        {
            if (File.Exists(FilePath))
            {
                return File.ReadAllTextAsync(FilePath);
            }

            throw new SecretStorePersistenceException($"The specified secret state persistence file, \"{FilePath}\", does not exist or the user does not have access to it.");
        }

        /// <summary>
        /// Asynchronously persists the state of <paramref name="inMemoryStore" />.
        /// </summary>
        /// <param name="semanticIdentity">
        /// The unique portion of the semantic identifier for the in-memory store.
        /// </param>
        /// <param name="inMemoryStore">
        /// The obscured, Base64-encoded ciphertext of the in-memory store for which state is persisted.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task PersistInMemoryStoreAsync(String semanticIdentity, String inMemoryStore) => File.WriteAllTextAsync(FilePath, inMemoryStore);

        /// <summary>
        /// Gets the full path of the persisted state file.
        /// </summary>
        public String FilePath
        {
            get;
        }

        /// <summary>
        /// Represents the default value indicating whether or not the persisted state file should be deleted when the persistence
        /// vehicle is disposed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Boolean DefaultDeleteStateFileUponDisposal = false;

        /// <summary>
        /// Represents a value indicating whether or not the persisted state file should be deleted when the persistence vehicle is
        /// disposed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean DeleteStateFileUponDisposal;
    }
}