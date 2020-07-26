// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Represents a symmetric-key algorithm and source bits for a derived key, encapsulates key derivation operations and secures
    /// key bits in memory.
    /// </summary>
    public interface ISymmetricKey : ICryptographicKey<SymmetricAlgorithmSpecification>
    {
    }
}