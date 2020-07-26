// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a utility that provides state persistence for an in-memory <see cref="ISecretStore" />.
    /// </summary>
    public interface ISecretStorePersistenceVehicle : ISecretStorePersistenceVehicle<SecretVault>
    {
    }

    /// <summary>
    /// Represents a utility that provides state persistence for an in-memory <see cref="ISecretStore" />.
    /// </summary>
    /// <typeparam name="TInMemorySecretStore">
    /// The type of the in-memory <see cref="ISecretStore" /> that the persistence vehicle providers state persistence for.
    /// </typeparam>
    public interface ISecretStorePersistenceVehicle<TInMemorySecretStore> : IInstrument
        where TInMemorySecretStore : class, ISecretStore
    {
        /// <summary>
        /// Requests the persisted state and hydrates <see cref="InMemoryStore" /> using the result.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while retrieving persisted state or hydrating <see cref="InMemoryStore" />.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        public void LoadPersistedState();

        /// <summary>
        /// Requests the persisted state and hydrates <see cref="InMemoryStore" /> using the result.
        /// </summary>
        /// <param name="timeoutThreshold">
        /// The maximum length of time to wait for the state to be hydrated before raising an exception, or
        /// <see cref="Timeout.InfiniteTimeSpan" /> to specify an infinite duration. The default value is one minute.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeoutThreshold" /> is a negative number other than <see cref="Timeout.Infinite" /> -or-
        /// <paramref name="timeoutThreshold" /> is greater than <see cref="Int32.MaxValue" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while retrieving persisted state or hydrating <see cref="InMemoryStore" />.
        /// </exception>
        /// <exception cref="SystemTimeoutException">
        /// The specified timeout threshold was exceeded while attempting to load persisted state.
        /// </exception>
        public void LoadPersistedState(TimeSpan timeoutThreshold);

        /// <summary>
        /// Asynchronously requests the persisted state and hydrates <see cref="InMemoryStore" /> using the result.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while retrieving persisted state or hydrating <see cref="InMemoryStore" />.
        /// </exception>
        public Task LoadPersistedStateAsync();

        /// <summary>
        /// Asynchronously persists the state of <see cref="InMemoryStore" />.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist <see cref="InMemoryStore" />.
        /// </exception>
        public Task PersistInMemoryStoreAsync();

        /// <summary>
        /// Gets the in-memory <see cref="ISecretStore" /> that the current
        /// <see cref="ISecretStorePersistenceVehicle{TInMemorySecretStore}" /> provides state persistence for.
        /// </summary>
        public TInMemorySecretStore InMemoryStore
        {
            get;
        }
    }
}