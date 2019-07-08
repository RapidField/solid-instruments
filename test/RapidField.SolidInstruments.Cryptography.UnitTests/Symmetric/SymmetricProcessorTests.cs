// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class SymmetricProcessorTests
    {
        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForMixedAlgorithms_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForMixedAlgorithms_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForMixedAlgorithms_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedCbc_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedCbc_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedCbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedEcb_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedEcb_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers_ForRepeatedEcb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForMixedAlgorithms_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForMixedAlgorithms_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForMixedAlgorithms_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedCbc_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedCbc_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedCbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedEcb_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedEcb_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers_ForRepeatedEcb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForMixedAlgorithms_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForMixedAlgorithms_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForMixedAlgorithms_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedCbc_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedCbc_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedCbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedEcb_InTruncationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedEcb_InXorLayeringMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers_ForRepeatedEcb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes128Cbc_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes128Cbc_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes128Cbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes256Ecb_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes256Ecb_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes256Ecb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        /* =================================================================================================================
         * Twofish and Threefish are out-of-scope for MVP-01.
         * =================================================================================================================
        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish1024Cbc_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish1024Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish1024Cbc_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish1024Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish1024Cbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish1024Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish256Ecb_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish256Ecb_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForThreefish256Ecb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Threefish256Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish128Cbc_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish128Cbc_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish128Cbc_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish128Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish192Ecb_InTruncationMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish192Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish192Ecb_InXorLayeringMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish192Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForTwofish192Ecb_InXorLayeringWithSubstitutionMode()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish192Ecb;
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayeringWithSubstitution;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm, derivationMode);
        }*/

        private static void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(CascadingSymmetricKey key)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var binarySerializer = new PassThroughSerializer();
                var target = new SymmetricProcessor<Byte[]>(randomnessProvider, binarySerializer);
                var plaintextObject = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

                // Act.
                var ciphertextResult = target.Encrypt(plaintextObject, key);

                // Assert.
                ciphertextResult.Should().NotBeNullOrEmpty();
                ciphertextResult.Count(value => value == 0x00).Should().NotBe(ciphertextResult.Length);

                // Act.
                var plaintextResult = target.Decrypt(ciphertextResult, key);

                // Assert.
                plaintextResult.Should().NotBeNullOrEmpty();
                plaintextResult.Length.Should().Be(plaintextObject.Length);
                plaintextResult.Count(value => value == 0x00).Should().NotBe(plaintextResult.Length);

                for (var i = 0; i < plaintextResult.Length; i++)
                {
                    // Assert.
                    plaintextResult[i].Should().Be(plaintextObject[i]);
                }
            }
        }

        private static void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers(SecureSymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm, SymmetricAlgorithmSpecification fourthLayerAlgorithm)
        {
            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
            {
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        private static void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers(SecureSymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm, SymmetricAlgorithmSpecification thirdLayerAlgorithm)
        {
            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm))
            {
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        private static void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers(SecureSymmetricKeyDerivationMode derivationMode, SymmetricAlgorithmSpecification firstLayerAlgorithm, SymmetricAlgorithmSpecification secondLayerAlgorithm)
        {
            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm))
            {
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        private static void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(SymmetricAlgorithmSpecification algorithm, SecureSymmetricKeyDerivationMode derivationMode)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var binarySerializer = new PassThroughSerializer();
                var target = new SymmetricProcessor<Byte[]>(randomnessProvider, binarySerializer);
                var plaintextObject = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

                using (var key = SecureSymmetricKey.New(algorithm, derivationMode))
                {
                    // Act.
                    var ciphertextResult = target.Encrypt(plaintextObject, key);

                    // Assert.
                    ciphertextResult.Should().NotBeNullOrEmpty();
                    ciphertextResult.Count(value => value == 0x00).Should().NotBe(ciphertextResult.Length);

                    // Act.
                    var plaintextResult = target.Decrypt(ciphertextResult, key);

                    // Assert.
                    plaintextResult.Should().NotBeNullOrEmpty();
                    plaintextResult.Length.Should().Be(plaintextObject.Length);
                    plaintextResult.Count(value => value == 0x00).Should().NotBe(plaintextResult.Length);

                    for (var i = 0; i < plaintextResult.Length; i++)
                    {
                        // Assert.
                        plaintextResult[i].Should().Be(plaintextObject[i]);
                    }
                }
            }
        }
    }
}