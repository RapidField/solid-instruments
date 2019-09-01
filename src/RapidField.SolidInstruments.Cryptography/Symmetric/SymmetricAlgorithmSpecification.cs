// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Symmetric
{
    /// <summary>
    /// Defines distinct combinations of symmetric key encryption algorithms, key bit-lengths and cipher modes.
    /// </summary>
    public enum SymmetricAlgorithmSpecification : Byte
    {
        /// <summary>
        /// The symmetric algorithm is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Specifies the AES symmetric-key encryption cipher using a 128-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Aes128Cbc = 0x01,

        /// <summary>
        /// Specifies the AES symmetric-key encryption cipher using a 128-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Aes128Ecb = 0x02,

        /// <summary>
        /// Specifies the AES symmetric-key encryption cipher using a 256-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Aes256Cbc = 0x03,

        /// <summary>
        /// Specifies the AES symmetric-key encryption cipher using a 256-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Aes256Ecb = 0x04
    }
}