// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangeKeyPair{TPrivateKey, TPublicKey}" /> is the default implementation of
    /// <see cref="IKeyExchangeKeyPair{TPrivateKey, TPublicKey}" />.
    /// </remarks>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public abstract class KeyExchangeKeyPair<TPrivateKey, TPublicKey> : AsymmetricKeyPair<KeyExchangeKey, TPrivateKey, TPublicKey>, IKeyExchangeKeyPair<TPrivateKey, TPublicKey>
        where TPrivateKey : KeyExchangePrivateKey
        where TPublicKey : KeyExchangePublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangeKeyPair{TPrivateKey, TPublicKey}" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected KeyExchangeKeyPair(AsymmetricAlgorithmSpecification algorithm)
            : base(algorithm)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangeKeyPair{TPrivateKey, TPublicKey}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    throw new NotImplementedException();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}