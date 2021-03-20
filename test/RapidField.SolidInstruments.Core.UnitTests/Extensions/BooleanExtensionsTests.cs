// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class BooleanExtensionsTests
    {
        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = true;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(1);
            BitConverter.ToBoolean(result).Should().Be(target);
        }

        [TestMethod]
        public void ToSerializedString_ShouldReturnValidResult_ForFalse()
        {
            // Arrange.
            var target = false;

            // Act.
            var result = target.ToSerializedString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("false");
        }

        [TestMethod]
        public void ToSerializedString_ShouldReturnValidResult_ForTrue()
        {
            // Arrange.
            var target = true;

            // Act.
            var result = target.ToSerializedString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("true");
        }
    }
}