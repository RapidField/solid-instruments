// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging.UnitTests.TransportPrimitives
{
    [TestClass]
    public class MessageLockTokenTests
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
            // Arrange.
            var expirationDateTime = DateTime.UtcNow;
            var messageIdentifier = Guid.Parse("c1572900-0080-4460-a5ef-d43e3e651d7c");
            var identifier = Guid.Parse("7f18e63b-4d27-46e4-b6d5-0926169044fd");
            var target = new MessageLockToken(identifier, messageIdentifier, expirationDateTime);
            var serializer = new DynamicSerializer<MessageLockToken>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.ExpirationDateTime.Should().Be(expirationDateTime);
            deserializedResult.Identifier.Should().Be(identifier);
            deserializedResult.MessageIdentifier.Should().Be(messageIdentifier);
        }
    }
}