// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Secrets;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a centralized utility for performing cryptographic operations.
    /// </summary>
    public interface ISecurityAppliance : ICryptographicComponent, IManagedKeyCipher, ISecretCertificateImporter, ISecretKeyProducer
    {
        /// <summary>
        /// Gets the secret reading facility for the current <see cref="ISecurityAppliance" />.
        /// </summary>
        public ISecretReader SecretReader
        {
            get;
        }
    }
}