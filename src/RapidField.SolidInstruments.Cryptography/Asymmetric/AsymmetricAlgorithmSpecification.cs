// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Defines distinct combinations of asymmetric key encryption algorithms, key bit-lengths and operational modes.
    /// </summary>
    public enum AsymmetricAlgorithmSpecification : Byte
    {
        /// <summary>
        /// The asymmetric algorithm is not specified.
        /// </summary>
        Unspecified = 0x00
    }
}