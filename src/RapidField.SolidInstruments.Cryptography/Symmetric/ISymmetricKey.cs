// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetric-key algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    public interface ISymmetricKey : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Converts the current <see cref="ISymmetricKey" /> to symmetric-key plaintext with correct bit-length for the encryption
        /// mode specified by <see cref="Algorithm" />.
        /// </summary>
        /// <returns>
        /// The derived key.
        /// </returns>
        public ISecureMemory ToDerivedKeyBytes();

        /// <summary>
        /// Converts the value of the current <see cref="ISymmetricKey" /> to a secure bit field.
        /// </summary>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="ISymmetricKey" />.
        /// </returns>
        public ISecureMemory ToSecureMemory();

        /// <summary>
        /// Gets the symmetric-key algorithm for which a key is derived.
        /// </summary>
        public SymmetricAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}