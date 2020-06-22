// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for one key in an asymmetric key pair.
    /// </summary>
    public interface IAsymmetricKey : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Converts the value of the current <see cref="IAsymmetricKey" /> to a secure bit field.
        /// </summary>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="IAsymmetricKey" />.
        /// </returns>
        public ISecureMemory ToSecureMemory();

        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key is used.
        /// </summary>
        public AsymmetricAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}