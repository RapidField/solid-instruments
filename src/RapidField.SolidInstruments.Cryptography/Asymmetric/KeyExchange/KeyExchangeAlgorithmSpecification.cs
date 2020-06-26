// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange
{
    /// <summary>
    /// Defines distinct combinations of asymmetric key exchange algorithms, key bit-lengths and operational modes.
    /// </summary>
    public enum KeyExchangeAlgorithmSpecification : Byte
    {
        /// <summary>
        /// The asymmetric algorithm is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Specifies ECDH using the brainpoolP256r1 curve.
        /// </summary>
        EcdhBrainpoolP256r1 = 0x01,

        /// <summary>
        /// Specifies ECDH using the brainpoolP384r1 curve.
        /// </summary>
        EcdhBrainpoolP384r1 = 0x02,

        /// <summary>
        /// Specifies ECDH using the brainpoolP512r1 curve.
        /// </summary>
        EcdhBrainpoolP512r1 = 0x03,

        /// <summary>
        /// Specifies ECDH using the nistP256 curve.
        /// </summary>
        EcdhNistP256 = 0x04,

        /// <summary>
        /// Specifies ECDH using the nistP384 curve.
        /// </summary>
        EcdhNistP384 = 0x05,

        /// <summary>
        /// Specifies ECDH using the nistP521 curve.
        /// </summary>
        EcdhNistP521 = 0x06
    }
}