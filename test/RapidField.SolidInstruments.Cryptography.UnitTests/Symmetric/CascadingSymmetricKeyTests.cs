// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class CascadingSymmetricKeyTests
    {
        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForUnspecifiedAlgorithm()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForUnspecifiedDerivationMode()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Unspecified;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            // Act.
            var action = new Action(() =>
            {
                using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void New_ShouldReturnValidKey_ForValidArguments()
        {
            // Arrange.
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
            {
                // Assert.
                target.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void ToBuffer_ShouldBeReversible_WithFourLayers()
        {
            // Arrange.
            var depth = 4;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
            {
                // Assert.
                ToBuffer_ShouldBeReversible(target, depth);
            }
        }

        [TestMethod]
        public void ToBuffer_ShouldBeReversible_WithThreeLayers()
        {
            // Arrange.
            var depth = 3;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm))
            {
                // Assert.
                ToBuffer_ShouldBeReversible(target, depth);
            }
        }

        [TestMethod]
        public void ToBuffer_ShouldBeReversible_WithTwoLayers()
        {
            // Arrange.
            var depth = 2;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm))
            {
                // Assert.
                ToBuffer_ShouldBeReversible(target, depth);
            }
        }

        [TestMethod]
        public void ToBuffer_ShouldReturnValidResult()
        {
            // Arrange.
            var cascadingKeyLengthInBytes = 1666;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var firstLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var secondLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var thirdLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            var fourthLayerAlgorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            using (var target = CascadingSymmetricKey.New(derivationMode, firstLayerAlgorithm, secondLayerAlgorithm, thirdLayerAlgorithm, fourthLayerAlgorithm))
            {
                // Act.
                using (var buffer = target.ToBuffer())
                {
                    buffer.Access(plaintext =>
                    {
                        // Assert.
                        plaintext.Should().NotBeNullOrEmpty();
                        plaintext.Length.Should().Be(cascadingKeyLengthInBytes);
                        plaintext.Count(value => value == 0x00).Should().NotBe(plaintext.Length);
                    });
                }
            }
        }

        private static void ToBuffer_ShouldBeReversible(CascadingSymmetricKey target, Int32 depth)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var processor = new SymmetricStringProcessor(randomnessProvider);
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";
                var ciphertext = processor.Encrypt(plaintextObject, target);

                // Act.
                using (var buffer = target.ToBuffer())
                {
                    using (var parityKey = CascadingSymmetricKey.FromBuffer(buffer))
                    {
                        // Arrange.
                        var result = processor.Decrypt(ciphertext, parityKey);

                        // Assert.
                        result.Should().Be(plaintextObject);
                    }
                }
            }
        }
    }
}