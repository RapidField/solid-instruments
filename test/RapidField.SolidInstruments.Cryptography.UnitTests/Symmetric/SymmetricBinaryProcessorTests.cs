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
    public class SymmetricBinaryProcessorTests
    {
        [TestMethod]
        public void Encrypt_ShouldReproduceTestVector_ForAes128Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes128Ecb;
            var blockLengthInBits = 128;
#pragma warning disable SecurityIntelliSenseCS
            var cipherMode = CipherMode.ECB;
#pragma warning restore SecurityIntelliSenseCS
            var key = new Byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
            var initializationVector = (Byte[])null;
            var plaintext = new Byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff };
            var ciphertext = new Byte[] { 0x69, 0xc4, 0xe0, 0xd8, 0x6a, 0x7b, 0x04, 0x30, 0xd8, 0xcd, 0xb7, 0x80, 0x70, 0xb4, 0xc5, 0x5a };

            // Assert.
            Encrypt_ShouldReproduceTestVector(algorithm, blockLengthInBits, cipherMode, key, initializationVector, plaintext, ciphertext);
        }

        [TestMethod]
        public void Encrypt_ShouldReproduceTestVector_ForAes256Cbc()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Aes256Cbc;
            var blockLengthInBits = 128;
            var cipherMode = CipherMode.CBC;
            var key = new Byte[] { 0x60, 0x3d, 0xeb, 0x10, 0x15, 0xca, 0x71, 0xbe, 0x2b, 0x73, 0xae, 0xf0, 0x85, 0x7d, 0x77, 0x81, 0x1f, 0x35, 0x2c, 0x07, 0x3b, 0x61, 0x08, 0xd7, 0x2d, 0x98, 0x10, 0xa3, 0x09, 0x14, 0xdf, 0xf4 };
            var initializationVector = new Byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
            var plaintext = new Byte[] { 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a, 0xae, 0x2d, 0x8a, 0x57, 0x1e, 0x03, 0xac, 0x9c, 0x9e, 0xb7, 0x6f, 0xac, 0x45, 0xaf, 0x8e, 0x51, 0x30, 0xc8, 0x1c, 0x46, 0xa3, 0x5c, 0xe4, 0x11, 0xe5, 0xfb, 0xc1, 0x19, 0x1a, 0x0a, 0x52, 0xef, 0xf6, 0x9f, 0x24, 0x45, 0xdf, 0x4f, 0x9b, 0x17, 0xad, 0x2b, 0x41, 0x7b, 0xe6, 0x6c, 0x37, 0x10 };
            var ciphertext = new Byte[] { 0xf5, 0x8c, 0x4c, 0x04, 0xd6, 0xe5, 0xf1, 0xba, 0x77, 0x9e, 0xab, 0xfb, 0x5f, 0x7b, 0xfb, 0xd6, 0x9c, 0xfc, 0x4e, 0x96, 0x7e, 0xdb, 0x80, 0x8d, 0x67, 0x9f, 0x77, 0x7b, 0xc6, 0x70, 0x2c, 0x7d, 0x39, 0xf2, 0x33, 0x69, 0xa9, 0xd9, 0xba, 0xcf, 0xa5, 0x30, 0xe2, 0x63, 0x04, 0x23, 0x14, 0x61, 0xb2, 0xeb, 0x05, 0xe2, 0xc3, 0x9b, 0xe9, 0xfc, 0xda, 0x6c, 0x19, 0x07, 0x8c, 0x6a, 0x9d, 0x1b };

            // Assert.
            Encrypt_ShouldReproduceTestVector(algorithm, blockLengthInBits, cipherMode, key, initializationVector, plaintext, ciphertext);
        }

        /* =================================================================================================================
         * Twofish and Threefish are out-of-scope for MVP-01.
         * =================================================================================================================
        [TestMethod]
        public void Encrypt_ShouldReproduceTestVector_ForTwofish128Ecb()
        {
            // Arrange.
            var algorithm = SymmetricAlgorithmSpecification.Twofish128Ecb;
            var blockLengthInBits = 128;
            var cipherMode = CipherMode.ECB;
            var key = new Byte[] { 0x9f, 0x58, 0x9f, 0x5c, 0xf6, 0x12, 0x2c, 0x32, 0xb6, 0xbf, 0xec, 0x2f, 0x2a, 0xe8, 0xc3, 0x5a };
            var initializationVector = (Byte[])null;
            var plaintext = new Byte[] { 0xd4, 0x91, 0xdB, 0x16, 0xe7, 0xb1, 0xc3, 0x9e, 0x86, 0xcb, 0x08, 0x6b, 0x78, 0x9f, 0x54, 0x19 };
            var ciphertext = new Byte[] { 0x01, 0x9f, 0x98, 0x09, 0xde, 0x17, 0x11, 0x85, 0x8f, 0xaa, 0xc3, 0xa3, 0xba, 0x20, 0xfb, 0xc3 };

            // Assert.
            Encrypt_ShouldReproduceTestVector(algorithm, blockLengthInBits, cipherMode, key, initializationVector, plaintext, ciphertext);
        }*/

        private static void Encrypt_ShouldReproduceTestVector(SymmetricAlgorithmSpecification algorithm, Int32 blockLengthInBits, CipherMode cipherMode, Byte[] key, Byte[] initializationVector, Byte[] plaintext, Byte[] ciphertext)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                using (var secureKey = new SecureBuffer(key.Length))
                {
                    // Arrange.
                    var blockLengthInBytes = (blockLengthInBits / 8);
                    var target = new SymmetricBinaryProcessor(randomnessProvider);
                    secureKey.Access(keyBuffer =>
                    {
                        Array.Copy(key, keyBuffer, key.Length);
                    });

                    // Act.
                    var encryptResult = target.Encrypt(plaintext, secureKey, algorithm, initializationVector);

                    // Assert.
                    encryptResult.Should().NotBeNullOrEmpty();

                    // Arrange.
                    var processedEncryptResult = cipherMode == CipherMode.CBC ? encryptResult.Skip(blockLengthInBytes).Take(ciphertext.Length).ToArray() : encryptResult.Take(ciphertext.Length).ToArray();

                    // Assert.
                    processedEncryptResult.Length.Should().Be(ciphertext.Length);

                    for (var i = 0; i < ciphertext.Length; i++)
                    {
                        // Assert.
                        processedEncryptResult[i].Should().Be(ciphertext[i]);
                    }
                }
            }
        }
    }
}