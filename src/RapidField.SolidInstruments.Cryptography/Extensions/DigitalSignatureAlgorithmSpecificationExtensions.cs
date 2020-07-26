// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.Extensions
{
    /// <summary>
    /// Extends the <see cref="DigitalSignatureAlgorithmSpecification" /> enumeration with cryptographic features.
    /// </summary>
    internal static class DigitalSignatureAlgorithmSpecificationExtensions
    {
        /// <summary>
        /// Returns a new <see cref="ECCurve" /> matching the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// A new <see cref="ECCurve" /> matching the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static ECCurve ToCurve(this DigitalSignatureAlgorithmSpecification target) => target switch
        {
            DigitalSignatureAlgorithmSpecification.Unspecified => default,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP256r1 => ECCurve.NamedCurves.brainpoolP256r1,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP384r1 => ECCurve.NamedCurves.brainpoolP384r1,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP512r1 => ECCurve.NamedCurves.brainpoolP512r1,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP256 => ECCurve.NamedCurves.nistP256,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP384 => ECCurve.NamedCurves.nistP384,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP521 => ECCurve.NamedCurves.nistP521,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(DigitalSignatureAlgorithmSpecification)}.", nameof(target))
        };

        /// <summary>
        /// Returns the private key bit-length component of the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the private key for the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToPrivateKeyBitLength(this DigitalSignatureAlgorithmSpecification target) => target switch
        {
            DigitalSignatureAlgorithmSpecification.Unspecified => default,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP256r1 => 976,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP384r1 => 1368,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP512r1 => 1768,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP256 => 968,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP384 => 1336,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP521 => 1784,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(DigitalSignatureAlgorithmSpecification)}.", nameof(target))
        };

        /// <summary>
        /// Returns the public key bit-length component of the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </param>
        /// <returns>
        /// The bit-length of the public key for the current <see cref="DigitalSignatureAlgorithmSpecification" />.
        /// </returns>
        [DebuggerHidden]
        internal static Int32 ToPublicKeyBitLength(this DigitalSignatureAlgorithmSpecification target) => target switch
        {
            DigitalSignatureAlgorithmSpecification.Unspecified => default,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP256r1 => 736,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP384r1 => 992,
            DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP512r1 => 1264,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP256 => 728,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP384 => 960,
            DigitalSignatureAlgorithmSpecification.EcdsaNistP521 => 1264,
            _ => throw new ArgumentException($"{target} is not a supported {nameof(DigitalSignatureAlgorithmSpecification)}.", nameof(target))
        };
    }
}