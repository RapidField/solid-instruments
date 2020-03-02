// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Messaging.UnitTests
{
    [TestClass]
    public class MessagingEntityPathTests
    {
        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = MessagingEntityPath.Parse("Foo-SimulatedMessage_LabelOne");
            var targetTwo = MessagingEntityPath.Parse("Foo-SimulatedMessage_LabelOne");
            var targetThree = MessagingEntityPath.Parse("Foo-SimulatedMessage_LabelOne_LabelTwo");
            var targetFour = MessagingEntityPath.Parse("Zip-SimulatedMessage_LabelOne");

            // Act.
            var resultOne = targetOne.CompareTo(targetTwo) == 0;
            var resultTwo = targetTwo.CompareTo(targetThree) == -1;
            var resultThree = targetTwo < targetOne;
            var resultFour = targetThree > targetOne;
            var resultFive = targetFour <= targetThree;
            var resultSix = targetTwo >= targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
        }

        [TestMethod]
        public void Parse_ShouldProduceDesiredResults_ForValidPathStrings()
        {
            // Arrange.
            var validPathStrings = new String[]
            {
                "0",
                "0-1",
                "0-1_a",
                "0-1_a_b",
                "0-1_a_b_c",
                "MessageType",
                "Prefix-MessageType",
                "0Prefix1-0MessageType1",
                "Prefix-MessageType_LabelOne",
                "0Prefix1-0MessageType1_0LabelOne1",
                "Prefix-MessageType_LabelOne_LabelTwo",
                "0Prefix1-0MessageType1_0LabelOne1_0LabelTwo1",
                "Prefix-MessageType_LabelOne_LabelTwo_LabelThree",
                "0Prefix1-0MessageType1_0LabelOne1_0LabelTwo1_0LabelThree1",
            };

            // Act.
            var target = validPathStrings.Select(value => MessagingEntityPath.Parse(value)).ToArray();

            for (var i = 0; i < validPathStrings.Length; i++)
            {
                // Assert.
                validPathStrings[i].Should().Be(target[i].ToString());
            }
        }

        [TestMethod]
        public void Parse_ShouldRaiseFormatException_ForInvalidPathStrings()
        {
            // Arrange.
            var target = (MessagingEntityPath)null;
            var invalidVersionStrings = new String[]
            {
                "0-",
                "-0-1",
                "_0-1_a",
                "0-1_a_b_",
                "0_-1_a_b_c",
                "Message__Type",
                "Prefix_-MessageType",
                "0Prefix1-_0MessageType1",
                "Prefix-MessageType_LabelOne$",
                "0+Prefix1-0MessageType1_0LabelOne1",
                "Prefix+MessageType_LabelOne_LabelTwo",
                "0Prefix111111111111111111111111111111111111111111111111111111111111111111111111-0MessageType1_0LabelOne1_0LabelTwo1",
                "Prefix-MessageType_LabelOne_LabelTwo_LabelThree111111111111111111111111111111111111111111111111111111111111111111111111",
                "0Prefix1-0MessageType1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111_0LabelOne1_0LabelTwo1_0LabelThree1",
            };

            for (var i = 0; i < invalidVersionStrings.Length; i++)
            {
                // Act.
                var action = new Action(() =>
                {
                    target = MessagingEntityPath.Parse(invalidVersionStrings[i]);
                });

                // Assert.
                action.Should().Throw<FormatException>();
            }
        }

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
            var messageType = typeof(SimulatedMessage);
            var prefix = "Foo";
            var labelOne = "LabelOne";
            var labelTwo = "LabelTwo";
            var labelThree = "LabelThree";
            var target = new MessagingEntityPath(messageType, prefix, labelOne, labelTwo, labelThree);
            var serializer = new DynamicSerializer<MessagingEntityPath>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().Be(target);
        }
    }
}