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
    public class Int64OverTimeSpanLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new Int64OverTimeSpanLineSeries(new Dictionary<TimeSpan, Int64>()
            {
                { new TimeSpan(7000), -3 },
                { new TimeSpan(11000), 1 },
                { new TimeSpan(16000), 6 },
                { new TimeSpan(6000), -4 },
                { new TimeSpan(10000), 0 },
                { new TimeSpan(5000), 5 },
                { new TimeSpan(8000), -2 }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(3);
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_NearestDataPoint()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new Int64OverTimeSpanLineSeries(new Dictionary<TimeSpan, Int64>()
            {
                { new TimeSpan(7000), -3 },
                { new TimeSpan(11000), 1 },
                { new TimeSpan(16000), 6 },
                { new TimeSpan(6000), -4 },
                { new TimeSpan(10000), 0 },
                { new TimeSpan(5000), 5 },
                { new TimeSpan(8000), -2 }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1);
        }
    }
}