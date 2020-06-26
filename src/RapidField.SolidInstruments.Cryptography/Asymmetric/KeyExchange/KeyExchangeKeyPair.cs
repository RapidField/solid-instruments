// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangeKeyPair{TProvider, TPrivateKey, TPublicKey}" /> is the default implementation of
    /// <see cref="IKeyExchangeKeyPair{TPrivateKey, TPublicKey}" />.
    /// </remarks>
    /// <typeparam name="TProvider">
    /// The type of the asymmetric algorithm provider that facilitates cryptographic operations for the key pair.
    /// </typeparam>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public abstract class KeyExchangeKeyPair<TProvider, TPrivateKey, TPublicKey> : AsymmetricKeyPair<KeyExchangeAlgorithmSpecification, TProvider, CryptographicKey, TPrivateKey, TPublicKey>, IKeyExchangeKeyPair<TPrivateKey, TPublicKey>
        where TProvider : AsymmetricAlgorithm
        where TPrivateKey : KeyExchangePrivateKey
        where TPublicKey : KeyExchangePublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangeKeyPair{TProvider, TPrivateKey, TPublicKey}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The globally unique identifier for the key pair.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keyLifespanDuration">
        /// The length of time for which the paired keys are valid.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal to
        /// <see cref="KeyExchangeAlgorithmSpecification.Unspecified" /> -or- <paramref name="keyLifespanDuration" /> is less than
        /// eight seconds.
        /// </exception>
        protected KeyExchangeKeyPair(Guid identifier, KeyExchangeAlgorithmSpecification algorithm, TimeSpan keyLifespanDuration)
            : base(identifier, algorithm, keyLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangeKeyPair{TProvider, TPrivateKey, TPublicKey}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}