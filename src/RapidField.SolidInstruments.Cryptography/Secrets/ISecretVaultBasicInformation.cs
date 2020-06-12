// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents basic information about a secure container for named secret values which are encrypted and pinned in memory at
    /// rest.
    /// </summary>
    public interface ISecretVaultBasicInformation : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets the unique semantic identifier for the <see cref="ISecretVault" />.
        /// </summary>
        public String Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the number of secrets that are stored by the <see cref="ISecretVault" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public Int32 SecretCount
        {
            get;
        }

        /// <summary>
        /// Gets the textual names that uniquely identify the secrets that are stored by the <see cref="ISecretVault" />.
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