// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Hashing
{
    /// <summary>
    /// Specifies whether or not salt is applied to a cryptographic plaintext.
    /// </summary>
    public enum SaltingMode : Byte
    {
        /// <summary>
        /// The salting mode is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Salt is applied to the plaintext.
        /// </summary>
        Salted = 0x01,

        /// <summary>
        /// Salt is not applied to the plaintext.
        /// </summary>
        Unsalted = 0x02
    }
}