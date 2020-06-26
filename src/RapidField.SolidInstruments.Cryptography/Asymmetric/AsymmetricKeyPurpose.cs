// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Defines the purpose of an <see cref="IAsymmetricKey" />.
    /// </summary>
    public enum AsymmetricKeyPurpose : Byte
    {
        /// <summary>
        /// The asymmetric key purpose is not specified.
        /// </summary>
        Unspecified = 0x00,

        /// <summary>
        /// The key is used for digitally signing data.
        /// </summary>
        DigitalSignature = 0x01,

        /// <summary>
        /// The key is used for securely exchanging symmetric keys.
        /// </summary>
        KeyExchange = 0x02
    }
}