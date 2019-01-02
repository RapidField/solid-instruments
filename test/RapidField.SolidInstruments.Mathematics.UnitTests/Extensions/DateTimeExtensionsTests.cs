// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult()
        {
            // Arrange.
            var lowerBoundary = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var upperBoundary = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var range = new DateTimeRange(lowerBoundary, upperBoundary);
            var target = range.Midpoint;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.5d);
        }
    }
}