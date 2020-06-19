﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Secrets;

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
    }
}