// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.EventAuthoring.UnitTests
{
    [TestClass]
    public class GeneralInformationEventTests
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
            var labels = new String[] { "foo", "bar" };
            var verbosity = EventVerbosity.Minimal;
            var description = "description";
            var timeStamp = TimeStamp.Current;
            var target = new GeneralInformationEvent(labels, verbosity, description, timeStamp);
            var serializer = new DynamicSerializer<GeneralInformationEvent>(format);
            target.Metadata.Add("bar", "baz");
            target.Metadata.Add("fizz", "buzz");

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.Labels.Should().NotBeNull();
            deserializedResult.Labels.Count.Should().Be(labels.Length);
            deserializedResult.Labels.Should().BeEquivalentTo(labels);
            deserializedResult.Metadata.Should().NotBeNull();
            deserializedResult.Metadata.Count.Should().Be(target.Metadata.Count);
            deserializedResult.Metadata.Should().BeEquivalentTo(target.Metadata);
            deserializedResult.Category.Should().Be(target.Category);
            deserializedResult.Description.Should().Be(target.Description);
            deserializedResult.TimeStamp.Should().Be(target.TimeStamp);
            deserializedResult.Verbosity.Should().Be(target.Verbosity);
        }
    }
}