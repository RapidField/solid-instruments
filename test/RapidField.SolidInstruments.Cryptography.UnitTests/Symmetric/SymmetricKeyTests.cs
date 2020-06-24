// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Secrets;
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
        public void FromPassword_ShouldBeRepeatable()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var processor = new SymmetricStringProcessor(randomnessProvider);
                var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
                var derivationMode = CryptographicKeyDerivationMode.XorLayering;
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";
                using var password = Password.FromUnicodeString(randomnessProvider.GetString(SymmetricKey.MinimumPasswordLength, true, true, true, true, true, true, false));

                // Act.
                using (var targetOne = SymmetricKey.FromPassword(password, algorithm, derivationMode))
                {
                    using (var targetTwo = SymmetricKey.FromPassword(password, algorithm, derivationMode))
                    {
                        // Arrange.
                        var ciphertextOne = processor.Encrypt(plaintextObject, targetOne);
                        var ciphertextTwo = processor.Encrypt(plaintextObject, targetTwo);
                        var plaintextOne = processor.Decrypt(ciphertextOne, targetOne);
                        var plaintextTwo = processor.Decrypt(ciphertextTwo, targetTwo);

                        // Assert.
                        ciphertextOne.Should().NotBeEquivalentTo(ciphertextTwo);
                        plaintextOne.Should().Be(plaintextTwo);
                        plaintextOne.Should().Be(plaintextObject);
                    }
                }
            }
        }

        [TestMethod]
        public void FromPassword_ShouldRaiseArgumentException_ForInvalidPasswordLength()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                using var password = Password.FromUnicodeString(randomnessProvider.GetString(12, true, true, true, true, true, true, false));

                // Act.
                var action = new Action(() =>
                {
                    using (var target = SymmetricKey.FromPassword(password))
                    {
                        return;
                    }
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void FromPassword_ShouldRaiseArgumentNullException_ForNullPassword()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var password = (Password)null;

                // Act.
                var action = new Action(() =>
                {
                    using (var target = SymmetricKey.FromPassword(password))
                    {
                        return;
                    }
                });

                // Assert.
                action.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public void FromPassword_ShouldReturnValidResult_ForAes128Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
            using var password = Password.FromAsciiString("zmDsQ5b58pE7p");

            // Act.
            using (var target = SymmetricKey.FromPassword(password, algorithm))
            {
                // Assert.
                ToDerivedKeyBytes_ShouldReturnValidResult(algorithm, target);
            }
        }

        [TestMethod]
        public void FromPassword_ShouldReturnValidResult_ForAes128Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            using var password = Password.FromAsciiString("zmDsQ5b58pE7p");

            // Act.
            using (var target = SymmetricKey.FromPassword(password, algorithm))
            {
                // Assert.
                ToDerivedKeyBytes_ShouldReturnValidResult(algorithm, target);
            }
        }

        [TestMethod]
        public void FromPassword_ShouldReturnValidResult_ForAes256Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            using var password = Password.FromAsciiString("zmDsQ5b58pE7p");

            // Act.
            using (var target = SymmetricKey.FromPassword(password, algorithm))
            {
                // Assert.
                ToDerivedKeyBytes_ShouldReturnValidResult(algorithm, target);
            }
        }

        [TestMethod]
        public void FromPassword_ShouldReturnValidResult_ForAes256Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Ecb;
            using var password = Password.FromAsciiString("zmDsQ5b58pE7p");

            // Act.
            using (var target = SymmetricKey.FromPassword(password, algorithm))
            {
                // Assert.
                ToDerivedKeyBytes_ShouldReturnValidResult(algorithm, target);
            }
        }

        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForInvalidLengthKeySource()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var derivationMode = CryptographicKeyDerivationMode.Truncation;
            var keyLength = 3;

            using (var keySource = new PinnedMemory(keyLength))
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
            var derivationMode = CryptographicKeyDerivationMode.Truncation;

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
            var derivationMode = CryptographicKeyDerivationMode.Unspecified;

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
        public void SubstitutionBoxBytes_ShouldBeValid()
        {
            // Arrange.
            var substitutionBoxBytes = SymmetricKey.SubstitutionBoxBytes;

            // Assert.
            substitutionBoxBytes.Should().NotBeNullOrEmpty();
            substitutionBoxBytes.Count().Should().Be(256);
            substitutionBoxBytes.Should().OnlyHaveUniqueItems();
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

        [TestMethod]
        public void ToSecureMemory_ShouldBeReversible()
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
                    using (var secureMemory = target.ToSecureMemory())
                    {
                        using (var parityKey = SymmetricKey.FromSecureMemory(secureMemory))
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
        public void ToSecureMemory_ShouldReturnValidResult()
        {
            // Arrange.
            var keyLengthInBytes = 416;

            using (var target = SymmetricKey.New())
            {
                // Act.
                using (var secureMemory = target.ToSecureMemory())
                {
                    secureMemory.Access(plaintext =>
                    {
                        // Assert.
                        plaintext.Should().NotBeNullOrEmpty();
                        plaintext.Length.Should().Be(keyLengthInBytes);
                        plaintext.Count(value => value == 0x00).Should().NotBe((Int32)plaintext.Length);
                    });
                }
            }
        }

        private static void ToDerivedKeyBytes_ShouldReturnValidResult(SymmetricAlgorithmSpecification algorithm)
        {
            using (var target = SymmetricKey.New(algorithm))
            {
                ToDerivedKeyBytes_ShouldReturnValidResult(algorithm, target);
            }
        }

        private static void ToDerivedKeyBytes_ShouldReturnValidResult(SymmetricAlgorithmSpecification algorithm, SymmetricKey target)
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