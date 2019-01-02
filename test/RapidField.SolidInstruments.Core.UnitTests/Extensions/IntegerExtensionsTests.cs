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
    public class IntegerExtensionsTests
    {
        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt32Target()
        {
            // Arrange.
            var target = 3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(4);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(4);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }
    }
}