// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.Serialization.UnitTests
{
    [TestClass]
    public class BinaryPassThroughSerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldRaiseArgumentNullException_ForNullBufferArgument()
        {
            // Arrange.
            var target = new BinaryPassThroughSerializer();
            var buffer = (Byte[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Deserialize(buffer);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Deserialize_ShouldReturnBufferArgument_ForNonNullBufferArgument()
        {
            // Arrange.
            var target = new BinaryPassThroughSerializer();
            var buffer = new Byte[] { 0x03 };

            // Act.
            var result = target.Deserialize(buffer);

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(buffer.Length);
            result[0].Should().Be(buffer[0]);
        }

        [TestMethod]
        public void Serialize_ShouldRaiseArgumentNullException_ForNullObjArgument()
        {
            // Arrange.
            var target = new BinaryPassThroughSerializer();
            var serializationTarget = (Byte[])null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Serialize(serializationTarget);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Serialize_ShouldReturnObjArgument_ForNonNullObjArgument()
        {
            // Arrange.
            var target = new BinaryPassThroughSerializer();
            var serializationTarget = new Byte[] { 0x03 };

            // Act.
            var result = target.Serialize(serializationTarget);

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(serializationTarget.Length);
            result[0].Should().Be(serializationTarget[0]);
        }
    }
}