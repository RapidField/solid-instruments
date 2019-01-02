// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class SymmetricStringProcessorTests
    {
        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithFourLayers()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
            {
                // Assert.
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithThreeLayers()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm))
            {
                // Assert.
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey_WithTwoLayers()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.XorLayering;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            using (var key = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm))
            {
                // Assert.
                Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(key);
            }
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes128Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes128Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes256Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey_ForAes256Ebc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;

            // Assert.
            Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(algorithm);
        }

        private static void Encrypt_ShouldBeReversible_UsingCascadingSymmetricKey(CascadingSymmetricKey key)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = new SymmetricStringProcessor(randomnessProvider);
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";

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

        private static void Encrypt_ShouldBeReversible_UsingSecureSymmetricKey(SymmetricAlgorithmSpecification algorithm)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = new SymmetricStringProcessor(randomnessProvider);
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";

                using (var key = SecureSymmetricKey.New(algorithm))
                {
                    // Act.
                    var ciphertext = target.Encrypt(plaintextObject, key);

                    // Assert.
                    ciphertext.Should().NotBeNullOrEmpty();
                    ciphertext.Count(value => value == 0x00).Should().NotBe(ciphertext.Length);

                    // Act.
                    var plaintext = target.Decrypt(ciphertext, key);

                    // Assert.
                    plaintext.Should().NotBeNullOrEmpty();
                    plaintext.Should().Be(plaintextObject);
                }
            }
        }
    }
}