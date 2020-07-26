// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="KeyExchangeAlgorithmSpecification" /> enumeration with cryptographic features.
    /// </summary>
    internal static class KeyExchangeAlgorithmSpecificationExtensions
    {
        /// <summary>
        /// Returns a new <see cref="ECCurve" /> matching the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// A new <see cref="ECCurve" /> matching the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static ECCurve ToCurve(this KeyExchangeAlgorithmSpecification target) => target switch
        {
            KeyExchangeAlgorithmSpecification.Unspecified => default,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP256r1 => ECCurve.NamedCurves.brainpoolP256r1,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP384r1 => ECCurve.NamedCurves.brainpoolP384r1,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP512r1 => ECCurve.NamedCurves.brainpoolP512r1,
            KeyExchangeAlgorithmSpecification.EcdhNistP256 => ECCurve.NamedCurves.nistP256,
            KeyExchangeAlgorithmSpecification.EcdhNistP384 => ECCurve.NamedCurves.nistP384,
            KeyExchangeAlgorithmSpecification.EcdhNistP521 => ECCurve.NamedCurves.nistP521,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(KeyExchangeAlgorithmSpecification)}.", nameof(target))
        };

        /// <summary>
        /// Returns the private key bit-length component of the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the private key for the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToPrivateKeyBitLength(this KeyExchangeAlgorithmSpecification target) => target switch
        {
            KeyExchangeAlgorithmSpecification.Unspecified => default,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP256r1 => 976,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP384r1 => 1368,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP512r1 => 1768,
            KeyExchangeAlgorithmSpecification.EcdhNistP256 => 968,
            KeyExchangeAlgorithmSpecification.EcdhNistP384 => 1336,
            KeyExchangeAlgorithmSpecification.EcdhNistP521 => 1784,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(KeyExchangeAlgorithmSpecification)}.", nameof(target))
        };

        /// <summary>
        /// Returns the public key bit-length component of the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the public key for the current <see cref="KeyExchangeAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToPublicKeyBitLength(this KeyExchangeAlgorithmSpecification target) => target switch
        {
            KeyExchangeAlgorithmSpecification.Unspecified => default,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP256r1 => 576,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP384r1 => 832,
            KeyExchangeAlgorithmSpecification.EcdhBrainpoolP512r1 => 1088,
            KeyExchangeAlgorithmSpecification.EcdhNistP256 => 576,
            KeyExchangeAlgorithmSpecification.EcdhNistP384 => 832,
            KeyExchangeAlgorithmSpecification.EcdhNistP521 => 1120,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(KeyExchangeAlgorithmSpecification)}.", nameof(target))
        };
    }
}