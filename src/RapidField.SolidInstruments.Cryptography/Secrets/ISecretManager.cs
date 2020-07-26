// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a management facility for named secret values which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretManager : ISecretExporter, ISecretImporter, ISecretKeyProducer, ISecretStore
    {
    }
}