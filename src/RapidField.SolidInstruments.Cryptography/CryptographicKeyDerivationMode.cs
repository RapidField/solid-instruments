// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Defines options for deriving key bits from an <see cref="ICryptographicKey{TAlgorithm}" />.
    /// </summary>
    public enum CryptographicKeyDerivationMode : Byte
    {
        /// <summary>
        /// The derivation mode is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Superfluous key bits are removed from the trailing end of the key word array, producing an output key of the desired
        /// length.
        /// </summary>
        /// <remarks>
        /// This is the fastest mode. Some distinct keys produce identical cryptographic output due to the fact that key bits are
        /// discarded.
        /// </remarks>
        Truncation = 0x01,

        /// <summary>
        /// The key bit field is divided into blocks that are XORed sequentially, producing an output key of the desired length.
        /// </summary>
        /// <remarks>
        /// No key bits are discarded. Some distinct <see cref="ICryptographicKey{TAlgorithm}" /> instances produce identical key
        /// outputs due to the fact that patterned substitutions produce identical XOR outputs.
        /// </remarks>
        XorLayering = 0x02,

        /// <summary>
        /// Substitution bits are introduced to the key bit field, which is divided into blocks that are XORed sequentially,
        /// producing an output key of the desired length.
        /// </summary>
        /// <remarks>
        /// No key bits are discarded and the probability of patterned-substitution collisions is extremely low when the key bits
        /// are entropic.
        /// </remarks>
        XorLayeringWithSubstitution = 0x03,

        /// <summary>
        /// A 1,024 bit password, a 1,024 bit salt and an iteration count between 16,384 and 49,024 are derived from the key bits,
        /// which serve as inputs for the PBKDF2 key derivation function.
        /// </summary>
        /// <remarks>
        /// This is the slowest and most secure mode. No key bits are discarded. Duplicate cryptographic output across distinct keys
        /// is extremely unlikely.
        /// </remarks>
        Pbkdf2 = 0x04
    }
}