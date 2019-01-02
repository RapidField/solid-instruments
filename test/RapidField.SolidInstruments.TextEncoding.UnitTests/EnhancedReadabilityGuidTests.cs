// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RapidField.SolidInstruments.TextEncoding.UnitTests
{
    [TestClass]
    public class EnhancedReadabilityGuidTests
    {
        [TestMethod]
        public void CompareTo_ShouldReturnValidResult()
        {
            // Arrange.
            var valueOne = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var valueTwo = Guid.Parse("5645c7ea-4729-4443-a41c-1c22dd0e61a7");
            var targetOne = new EnhancedReadabilityGuid(valueOne);
            var targetTwo = new EnhancedReadabilityGuid(valueOne);
            var targetThree = new EnhancedReadabilityGuid(valueTwo);
            var targetFour = new EnhancedReadabilityGuid(valueTwo);

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
            resultFive.Should().BeTrue();
            resultSix.Should().BeTrue();
        }

        [TestMethod]
        public void EqualityComparer_ShouldReturnValidResult()
        {
            // Arrange.
            var valueOne = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var valueTwo = Guid.Parse("5645c7ea-4729-4443-a41c-1c22dd0e61a7");
            var targetOne = new EnhancedReadabilityGuid(valueOne);
            var targetTwo = new EnhancedReadabilityGuid(valueOne);
            var targetThree = new EnhancedReadabilityGuid(valueTwo);

            // Act.
            var resultOne = targetOne.Equals(targetTwo);
            var resultTwo = targetTwo.Equals(targetThree);
            var resultThree = targetTwo == targetOne;
            var resultFour = targetThree == targetOne;
            var resultFive = targetOne != targetThree;
            var resultSix = targetTwo != targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new EnhancedReadabilityGuid(Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8")),
                new EnhancedReadabilityGuid(Guid.Parse("5645c7ea-4729-4443-a41c-1c22dd0e61a7")),
                new EnhancedReadabilityGuid(Guid.Parse("f4f3c434-bb35-4a19-8687-2bd776182e8b")),
                new EnhancedReadabilityGuid(Guid.Parse("60f4a150-da47-4523-ae7b-d5be070f7959"))
            };

            // Act.
            var results = targets.Select(target => target.GetHashCode());

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void ImplicitCastOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Assert.
            value.Should().Be(target);
        }

        [TestMethod]
        public void New_ShouldReturnUniqueIdentifiers()
        {
            // Arrange.
            var target = (EnhancedReadabilityGuid[])null;

            // Act.
            target = new EnhancedReadabilityGuid[]
            {
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New(),
                EnhancedReadabilityGuid.New()
            };

            // Assert.
            target.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Act.
            var result = EnhancedReadabilityGuid.Parse(target.ToString());

            // Assert.
            result.Should().Be(target);
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

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(16);
        }

        [TestMethod]
        public void ToGuid_ShouldReturnValidResult()
        {
            // Arrange.
            var target = EnhancedReadabilityGuid.Parse("jazikf3ckyzdw9n3azw6yfj7a8");

            // Act.
            var result = target.ToGuid();

            // Assert.
            result.Should().Be(Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8"));
        }

        [TestMethod]
        public void ToString_ShouldReturnValidResult()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().Be("jazikf3ckyzdw9n3azw6yfj7a8");
        }

        [TestMethod]
        public void TryParse_ShouldReturnValidResult_ForValidShortIdentifierString_AsLowercaseString()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Act.
            var resultTwo = EnhancedReadabilityGuid.TryParse(target.ToString().ToLower(), out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_ShouldReturnValidResult_ForValidShortIdentifierString_AsUppercaseString()
        {
            // Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var target = new EnhancedReadabilityGuid(value);

            // Act.
            var resultTwo = EnhancedReadabilityGuid.TryParse(target.ToString().ToUpper(), out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            // Arrange. Arrange.
            var value = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");
            var identifier = new EnhancedReadabilityGuid(value);
            var target = new EnhancedReadabilityGuidContainer(identifier);
            var serializer = new DynamicSerializer<EnhancedReadabilityGuidContainer>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Identifier.Should().Be(identifier);
        }

        [DataContract]
        private sealed class EnhancedReadabilityGuidContainer
        {
            public EnhancedReadabilityGuidContainer(EnhancedReadabilityGuid identifier)
            {
                Identifier = identifier;
            }

            [DataMember]
            public EnhancedReadabilityGuid Identifier
            {
                get;
                set;
            }
        }
    }
}