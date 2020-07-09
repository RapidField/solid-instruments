// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Secrets;
using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Represents a centralized facility for safeguarding digital secrets and performing cryptographic operations.
    /// </summary>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public interface ISoftwareSecurityModule<TPersistenceVehicle> : ISoftwareSecurityModule<SecretVault, TPersistenceVehicle>
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle<SecretVault>
    {
    }

    /// <summary>
    /// Represents a centralized facility for safeguarding digital secrets and performing cryptographic operations.
    /// </summary>
    /// <typeparam name="TInMemorySecretStore">
    /// The type of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    /// <typeparam name="TPersistenceVehicle">
    /// The type of the provider that facilitates persistence of the in-memory <see cref="ISecretStore" />.
    /// </typeparam>
    public interface ISoftwareSecurityModule<TInMemorySecretStore, TPersistenceVehicle> : IPersistentSecretStore<TInMemorySecretStore, TPersistenceVehicle>, ISoftwareSecurityModule
        where TInMemorySecretStore : class, ISecretStore
        where TPersistenceVehicle : class, ISecretStorePersistenceVehicle<TInMemorySecretStore>
    {
    }

    /// <summary>
    /// Represents a centralized facility for safeguarding digital secrets and performing cryptographic operations.
    /// </summary>
    public interface ISoftwareSecurityModule : IPersistentSecretStore, ISecurityAppliance
    {
        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISoftwareSecurityModule" /> can be used to digitally sign
        /// information using asymmetric key cryptography.
        /// </summary>
        public Boolean SupportsDigitalSignature
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISoftwareSecurityModule" /> can be used to produce hash
        /// values for plaintext information.
        /// </summary>
        public Boolean SupportsHashing
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISoftwareSecurityModule" /> can be used to securely
        /// exchange symmetric keys with remote parties.
        /// </summary>
        public Boolean SupportsKeyExchange
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="ISoftwareSecurityModule" /> can be used to encrypt or
        /// decrypt information using symmetric key cryptography.
        /// </summary>
        public Boolean SupportsSymmetricKeyEncryption
        {
            get;
        }
    }
}