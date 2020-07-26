// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    public interface IAsymmetricPublicKey : IAsymmetricKey
    {
        /// <summary>
        /// Converts the current <see cref="IAsymmetricPublicKey" /> to its textual Base64 representation.
        /// </summary>
        /// <returns>
        /// A Base64 string representation of the public key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String ToBase64String();

        /// <summary>
        /// Converts the current <see cref="IAsymmetricPublicKey" /> to a serializable model.
        /// </summary>
        /// <returns>
        /// A serializable model representation of the public key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public AsymmetricPublicKeyModel ToModel();
    }
}