// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature;
using RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents information about the length and location of different pieces of information within raw, exported
    /// <see cref="IAsymmetricPublicKey" /> bytes.
    /// </summary>
    internal sealed class AsymmetricPublicKeyCompositionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricPublicKeyCompositionInformation" /> class.
        /// </summary>
        /// <param name="publicKey">
        /// The full key from which composition information is gathered.
        /// </param>
        /// <param name="purpose">
        /// The purpose of the evaluated key.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The length of <paramref name="publicKey" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="purpose" /> is equal to <see cref="AsymmetricKeyPurpose.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal AsymmetricPublicKeyCompositionInformation(Memory<Byte> publicKey, AsymmetricKeyPurpose purpose)
            : this(purpose)
        {
            KeyMemoryLengthInBytes = publicKey.Length - PurposeLengthInBytes - AlgorithmLengthInBytes;
            KeyMemoryStartIndex = AlgorithmStartIndex + AlgorithmLengthInBytes;

            if (KeyMemoryLengthInBytes <= 0)
            {
                throw new ArgumentException("The length of the specified public key is invalid.", nameof(publicKey));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricPublicKeyCompositionInformation" /> class.
        /// </summary>
        /// <param name="keyMemoryLengthInBytes">
        /// The full key from which composition information is gathered.
        /// </param>
        /// <param name="purpose">
        /// The purpose of the evaluated key.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="keyMemoryLengthInBytes" /> is less than or equal to zero -or- <paramref name="purpose" /> is equal to
        /// <see cref="AsymmetricKeyPurpose.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal AsymmetricPublicKeyCompositionInformation(Int32 keyMemoryLengthInBytes, AsymmetricKeyPurpose purpose)
            : this(purpose)
        {
            KeyMemoryLengthInBytes = keyMemoryLengthInBytes.RejectIf().IsLessThanOrEqualTo(0, nameof(keyMemoryLengthInBytes));
            KeyMemoryStartIndex = AlgorithmStartIndex + AlgorithmLengthInBytes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricPublicKeyCompositionInformation" /> class.
        /// </summary>
        /// <param name="purpose">
        /// The purpose of the evaluated key.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="purpose" /> is equal to <see cref="AsymmetricKeyPurpose.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        private AsymmetricPublicKeyCompositionInformation(AsymmetricKeyPurpose purpose)
        {
            AlgorithmLengthInBytes = purpose.RejectIf().IsEqualToValue(AsymmetricKeyPurpose.Unspecified, nameof(purpose)) == AsymmetricKeyPurpose.DigitalSignature ? sizeof(DigitalSignatureAlgorithmSpecification) : sizeof(KeyExchangeAlgorithmSpecification);
        }

        /// <summary>
        /// Gets the length, in bytes, of the entire key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Int32 TotalLengthInBytes => PurposeLengthInBytes + AlgorithmLengthInBytes + KeyMemoryLengthInBytes;

        /// <summary>
        /// Represents the zero-based starting index of the algorithm information within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 AlgorithmStartIndex = PurposeStartIndex + PurposeLengthInBytes;

        /// <summary>
        /// Represents the length, in bytes, of the purpose information within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 PurposeLengthInBytes = sizeof(AsymmetricKeyPurpose);

        /// <summary>
        /// Represents the zero-based starting index of the purpose information within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal const Int32 PurposeStartIndex = 0;

        /// <summary>
        /// Represents the length, in bytes, of the algorithm information within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 AlgorithmLengthInBytes;

        /// <summary>
        /// Represents the length, in bytes, of the contiguous raw key bytes within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 KeyMemoryLengthInBytes;

        /// <summary>
        /// Represents the zero-based starting index of the contiguous raw key bytes within a key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly Int32 KeyMemoryStartIndex;
    }
}