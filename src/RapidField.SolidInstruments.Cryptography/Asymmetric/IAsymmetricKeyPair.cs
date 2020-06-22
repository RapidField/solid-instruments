// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TKey">
    /// A shared key type from which both the private and public key types derive.
    /// </typeparam>
    /// <typeparam name="TPrivateKey">
    /// The type of the private key.
    /// </typeparam>
    /// <typeparam name="TPublicKey">
    /// The type of the public key.
    /// </typeparam>
    public interface IAsymmetricKeyPair<TKey, TPrivateKey, TPublicKey> : IAsymmetricKeyPair
        where TKey : class, IAsymmetricKey
        where TPrivateKey : class, TKey, IAsymmetricPrivateKey
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
    public interface IAsymmetricKeyPair : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Gets the asymmetric-key algorithm for which the key pair is used.
        /// </summary>
        public AsymmetricAlgorithmSpecification Algorithm
        {
            get;
        }
    }
}