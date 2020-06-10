// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Defines distinct hashing algorithms.
    /// </summary>
    public enum HashingAlgorithmSpecification : Byte
    {
        /// <summary>
        /// The hashing algorithm is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Specifies the MD5 hashing algorithm.
        /// </summary>
        Md5 = 0x01,

        /// <summary>
        /// Specifies the PBKDF2 key-derivation function (PRF: 512-bit SHA-2, 256-bit salt) using 17,711 iterations to produce a
        /// 256-bit digest.
        /// </summary>
        Pbkdf2 = 0x02,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 256-bit digest.
        /// </summary>
        ShaTwo256 = 0x03,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 384-bit digest.
        /// </summary>
        ShaTwo384 = 0x04,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 512-bit digest.
        /// </summary>
        ShaTwo512 = 0x05
    }
}