// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    [TestClass]
    public class ResponseMessageTests
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
            // Arrange. Arrange.
            var requestMessageIdentifier = Guid.Parse("fab2812e-88f4-4a03-8723-6d285a98b149");
            var correlationIdentifier = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var result = TimeSpan.FromMilliseconds(99999);
            var target = new SimulatedResponseMessage(requestMessageIdentifier, correlationIdentifier, result);
            var identifier = target.Identifier;
            var resultType = target.ResultType;
            var serializer = new DynamicSerializer<SimulatedResponseMessage>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.CorrelationIdentifier.Should().Be(correlationIdentifier);
            deserializedResult.Identifier.Should().Be(identifier);
            deserializedResult.RequestMessageIdentifier.Should().Be(requestMessageIdentifier);
            deserializedResult.Result.Should().Be(result);
            deserializedResult.ResultType.Should().Be(resultType);
        }
    }
}