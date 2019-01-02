// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class SecureSymmetricKeyTests
    {
        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForInvalidLengthKeySource()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;
            var keyLength = 3;

            using (var keySource = new PinnedBuffer(keyLength))
            {
                // Act.
                var action = new Action(() =>
                {
                    using (var target = SecureSymmetricKey.New(algorithm, derivationMode, keySource))
                    {
                        return;
                    }
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForUnspecifiedAlgorithm()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Unspecified;
            var derivationMode = SecureSymmetricKeyDerivationMode.Truncation;

            // Act.
            var action = new Action(() =>
            {
                using (var target = SecureSymmetricKey.New(algorithm, derivationMode))
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
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var derivationMode = SecureSymmetricKeyDerivationMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                using (var target = SecureSymmetricKey.New(algorithm, derivationMode))
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
            using (var target = SecureSymmetricKey.New())
            {
                // Assert.
                target.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void ToBuffer_ShouldBeReversible()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var processor = new SymmetricStringProcessor(randomnessProvider);
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";

                using (var target = SecureSymmetricKey.New())
                {
                    // Arrange.
                    var ciphertext = processor.Encrypt(plaintextObject, target);

                    // Act.
                    using (var buffer = target.ToBuffer())
                    {
                        using (var parityKey = SecureSymmetricKey.FromBuffer(buffer))
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

        [TestMethod]
        public void ToBuffer_ShouldReturnValidResult()
        {
            // Arrange.
            var secureKeyLengthInBytes = 416;

            using (var target = SecureSymmetricKey.New())
            {
                // Act.
                using (var buffer = target.ToBuffer())
                {
                    buffer.Access(plaintext =>
                    {
                        // Assert.
                        plaintext.Should().NotBeNullOrEmpty();
                        plaintext.Length.Should().Be(secureKeyLengthInBytes);
                        plaintext.Count(value => value == 0x00).Should().NotBe(plaintext.Length);
                    });
                }
            }
        }
    }
}