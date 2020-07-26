// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a secure container for named secret values.
    /// </summary>
    public interface ISecretStore : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Removes and safely disposes of all secrets that are stored by the current <see cref="ISecretStore" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void Clear();

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
        public Boolean TryRemove(String name);

        /// <summary>
        /// Gets the unique semantic identifier for the <see cref="ISecretStore" />.
        /// </summary>
        public String Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the number of secrets that are stored by the <see cref="ISecretStore" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 SecretCount
        {
            get;
        }

        /// <summary>
        /// Gets the textual names that uniquely identify the secrets that are stored by the <see cref="ISecretStore" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public IEnumerable<String> SecretNames
        {
            get;
        }
    }
}