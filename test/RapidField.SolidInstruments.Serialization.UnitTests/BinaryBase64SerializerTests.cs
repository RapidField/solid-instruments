// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.Serialization.UnitTests
{
    [TestClass]
    public class BinaryBase64SerializerTests
    {
        [TestMethod]
        public void Deserialize_ShouldRaiseArgumentNullException_ForNullBufferArgument()
        {
            // Arrange.
            var target = new BinaryBase64Serializer();
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
        public void Serialize_ShouldBeReversible()
        {
            // Arrange.
            var target = new BinaryBase64Serializer();
            var serializationTarget = "YW55IGNhcm5hbCBwbGVhc3VyZS4=";

            // Act.
            var serializeResult = target.Serialize(serializationTarget);
            var deserializeResult = target.Deserialize(serializeResult);

            // Assert.
            deserializeResult.Should().BeEquivalentTo(serializationTarget);
        }

        [TestMethod]
        public void Serialize_ShouldRaiseArgumentNullException_ForNullObjArgument()
        {
            // Arrange.
            var target = new BinaryBase64Serializer();
            var serializationTarget = (String)null;

            // Act.
            var action = new Action(() =>
            {
                var result = target.Serialize(serializationTarget);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Serialize_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new BinaryBase64Serializer();
            var serializationTarget = "YW55IGNhcm5hbCBwbGVhc3VyZS4=";

            // Act.
            var result = target.Serialize(serializationTarget);

            // Assert.
            result.Should().BeEquivalentTo(Convert.FromBase64String(serializationTarget));
        }
    }
}