// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Defines distinct combinations of asymmetric digital signature algorithms, key bit-lengths and operational modes.
    /// </summary>
    public enum DigitalSignatureAlgorithmSpecification : Byte
    {
        /// <summary>
        /// The asymmetric algorithm is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// Specifies ECDSA using the brainpoolP256r1 curve.
        /// </summary>
        EcdsaBrainpoolP256r1 = 0x01,

        /// <summary>
        /// Specifies ECDSA using the brainpoolP384r1 curve.
        /// </summary>
        EcdsaBrainpoolP384r1 = 0x02,

        /// <summary>
        /// Specifies ECDSA using the brainpoolP512r1 curve.
        /// </summary>
        EcdsaBrainpoolP512r1 = 0x03,

        /// <summary>
        /// Specifies ECDSA using the nistP256 curve.
        /// </summary>
        EcdsaNistP256 = 0x04,

        /// <summary>
        /// Specifies ECDSA using the nistP384 curve.
        /// </summary>
        EcdsaNistP384 = 0x05,

        /// <summary>
        /// Specifies ECDSA using the nistP521 curve.
        /// </summary>
        EcdsaNistP521 = 0x06
    }
}