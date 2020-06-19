// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a persistent, secure storage facility for named secret values.
    /// </summary>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public interface IPersistentSecretStore<TPersistenceVehicle> : IPersistentSecretStore<SecretVault, TPersistenceVehicle>
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle<SecretVault>
    {
    }

    /// <summary>
    /// Represents a persistent, secure storage facility for named secret values.
    /// </summary>
    /// <typeparam name="TInMemorySecretStore">
    /// The type of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public interface IPersistentSecretStore<TInMemorySecretStore, TPersistenceVehicle> : IPersistentSecretStore
        where TInMemorySecretStore : class, ISecretStore
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle<TInMemorySecretStore>
    {
        /// <summary>
        /// Gets an <see cref="ISecretStore" /> that represents the in-memory state of the current
        /// <see cref="IPersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
        /// </summary>
        public TInMemorySecretStore InMemoryStore
        {
            get;
        }

        /// <summary>
        /// Gets a provider that facilitates persistence of <see cref="InMemoryStore" />.
        /// </summary>
        public TPersistenceVehicle PersistenceVehicle
        {
            get;
        }
    }

    /// <summary>
    /// Represents a persistent, secure storage facility for named secret values.
    /// </summary>
    public interface IPersistentSecretStore : ISecretStore, ISecretWriter
    {
    }
}