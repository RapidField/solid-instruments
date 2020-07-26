// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    public interface IKeyExchangePrivateKey : IAsymmetricPrivateKey<KeyExchangeAlgorithmSpecification>, IKeyExchangeKey
    {
    }
}