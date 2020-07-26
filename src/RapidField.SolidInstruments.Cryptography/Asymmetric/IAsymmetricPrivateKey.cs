// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Represents an asymmetric-key algorithm and the private key bits for an asymmetric key pair.
    /// </summary>
    /// <typeparam name="TAlgorithm">
    /// The type of the asymmetric algorithm for which a key is derived.
    /// </typeparam>
    public interface IAsymmetricPrivateKey<TAlgorithm> : IAsymmetricKey<TAlgorithm>
        where TAlgorithm : struct, Enum
    {
    }
}