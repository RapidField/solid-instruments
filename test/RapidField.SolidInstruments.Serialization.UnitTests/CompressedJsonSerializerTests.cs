// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidField.SolidInstruments.Serialization.UnitTests
{
    [TestClass]
    public class CompressedJsonSerializerTests
    {
        [TestMethod]
        public void Serialize_ShouldBeReversible()
        {
            // Arrange.
            var target = new CompressedJsonSerializer<SimulatedObject>();
            var serializationTarget = SimulatedObject.Random();

            // Act.
            var serializeResult = target.Serialize(serializationTarget);
            var deserializeResult = target.Deserialize(serializeResult);

            // Assert.
            deserializeResult.Should().Be(serializationTarget);
        }
    }
}