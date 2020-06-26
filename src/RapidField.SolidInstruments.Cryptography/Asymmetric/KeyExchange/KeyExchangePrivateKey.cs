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

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangePrivateKey" /> is the default implementation of <see cref="IKeyExchangePrivateKey" />.
    /// </remarks>
    public abstract class KeyExchangePrivateKey : CryptographicKey<KeyExchangeAlgorithmSpecification>, IKeyExchangePrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePrivateKey" /> class.
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
        /// to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected KeyExchangePrivateKey(Guid keyPairIdentifier, KeyExchangeAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource)
            : this(keyPairIdentifier, algorithm, derivationMode, keySource, DefaultLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePrivateKey" /> class.
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
        /// to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" /> -or- <paramref name="lifespanDuration" /> is less than
        /// eight seconds.
        /// </exception>
        protected KeyExchangePrivateKey(Guid keyPairIdentifier, KeyExchangeAlgorithmSpecification algorithm, CryptographicKeyDerivationMode derivationMode, PinnedMemory keySource, TimeSpan lifespanDuration)
            : base(algorithm, derivationMode, keySource, algorithm.ToPrivateKeyBitLength())
        {
            Curve = algorithm.ToCurve();
            ExpirationTimeStamp = TimeStamp.Current.Add(lifespanDuration.RejectIf().IsLessThan(MinimumLifespanDuration, nameof(lifespanDuration)));
            KeyPairIdentifier = keyPairIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(keyPairIdentifier));
            Purpose = AsymmetricKeyPurpose.KeyExchange;
        }

        /// <summary>
        /// Converts the value of the current <see cref="KeyExchangePrivateKey" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="KeyExchangePrivateKey" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Purpose)}\": {Purpose}, \"{nameof(Algorithm)}\": {Algorithm}, \"{nameof(KeyPairIdentifier)}\": {KeyPairIdentifier.ToSerializedString()} }}";

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangePrivateKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the date and time when the current <see cref="KeyExchangePrivateKey" /> expires and is no longer valid for use.
        /// </summary>
        public DateTime ExpirationTimeStamp
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="KeyExchangePrivateKey" /> is expired.
        /// </summary>
        public Boolean IsExpired => TimeStamp.Current >= ExpirationTimeStamp;

        /// <summary>
        /// Gets the globally unique identifier for the key pair to which the current <see cref="KeyExchangePrivateKey" /> belongs.
        /// </summary>
        public Guid KeyPairIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that specifies what the current <see cref="KeyExchangePrivateKey" /> is used for.
        /// </summary>
        public AsymmetricKeyPurpose Purpose
        {
            get;
        }

        /// <summary>
        /// Represents an elliptic curve matching <see cref="CryptographicKey{TAlgorithm}.Algorithm" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly ECCurve Curve;
    }
}