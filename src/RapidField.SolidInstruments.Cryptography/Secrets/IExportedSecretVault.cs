// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a serializable collection of <see cref="ISecret" /> objects that were exported from an
    /// <see cref="ISecretVault" />.
    /// </summary>
    public interface IExportedSecretVault : IModel
    {
        /// <summary>
        /// Returns the collection of secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </summary>
        /// <returns>
        /// The secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </returns>
        public IEnumerable<IExportedSecret> GetExportedSecrets();

        /// <summary>
        /// Gets the unique semantic identifier for the associated <see cref="ISecretVault" />.
        /// </summary>
        public String Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the number of secrets that were exported from the associated <see cref="ISecretVault" />.
        /// </summary>
        public Int32 SecretCount
        {
            get;
        }
    }
}