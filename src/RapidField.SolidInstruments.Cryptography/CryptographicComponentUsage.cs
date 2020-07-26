// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography
{
    /// <summary>
    /// Defines the valid purposes and uses of a cryptographic key or instrument.
    /// </summary>
    [Flags]
    public enum CryptographicComponentUsage : Byte
    {
        /// <summary>
        /// No valid uses are defined for the component.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The component can be used to produce hash values for plaintext information.
        /// </summary>
        Hashing = 0x01,

        /// <summary>
        /// The component can be used to encrypt or decrypt information using symmetric key cryptography.
        /// </summary>
        SymmetricKeyEncryption = 0x02,

        /// <summary>
        /// The component can be used to digitally sign information using asymmetric key cryptography.
        /// </summary>
        DigitalSignature = 0x04,

        /// <summary>
        /// The component can be used to securely exchange symmetric keys with remote parties.
        /// </summary>
        KeyExchange = 0x08
    }
}