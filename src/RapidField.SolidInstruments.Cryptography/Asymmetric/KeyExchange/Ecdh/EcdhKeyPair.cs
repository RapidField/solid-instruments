﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange.Ecdh
{
    /// <summary>
    /// Represents an asymmetric, elliptic curve Diffie-Hellman key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    public sealed class EcdhKeyPair : KeyExchangeKeyPair<EcdhPrivateKey, EcdhPublicKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcdhKeyPair" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal EcdhKeyPair(AsymmetricAlgorithmSpecification algorithm)
            : base(algorithm)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EcdhKeyPair" />.
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