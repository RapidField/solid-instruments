// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public class ISymmetricProcessorExtensionsTests
    {
        [TestMethod]
        public void EncryptToBase64String_ShouldBeReversible()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = new SymmetricStringProcessor(randomnessProvider);
                var algorithm = SymmetricAlgorithmSpecification.Aes128Cbc;
                var plaintextObject = "aZ09`ಮ";

                using (var key = SymmetricKey.New(algorithm))
                {
                    // Act.
                    var ciphertext = target.EncryptToBase64String(plaintextObject, key);

                    // Assert.
                    ciphertext.Should().NotBeNullOrEmpty();

                    // Act.
                    var result = target.DecryptFromBase64String(ciphertext, key);

                    // Assert.
                    result.Should().NotBeNullOrEmpty();
                    result.Should().Be(plaintextObject);
                }
            }
        }
    }
}