// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Represents an asymmetric key exchange algorithm and the public key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="KeyExchangePublicKey" /> is the default implementation of <see cref="IKeyExchangePublicKey" />.
    /// </remarks>
    public abstract class KeyExchangePublicKey : CryptographicKey, IKeyExchangePublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePublicKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keyMemory">
        /// The plaintext key bits for the public key.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMemory" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="keyMemory" /> is invalid for <paramref name="algorithm" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMemory" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected KeyExchangePublicKey(Guid keyPairIdentifier, KeyExchangeAlgorithmSpecification algorithm, Byte[] keyMemory)
            : this(keyPairIdentifier, algorithm, keyMemory, DefaultLifespanDuration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePublicKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keyMemory">
        /// The plaintext key bits for the public key.
        /// </param>
        /// <param name="lifespanDuration">
        /// The length of time for which the key is valid. The default value is ninety (90) days.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMemory" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="keyMemory" /> is invalid for <paramref name="algorithm" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMemory" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" /> -or- <paramref name="lifespanDuration" /> is less than
        /// eight seconds.
        /// </exception>
        protected KeyExchangePublicKey(Guid keyPairIdentifier, KeyExchangeAlgorithmSpecification algorithm, Byte[] keyMemory, TimeSpan lifespanDuration)
            : this(keyPairIdentifier, algorithm, keyMemory, TimeStamp.Current.Add(lifespanDuration.RejectIf().IsLessThan(MinimumLifespanDuration, nameof(lifespanDuration))))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangePublicKey" /> class.
        /// </summary>
        /// <param name="keyPairIdentifier">
        /// The globally unique identifier for the key pair to which the key belongs.
        /// </param>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <param name="keyMemory">
        /// The plaintext key bits for the public key.
        /// </param>
        /// <param name="expirationTimeStamp">
        /// The date and time when the key expires and is no longer valid for use.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="keyMemory" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="keyMemory" /> is invalid for <paramref name="algorithm" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="keyMemory" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyPairIdentifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal
        /// to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" /> -or- <paramref name="expirationTimeStamp" /> is equal to
        /// <see cref="DateTime.MaxValue" />.
        /// </exception>
        protected KeyExchangePublicKey(Guid keyPairIdentifier, KeyExchangeAlgorithmSpecification algorithm, Byte[] keyMemory, DateTime expirationTimeStamp)
            : base()
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(KeyExchangeAlgorithmSpecification.Unspecified, nameof(algorithm));
            ExpirationTimeStamp = expirationTimeStamp.RejectIf().IsEqualToValue(DateTime.MaxValue, nameof(expirationTimeStamp));
            KeyMemory = new PinnedMemory(keyMemory.RejectIf().IsNullOrEmpty(nameof(keyMemory)).OrIf(argument => argument.Length != (algorithm.ToPublicKeyBitLength() / 8), nameof(keyMemory), $"The length of the specified key is invalid for the specified algorithm, \"{algorithm}\"."), true);
            KeyPairIdentifier = keyPairIdentifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(keyPairIdentifier));
            Purpose = AsymmetricKeyPurpose.KeyExchange;
        }

        /// <summary>
        /// Converts the current <see cref="KeyExchangePublicKey" /> to its textual Base64 representation.
        /// </summary>
        /// <returns>
        /// A Base64 string representation of the public key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String ToBase64String()
        {
            RejectIfDisposed();
            var result = (String)null;

            using (var secureMemory = ToSecureMemory())
            {
                secureMemory.Access(memory =>
                {
                    result = memory.ToBase64String();
                });
            }

            return result;
        }

        /// <summary>
        /// Converts the current <see cref="KeyExchangePublicKey" /> to a serializable model.
        /// </summary>
        /// <returns>
        /// A serializable model representation of the public key.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public AsymmetricPublicKeyModel ToModel() => new AsymmetricPublicKeyModel(KeyPairIdentifier, ToBase64String(), ExpirationTimeStamp);

        /// <summary>
        /// Converts the value of the current <see cref="KeyExchangePublicKey" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="KeyExchangePublicKey" />.
        /// </returns>
        public override String ToString() => $"{{ \"{nameof(Purpose)}\": {Purpose}, \"{nameof(Algorithm)}\": {Algorithm}, \"{nameof(KeyPairIdentifier)}\": {KeyPairIdentifier.ToSerializedString()} }}";

        /// <summary>
        /// Extracts the key memory bytes from the specified, Base64-encoded public key.
        /// </summary>
        /// <param name="publicKey">
        /// A Base64 string representation of the public key.
        /// </param>
        /// <returns>
        /// The public key memory bytes.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="publicKey" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="publicKey" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publicKey" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="publicKey" /> is not a valid Base64 string.
        /// </exception>
        [DebuggerHidden]
        internal static KeyExchangeAlgorithmSpecification ExtractAlgorithm(String publicKey)
        {
            var publicKeyMemory = new Memory<Byte>(Convert.FromBase64String(publicKey.RejectIf().IsNullOrEmpty(nameof(publicKey))));
            var keyCompositionInformation = new AsymmetricPublicKeyCompositionInformation(publicKeyMemory, AsymmetricKeyPurpose.KeyExchange);
            var algorithmBytes = publicKeyMemory.Slice(AsymmetricPublicKeyCompositionInformation.AlgorithmStartIndex, keyCompositionInformation.AlgorithmLengthInBytes).ToArray();

            return algorithmBytes.Length switch
            {
                1 => (KeyExchangeAlgorithmSpecification)algorithmBytes[0],
                2 => (KeyExchangeAlgorithmSpecification)BitConverter.ToInt16(algorithmBytes),
                4 => (KeyExchangeAlgorithmSpecification)BitConverter.ToInt32(algorithmBytes),
                _ => throw new UnsupportedSpecificationException($"The bit length of {nameof(KeyExchangeAlgorithmSpecification)} is invalid."),
            };
        }

        /// <summary>
        /// Extracts the key memory bytes from the specified, Base64-encoded public key.
        /// </summary>
        /// <param name="publicKey">
        /// A Base64 string representation of the public key.
        /// </param>
        /// <returns>
        /// The public key memory bytes.
        /// </returns>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="publicKey" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="publicKey" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publicKey" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="publicKey" /> is not a valid Base64 string.
        /// </exception>
        [DebuggerHidden]
        internal static Span<Byte> ExtractKeyMemory(String publicKey)
        {
            var publicKeyMemory = new Memory<Byte>(Convert.FromBase64String(publicKey.RejectIf().IsNullOrEmpty(nameof(publicKey))));
            var keyCompositionInformation = new AsymmetricPublicKeyCompositionInformation(publicKeyMemory, AsymmetricKeyPurpose.KeyExchange);
            return publicKeyMemory.Slice(keyCompositionInformation.KeyMemoryStartIndex, keyCompositionInformation.KeyMemoryLengthInBytes).Span;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="KeyExchangePublicKey" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                KeyMemory?.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="KeyExchangePublicKey" /> to a secure bit field.
        /// </summary>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// A secure bit field containing a representation of the current <see cref="KeyExchangePublicKey" />.
        /// </returns>
        protected sealed override ISecureMemory ToSecureMemory(IConcurrencyControlToken controlToken)
        {
            var keyCompositionInformation = new AsymmetricPublicKeyCompositionInformation(KeyMemory.LengthInBytes, Purpose);
            var secureMemory = new SecureMemory(keyCompositionInformation.TotalLengthInBytes);

            try
            {
                secureMemory.Access(memory =>
                {
                    memory[AsymmetricPublicKeyCompositionInformation.PurposeStartIndex] = Convert.ToByte(Purpose);
                    memory[AsymmetricPublicKeyCompositionInformation.AlgorithmStartIndex] = Convert.ToByte(Algorithm);
                    Array.Copy(KeyMemory, 0, memory, keyCompositionInformation.KeyMemoryStartIndex, keyCompositionInformation.KeyMemoryLengthInBytes);
                });

                return secureMemory;
            }
            catch
            {
                secureMemory.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key is used.
        /// </summary>
        public KeyExchangeAlgorithmSpecification Algorithm
        {
            get;
        }

        /// <summary>
        /// Gets the date and time when the current <see cref="KeyExchangePublicKey" /> expires and is no longer valid for use.
        /// </summary>
        public DateTime ExpirationTimeStamp
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="KeyExchangePublicKey" /> is expired.
        /// </summary>
        public Boolean IsExpired => TimeStamp.Current >= ExpirationTimeStamp;

        /// <summary>
        /// Gets the globally unique identifier for the key pair to which the current <see cref="KeyExchangePublicKey" /> belongs.
        /// </summary>
        public Guid KeyPairIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that specifies what the current <see cref="KeyExchangePublicKey" /> is used for.
        /// </summary>
        public AsymmetricKeyPurpose Purpose
        {
            get;
        }

        /// <summary>
        /// Gets a value specifying the valid purposes and uses of the current <see cref="KeyExchangePublicKey" />.
        /// </summary>
        public override sealed CryptographicComponentUsage Usage => CryptographicComponentUsage.KeyExchange;

        /// <summary>
        /// Represents the plaintext key bits for the current <see cref="KeyExchangePublicKey" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PinnedMemory KeyMemory;
    }
}