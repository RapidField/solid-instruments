// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Represents an asymmetric digital signature algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="DigitalSignaturePrivateKey" /> is the default implementation of <see cref="IDigitalSignaturePrivateKey" />.
    /// </remarks>
    public abstract class DigitalSignaturePrivateKey : CryptographicKey<DigitalSignatureAlgorithmSpecification>, IDigitalSignaturePrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignaturePrivateKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> contains too many elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected DigitalSignaturePrivateKey(Guid keyPairIdentifier, DigitalSignatureAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource)
            : this(keyPairIdentifier, algorithm, derivationMode, keySource, DefaultLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignaturePrivateKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <param name="lifespanDuration">
        /// The length of time for which the key is valid. The default value is ninety (90) days.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> contains too many elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" /> -or- <paramref name="lifespanDuration" /> is less
        /// than eight seconds.
        /// </exception>
        protected DigitalSignaturePrivateKey(Guid keyPairIdentifier, DigitalSignatureAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource, TimeSpan lifespanDuration)
            : this(keyPairIdentifier, algorithm, derivationMode, keySource, TimeStamp.Current.Add(lifespanDuration.RejectIf().IsLessThan(MinimumLifespanDuration, nameof(lifespanDuration))))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignaturePrivateKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="derivationMode">
        /// The mode used to derive the output key.
        /// </param>
        /// <param name="keySource">
        /// A bit field that is used to derive key bits.
        /// </param>
        /// <param name="expirationTimeStamp">
        /// The date and time when the key expires and is no longer valid for use.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySource" /> contains too many elements.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keySource" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="DigitalSignatureAlgorithmSpecification.Unspecified" /> -or- <paramref name="expirationTimeStamp" /> is
        /// equal to <see cref="DateTime.MaxValue" />.
        /// </exception>
        protected DigitalSignaturePrivateKey(Guid keyPairIdentifier, DigitalSignatureAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource, DateTime expirationTimeStamp)
            : base(algorithm, derivationMode, keySource, algorithm.ToPrivateKeyBitLength())
        {
            Curve = algorithm.ToCurve();
            ExpirationTimeStamp = expirationTimeStamp.RejectIf().IsEqualToValue(DateTime.MaxValue, nameof(expirationTimeStamp));
            KeyPairIdentifier = keyPairIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(keyPairIdentifier));
            Purpose = AsymmetricKeyPurpose.DigitalSignature;
        }

        /// <summary>
        /// Converts the value of the current <see cref="DigitalSignaturePrivateKey" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="DigitalSignaturePrivateKey" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Purpose)}\": {Purpose}, \"{nameof(Algorithm)}\": {Algorithm}, \"{nameof(KeyPairIdentifier)}\": {KeyPairIdentifier.ToSerializedString()} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DigitalSignaturePrivateKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the date and time when the current <see cref="DigitalSignaturePrivateKey" /> expires and is no longer valid for
        /// use.
        /// </summary>
        public DateTime ExpirationTimeStamp
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="DigitalSignaturePrivateKey" /> is expired.
        /// </summary>
        public Boolean IsExpired => TimeStamp.Current >= ExpirationTimeStamp;

        /// <summary>
        /// Gets the globally unique identifier for the key pair to which the current <see cref="DigitalSignaturePrivateKey" />
        /// belongs.
        /// </summary>
        public Guid KeyPairIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that specifies what the current <see cref="DigitalSignaturePrivateKey" /> is used for.
        /// </summary>
        public AsymmetricKeyPurpose Purpose
        {
            get;
        }

        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="DigitalSignaturePrivateKey" />.
        /// </summary>
        public override sealed CryptographicComponentUsage Usage => CryptographicComponentUsage.DigitalSignature;

        /// <summary>
        /// Represents an elliptic curve matching <see cref="CryptographicKey{TAlgorithm}.Algorithm" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ECCurve Curve;
    }
}