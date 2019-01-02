// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    [TestClass]
    public class RequestMessageTests
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
            var correlationIdentifier = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var dateRange = new DateTimeRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            var target = new SimulatedRequestMessage(correlationIdentifier, dateRange);
            var identifier = target.Identifier;
            var resultType = target.ResultType;
            var serializer = new DynamicSerializer<SimulatedRequestMessage>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.CorrelationIdentifier.Should().Be(correlationIdentifier);
            deserializedResult.DateRange.Should().Be(dateRange);
            deserializedResult.Identifier.Should().Be(identifier);
            deserializedResult.ResultType.Should().Be(resultType);
        }
    }
}