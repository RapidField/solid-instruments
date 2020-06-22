// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="DigitalSignaturePrivateKey" /> is the default implementation of <see cref="IDigitalSignaturePrivateKey" />.
    /// </remarks>
    public abstract class DigitalSignaturePrivateKey : DigitalSignatureKey, IDigitalSignaturePrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignaturePrivateKey" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected DigitalSignaturePrivateKey(AsymmetricAlgorithmSpecification algorithm)
            : base(algorithm)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DigitalSignaturePrivateKey" />.
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