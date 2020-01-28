// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
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
        public Task ReadAsync(String name, Action<IReadOnlyPinnedBuffer<Byte>> readAction) => ReadAsync<IReadOnlyPinnedBuffer<Byte>>(name, readAction.RejectIf().IsNull(nameof(readAction)));

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
                    using (var controlToken = StateControl.Enter())
                    {
                        LazyReferenceManager.Dispose();
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

                    throw new SecretAccessException("The secret vault does not contain a valid secret of the specified type.");
                }

                throw new ArgumentException("The secret vault does not contain a secret with the specified name.", nameof(name));
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
        /// Gets a utility that disposes of the secrets that are managed by the current <see cref="SecretVault" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IReferenceManager ReferenceManager => LazyReferenceManager.Value;

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