// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric-key algorithm for which the key pair is used.
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
    public interface IAsymmetricKeyPair<TAlgorithm, TKey, TPrivateKey, TPublicKey> : IAsymmetricKeyPair<TAlgorithm>
        where TAlgorithm : struct, Enum
        where TKey : class, ICryptographicKey
        where TPrivateKey : class, TKey, IAsymmetricPrivateKey<TAlgorithm>
        where TPublicKey : class, TKey, IAsymmetricPublicKey
    {
        /// <summary>
        /// Gets the private key.
        /// </summary>
        public TPrivateKey PrivateKey
        {
            get;
        }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public TPublicKey PublicKey
        {
            get;
        }
    }

    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric-key algorithm for which the key pair is used.
    /// </typeparam>
    public interface IAsymmetricKeyPair<TAlgorithm> : IAsymmetricKeyPair
        where TAlgorithm : struct, Enum
    {
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
    public interface IAsymmetricKeyPair : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets the globally unique identifier for the current <see cref="IAsymmetricKeyPair" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }
    }
}