// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data
{
    [TestClass]
    public class ScalarLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldRaiseArgumentException_ForUnmatchedXAxisValue_UsingInterpolationMode_None()
        {
            // Arrange.
            var xAxisValue = 3;
            var interpolationMode = InterpolationMode.None;
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
                target.GetYAxisValue(xAxisValue, interpolationMode);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetYAxisValue_ShouldRaiseArgumentOutOfRangeException_ForXAxisValueAboveSeriesRange()
        {
            // Arrange.
            var xAxisValue = 7;
            var interpolationMode = InterpolationMode.NearestDataPoint;
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
                target.GetYAxisValue(xAxisValue, interpolationMode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void GetYAxisValue_ShouldRaiseArgumentOutOfRangeException_ForXAxisValueBelowSeriesRange()
        {
            // Arrange.
            var xAxisValue = -5;
            var interpolationMode = InterpolationMode.NearestDataPoint;
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
                target.GetYAxisValue(xAxisValue, interpolationMode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnYAxisValue_ForMatchedXAxisValue()
        {
            // Arrange.
            var xAxisValue = -4;
            var interpolationMode = InterpolationMode.None;
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
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(-4m);
        }
    }
}