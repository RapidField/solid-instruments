// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Threading;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricKeyPair{TKey, TPrivateKey, TPublicKey}" /> is the default implementation of
    /// <see cref="IAsymmetricKeyPair{TKey, TPrivateKey, TPublicKey}" />.
    /// </remarks>
    /// <typeparam name="TKey">
    /// A shared key type from which both the private and public key types derive.
    /// </typeparam>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public abstract class AsymmetricKeyPair<TKey, TPrivateKey, TPublicKey> : AsymmetricKeyPair
        where TKey : class, IAsymmetricKey
        where TPrivateKey : class, TKey, IAsymmetricPrivateKey
        where TPublicKey : class, TKey, IAsymmetricPublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKeyPair{TKey, TPrivateKey, TPublicKey}" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected AsymmetricKeyPair(AsymmetricAlgorithmSpecification algorithm)
            : base(algorithm)
        {
            LazyPrivateKey = new Lazy<TPrivateKey>(InitializePrivateKey, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyPublicKey = new Lazy<TPublicKey>(InitializePublicKey, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AsymmetricKeyPair{TKey, TPrivateKey, TPublicKey}" />.
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
                    LazyPrivateKey?.Dispose();
                    LazyPublicKey?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Initializes the private key.
        /// </summary>
        /// <returns>
        /// The private key.
        /// </returns>
        [DebuggerHidden]
        private TPrivateKey InitializePrivateKey() => throw new NotImplementedException();

        /// <summary>
        /// Initializes the public key.
        /// </summary>
        /// <returns>
        /// The public key.
        /// </returns>
        [DebuggerHidden]
        private TPublicKey InitializePublicKey() => throw new NotImplementedException();

        /// <summary>
        /// Gets the private key.
        /// </summary>
        public TPrivateKey PrivateKey => LazyPrivateKey.Value;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public TPublicKey PublicKey => LazyPublicKey.Value;

        /// <summary>
        /// Represents the lazily-initialized private key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TPrivateKey> LazyPrivateKey;

        /// <summary>
        /// Represents the lazily-initialized public key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TPublicKey> LazyPublicKey;
    }

    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricKeyPair" /> is the default implementation of <see cref="IAsymmetricKeyPair" />.
    /// </remarks>
    public abstract class AsymmetricKeyPair : Instrument, IAsymmetricKeyPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKeyPair" /> class.
        /// </summary>
        /// <param name="algorithm">
        /// The asymmetric-key algorithm for which the key is used.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="algorithm" /> is equal to <see cref="AsymmetricAlgorithmSpecification.Unspecified" />.
        /// </exception>
        protected AsymmetricKeyPair(AsymmetricAlgorithmSpecification algorithm)
            : base()
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(AsymmetricAlgorithmSpecification.Unspecified, nameof(algorithm));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AsymmetricKeyPair" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key pair is used.
        /// </summary>
        public AsymmetricAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}