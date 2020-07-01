// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security;
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
        /// Derives material bytes that are used to create a new symmetric key that can be used to secure communications with a
        /// second party by whom the specified public key was provided.
        /// </summary>
        /// <param name="secondPartyKey">
        /// A public key which, in combination with the current first party key pair, is used to derive the symmetric key material.
        /// </param>
        /// <returns>
        /// Material bytes that are used to create a new symmetric key that can be used to secure communications with the party who
        /// provided <paramref name="secondPartyKey" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The purpose and/or algorithm of <paramref name="secondPartyKey" /> does not match the current key pair.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="secondPartyKey" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// <paramref name="secondPartyKey" /> is invalid or an exception was raised while deriving the symmetric key.
        /// </exception>
        public ISecureMemory DeriveSymmetricKeyMaterial(IAsymmetricPublicKeyModel secondPartyKey)
        {
            var secondPartyKeyPurpose = secondPartyKey.ExtractKeyPurpose();

            if (secondPartyKeyPurpose != AsymmetricKeyPurpose.KeyExchange)
            {
                throw new ArgumentException("The purpose of the specified key does not match the current key pair.", nameof(secondPartyKey));
            }

            var secondPartyKeyAlgorithm = secondPartyKey.ExtractKeyExchangeAlgorithm();

            if (secondPartyKeyAlgorithm != Algorithm)
            {
                throw new ArgumentException("The algorithm of the specified key does not match the current key pair.", nameof(secondPartyKey));
            }

            var secondPartyKeyMemory = secondPartyKey.ExtractKeyMemory();

            try
            {
                return DeriveSymmetricKeyMaterial(Provider, PrivateKey, secondPartyKeyMemory);
            }
            catch (Exception exception)
            {
                throw new SecurityException("The second party key is invalid.", exception);
            }
        }

        /// <summary>
        /// Derives material bytes that are used to create a new symmetric key that can be used to secure communications with a
        /// second party by whom the specified public key was provided.
        /// </summary>
        /// <param name="provider">
        /// The asymmetric algorithm provider that facilitates key material derivation.
        /// </param>
        /// <param name="firstPartyPrivateKey">
        /// The private key which, in combination with the second party public key, is used to derive the symmetric key material.
        /// </param>
        /// <param name="secondPartyPublicKey">
        /// The public key which, in combination with the first party private key, is used to derive the symmetric key material.
        /// </param>
        /// <returns>
        /// The resulting key material bytes.
        /// </returns>
        protected abstract ISecureMemory DeriveSymmetricKeyMaterial(TProvider provider, TPrivateKey firstPartyPrivateKey, Span<Byte> secondPartyPublicKey);

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangeKeyPair{TProvider, TPrivateKey, TPublicKey}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}