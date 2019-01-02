// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public class TimeSpanExtensionsTests
    {
        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult()
        {
            // Arrange.
            var target = TimeSpan.FromTicks(13503);
            var lowerBoundary = TimeSpan.FromTicks(12002);
            var upperBoundary = TimeSpan.FromTicks(15004);

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.5d);
        }
    }
}