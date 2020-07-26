// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public interface IDigitalSignatureKeyPair<TPrivateKey, TPublicKey> : IAsymmetricKeyPair<DigitalSignatureAlgorithmSpecification, CryptographicKey, TPrivateKey, TPublicKey>, IDigitalSignatureKeyPair
        where TPrivateKey : DigitalSignaturePrivateKey
        where TPublicKey : DigitalSignaturePublicKey
    {
    }

    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    public interface IDigitalSignatureKeyPair : IAsymmetricKeyPair
    {
    }
}