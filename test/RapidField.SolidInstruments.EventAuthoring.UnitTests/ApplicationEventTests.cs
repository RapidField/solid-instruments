// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;

namespace RapidField.SolidInstruments.EventAuthoring.UnitTests
{
    [TestClass]
    public class ApplicationEventTests
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

        [TestMethod]
        public void Summary_ShouldContainConstituents()
        {
            // Arrange.
            var category = ApplicationEventCategory.Information;
            var verbosity = ApplicationEventVerbosity.Normal;
            var description = "description";
            var environmentName = "environmentName";
            var userInformation = "userInformation";
            var target = new ApplicationEvent(category, verbosity, description, environmentName, userInformation);

            // Act.
            var result = target.Summary;

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(description);
            result.Should().Contain(environmentName);
            result.Should().Contain(userInformation);
        }

        [TestMethod]
        public void ToString_ShouldReturnSummary()
        {
            // Arrange.
            var category = ApplicationEventCategory.Information;
            var verbosity = ApplicationEventVerbosity.Normal;
            var description = "description";
            var environmentName = "environmentName";
            var userInformation = "userInformation";
            var target = new ApplicationEvent(category, verbosity, description, environmentName, userInformation);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(target.Summary);
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            // Arrange.
            var category = ApplicationEventCategory.Information;
            var verbosity = ApplicationEventVerbosity.Normal;
            var description = "description";
            var environmentName = "environmentName";
            var userInformation = "userInformation";
            var target = new ApplicationEvent(category, verbosity, description, environmentName, userInformation);
            var serializer = new DynamicSerializer<ApplicationEvent>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().NotBeNull();
            deserializedResult.Category.Should().Be(target.Category);
            deserializedResult.Description.Should().Be(target.Description);
            deserializedResult.EnvironmentName.Should().Be(target.EnvironmentName);
            deserializedResult.OccurrenceDateTime.Should().Be(target.OccurrenceDateTime);
            deserializedResult.Summary.Should().Be(target.Summary);
            deserializedResult.UserInformation.Should().Be(target.UserInformation);
            deserializedResult.Verbosity.Should().Be(target.Verbosity);
        }
    }
}