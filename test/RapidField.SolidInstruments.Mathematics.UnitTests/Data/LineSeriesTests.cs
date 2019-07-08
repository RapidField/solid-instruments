// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data
{
    [TestClass]
    public class LineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldRaiseArgumentException_ForUnmatchedXAxisValue()
        {
            // Arrange.
            var xAxisValue = 3;
            var target = new SimulatedScalarLineSeries(new Dictionary<Int32, Decimal>()
            {
                { -3, -3m },
                { 1, 1m },
                { 6, 6m },
                { -4, -4m },
                { 0, 0m },
                { 5, 5m },
                { -2, -2m }
            });

            // Act.
            var action = new Action(() =>
            {
                target.GetYAxisValue(xAxisValue);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnYAxisValue_ForMatchedXAxisValue()
        {
            // Arrange.
            var xAxisValue = -4;
            var target = new SimulatedScalarLineSeries(new Dictionary<Int32, Decimal>()
            {
                { -3, -3m },
                { 1, 1m },
                { 6, 6m },
                { -4, -4m },
                { 0, 0m },
                { 5, 5m },
                { -2, -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue);

            // Assert.
            result.Should().Be(-4m);
        }

        [TestMethod]
        public void ShouldBeOrderedSequentially()
        {
            // Arrange.
            var target = new SimulatedScalarLineSeries(new Dictionary<Int32, Decimal>()
            {
                { -3, -3m },
                { 1, 1m },
                { 6, 6m },
                { -4, -4m },
                { 0, 0m },
                { 5, 5m },
                { -2, -2m }
            });

            // Assert.
            target.ElementAt(0).YValue.Should().Be(-4m);
            target.ElementAt(1).YValue.Should().Be(-3m);
            target.ElementAt(2).YValue.Should().Be(-2m);
            target.ElementAt(3).YValue.Should().Be(0m);
            target.ElementAt(4).YValue.Should().Be(1m);
            target.ElementAt(5).YValue.Should().Be(5m);
            target.ElementAt(6).YValue.Should().Be(6m);
        }

        [TestMethod]
        public void TryGetYAxisValue_ShouldNotReturnYAxisValue_ForUnmatchedXAxisValue()
        {
            // Arrange.
            var xAxisValue = 3;
            var target = new SimulatedScalarLineSeries(new Dictionary<Int32, Decimal>()
            {
                { -3, -3m },
                { 1, 1m },
                { 6, 6m },
                { -4, -4m },
                { 0, 0m },
                { 5, 5m },
                { -2, -2m }
            });

            // Act.
            var result = target.TryGetYAxisValue(xAxisValue, out var yAxisValue);

            // Assert.
            result.Should().BeFalse();
            yAxisValue.Should().Be(default);
        }

        [TestMethod]
        public void TryGetYAxisValue_ShouldReturnYAxisValue_ForMatchedXAxisValue()
        {
            // Arrange.
            var xAxisValue = -4;
            var target = new SimulatedScalarLineSeries(new Dictionary<Int32, Decimal>()
            {
                { -3, -3m },
                { 1, 1m },
                { 6, 6m },
                { -4, -4m },
                { 0, 0m },
                { 5, 5m },
                { -2, -2m }
            });

            // Act.
            var result = target.TryGetYAxisValue(xAxisValue, out var yAxisValue);

            // Assert.
            result.Should().BeTrue();
            yAxisValue.Should().Be(-4m);
        }
    }
}