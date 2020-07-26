// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    public interface IDigitalSignaturePublicKey : IAsymmetricPublicKey, IDigitalSignatureKey
    {
        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key is used.
        /// </summary>
        public DigitalSignatureAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}