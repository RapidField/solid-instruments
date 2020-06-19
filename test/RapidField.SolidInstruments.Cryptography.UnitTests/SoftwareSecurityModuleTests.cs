// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Secrets;
using System;
using System.Security;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests
{
    [TestClass]
    public class SoftwareSecurityModuleTests
    {
        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForModelPlaintext_UsingMultipleModules_WithSharedMasterPassword()
        {
            // Arrange.
            using var randomnessProvider = RandomNumberGenerator.Create();
            var plaintext = SimulatedModel.Random(randomnessProvider);
            using var masterPassword = Password.NewStrongPassword();
            using var targetOne = new SoftwareSecurityModule(masterPassword, true);
            using var targetTwo = new SoftwareSecurityModule(masterPassword, true);

            // Act.
            var ciphertext = targetOne.Encrypt(plaintext);
            var result = targetTwo.Decrypt(ciphertext);

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForModelPlaintext_UsingSingleModule_WithMatchingCascadingSymmetricKey()
        {
            // Arrange.
            using var randomnessProvider = RandomNumberGenerator.Create();
            var plaintext = SimulatedModel.Random(randomnessProvider);
            using var masterPassword = Password.NewStrongPassword();
            using var target = new SoftwareSecurityModule(masterPassword, true);
            var keyName = target.NewCascadingSymmetricKey();

            // Act.
            var ciphertext = target.Encrypt(plaintext, keyName);
            var result = target.Decrypt(ciphertext, keyName);

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForModelPlaintext_UsingSingleModule_WithMatchingSymmetricKeys()
        {
            // Arrange.
            using var randomnessProvider = RandomNumberGenerator.Create();
            var plaintext = SimulatedModel.Random(randomnessProvider);
            using var masterPassword = Password.NewStrongPassword();
            using var target = new SoftwareSecurityModule(masterPassword, true);
            var keyName = target.NewSymmetricKey();

            // Act.
            var ciphertext = target.Encrypt(plaintext, keyName);
            var result = target.Decrypt(ciphertext, keyName);

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForStringPlaintext_UsingMultipleModules_WithSharedMasterPassword()
        {
            // Arrange.
            var plaintext = "foo";
            using var masterPassword = Password.NewStrongPassword();
            using var targetOne = new SoftwareSecurityModule(masterPassword, true);
            using var targetTwo = new SoftwareSecurityModule(masterPassword, true);

            // Act.
            var ciphertext = targetOne.Encrypt(plaintext);
            var result = targetTwo.Decrypt(ciphertext);

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForStringPlaintext_UsingSingleModule_WithMatchingCascadingSymmetricKey()
        {
            // Arrange.
            var plaintext = "foo";
            using var masterPassword = Password.NewStrongPassword();
            using var target = new SoftwareSecurityModule(masterPassword, true);
            var keyName = target.NewCascadingSymmetricKey();

            // Act.
            var ciphertext = target.Encrypt(plaintext, keyName);
            var result = target.Decrypt(ciphertext, keyName);

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldBeReversible_ForStringPlaintext_UsingSingleModule_WithMatchingSymmetricKeys()
        {
            // Arrange.
            var plaintext = "foo";
            using var masterPassword = Password.NewStrongPassword();
            using var target = new SoftwareSecurityModule(masterPassword, true);
            var keyName = target.NewSymmetricKey();

            // Act.
            var ciphertext = target.Encrypt(plaintext, keyName);
            var result = target.Decrypt(ciphertext, keyName);

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(plaintext);
        }

        [TestMethod]
        public void Encrypt_ShouldNotBeReversible_ForModelPlaintext_UsingMultipleModules_WithDifferingMasterPassword()
        {
            // Arrange.
            using var randomnessProvider = RandomNumberGenerator.Create();
            var plaintext = SimulatedModel.Random(randomnessProvider);
            using var masterPasswordOne = Password.NewStrongPassword();
            using var masterPasswordTwo = Password.NewStrongPassword();
            using var targetOne = new SoftwareSecurityModule(masterPasswordOne, true);
            using var targetTwo = new SoftwareSecurityModule(masterPasswordTwo, true);

            // Act.
            var ciphertext = targetOne.Encrypt(plaintext);
            var action = new Action(() => targetTwo.Decrypt(ciphertext));

            // Assert.
            action.Should().Throw<SecurityException>();
        }

        [TestMethod]
        public void Encrypt_ShouldNotBeReversible_ForStringPlaintext_UsingMultipleModules_WithDifferingMasterPassword()
        {
            // Arrange.
            var plaintext = "foo";
            using var masterPasswordOne = Password.NewStrongPassword();
            using var masterPasswordTwo = Password.NewStrongPassword();
            using var targetOne = new SoftwareSecurityModule(masterPasswordOne, true);
            using var targetTwo = new SoftwareSecurityModule(masterPasswordTwo, true);

            // Act.
            var ciphertext = targetOne.Encrypt(plaintext);
            var action = new Action(() => targetTwo.Decrypt(ciphertext));

            // Assert.
            action.Should().Throw<SecurityException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            using (var masterPassword = Password.NewStrongPassword())
            {
                // Arrange.
                var plaintext = "foo";
                var guidSecretValue = Guid.NewGuid();
                var guidSecretName = "GuidSecret";
                var ciphertextOne = (String)null;
                var ciphertextTwo = (String)null;
                var keyName = (String)null;
                var resultOne = (String)null;
                var resultTwo = (String)null;

                using (var target = new SoftwareSecurityModule(masterPassword, false))
                {
                    // Act.
                    keyName = target.NewSymmetricKey();
                    ciphertextOne = target.Encrypt(plaintext);
                    ciphertextTwo = target.Encrypt(plaintext, keyName);
                    target.AddOrUpdate(guidSecretName, guidSecretValue);

                    // Assert.
                    target.SecretCount.Should().Be(3);
                    target.SecretNames.Should().Contain(keyName);
                    target.SecretNames.Should().Contain(guidSecretName);
                    ciphertextOne.Should().NotBeNullOrEmpty();
                    ciphertextTwo.Should().NotBeNullOrEmpty();
                    ciphertextOne.Should().NotBe(ciphertextTwo);
                }

                using (var target = new SoftwareSecurityModule(masterPassword, true))
                {
                    // Assert.
                    target.SecretCount.Should().Be(3);
                    target.SecretNames.Should().Contain(keyName);
                    target.SecretNames.Should().Contain(guidSecretName);

                    // Act.
                    resultOne = target.Decrypt(ciphertextOne);
                    resultTwo = target.Decrypt(ciphertextTwo, keyName);

                    // Assert.
                    target.SecretReader.ReadAsync(guidSecretName, (Guid secretValue) => { secretValue.Should().Be(guidSecretValue); }).Wait();
                }

                // Assert.
                resultOne.Should().NotBeNullOrEmpty();
                resultOne.Should().Be(plaintext);
                resultTwo.Should().NotBeNullOrEmpty();
                resultTwo.Should().Be(plaintext);
            }
        }
    }
}