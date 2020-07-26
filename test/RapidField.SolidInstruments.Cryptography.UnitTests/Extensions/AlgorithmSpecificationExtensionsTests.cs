// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature;
using RapidField.SolidInstruments.Cryptography.Asymmetric.KeyExchange;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public class AlgorithmSpecificationExtensionsTests
    {
        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForBrainpoolP256r1()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.brainpoolP256r1;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhBrainpoolP256r1;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP256r1;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForBrainpoolP384r1()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.brainpoolP384r1;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhBrainpoolP384r1;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP384r1;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForBrainpoolP512r1()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.brainpoolP512r1;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhBrainpoolP512r1;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaBrainpoolP512r1;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForNistP256()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.nistP256;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhNistP256;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaNistP256;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForNistP384()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.nistP384;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhNistP384;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaNistP384;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        [TestMethod]
        public void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForNistP521()
        {
            // Arrange.
            var curve = ECCurve.NamedCurves.nistP521;
            var keyExchangeSpecification = KeyExchangeAlgorithmSpecification.EcdhNistP521;
            var digitalSignatureSpecification = DigitalSignatureAlgorithmSpecification.EcdsaNistP521;

            // Assert.
            ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(curve, keyExchangeSpecification, digitalSignatureSpecification);
        }

        private static void ExtensionMappingOutput_ShouldMatchRelevantDotNetAnalogs_ForEllipticCurveAlgorithms(ECCurve curve, KeyExchangeAlgorithmSpecification keyExchangeSpecification, DigitalSignatureAlgorithmSpecification digitalSignatureSpecification)
        {
            // Arrange.
            using var ecdhAlgorithm = ECDiffieHellman.Create(curve);
            using var ecdsaAlgorithm = ECDsa.Create(curve);
            var expectedEcdhPrivateKeyLengthInBytes = ecdhAlgorithm.ExportECPrivateKey().Length;
            var expectedEcdhPublicKeyLengthInBytes = ecdhAlgorithm.PublicKey.ToByteArray().Length;
            var expectedEcdsaPrivateKeyLengthInBytes = ecdsaAlgorithm.ExportECPrivateKey().Length;
            var expectedEcdsaPublicKeyLengthInBytes = ecdsaAlgorithm.ExportSubjectPublicKeyInfo().Length;

            // Act.
            var actualEcdhCurve = keyExchangeSpecification.ToCurve();
            var actualEcdsaCurve = digitalSignatureSpecification.ToCurve();
            var actualEcdhPrivateKeyLengthInBytes = keyExchangeSpecification.ToPrivateKeyBitLength() / 8;
            var actualEcdhPublicKeyLengthInBytes = keyExchangeSpecification.ToPublicKeyBitLength() / 8;
            var actualEcdsaPrivateKeyLengthInBytes = digitalSignatureSpecification.ToPrivateKeyBitLength() / 8;
            var actualEcdsaPublicKeyLengthInBytes = digitalSignatureSpecification.ToPublicKeyBitLength() / 8;

            // Assert.
            actualEcdhCurve.Oid.FriendlyName.Should().Be(curve.Oid.FriendlyName);
            actualEcdsaCurve.Oid.FriendlyName.Should().Be(curve.Oid.FriendlyName);
            actualEcdhPrivateKeyLengthInBytes.Should().Be(expectedEcdhPrivateKeyLengthInBytes);
            actualEcdhPublicKeyLengthInBytes.Should().Be(expectedEcdhPublicKeyLengthInBytes);
            actualEcdsaPrivateKeyLengthInBytes.Should().Be(expectedEcdsaPrivateKeyLengthInBytes);
            actualEcdsaPublicKeyLengthInBytes.Should().Be(expectedEcdsaPublicKeyLengthInBytes);
        }
    }
}