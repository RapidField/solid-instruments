// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Defines the direction of a cryptographic cipher transformation.
    /// </summary>
    internal enum EncryptionDirection : Byte
    {
        /// <summary>
        /// The encryption direction is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// The data is being encrypted.
        /// </summary>
        Encryption = 0x01,

        /// <summary>
        /// The data is being decrypted.
        /// </summary>
        Decryption = 0x02
    }
}