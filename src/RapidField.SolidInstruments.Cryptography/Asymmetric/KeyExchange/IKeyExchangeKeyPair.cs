// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public interface IKeyExchangeKeyPair<TPrivateKey, TPublicKey> : IAsymmetricKeyPair<KeyExchangeAlgorithmSpecification, CryptographicKey, TPrivateKey, TPublicKey>, IKeyExchangeKeyPair
        where TPrivateKey : KeyExchangePrivateKey
        where TPublicKey : KeyExchangePublicKey
    {
    }

    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    public interface IKeyExchangeKeyPair : IAsymmetricKeyPair
    {
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
        public ISecureMemory DeriveSymmetricKeyMaterial(IAsymmetricPublicKeyModel secondPartyKey);
    }
}