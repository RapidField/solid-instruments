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
    public class SingleOverTimeSpanLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new SingleOverTimeSpanLineSeries(new Dictionary<TimeSpan, Single>()
            {
                { new TimeSpan(7000), -3f },
                { new TimeSpan(11000), 1f },
                { new TimeSpan(16000), 6f },
                { new TimeSpan(6000), -4f },
                { new TimeSpan(10000), 0f },
                { new TimeSpan(5000), 5f },
                { new TimeSpan(8000), -2f }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(3f);
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_NearestDataPoint()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new SingleOverTimeSpanLineSeries(new Dictionary<TimeSpan, Single>()
            {
                { new TimeSpan(7000), -3f },
                { new TimeSpan(11000), 1f },
                { new TimeSpan(16000), 6f },
                { new TimeSpan(6000), -4f },
                { new TimeSpan(10000), 0f },
                { new TimeSpan(5000), 5f },
                { new TimeSpan(8000), -2f }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1f);
        }
    }
}