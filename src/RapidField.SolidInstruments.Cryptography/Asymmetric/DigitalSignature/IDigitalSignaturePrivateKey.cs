// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    public interface IDigitalSignaturePrivateKey : IAsymmetricPrivateKey<DigitalSignatureAlgorithmSpecification>, IDigitalSignatureKey
    {
    }
}