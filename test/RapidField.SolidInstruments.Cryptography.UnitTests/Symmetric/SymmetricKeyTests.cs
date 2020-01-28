// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Symmetric
{
    [TestClass]
    public class SymmetricKeyTests
    {
        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForInvalidLengthKeySource()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var derivationMode = SymmetricKeyDerivationMode.Truncation;
            var keyLength = 3;

            using (var keySource = new PinnedBuffer(keyLength))
            {
                // Act.
                var action = new Action(() =>
                {
                    using (var target = SymmetricKey.New(algorithm, derivationMode, keySource))
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
            var derivationMode = SymmetricKeyDerivationMode.Truncation;

            // Act.
            var action = new Action(() =>
            {
                using (var target = SymmetricKey.New(algorithm, derivationMode))
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
            var derivationMode = SymmetricKeyDerivationMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                using (var target = SymmetricKey.New(algorithm, derivationMode))
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
            using (var target = SymmetricKey.New())
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

                using (var target = SymmetricKey.New())
                {
                    // Arrange.
                    var ciphertext = processor.Encrypt(plaintextObject, target);

                    // Act.
                    using (var buffer = target.ToBuffer())
                    {
                        using (var parityKey = SymmetricKey.FromBuffer(buffer))
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
            var keyLengthInBytes = 416;

            using (var target = SymmetricKey.New())
            {
                // Act.
                using (var buffer = target.ToBuffer())
                {
                    buffer.Access((plaintext =>
                    {
                        // Assert.
                        plaintext.Should().NotBeNullOrEmpty();
                        plaintext.Length.Should().Be(keyLengthInBytes);
                        plaintext.Count(value => value == 0x00).Should().NotBe((Int32)plaintext.Length);
                    }));
                }
            }
        }

        [TestMethod]
        public void ToDerivedKeyBytes_ShouldReturnValidResult_ForAes128Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;

            // Assert.
            ToDerivedKeyBytes_ShouldReturnValidResult(algorithm);
        }

        [TestMethod]
        public void ToDerivedKeyBytes_ShouldReturnValidResult_ForAes128Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Ecb;

            // Assert.
            ToDerivedKeyBytes_ShouldReturnValidResult(algorithm);
        }

        [TestMethod]
        public void ToDerivedKeyBytes_ShouldReturnValidResult_ForAes256Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;

            // Assert.
            ToDerivedKeyBytes_ShouldReturnValidResult(algorithm);
        }

        [TestMethod]
        public void ToDerivedKeyBytes_ShouldReturnValidResult_ForAes256Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;

            // Assert.
            ToDerivedKeyBytes_ShouldReturnValidResult(algorithm);
        }

        private static void ToDerivedKeyBytes_ShouldReturnValidResult(SymmetricAlgorithmSpecification algorithm)
        {
            using (var target = SymmetricKey.New(algorithm))
            {
                // Act.
                using (var result = target.ToDerivedKeyBytes())
                {
                    // Assert.
                    result.Should().NotBeNull();
                    result.LengthInBytes.Should().Be(algorithm.ToKeyBitLength() / 8);
                }
            }
        }
    }
}