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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class SecretVaultTests
    {
        [TestMethod]
        public void ExportAsync_ShouldBeReversible_UsingExplicitCascadingSymmetricKey()
        {
            // TODO Write this test.
        }

        [TestMethod]
        public void ExportAsync_ShouldBeReversible_UsingExplicitSymmetricKey()
        {
            // TODO Write this test.
        }

        [TestMethod]
        public void ExportAsync_ShouldBeReversible_UsingMasterKey()
        {
            // TODO Write this test.
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForCascadingSymmetricKeySecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            using var secret = CascadingSymmetricKey.New();
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(secretName, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForCascadingSymmetricKeySecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            using var secret = CascadingSymmetricKey.New();
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(secretName, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForGuidSecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            var secret = Guid.NewGuid();
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForGuidSecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            var secret = Guid.NewGuid();
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForNumericSecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            var secret = 1234.56789d;
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForNumericSecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            var secret = 1234.56789d;
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForStringSecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            var secret = "baz";
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForStringSecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            var secret = "baz";
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(secretName, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForSymmetricKeySecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            using var secret = SymmetricKey.New();
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(secretName, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForSymmetricKeySecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            using var secret = SymmetricKey.New();
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(secretName, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForX509CertificateSecrets_AsCiphertext_UsingExplicitKey()
        {
            // Arrange.
            var secretName = "foo";
            var keyName = "bar";
            var fileName = "TestRootOne.testcert";
            var subject = "CN=TestRootOne";
            var secret = new X509Certificate2(fileName);
            using var key = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(keyName, key);
            targetTwo.AddOrUpdate(keyName, key);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result, keyName);

            // Assert.
            targetOne.ReadAsync(secretName, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
            targetTwo.ReadAsync(secretName, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
        }

        [TestMethod]
        public void ExportEncryptedSecret_ShouldBeReversible_ForX509CertificateSecrets_AsCiphertext_UsingMasterKey()
        {
            // Arrange.
            var secretName = "foo";
            var fileName = "TestRootOne.testcert";
            var subject = "CN=TestRootOne";
            var secret = new X509Certificate2(fileName);
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);

            // Act.
            targetOne.AddOrUpdate(secretName, secret);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportEncryptedSecretAsync(secretName);
            exportSecretTaskTwo.Wait();
            targetOne.ImportEncryptedSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(secretName, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
            targetTwo.ReadAsync(secretName, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
        }

        [TestMethod]
        public void ExportMasterKey_ShouldBeReversible()
        {
            // Arrange.
            using var masterPassword = Password.FromUnicodeString("12345 bar BAZ !");
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault();

            // Act.
            using var exportMasterKeyTaskOne = targetOne.ExportMasterKeyAsync();
            exportMasterKeyTaskOne.Wait();
            targetTwo.ImportSecret(exportMasterKeyTaskOne.Result);
            using var exportMasterKeyTaskTwo = targetTwo.ExportSecretAsync(exportMasterKeyTaskOne.Result.Name);
            exportMasterKeyTaskTwo.Wait();
            targetOne.ImportSecret(exportMasterKeyTaskTwo.Result);

            // Arrange.
            using var masterKeySecretOne = exportMasterKeyTaskOne.Result.ToSecret();
            using var masterKeySecretTwo = exportMasterKeyTaskTwo.Result.ToSecret();

            masterKeySecretOne.Read(masterKeyOne =>
            {
                masterKeySecretTwo.Read(masterKeyTwo =>
                {
                    // Assert.
                    masterKeyOne.Should().NotBeNullOrEmpty();
                    masterKeyTwo.Should().NotBeNullOrEmpty();
                    masterKeyOne.Should().BeEquivalentTo(masterKeyTwo);
                });
            });
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForCascadingSymmetricKeySecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            using var secret = CascadingSymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(name, (CascadingSymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForGuidSecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            var secret = Guid.NewGuid();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(name, (Guid value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForNumericSecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            var secret = 1234.56789d;
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(name, (Double value) =>
            {
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForStandardSecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            var secret = new Byte[3] { 0xff, 0xa9, 0x00 };
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (IReadOnlyPinnedMemory<Byte> value) =>
            {
                value.Should().NotBeNull();
                value.Should().BeEquivalentTo(secret);
            }).Wait();
            targetTwo.ReadAsync(name, (IReadOnlyPinnedMemory<Byte> value) =>
            {
                value.Should().NotBeNull();
                value.Should().BeEquivalentTo(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForStringSecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            var secret = "bar";
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
            targetTwo.ReadAsync(name, (String value) =>
            {
                value.Should().NotBeNullOrEmpty();
                value.Should().Be(secret);
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForSymmetricKeySecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            using var secret = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
            targetTwo.ReadAsync(name, (SymmetricKey value) =>
            {
                value.Should().NotBeNull();
            }).Wait();
        }

        [TestMethod]
        public void ExportSecret_ShouldBeReversible_ForX509CertificateSecrets_AsPlaintext()
        {
            // Arrange.
            var name = "foo";
            var fileName = "TestRootOne.testcert";
            var subject = "CN=TestRootOne";
            var secret = new X509Certificate2(fileName);
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();

            // Act.
            targetOne.AddOrUpdate(name, secret);
            using var exportSecretTaskOne = targetOne.ExportSecretAsync(name);
            exportSecretTaskOne.Wait();
            targetTwo.ImportSecret(exportSecretTaskOne.Result);
            using var exportSecretTaskTwo = targetTwo.ExportSecretAsync(name);
            exportSecretTaskTwo.Wait();
            targetOne.ImportSecret(exportSecretTaskTwo.Result);

            // Assert.
            targetOne.ReadAsync(name, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
            targetTwo.ReadAsync(name, (X509Certificate2 value) =>
            {
                value.Should().NotBeNull();
                value.Subject.Should().Be(subject);
            }).Wait();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForCascadingSymmetricKeySecrets()
        {
            // Arrange.
            using var secretOne = CascadingSymmetricKey.New();
            using var secretTwo = CascadingSymmetricKey.New();
            using var secretThree = CascadingSymmetricKey.New();
            var secretOneName = "foo";
            var secretTwoName = "bar";

            using (var target = new SecretVault())
            {
                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);
                target.ReadAsync(secretTwoName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretOneName, secretThree);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.TryRemove("baz").Should().BeFalse();
                target.TryRemove(secretOneName).Should().BeTrue();

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForGuidSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = Guid.NewGuid();
                var secretTwo = Guid.NewGuid();
                var secretThree = Guid.NewGuid();
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Guid value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForNumericSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = randomnessProvider.GetDouble();
                var secretTwo = randomnessProvider.GetDouble();
                var secretThree = randomnessProvider.GetDouble();
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Double value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForStandardSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = new Byte[3];
                var secretTwo = new Byte[21];
                var secretThree = new Byte[89];
                randomnessProvider.GetBytes(secretOne);
                randomnessProvider.GetBytes(secretTwo);
                randomnessProvider.GetBytes(secretThree);
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForStringSecrets()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var secretOne = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretTwo = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretThree = randomnessProvider.GetString(8, false, true, true, true, true, false, false);
                var secretOneName = "foo";
                var secretTwoName = "bar";

                using (var target = new SecretVault())
                {
                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (String value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.Count.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.Count.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.Count.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForSymmetricKeySecrets()
        {
            // Arrange.
            using var secretOne = SymmetricKey.New();
            using var secretTwo = SymmetricKey.New();
            using var secretThree = SymmetricKey.New();
            var secretOneName = "foo";
            var secretTwoName = "bar";

            using (var target = new SecretVault())
            {
                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);
                target.ReadAsync(secretTwoName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretOneName, secretThree);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.TryRemove("baz").Should().BeFalse();
                target.TryRemove(secretOneName).Should().BeTrue();

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForX509CertificateSecrets()
        {
            // Arrange.
            var fileNameOne = "TestRootOne.testcert";
            var fileNameTwo = "TestRootTwo.testcert";
            var fileNameThree = "TestRootThree.testcert";
            var subjectOne = "CN=TestRootOne";
            var subjectTwo = "CN=TestRootTwo";
            var subjectThree = "CN=TestRootThree";
            var secretOne = new X509Certificate2(fileNameOne);
            var secretTwo = new X509Certificate2(fileNameTwo);
            var secretThree = new X509Certificate2(fileNameThree);
            var secretOneName = "foo";
            var secretTwoName = "bar";

            using (var target = new SecretVault())
            {
                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (X509Certificate2 value) =>
                {
                    value.Should().NotBeNull();
                    value.Subject.Should().Be(subjectOne);
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);
                target.ReadAsync(secretTwoName, (X509Certificate2 value) =>
                {
                    value.Should().NotBeNull();
                    value.Subject.Should().Be(subjectTwo);
                }).Wait();

                // Act.
                target.AddOrUpdate(secretOneName, secretThree);

                // Assert.
                target.Count.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (X509Certificate2 value) =>
                {
                    value.Should().NotBeNull();
                    value.Subject.Should().Be(subjectThree);
                }).Wait();

                // Act.
                target.TryRemove("baz").Should().BeFalse();
                target.TryRemove(secretOneName).Should().BeTrue();

                // Assert.
                target.Count.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.Count.Should().Be(0);
                target.SecretNames.Should().BeEmpty();
            }
        }
    }
}