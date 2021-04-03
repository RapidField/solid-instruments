// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a persistent, secure storage facility for named secret values.
    /// </summary>
    /// <remarks>
    /// <see cref="PersistentSecretStore{TPersistenceVehicle}" /> is the default implementation of
    /// <see cref="IPersistentSecretStore{TPersistenceVehicle}" />.
    /// </remarks>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public abstract class PersistentSecretStore<TPersistenceVehicle> : PersistentSecretStore<SecretVault, TPersistenceVehicle>, IPersistentSecretStore<TPersistenceVehicle>
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSecretStore{TPersistenceVehicle}" /> class.
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
        protected PersistentSecretStore(TPersistenceVehicle persistenceVehicle)
            : base(persistenceVehicle)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSecretStore{TPersistenceVehicle}" /> class.
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
        protected PersistentSecretStore(TPersistenceVehicle persistenceVehicle, Boolean loadPersistedState)
            : base(persistenceVehicle, loadPersistedState)
        {
            return;
        }

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, Byte[] secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, String secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, Guid secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, Double secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, SymmetricKey secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, CascadingSymmetricKey secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TPersistenceVehicle}" />, or updates it if a secret with the same name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected sealed override void AddOrUpdate(String name, X509Certificate2 secret, IConcurrencyControlToken controlToken) => InMemoryStore.AddOrUpdate(name, secret);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="PersistentSecretStore{TPersistenceVehicle}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Represents a persistent, secure storage facility for named secret values.
    /// </summary>
    /// <remarks>
    /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" /> is the default implementation of
    /// <see cref="IPersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
    /// </remarks>
    /// <typeparam name="TInMemorySecretStore">
    /// The type of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public abstract class PersistentSecretStore<TInMemorySecretStore, TPersistenceVehicle> : Instrument, IPersistentSecretStore<TInMemorySecretStore, TPersistenceVehicle>
        where TInMemorySecretStore : class, ISecretStore
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle<TInMemorySecretStore>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" /> class.
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
        protected PersistentSecretStore(TPersistenceVehicle persistenceVehicle)
            : this(persistenceVehicle, false)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" /> class.
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
        protected PersistentSecretStore(TPersistenceVehicle persistenceVehicle, Boolean loadPersistedState)
            : base(ConcurrencyControlMode.ProcessorCountSemaphore)
        {
            PersistenceVehicle = persistenceVehicle.RejectIf().IsNull(nameof(persistenceVehicle));
            InMemoryStore = persistenceVehicle.InMemoryStore;

            if (loadPersistedState)
            {
                PersistenceVehicle.LoadPersistedState();
            }
        }

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, Byte[] secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, String secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, Guid secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, Double secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, SymmetricKey secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, CascadingSymmetricKey secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void AddOrUpdate(String name, X509Certificate2 secret) => WithStateControl(controlToken =>
        {
            AddOrUpdate(name, secret, controlToken);
            PersistInMemoryStore();
        });

        /// <summary>
        /// Removes and safely disposes of all secrets that are stored by the current <see cref="ISecretStore" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public void Clear() => WithStateControl(controlToken =>
        {
            InMemoryStore.Clear();
            PersistInMemoryStore();
        });

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
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        public Boolean TryRemove(String name) => WithStateControl(controlToken =>
        {
            if (InMemoryStore.TryRemove(name))
            {
                PersistInMemoryStore();
                return true;
            }

            return false;
        });

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, Byte[] secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, String secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, Guid secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, Double secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, SymmetricKey secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, CascadingSymmetricKey secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Adds the specified secret using the specified name to the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />, or updates it if a secret with the same
        /// name already exists.
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
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        protected abstract void AddOrUpdate(String name, X509Certificate2 secret, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Persists the state of <see cref="InMemoryStore" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretStorePersistenceException">
        /// An exception was raised while attempting to persist the state of the in-memory store.
        /// </exception>
        protected void PersistInMemoryStore()
        {
            RejectIfDisposed();

            try
            {
                PersistenceVehicle.PersistInMemoryStoreAsync().Wait();
            }
            catch (Exception exception)
            {
                throw new SecretStorePersistenceException("Secret store persistence failed. See inner exception.", exception);
            }
        }

        /// <summary>
        /// Gets the unique semantic identifier for the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
        /// </summary>
        public String Identifier => InMemoryStore.Identifier;

        /// <summary>
        /// Gets an <see cref="ISecretStore" /> that represents the in-memory state of the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
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

        /// <summary>
        /// Gets the number of secrets that are stored by the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 SecretCount => InMemoryStore.SecretCount;

        /// <summary>
        /// Gets the textual names that uniquely identify the secrets that are stored by the current
        /// <see cref="PersistentSecretStore{TInMemorySecretStore, TPersistenceVehicle}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> SecretNames => InMemoryStore.SecretNames;
    }
}