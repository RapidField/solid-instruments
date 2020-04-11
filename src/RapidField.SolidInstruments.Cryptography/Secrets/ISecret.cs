// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a named secret value that is pinned in memory and encrypted at rest.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the value.
    /// </typeparam>
    public interface ISecret<TValue> : IReadOnlySecret<TValue>, ISecret
    {
        /// <summary>
        /// Performs the specified write operation and encrypts the resulting value as a thread-safe, atomic operation.
        /// </summary>
        /// <param name="writeFunction">
        /// The write operation to perform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writeFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecretAccessException">
        /// <paramref name="writeFunction" /> raised an exception or returned an invalid <typeparamref name="TValue" />.
        /// </exception>
        public void Write(Func<TValue> writeFunction);
    }

    /// <summary>
    /// Represents a named secret value that is pinned in memory and encrypted at rest.
    /// </summary>
    public interface ISecret : IReadOnlySecret
    {
    }
}