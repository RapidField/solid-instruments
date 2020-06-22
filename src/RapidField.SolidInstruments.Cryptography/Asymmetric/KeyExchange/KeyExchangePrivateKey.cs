// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangePrivateKey" /> is the default implementation of <see cref="IKeyExchangePrivateKey" />.
    /// </remarks>
    public abstract class KeyExchangePrivateKey : KeyExchangeKey, IKeyExchangePrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePrivateKey" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected KeyExchangePrivateKey(AsymmetricAlgorithmSpecification algorithm)
            : base(algorithm)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangePrivateKey" />.
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