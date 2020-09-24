// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="DigitalSignatureKeyPair{TProvider, TPrivateKey, TPublicKey}" /> is the default implementation of
    /// <see cref="IDigitalSignatureKeyPair{TPrivateKey, TPublicKey}" />.
    /// </remarks>
    /// <typeparam name="TProvider">
    /// The type of the asymmetric algorithm provider that facilitates cryptographic operations for the key pair.
    /// </typeparam>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public abstract class DigitalSignatureKeyPair<TProvider, TPrivateKey, TPublicKey> : AsymmetricKeyPair<DigitalSignatureAlgorithmSpecification, TProvider, CryptographicKey, TPrivateKey, TPublicKey>, IDigitalSignatureKeyPair<TPrivateKey, TPublicKey>
        where TProvider : AsymmetricAlgorithm
        where TPrivateKey : DigitalSignaturePrivateKey
        where TPublicKey : DigitalSignaturePublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignatureKeyPair{TProvider, TPrivateKey, TPublicKey}" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The globally unique identifier for the key pair.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keyLifespanDuration">
        /// The length of time for which the paired keys are valid.
        /// </param>
        /// <param name="isReconstituted">
        /// A value indicating whether or not the key pair is constructed from serialized memory bits.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal to
        /// <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" /> -or- <paramref name="keyLifespanDuration" /> is less
        /// than eight seconds.
        /// </exception>
        protected DigitalSignatureKeyPair(Guid identifier, DigitalSignatureAlgorithmSpecification algorithm, TimeSpan keyLifespanDuration, Boolean isReconstituted)
            : base(identifier, algorithm, keyLifespanDuration, isReconstituted)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="DigitalSignatureKeyPair{TProvider, TPrivateKey, TPublicKey}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets a value that specifies what the key pair is used for.
        /// </summary>
        public override sealed AsymmetricKeyPurpose Purpose => AsymmetricKeyPurpose.DigitalSignature;
    }
}