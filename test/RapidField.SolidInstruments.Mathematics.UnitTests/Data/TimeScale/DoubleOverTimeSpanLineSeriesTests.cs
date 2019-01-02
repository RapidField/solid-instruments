// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Data.TimeScale;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data.TimeScale
{
    [TestClass]
    public class DoubleOverTimeSpanLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new DoubleOverTimeSpanLineSeries(new Dictionary<TimeSpan, Double>()
            {
                { new TimeSpan(7000), -3d },
                { new TimeSpan(11000), 1d },
                { new TimeSpan(16000), 6d },
                { new TimeSpan(6000), -4d },
                { new TimeSpan(10000), 0d },
                { new TimeSpan(5000), 5d },
                { new TimeSpan(8000), -2d }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(3d);
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_NearestDataPoint()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DoubleOverTimeSpanLineSeries(new Dictionary<TimeSpan, Double>()
            {
                { new TimeSpan(7000), -3d },
                { new TimeSpan(11000), 1d },
                { new TimeSpan(16000), 6d },
                { new TimeSpan(6000), -4d },
                { new TimeSpan(10000), 0d },
                { new TimeSpan(5000), 5d },
                { new TimeSpan(8000), -2d }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1d);
        }
    }
}