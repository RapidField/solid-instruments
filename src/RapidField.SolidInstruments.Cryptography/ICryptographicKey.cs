// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a cryptographic algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the cryptographic algorithm for which a key is derived.
    /// </typeparam>
    public interface ICryptographicKey<TAlgorithm> : ICryptographicKey
        where TAlgorithm : struct, Enum
    {
        /// <summary>
        /// Converts the current <see cref="ICryptographicKey{TAlgorithm}" /> to cryptographic key plaintext with correct bit-length
        /// for the encryption mode specified by <see cref="Algorithm" />.
        /// </summary>
        /// <returns>
        /// The derived key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ISecureMemory ToDerivedKeyBytes();

        /// <summary>
        /// Gets the symmetric-key algorithm for which a key is derived.
        /// </summary>
        public TAlgorithm Algorithm
        {
            get;
        }
    }

    /// <summary>
    /// Represents a cryptographic algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    public interface ICryptographicKey : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Converts the value of the current <see cref="ICryptographicKey" /> to a secure bit field.
        /// </summary>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="ICryptographicKey" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public ISecureMemory ToSecureMemory();
    }
}