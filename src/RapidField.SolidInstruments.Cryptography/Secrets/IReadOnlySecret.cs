﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named read-only secret value that is pinned in memory and encrypted at rest.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the value.
    /// </typeparam>
    public interface IReadOnlySecret<TValue> : IReadOnlySecret
    {
        /// <summary>
        /// Decrypts the secret value, pins a copy of it in memory and performs the specified read operation against it as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="IReadOnlySecret.HasValue" />
        /// before invoking the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public void Read(Action<TValue> readAction);

        /// <summary>
        /// Asynchronously decrypts the secret value, pins a copy of it in memory and performs the specified read operation against
        /// it as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="IReadOnlySecret.HasValue" />
        /// before invoking the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public Task ReadAsync(Action<TValue> readAction);
    }

    /// <summary>
    /// Represents a named read-only secret value that is pinned in memory and encrypted at rest.
    /// </summary>
    public interface IReadOnlySecret : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Decrypts the secret value, pins it in memory and performs the specified read operation against the resulting bytes as a
        /// thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="HasValue" /> before invoking
        /// the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public void Read(Action<IReadOnlyPinnedMemory<Byte>> readAction);

        /// <summary>
        /// Asynchronously decrypts the secret value, pins it in memory and performs the specified read operation against the
        /// resulting bytes as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="readAction">
        /// The read operation to perform.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="readAction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The secret does not have a value. This exception can be avoided by evaluating <see cref="HasValue" /> before invoking
        /// the method.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="readAction" /> raised an exception.
        /// </exception>
        public Task ReadAsync(Action<IReadOnlyPinnedMemory<Byte>> readAction);

        /// <summary>
        /// Regenerates and replaces the in-memory key that is used to secure the current <see cref="IReadOnlySecret{TValue}" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        internal void RegenerateInMemoryKey();

        /// <summary>
        /// Gets a globally unique identifier that is derived from a cryptographically secure hash of the secret value, or
        /// <see cref="Guid.Empty" /> if <see cref="HasValue" /> is <see langword="false" />.
        /// </summary>
        public Guid DerivedIdentity
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IReadOnlySecret{TValue}" /> has a value.
        /// </summary>
        public Boolean HasValue
        {
            get;
        }

        /// <summary>
        /// Gets a textual name that uniquely identifies the current <see cref="IReadOnlySecret{TValue}" />.
        /// </summary>
        public String Name
        {
            get;
        }

        /// <summary>
        /// Gets the type of the secret value.
        /// </summary>
        public Type ValueType
        {
            get;
        }

        /// <summary>
        /// Gets the date and time when the in-memory key that is used to secure the current <see cref="IReadOnlySecret{TValue}" />
        /// was generated.
        /// </summary>
        internal DateTime InMemoryKeyTimeStamp
        {
            get;
        }
    }
}