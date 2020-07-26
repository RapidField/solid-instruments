// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Threading;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <remarks>
    /// <see cref="AsymmetricKeyPair{TAlgorithm, TProvider, TKey, TPrivateKey, TPublicKey}" /> is the default implementation of
    /// <see cref="IAsymmetricKeyPair{TAlgorithm, TKey, TPrivateKey, TPublicKey}" />.
    /// </remarks>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric-key algorithm for which the key pair is used.
    /// </typeparam>
    /// <typeparam name="TProvider">
    /// The type of the asymmetric algorithm provider that facilitates cryptographic operations for the key pair.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// A shared key type from which both the private and public key types derive.
    /// </typeparam>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public abstract class AsymmetricKeyPair<TAlgorithm, TProvider, TKey, TPrivateKey, TPublicKey> : AsymmetricKeyPair<TAlgorithm>
        where TAlgorithm : struct, Enum
        where TProvider : AsymmetricAlgorithm
        where TKey : class, ICryptographicKey
        where TPrivateKey : class, TKey, IAsymmetricPrivateKey<TAlgorithm>
        where TPublicKey : class, TKey, IAsymmetricPublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKeyPair{TAlgorithm, TProvider, TKey, TPrivateKey, TPublicKey}" />
        /// class.
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
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal to the
        /// default/unspecified value -or- <paramref name="keyLifespanDuration" /> is less than eight seconds.
        /// </exception>
        protected AsymmetricKeyPair(Guid identifier, TAlgorithm algorithm, TimeSpan keyLifespanDuration, Boolean isReconstituted)
            : base(identifier, algorithm, keyLifespanDuration)
        {
            IsReconstituted = isReconstituted;
            LazyPrivateKey = new Lazy<TPrivateKey>(InitializePrivateKey, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyProvider = new Lazy<TProvider>(InitializeProvider, LazyThreadSafetyMode.ExecutionAndPublication);
            LazyPublicKey = new Lazy<TPublicKey>(InitializePublicKey, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="AsymmetricKeyPair{TAlgorithm, TProvider, TKey, TPrivateKey, TPublicKey}" />.
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
                    LazyProvider?.Dispose();
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
        protected abstract TPrivateKey InitializePrivateKey(TAlgorithm algorithm, TProvider provider, Boolean isReconstituted);

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
        protected abstract TProvider InitializeProvider(TAlgorithm algorithm, Boolean isReconstituted);

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
        protected abstract TPublicKey InitializePublicKey(TAlgorithm algorithm, TProvider provider, Boolean isReconstituted);

        /// <summary>
        /// Initializes the private key.
        /// </summary>
        /// <returns>
        /// The private key.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised while attempting to initialize the key.
        /// </exception>
        [DebuggerHidden]
        private TPrivateKey InitializePrivateKey()
        {
            try
            {
                return InitializePrivateKey(Algorithm, Provider, IsReconstituted);
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while attempting to initialize the private key.", exception);
            }
        }

        /// <summary>
        /// Initializes the algorithm provider that facilitates cryptographic operations for the key pair.
        /// </summary>
        /// <returns>
        /// The private key.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised while attempting to initialize the provider.
        /// </exception>
        [DebuggerHidden]
        private TProvider InitializeProvider()
        {
            try
            {
                return InitializeProvider(Algorithm, IsReconstituted);
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while attempting to initialize the provider.", exception);
            }
        }

        /// <summary>
        /// Initializes the public key.
        /// </summary>
        /// <returns>
        /// The public key.
        /// </returns>
        /// <exception cref="SecurityException">
        /// An exception was raised while attempting to initialize the key.
        /// </exception>
        [DebuggerHidden]
        private TPublicKey InitializePublicKey()
        {
            try
            {
                return InitializePublicKey(Algorithm, Provider, IsReconstituted);
            }
            catch (Exception exception)
            {
                throw new SecurityException("An exception was raised while attempting to initialize the public key.", exception);
            }
        }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        public TPrivateKey PrivateKey => LazyPrivateKey.Value;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public TPublicKey PublicKey => LazyPublicKey.Value;

        /// <summary>
        /// Gets the asymmetric algorithm provider that facilitates cryptographic operations for the key pair.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal TProvider Provider => LazyProvider.Value;

        /// <summary>
        /// Represents a value indicating whether or not the current
        /// <see cref="AsymmetricKeyPair{TAlgorithm, TProvider, TKey, TPrivateKey, TPublicKey}" /> is constructed from serialized
        /// memory bits.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Boolean IsReconstituted;

        /// <summary>
        /// Represents the lazily-initialized private key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TPrivateKey> LazyPrivateKey;

        /// <summary>
        /// Represents the lazily-initialized asymmetric algorithm provider that facilitates cryptographic operations for the key
        /// pair.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TProvider> LazyProvider;

        /// <summary>
        /// Represents the lazily-initialized public key.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<TPublicKey> LazyPublicKey;
    }

    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric-key algorithm for which the key pair is used.
    /// </typeparam>
    /// <remarks>
    /// <see cref="AsymmetricKeyPair{TAlgorithm}" /> is the default implementation of <see cref="IAsymmetricKeyPair{TAlgorithm}" />.
    /// </remarks>
    public abstract class AsymmetricKeyPair<TAlgorithm> : AsymmetricKeyPair, IAsymmetricKeyPair<TAlgorithm>
        where TAlgorithm : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricKeyPair{TAlgorithm}" /> class.
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="algorithm" /> is equal to the
        /// default/unspecified value -or- <paramref name="keyLifespanDuration" /> is less than eight seconds.
        /// </exception>
        protected AsymmetricKeyPair(Guid identifier, TAlgorithm algorithm, TimeSpan keyLifespanDuration)
            : base(identifier, keyLifespanDuration)
        {
            Algorithm = algorithm.RejectIf().IsEqualToValue(default, nameof(algorithm));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AsymmetricKeyPair{TAlgorithm}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key pair is used.
        /// </summary>
        public TAlgorithm Algorithm
        {
            get;
        }
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
        /// <param name="identifier">
        /// The globally unique identifier for the key pair.
        /// </param>
        /// <param name="keyLifespanDuration">
        /// The length of time for which the paired keys are valid.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="identifier" /> is equal to <see cref="Guid.Empty" /> -or- <paramref name="keyLifespanDuration" /> is
        /// less than eight seconds.
        /// </exception>
        protected AsymmetricKeyPair(Guid identifier, TimeSpan keyLifespanDuration)
            : base()
        {
            Identifier = identifier.RejectIf().IsEqualToValue(Guid.Empty, nameof(identifier));
            KeyLifespanDuration = keyLifespanDuration.RejectIf().IsLessThan(CryptographicKey.MinimumLifespanDuration, nameof(keyLifespanDuration));
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="AsymmetricKeyPair" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Gets the globally unique identifier for the current <see cref="AsymmetricKeyPair" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that specifies what the key pair is used for.
        /// </summary>
        public abstract AsymmetricKeyPurpose Purpose
        {
            get;
        }

        /// <summary>
        /// Gets the length of time for which the paired keys are valid.
        /// </summary>
        protected internal TimeSpan KeyLifespanDuration
        {
            get;
        }
    }
}