// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange.Ecdh
{
    /// <summary>
    /// Represents an asymmetric, elliptic curve Diffie-Hellman key exchange algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    public sealed class EcdhKeyPair : KeyExchangeKeyPair<ECDiffieHellman, EcdhPrivateKey, EcdhPublicKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcdhKeyPair" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal EcdhKeyPair(KeyExchangeAlgorithmSpecification algorithm)
            : base(Guid.NewGuid(), algorithm, DefaultKeyLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EcdhKeyPair" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Initializes the private key.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key pair is used.
        /// </param>
        /// <param name="provider">
        /// The algorithm provider that facilitates cryptographic operations for the key pair.
        /// </param>
        /// <returns>
        /// The private key.
        /// </returns>
        protected sealed override EcdhPrivateKey InitializePrivateKey(KeyExchangeAlgorithmSpecification algorithm, ECDiffieHellman provider)
        {
            using var keySource = new PinnedMemory(provider.ExportECPrivateKey());
            return new EcdhPrivateKey(Identifier, algorithm, keySource, KeyLifespanDuration);
        }

        /// <summary>
        /// Initializes the algorithm provider that facilitates cryptographic operations for the key pair.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key pair is used.
        /// </param>
        /// <returns>
        /// The algorithm provider that facilitates cryptographic operations for the key pair.
        /// </returns>
        protected sealed override ECDiffieHellman InitializeProvider(KeyExchangeAlgorithmSpecification algorithm) => ECDiffieHellman.Create(algorithm.ToCurve());

        /// <summary>
        /// Initializes the private key.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key pair is used.
        /// </param>
        /// <param name="provider">
        /// The algorithm provider that facilitates cryptographic operations for the key pair.
        /// </param>
        /// <returns>
        /// The private key.
        /// </returns>
        protected sealed override EcdhPublicKey InitializePublicKey(KeyExchangeAlgorithmSpecification algorithm, ECDiffieHellman provider)
        {
            var keyMemory = provider.ExportSubjectPublicKeyInfo();
            return new EcdhPublicKey(Identifier, algorithm, keyMemory, KeyLifespanDuration);
        }

        /// <summary>
        /// Represents the default length of time for which paired keys are valid.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan DefaultKeyLifespanDuration = TimeSpan.FromHours(8);
    }
}