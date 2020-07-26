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
    public class PrimitiveMessageTests
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
            var correlationIdentifier = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var testObject = SimulatedObject.Random();
            var simulatedMessage = new SimulatedMessage(correlationIdentifier, testObject);
            var expirationDateTime = DateTime.UtcNow;
            var lockTokenIdentifier = Guid.Parse("037d702f-9b07-4686-a09c-29073ef29a85");
            var lockToken = new MessageLockToken(lockTokenIdentifier, simulatedMessage.Identifier, expirationDateTime);
            var lockKey = Guid.Parse("c7692faa-6cdb-402a-ae65-2f2831e599b8");
            var primitiveMessage = new PrimitiveMessage(simulatedMessage, lockToken);
            var serializer = new DynamicSerializer<PrimitiveMessage>(format);

            // Act.
            var serializedTarget = serializer.Serialize(primitiveMessage);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.CorrelationIdentifier.Should().Be(correlationIdentifier);
            deserializedResult.GetBody<SimulatedMessage>().TestObject.Should().Be(testObject);
            deserializedResult.LockToken.Should().NotBeNull();
            deserializedResult.LockToken.ExpirationDateTime.Should().Be(expirationDateTime);
            deserializedResult.LockToken.MessageIdentifier.Should().Be(simulatedMessage.Identifier);
            deserializedResult.LockToken.Identifier.Should().Be(lockTokenIdentifier);
        }
    }
}