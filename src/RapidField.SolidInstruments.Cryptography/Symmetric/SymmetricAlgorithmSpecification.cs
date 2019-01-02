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

        /* =========================================================================================================================
         * Twofish and Threefish are out-of-scope for MVP-01.
         * =========================================================================================================================
        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 128-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Twofish128Cbc = 0x05,

        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 128-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Twofish128Ecb = 0x06,

        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 192-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Twofish192Cbc = 0x07,

        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 192-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Twofish192Ecb = 0x08,

        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 256-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Twofish256Cbc = 0x09,

        /// <summary>
        /// Specifies the Twofish symmetric-key encryption cipher using a 256-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Twofish256Ecb = 0x0a,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 256-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Threefish256Cbc = 0x0b,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 256-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Threefish256Ecb = 0x0c,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 512-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Threefish512Cbc = 0x0d,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 512-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Threefish512Ecb = 0x0e,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 1024-bit key in Cipher Block Chaining (CBC) mode.
        /// </summary>
        Threefish1024Cbc = 0x0f,

        /// <summary>
        /// Specifies the Threefish symmetric-key encryption cipher using a 1024-bit key in Electronic Codebook (ECB) mode.
        /// </summary>
        Threefish1024Ecb = 0x10*/
    }
}