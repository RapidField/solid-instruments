// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for one key in an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric algorithm for which a key is derived.
    /// </typeparam>
    public interface IAsymmetricKey<TAlgorithm> : IAsymmetricKey, ICryptographicKey<TAlgorithm>
        where TAlgorithm : struct, Enum
    {
    }

    /// <summary>
    /// Represents an asymmetric-key algorithm and the key bits for one key in an asymmetric key pair.
    /// </summary>
    public interface IAsymmetricKey : ICryptographicKey
    {
        /// <summary>
        /// Gets the date and time when the current <see cref="IAsymmetricKey" /> expires and is no longer valid for use.
        /// </summary>
        public DateTime ExpirationTimeStamp
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="IAsymmetricKey" /> is expired.
        /// </summary>
        public Boolean IsExpired
        {
            get;
        }

        /// <summary>
        /// Gets the globally unique identifier for the key pair to which the current <see cref="IAsymmetricKey" /> belongs.
        /// </summary>
        public Guid KeyPairIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that specifies what the current <see cref="IAsymmetricKey" /> is used for.
        /// </summary>
        public AsymmetricKeyPurpose Purpose
        {
            get;
        }
    }
}