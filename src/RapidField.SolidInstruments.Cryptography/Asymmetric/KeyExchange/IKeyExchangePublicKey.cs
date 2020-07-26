// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    public interface IKeyExchangePublicKey : IAsymmetricPublicKey, IKeyExchangeKey
    {
        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key is used.
        /// </summary>
        public KeyExchangeAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}