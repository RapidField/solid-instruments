// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature.Ecdsa
{
    /// <summary>
    /// Represents an asymmetric, elliptic curve digital signature algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    public sealed class EcdsaPrivateKey : DigitalSignaturePrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcdsaPrivateKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <param name="lifespanDuration">
        /// The length of time for which the key is valid.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" /> -or- <paramref name="lifespanDuration" /> is less
        /// than eight seconds.
        /// </exception>
        [DebuggerHidden]
        internal EcdsaPrivateKey(Guid keyPairIdentifier, DigitalSignatureAlgorithmSpecification algorithm, PinnedMemory keySource, TimeSpan lifespanDuration)
            : base(keyPairIdentifier, algorithm, KeyDerivationMode, keySource, lifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcdsaPrivateKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <param name="expirationTimeStamp">
        /// The date and time when the key expires and is no longer valid for use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" /> -or- <paramref name="expirationTimeStamp" /> is
        /// equal to <see cref="DateTime.MaxValue" />.
        /// </exception>
        [DebuggerHidden]
        internal EcdsaPrivateKey(Guid keyPairIdentifier, DigitalSignatureAlgorithmSpecification algorithm, PinnedMemory keySource, DateTime expirationTimeStamp)
            : base(keyPairIdentifier, algorithm, KeyDerivationMode, keySource, expirationTimeStamp)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EcdsaPrivateKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the key derivation mode for <see cref="EcdsaPrivateKey" /> instances.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const CryptographicKeyDerivationMode KeyDerivationMode = CryptographicKeyDerivationMode.Truncation;
    }
}