// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Provides facilities for performing cryptographic operations upon typed objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object upon which cryptographic operations are performed.
    /// </typeparam>
    public interface ICryptographicProcessor<T> : ICryptographicProcessor
        where T : class
    {
        /// <summary>
        /// Gets the type of the object upon which cryptographic operations are performed.
        /// </summary>
        public Type TargetObjectType
        {
            get;
        }
    }

    /// <summary>
    /// Provides facilities for performing cryptographic operations upon typed objects.
    /// </summary>
    public interface ICryptographicProcessor : ICryptographicComponent
    {
        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICryptographicProcessor" /> can be used to digitally sign
        /// information using asymmetric key cryptography.
        /// </summary>
        public Boolean SupportsDigitalSignature
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICryptographicProcessor" /> can be used to produce hash
        /// values for plaintext information.
        /// </summary>
        public Boolean SupportsHashing
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICryptographicProcessor" /> can be used to securely
        /// exchange symmetric keys with remote parties.
        /// </summary>
        public Boolean SupportsKeyExchange
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ICryptographicProcessor" /> can be used to encrypt or
        /// decrypt information using symmetric key cryptography.
        /// </summary>
        public Boolean SupportsSymmetricKeyEncryption
        {
            get;
        }
    }
}