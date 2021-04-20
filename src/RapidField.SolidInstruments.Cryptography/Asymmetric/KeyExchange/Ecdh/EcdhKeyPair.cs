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
        /// <param name="isReconstituted">
        /// A value indicating whether or not the key pair is constructed from serialized memory bits.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="KeyExchangeAlgorithmSpecification.Unspecified" />.
        /// </exception>
        [DebuggerHidden]
        internal EcdhKeyPair(KeyExchangeAlgorithmSpecification algorithm, Boolean isReconstituted)
            : base(Guid.NewGuid(), algorithm, DefaultKeyLifespanDuration, isReconstituted)
        {
            return;
        }

        /// <summary>
        /// Derives material bytes that are used to create a new symmetric key that can be used to secure communications with a
        /// second party by whom the specified public key was provided.
        /// </summary>
        /// <param name="provider">
        /// The asymmetric algorithm provider that facilitates key material derivation.
        /// </param>
        /// <param name="firstPartyPrivateKey">
        /// The private key which, in combination with the second party public key, is used to derive the symmetric key material.
        /// </param>
        /// <param name="secondPartyPublicKey">
        /// The public key which, in combination with the first party private key, is used to derive the symmetric key material.
        /// </param>
        /// <returns>
        /// The resulting key material bytes.
        /// </returns>
        protected sealed override ISecureMemory DeriveSymmetricKeyMaterial(ECDiffieHellman provider, EcdhPrivateKey firstPartyPrivateKey, Span<Byte> secondPartyPublicKey)
        {
            using var keyMaterialMemory = new PinnedMemory(provider.DeriveKeyMaterial(new SecondPartyEcdhPublicKey(secondPartyPublicKey.ToArray())), true);
            var keyMaterial = new SecureMemory(keyMaterialMemory.LengthInBytes);
            keyMaterial.Access(memory => keyMaterialMemory.ReadOnlySpan.CopyTo(memory.Span));
            return keyMaterial;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EcdhKeyPair" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
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
        /// <param name="isReconstituted">
        /// A value indicating whether or not the key pair is constructed from serialized memory bits.
        /// </param>
        /// <returns>
        /// The private key.
        /// </returns>
        protected sealed override EcdhPrivateKey InitializePrivateKey(KeyExchangeAlgorithmSpecification algorithm, ECDiffieHellman provider, Boolean isReconstituted)
        {
            using var keySource = new PinnedMemory(provider.ExportECPrivateKey());
            return new(Identifier, algorithm, keySource, KeyLifespanDuration);
        }

        /// <summary>
        /// Initializes the algorithm provider that facilitates cryptographic operations for the key pair.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key pair is used.
        /// </param>
        /// <param name="isReconstituted">
        /// A value indicating whether or not the key pair is constructed from serialized memory bits.
        /// </param>
        /// <returns>
        /// The algorithm provider that facilitates cryptographic operations for the key pair.
        /// </returns>
        protected sealed override ECDiffieHellman InitializeProvider(KeyExchangeAlgorithmSpecification algorithm, Boolean isReconstituted) => ECDiffieHellman.Create(algorithm.ToCurve());

        /// <summary>
        /// Initializes the private key.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key pair is used.
        /// </param>
        /// <param name="provider">
        /// The algorithm provider that facilitates cryptographic operations for the key pair.
        /// </param>
        /// <param name="isReconstituted">
        /// A value indicating whether or not the key pair is constructed from serialized memory bits.
        /// </param>
        /// <returns>
        /// The private key.
        /// </returns>
        protected sealed override EcdhPublicKey InitializePublicKey(KeyExchangeAlgorithmSpecification algorithm, ECDiffieHellman provider, Boolean isReconstituted)
        {
            var keyMemory = provider.ExportSubjectPublicKeyInfo();
            return new(Identifier, algorithm, keyMemory, KeyLifespanDuration);
        }

        /// <summary>
        /// Represents the default length of time for which paired keys are valid.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly TimeSpan DefaultKeyLifespanDuration = TimeSpan.FromHours(36);

        /// <summary>
        /// Represents an implementation of <see cref="ECDiffieHellmanPublicKey" /> which supports reconstitution of a second party
        /// public key.
        /// </summary>
        private sealed class SecondPartyEcdhPublicKey : ECDiffieHellmanPublicKey
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SecondPartyEcdhPublicKey" /> class.
            /// </summary>
            /// <param name="keyBlob">
            /// Public key bytes which, in combination with first party private key bytes, are used to derive symmetric key
            /// material.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="keyBlob" /> is <see langword="null" />.
            /// </exception>
            [DebuggerHidden]
            internal SecondPartyEcdhPublicKey(Byte[] keyBlob)
                : base(keyBlob)
            {
                return;
            }
        }
    }
}