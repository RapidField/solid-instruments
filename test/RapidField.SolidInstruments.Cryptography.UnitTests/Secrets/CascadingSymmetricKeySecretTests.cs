// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Secrets;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using System;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class CascadingSymmetricKeySecretTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var name = "foo";
            var valueOne = CascadingSymmetricKey.New();
            var valueTwo = CascadingSymmetricKey.New();
            var valueThree = CascadingSymmetricKey.New();
            var hashCode = 0;
            var symmetricProcessor = new SymmetricStringProcessor(SecureMemory.RandomnessProvider);
            var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";
            var ciphertextObject = Array.Empty<Byte>();

            using (var target = new CascadingSymmetricKeySecret(name))
            {
                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.Name.Should().Be(name);
                target.HasValue.Should().BeFalse();
                target.ValueType.Should().Be(typeof(CascadingSymmetricKey));

                // Act.
                target.Write(() => valueOne);

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    var ciphertext = symmetricProcessor.Encrypt(plaintextObject, secret);
                    ciphertext.Should().NotBeEquivalentTo(ciphertextObject);
                    ciphertextObject = ciphertext;
                    var plaintext = symmetricProcessor.Decrypt(ciphertext, secret);
                    plaintext.Should().NotBeNullOrEmpty();
                    plaintext.Should().Be(plaintextObject);
                });

                // Act.
                target.Write(() => valueTwo);

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    var ciphertext = symmetricProcessor.Encrypt(plaintextObject, secret);
                    ciphertext.Should().NotBeEquivalentTo(ciphertextObject);
                    ciphertextObject = ciphertext;
                    var plaintext = symmetricProcessor.Decrypt(ciphertext, secret);
                    plaintext.Should().NotBeNullOrEmpty();
                    plaintext.Should().Be(plaintextObject);
                });

                // Act.
                target.Write(() => valueThree);

                // Assert.
                hashCode.Should().NotBe(target.GetHashCode());
                hashCode = target.GetHashCode();
                hashCode.Should().Be(target.GetHashCode());
                target.HasValue.Should().BeTrue();
                target.Read(secret =>
                {
                    var ciphertext = symmetricProcessor.Encrypt(plaintextObject, secret);
                    ciphertext.Should().NotBeEquivalentTo(ciphertextObject);
                    ciphertextObject = ciphertext;
                    var plaintext = symmetricProcessor.Decrypt(ciphertext, secret);
                    plaintext.Should().NotBeNullOrEmpty();
                    plaintext.Should().Be(plaintextObject);
                });
            }

            // Assert.
            valueOne.Should().NotBeNull();
            valueTwo.Should().NotBeNull();
            valueThree.Should().NotBeNull();
        }
    }
}