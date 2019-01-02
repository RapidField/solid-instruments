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
    public class TimeSpanExtensionsTests
    {
        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = TimeSpan.FromDays(3d);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }

        [TestMethod]
        public void ToSerializedString_ShouldReturnValidResult()
        {
            // Arrange.
            var timeSpan = new TimeSpan(271, 18, 47, 21, 894);

            // Act.
            var result = timeSpan.ToSerializedString();

            // Assert.
            result.Should().Be("271:18:47:21.8940000");
        }
    }
}