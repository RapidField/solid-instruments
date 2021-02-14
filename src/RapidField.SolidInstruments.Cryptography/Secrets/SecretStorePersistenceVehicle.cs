// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemTimeoutException = System.TimeoutException;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a utility that provides state persistence for an in-memory <see cref="ISecretStore" />.
    /// </summary>
    /// <remarks>
    /// <see cref="SecretStorePersistenceVehicle" /> is the default implementation of <see cref="ISecretStorePersistenceVehicle" />.
    /// </remarks>
    public abstract class SecretStorePersistenceVehicle : SecretStorePersistenceVehicle<SecretVault>, ISecretStorePersistenceVehicle
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
        protected SecretStorePersistenceVehicle(SecretVault inMemoryStore)
            : base(inMemoryStore)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecretStorePersistenceVehicle" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
        protected abstract Task<String> LoadPersistedStateAsync(String semanticIdentity, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously requests the persisted state and hydrates <paramref name="inMemoryStore" /> using the result.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store to which state is loaded.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task LoadPersistedStateAsync(SecretVault inMemoryStore, IConcurrencyControlToken controlToken) => LoadPersistedStateAsync(inMemoryStore.SemanticIdentity, controlToken).ContinueWith(loadPersistedStateTask =>
        {
            var inMemoryStoreCiphertext = ObscurityProcessor.DecryptFromBase64String(loadPersistedStateTask.Result, ObscurityKey);
            InMemoryStore.ImportEncryptedSecretVault(inMemoryStoreCiphertext);
        });

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
        protected abstract Task PersistInMemoryStoreAsync(String semanticIdentity, String inMemoryStore);

        /// <summary>
        /// Asynchronously persists the state of <paramref name="inMemoryStore" />.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected sealed override Task PersistInMemoryStoreAsync(SecretVault inMemoryStore, IConcurrencyControlToken controlToken) => inMemoryStore.ExportAsync().ContinueWith(exportTask =>
        {
            var inMemoryStoreCiphertext = ObscurityProcessor.EncryptToBase64String(exportTask.Result, ObscurityKey);
            PersistInMemoryStoreAsync(inMemoryStore.SemanticIdentity, inMemoryStoreCiphertext).Wait();
        });

        /// <summary>
        /// Finalizes static members of the <see cref="SecretStorePersistenceVehicle" /> class.
        /// </summary>
        [DebuggerHidden]
        private static void FinalizeStaticMembers()
        {
            try
            {
                LazyObscurityKey.Dispose();
            }
            finally
            {
                LazyObscurityPassword.Dispose();
            }
        }

        /// <summary>
        /// Initializes a universal static key that is used to obscure serialized <see cref="EncryptedExportedSecretVault" />
        /// instances.
        /// </summary>
        /// <returns>
        /// A static key that is used to obscure serialized <see cref="EncryptedExportedSecretVault" /> instances.
        /// </returns>
        [DebuggerHidden]
        private static SymmetricKey InitializeObscurityKey()
        {
            try
            {
                return SymmetricKey.FromPassword(ObscurityPassword);
            }
            finally
            {
                LazyObscurityPassword.Dispose();
            }
        }

        /// <summary>
        /// Initializes the password from which <see cref="ObscurityKey" /> is derived.
        /// </summary>
        /// <returns>
        /// The password from which <see cref="ObscurityKey" /> is derived.
        /// </returns>
        [DebuggerHidden]
        private static IPassword InitializeObscurityPassword() => Password.FromAsciiString(ObscurityKeyPasswordString);

        /// <summary>
        /// Gets a universal static key that is used to obscure serialized <see cref="EncryptedExportedSecretVault" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SymmetricKey ObscurityKey => LazyObscurityKey.Value;

        /// <summary>
        /// Gets the password from which <see cref="ObscurityKey" /> is derived.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IPassword ObscurityPassword => LazyObscurityPassword.Value;

        /// <summary>
        /// Represents the plaintext password from which <see cref="ObscurityKey" /> is derived.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String ObscurityKeyPasswordString = "Z=h^Yixu[yTaL+2ISD4tr!BG";

        /// <summary>
        /// Represents a lazily-initialized, universal static key that obscures serialized
        /// <see cref="EncryptedExportedSecretVault" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<SymmetricKey> LazyObscurityKey = new(InitializeObscurityKey, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents the lazily-initialized password from which <see cref="ObscurityKey" /> is derived.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<IPassword> LazyObscurityPassword = new(InitializeObscurityPassword, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Represents the <see cref="ISymmetricProcessor{T}" /> that is used to encrypt and decrypt persisted state objects.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly ISymmetricProcessor<EncryptedExportedSecretVault> ObscurityProcessor = SymmetricProcessor.ForType<EncryptedExportedSecretVault>();

        /// <summary>
        /// Represents a finalizer for static members of the <see cref="SecretStorePersistenceVehicle" /> class.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly StaticMemberFinalizer StaticMemberFinalizer = new(FinalizeStaticMembers);
    }

    /// <summary>
    /// Represents a utility that provides state persistence for an in-memory <see cref="ISecretStore" />.
    /// </summary>
    /// <remarks>
    /// <see cref="SecretStorePersistenceVehicle{TInMemorySecretStore}" /> is the default implementation of
    /// <see cref="ISecretStorePersistenceVehicle{TInMemorySecretStore}" />.
    /// </remarks>
    /// <typeparam name="TInMemorySecretStore">
    /// The type of the in-memory <see cref="ISecretStore" /> that the persistence vehicle providers state persistence for.
    /// </typeparam>
    public abstract class SecretStorePersistenceVehicle<TInMemorySecretStore> : Instrument, ISecretStorePersistenceVehicle<TInMemorySecretStore>
        where TInMemorySecretStore : class, ISecretStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceVehicle{TInMemorySecretStore}" /> class.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted by the persistence vehicle.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inMemoryStore" /> is <see langword="null" />.
        /// </exception>
        protected SecretStorePersistenceVehicle(TInMemorySecretStore inMemoryStore)
            : this(inMemoryStore, ConcurrencyControlMode.SingleThreadLock)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretStorePersistenceVehicle{TInMemorySecretStore}" /> class.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted by the persistence vehicle.
        /// </param>
        /// <param name="stateControlMode">
        /// The concurrency control mode that is used to manage state. The default value is
        /// <see cref="ConcurrencyControlMode.SingleThreadLock" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inMemoryStore" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="stateControlMode" /> is equal to <see cref="ConcurrencyControlMode.Unspecified" />.
        /// </exception>
        protected SecretStorePersistenceVehicle(TInMemorySecretStore inMemoryStore, ConcurrencyControlMode stateControlMode)
            : base(stateControlMode)
        {
            InMemoryStore = inMemoryStore.RejectIf().IsNull(nameof(inMemoryStore));
        }

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
        public void LoadPersistedState() => LoadPersistedState(DefaultLoadPersistedStateTimeoutThreshold);

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
        public void LoadPersistedState(TimeSpan timeoutThreshold)
        {
            RejectIfDisposed();

            try
            {
                LoadPersistedStateAsync().Wait(timeoutThreshold);
            }
            catch (AggregateException exception)
            {
                if (exception.InnerExceptions.Any(innerException => innerException.GetType() == typeof(TaskCanceledException)))
                {
                    throw new SystemTimeoutException("The timeout threshold was exceeded while attempting to load persisted state.", exception);
                }

                throw new SecretStorePersistenceException(exception);
            }
        }

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
        public Task LoadPersistedStateAsync()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                try
                {
                    return LoadPersistedStateAsync(InMemoryStore, controlToken);
                }
                catch (SecretStorePersistenceException exception)
                {
                    return Task.FromException(exception);
                }
                catch (Exception exception)
                {
                    return Task.FromException(new SecretStorePersistenceException(exception));
                }
            }
        }

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
        public Task PersistInMemoryStoreAsync()
        {
            using (var controlToken = StateControl.Enter())
            {
                RejectIfDisposed();

                try
                {
                    return PersistInMemoryStoreAsync(InMemoryStore, controlToken);
                }
                catch (SecretStorePersistenceException exception)
                {
                    return Task.FromException(exception);
                }
                catch (Exception exception)
                {
                    return Task.FromException(new SecretStorePersistenceException(exception));
                }
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SecretStorePersistenceVehicle{TInMemorySecretStore}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Asynchronously requests the persisted state and hydrates <see cref="InMemoryStore" /> using the result.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store to which state is loaded.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task LoadPersistedStateAsync(TInMemorySecretStore inMemoryStore, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Asynchronously persists the state of <see cref="InMemoryStore" />.
        /// </summary>
        /// <param name="inMemoryStore">
        /// The in-memory store for which state is persisted.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        protected abstract Task PersistInMemoryStoreAsync(TInMemorySecretStore inMemoryStore, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the in-memory <see cref="ISecretStore" /> that the current
        /// <see cref="SecretStorePersistenceVehicle{TInMemorySecretStore}" /> provides state persistence for.
        /// </summary>
        public TInMemorySecretStore InMemoryStore
        {
            get;
        }

        /// <summary>
        /// Represents the default timeout threshold duration that is used by <see cref="LoadPersistedState()" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TimeSpan DefaultLoadPersistedStateTimeoutThreshold = TimeSpan.FromMinutes(1);
    }
}