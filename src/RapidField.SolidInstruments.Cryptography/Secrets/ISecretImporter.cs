// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents an import facility for named secrets which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretImporter : ISecretCertificateImporter, ISecretValueImporter, ISecretVaultImporter
    {
    }
}