// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

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
    }
}