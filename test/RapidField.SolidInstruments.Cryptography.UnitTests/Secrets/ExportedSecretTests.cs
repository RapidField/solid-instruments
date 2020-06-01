// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Secrets;
using RapidField.SolidInstruments.Cryptography.Symmetric;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Security.Cryptography.X509Certificates;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Secrets
{
    [TestClass]
    public class ExportedSecretTests
    {
        [TestMethod]
        public void ShouldBeSerializable_UsingBinaryFormat()
        {
            // Arrange.
            var format = SerializationFormat.Binary;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedJson;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedXml;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.Json;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.Xml;

            // Assert.
            ShouldBeSerializable(format);
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            ShouldBeSerializable_ForCascadingSymmetricKeySecret(format);
            ShouldBeSerializable_ForGuidSecret(format);
            ShouldBeSerializable_ForNumericSecret(format);
            ShouldBeSerializable_ForStringSecret(format);
            ShouldBeSerializable_ForSymmetricKeySecret(format);
            ShouldBeSerializable_ForX509CertificateSecret(format);
        }

        private static void ShouldBeSerializable<TValue, TSecret>(TValue value, TSecret secret, SerializationFormat format)
            where TSecret : ISecret<TValue>
        {
            var serializer = new DynamicSerializer<ExportedSecret>(format);
            var target = new ExportedSecret(secret);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.ValueType.Should().Be(value.GetType());
            deserializedResult.Name.Should().Be(secret.Name);
            deserializedResult.Should().Be(target);
        }

        private static void ShouldBeSerializable_ForCascadingSymmetricKeySecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = CascadingSymmetricKey.New();
            using var secret = CascadingSymmetricKeySecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }

        private static void ShouldBeSerializable_ForGuidSecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = Guid.NewGuid();
            using var secret = GuidSecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }

        private static void ShouldBeSerializable_ForNumericSecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = 1234.56789d;
            using var secret = NumericSecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }

        private static void ShouldBeSerializable_ForStringSecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = "bar";
            using var secret = StringSecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }

        private static void ShouldBeSerializable_ForSymmetricKeySecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = SymmetricKey.New();
            using var secret = SymmetricKeySecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }

        private static void ShouldBeSerializable_ForX509CertificateSecret(SerializationFormat format)
        {
            // Arrange.
            var name = "foo";
            var value = new X509Certificate2("TestRootOne.testcert");
            using var secret = X509CertificateSecret.FromValue(name, value);

            // Act.
            ShouldBeSerializable(value, secret, format);
        }
    }
}