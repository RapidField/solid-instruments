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
            // Arrange.
            var exportKeyName = "ExportKey";
            var secretOneName = "SecretOne";
            var secretTwoName = "SecretTwo";
            var secretNames = new String[] { exportKeyName, secretOneName, secretTwoName };
            var secretOneValue = "foo";
            var secretTwoValue = 123.456d;
            using var exportKey = CascadingSymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(exportKeyName, exportKey);
            targetOne.AddOrUpdate(secretOneName, secretOneValue);
            targetOne.AddOrUpdate(secretTwoName, secretTwoValue);
            targetTwo.AddOrUpdate(exportKeyName, exportKey);

            // Act.
            targetOne.ExportAsync(exportKeyName).ContinueWith(exportTask =>
            {
                targetTwo.ImportEncryptedSecretVault(exportTask.Result, exportKeyName);
            }).Wait();

            // Assert.
            targetOne.SecretCount.Should().Be(3);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetOne.SymmetricKeySecretNames.Should().Contain(exportKeyName);
            targetTwo.SecretCount.Should().Be(3);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetTwo.SymmetricKeySecretNames.Should().Contain(exportKeyName);
            targetTwo.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetTwo.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();

            // Arrange.
            secretOneValue = "bar";
            secretTwoValue = 654.321d;
            targetTwo.AddOrUpdate(secretOneName, secretOneValue);
            targetTwo.AddOrUpdate(secretTwoName, secretTwoValue);

            // Act.
            targetTwo.ExportAsync(exportKeyName).ContinueWith(exportTask =>
            {
                targetOne.ImportEncryptedSecretVault(exportTask.Result, exportKeyName);
            }).Wait();

            // Assert.
            targetTwo.SecretCount.Should().Be(3);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetOne.SecretCount.Should().Be(3);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetOne.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetOne.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();
        }

        [TestMethod]
        public void ExportAsync_ShouldBeReversible_UsingExplicitSymmetricKey()
        {
            // Arrange.
            var exportKeyName = "ExportKey";
            var secretOneName = "SecretOne";
            var secretTwoName = "SecretTwo";
            var secretNames = new String[] { exportKeyName, secretOneName, secretTwoName };
            var secretOneValue = "foo";
            var secretTwoValue = 123.456d;
            using var exportKey = SymmetricKey.New();
            using var targetOne = new SecretVault();
            using var targetTwo = new SecretVault();
            targetOne.AddOrUpdate(exportKeyName, exportKey);
            targetOne.AddOrUpdate(secretOneName, secretOneValue);
            targetOne.AddOrUpdate(secretTwoName, secretTwoValue);
            targetTwo.AddOrUpdate(exportKeyName, exportKey);

            // Act.
            targetOne.ExportAsync(exportKeyName).ContinueWith(exportTask =>
            {
                targetTwo.ImportEncryptedSecretVault(exportTask.Result, exportKeyName);
            }).Wait();

            // Assert.
            targetOne.SecretCount.Should().Be(3);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetOne.SymmetricKeySecretNames.Should().Contain(exportKeyName);
            targetTwo.SecretCount.Should().Be(3);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetTwo.SymmetricKeySecretNames.Should().Contain(exportKeyName);
            targetTwo.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetTwo.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();

            // Arrange.
            secretOneValue = "bar";
            secretTwoValue = 654.321d;
            targetTwo.AddOrUpdate(secretOneName, secretOneValue);
            targetTwo.AddOrUpdate(secretTwoName, secretTwoValue);

            // Act.
            targetTwo.ExportAsync(exportKeyName).ContinueWith(exportTask =>
            {
                targetOne.ImportEncryptedSecretVault(exportTask.Result, exportKeyName);
            }).Wait();

            // Assert.
            targetTwo.SecretCount.Should().Be(3);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetOne.SecretCount.Should().Be(3);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetOne.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetOne.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();
        }

        [TestMethod]
        public void ExportAsync_ShouldBeReversible_UsingMasterKey()
        {
            // Arrange.
            var secretOneName = "SecretOne";
            var secretTwoName = "SecretTwo";
            var secretNames = new String[] { secretOneName, secretTwoName };
            var secretOneValue = "foo";
            var secretTwoValue = 123.456d;
            using var masterPassword = Password.NewStrongPassword();
            using var exportKey = SymmetricKey.New();
            using var targetOne = new SecretVault(masterPassword);
            using var targetTwo = new SecretVault(masterPassword);
            targetOne.AddOrUpdate(secretOneName, secretOneValue);
            targetOne.AddOrUpdate(secretTwoName, secretTwoValue);

            // Act.
            targetOne.ExportAsync().ContinueWith(exportTask =>
            {
                targetTwo.ImportEncryptedSecretVault(exportTask.Result);
            }).Wait();

            // Assert.
            targetOne.SymmetricKeySecretCount.Should().Be(1);
            targetOne.SecretCount.Should().Be(3);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetTwo.SymmetricKeySecretCount.Should().Be(2);
            targetTwo.SecretCount.Should().Be(4);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetTwo.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetTwo.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();

            // Arrange.
            secretOneValue = "bar";
            secretTwoValue = 654.321d;
            targetTwo.AddOrUpdate(secretOneName, secretOneValue);
            targetTwo.AddOrUpdate(secretTwoName, secretTwoValue);

            // Act.
            targetTwo.ExportAsync().ContinueWith(exportTask =>
            {
                targetOne.ImportEncryptedSecretVault(exportTask.Result);
            }).Wait();

            // Assert.
            targetOne.SymmetricKeySecretCount.Should().Be(2);
            targetTwo.SecretCount.Should().Be(4);
            targetTwo.SecretNames.Should().Contain(secretNames);
            targetTwo.SymmetricKeySecretCount.Should().Be(2);
            targetOne.SecretCount.Should().Be(4);
            targetOne.SecretNames.Should().Contain(secretNames);
            targetOne.ReadAsync(secretOneName, (String value) => { value.Should().Be(secretOneValue); }).Wait();
            targetOne.ReadAsync(secretTwoName, (Double value) => { value.Should().Be(secretTwoValue); }).Wait();
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
            targetOne.X509CertificateSecretCount.Should().Be(1);
            targetOne.X509CertificateSecretNames.Should().Contain(secretName);
            using var exportSecretTaskOne = targetOne.ExportEncryptedSecretAsync(secretName, keyName);
            exportSecretTaskOne.Wait();
            targetTwo.ImportEncryptedSecret(exportSecretTaskOne.Result, keyName);
            targetTwo.X509CertificateSecretCount.Should().Be(1);
            targetTwo.X509CertificateSecretNames.Should().Contain(secretName);
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
                target.SecretCount.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.SecretCount.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);
                target.ReadAsync(secretTwoName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretOneName, secretThree);

                // Assert.
                target.SecretCount.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (CascadingSymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.TryRemove("baz").Should().BeFalse();
                target.TryRemove(secretOneName).Should().BeTrue();

                // Assert.
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.SecretCount.Should().Be(0);
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
                    target.SecretCount.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Guid value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Guid value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.SecretCount.Should().Be(0);
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
                    target.SecretCount.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (Double value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (Double value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.SecretCount.Should().Be(0);
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
                    target.SecretCount.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (IReadOnlyPinnedMemory<Byte> value) =>
                    {
                        value.Should().BeEquivalentTo(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.SecretCount.Should().Be(0);
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
                    target.SecretCount.Should().Be(0);
                    target.SecretNames.Should().BeEmpty();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretOne);

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretOne);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretTwoName, secretTwo);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);
                    target.ReadAsync(secretTwoName, (String value) =>
                    {
                        value.Should().Be(secretTwo);
                    }).Wait();

                    // Act.
                    target.AddOrUpdate(secretOneName, secretThree);

                    // Assert.
                    target.SecretCount.Should().Be(2);
                    target.SecretNames.Should().Contain(secretOneName);
                    target.ReadAsync(secretOneName, (String value) =>
                    {
                        value.Should().Be(secretThree);
                    }).Wait();

                    // Act.
                    target.TryRemove("baz").Should().BeFalse();
                    target.TryRemove(secretOneName).Should().BeTrue();

                    // Assert.
                    target.SecretCount.Should().Be(1);
                    target.SecretNames.Should().NotContain(secretOneName);
                    target.SecretNames.Should().Contain(secretTwoName);

                    // Act.
                    target.Clear();

                    // Assert.
                    target.SecretCount.Should().Be(0);
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
                target.SecretCount.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.SecretCount.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);
                target.ReadAsync(secretTwoName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.AddOrUpdate(secretOneName, secretThree);

                // Assert.
                target.SecretCount.Should().Be(2);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (SymmetricKey value) =>
                {
                    value.Should().NotBeNull();
                }).Wait();

                // Act.
                target.TryRemove("baz").Should().BeFalse();
                target.TryRemove(secretOneName).Should().BeTrue();

                // Assert.
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.SecretCount.Should().Be(0);
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
                target.X509CertificateSecretCount.Should().Be(0);
                target.X509CertificateSecretNames.Should().BeEmpty();
                target.SecretCount.Should().Be(0);
                target.SecretNames.Should().BeEmpty();

                // Act.
                target.AddOrUpdate(secretOneName, secretOne);

                // Assert.
                target.X509CertificateSecretCount.Should().Be(1);
                target.X509CertificateSecretNames.Should().Contain(secretOneName);
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().Contain(secretOneName);
                target.ReadAsync(secretOneName, (X509Certificate2 value) =>
                {
                    value.Should().NotBeNull();
                    value.Subject.Should().Be(subjectOne);
                }).Wait();

                // Act.
                target.AddOrUpdate(secretTwoName, secretTwo);

                // Assert.
                target.X509CertificateSecretCount.Should().Be(2);
                target.X509CertificateSecretNames.Should().Contain(secretOneName);
                target.X509CertificateSecretNames.Should().Contain(secretTwoName);
                target.SecretCount.Should().Be(2);
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
                target.SecretCount.Should().Be(2);
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
                target.X509CertificateSecretCount.Should().Be(1);
                target.X509CertificateSecretNames.Should().NotContain(secretOneName);
                target.X509CertificateSecretNames.Should().Contain(secretTwoName);
                target.SecretCount.Should().Be(1);
                target.SecretNames.Should().NotContain(secretOneName);
                target.SecretNames.Should().Contain(secretTwoName);

                // Act.
                target.Clear();

                // Assert.
                target.X509CertificateSecretCount.Should().Be(0);
                target.X509CertificateSecretNames.Should().BeEmpty();
                target.SecretCount.Should().Be(0);
                target.SecretNames.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void ImportStoreCertificates_ShouldProduceDesiredResults()
        {
            // Arrange.
            using var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            var certificates = x509Store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true);
            var validCertificateCount = 0;

            foreach (var certificate in certificates)
            {
                if (certificate.Verify())
                {
                    validCertificateCount++;
                }
            }

            using (var target = new SecretVault())
            {
                // Act.
                target.ImportStoreCertificates();

                // Assert.
                target.X509CertificateSecretCount.Should().Be(validCertificateCount);
            }
        }

        [TestMethod]
        public void NewCascadingSymmetricKey_ShouldProduceDesiredResults_UsingDefaultName()
        {
            using (var target = new SecretVault())
            {
                // Act.
                var keyName = target.NewCascadingSymmetricKey();

                // Assert.
                target.SymmetricKeySecretCount.Should().Be(1);
                target.ReadAsync(keyName, (CascadingSymmetricKey key) => { key.Should().NotBeNull(); }).Wait();
            }
        }

        [TestMethod]
        public void NewCascadingSymmetricKey_ShouldProduceDesiredResults_UsingExplicitName()
        {
            // Arrange.
            var keyName = "foo";

            using (var target = new SecretVault())
            {
                // Act.
                target.NewCascadingSymmetricKey(keyName);

                // Assert.
                target.SymmetricKeySecretCount.Should().Be(1);
                target.SymmetricKeySecretNames.Should().Contain(keyName);
                target.ReadAsync(keyName, (CascadingSymmetricKey key) => { key.Should().NotBeNull(); }).Wait();
            }
        }

        [TestMethod]
        public void NewSymmetricKey_ShouldProduceDesiredResults_UsingDefaultName()
        {
            using (var target = new SecretVault())
            {
                // Act.
                var keyName = target.NewSymmetricKey();

                // Assert.
                target.SymmetricKeySecretCount.Should().Be(1);
                target.ReadAsync(keyName, (SymmetricKey key) => { key.Should().NotBeNull(); }).Wait();
            }
        }

        [TestMethod]
        public void NewSymmetricKey_ShouldProduceDesiredResults_UsingExplicitName()
        {
            // Arrange.
            var keyName = "foo";

            using (var target = new SecretVault())
            {
                // Act.
                target.NewSymmetricKey(keyName);

                // Assert.
                target.SymmetricKeySecretCount.Should().Be(1);
                target.SymmetricKeySecretNames.Should().Contain(keyName);
                target.ReadAsync(keyName, (SymmetricKey key) => { key.Should().NotBeNull(); }).Wait();
            }
        }
    }
}