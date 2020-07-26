// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature;
using RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange;
using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents a serializable asymmetric-key algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    public interface IAsymmetricPublicKeyModel : IModel<Guid>
    {
        /// <summary>
        /// Extracts the digital signature algorithm specification from <see cref="Key" />, if the key is purposed for digital
        /// signature.
        /// </summary>
        /// <returns>
        /// A value that specifies the distinct algorithm for the key.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The key is not purposed for digital signature.
        /// </exception>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        internal DigitalSignatureAlgorithmSpecification ExtractDigitalSignatureAlgorithm();

        /// <summary>
        /// Extracts the key exchange algorithm specification from <see cref="Key" />, if the key is purposed for key exchange.
        /// </summary>
        /// <returns>
        /// A value that specifies the distinct algorithm for the key.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The key is not purposed for key exchange.
        /// </exception>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        internal KeyExchangeAlgorithmSpecification ExtractKeyExchangeAlgorithm();

        /// <summary>
        /// Extracts the key memory bytes from <see cref="Key" />.
        /// </summary>
        /// <returns>
        /// The public key memory bytes.
        /// </returns>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        internal Span<Byte> ExtractKeyMemory();

        /// <summary>
        /// Extracts the purpose of the key from <see cref="Key" />.
        /// </summary>
        /// <returns>
        /// A value that specifies the purpose of the asymmetric key.
        /// </returns>
        /// <exception cref="SecurityException">
        /// <see cref="Key" /> is invalid.
        /// </exception>
        internal AsymmetricKeyPurpose ExtractKeyPurpose();

        /// <summary>
        /// Gets the date and time when the associated key expires and is no longer valid for use.
        /// </summary>
        public DateTime ExpirationTimeStamp
        {
            get;
        }

        /// <summary>
        /// Gets a textual, Base64-encoded representation of the public key.
        /// </summary>
        public String Key
        {
            get;
        }
    }
}