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
        /// Specifies an Argon2id algorithm that performs eight (8) iterations using 8MB of memory and eight (8) threads to produce
        /// a 256-bit digest.
        /// </summary>
        Argon2idBalanced = 0x01,

        /// <summary>
        /// Specifies an Argon2id algorithm that performs twenty-four (24) iterations using 4MB of memory and eight (8) threads to
        /// produce a 256-bit digest.
        /// </summary>
        Argon2idIterativelyExpensive = 0x02,

        /// <summary>
        /// Specifies an Argon2id algorithm that performs eight (8) iterations using 16MB of memory and four (4) threads to produce
        /// a 256-bit digest.
        /// </summary>
        Argon2idMemoryExpensive = 0x03,

        /// <summary>
        /// Specifies an Argon2id algorithm that performs four (4) iterations using 8MB of memory and sixteen (16) threads to
        /// produce a 256-bit digest.
        /// </summary>
        Argon2idThreadExpensive = 0x04,

        /// <summary>
        /// Specifies the MD5 hashing algorithm, which produces a 128-bit digest.
        /// </summary>
        Md5 = 0x05,

        /// <summary>
        /// Specifies the PBKDF2 key-derivation function (PRF: 512-bit SHA-2, 256-bit salt) using 17,711 iterations to produce a
        /// 256-bit digest.
        /// </summary>
        Pbkdf2 = 0x06,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 256-bit digest.
        /// </summary>
        ShaTwo256 = 0x07,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 384-bit digest.
        /// </summary>
        ShaTwo384 = 0x08,

        /// <summary>
        /// Specifies the SHA-2 hashing algorithm using a 512-bit digest.
        /// </summary>
        ShaTwo512 = 0x09
    }
}