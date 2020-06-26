// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature.Ecdsa
{
    /// <summary>
    /// Represents an asymmetric, elliptic curve digital signature algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    public sealed class EcdsaKeyPair : DigitalSignatureKeyPair<ECDsa, EcdsaPrivateKey, EcdsaPublicKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcdsaKeyPair" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal EcdsaKeyPair(DigitalSignatureAlgorithmSpecification algorithm)
            : base(Guid.NewGuid(), algorithm, DefaultKeyLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EcdsaKeyPair" />.
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
        protected sealed override EcdsaPrivateKey InitializePrivateKey(DigitalSignatureAlgorithmSpecification algorithm, ECDsa provider)
        {
            using var keySource = new PinnedMemory(provider.ExportECPrivateKey());
            return new EcdsaPrivateKey(Identifier, algorithm, keySource, KeyLifespanDuration);
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
        protected sealed override ECDsa InitializeProvider(DigitalSignatureAlgorithmSpecification algorithm) => ECDsa.Create(algorithm.ToCurve());

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
        protected sealed override EcdsaPublicKey InitializePublicKey(DigitalSignatureAlgorithmSpecification algorithm, ECDsa provider)
        {
            var keyMemory = provider.ExportSubjectPublicKeyInfo();
            return new EcdsaPublicKey(Identifier, algorithm, keyMemory, KeyLifespanDuration);
        }

        /// <summary>
        /// Represents the default length of time for which paired keys are valid.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan DefaultKeyLifespanDuration = TimeSpan.FromDays(366);
    }
}