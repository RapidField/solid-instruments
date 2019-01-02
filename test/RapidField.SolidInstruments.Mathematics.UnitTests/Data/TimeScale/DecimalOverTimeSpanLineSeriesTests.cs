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
    public class DecimalOverTimeSpanLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new DecimalOverTimeSpanLineSeries(new Dictionary<TimeSpan, Decimal>()
            {
                { new TimeSpan(7000), -3m },
                { new TimeSpan(11000), 1m },
                { new TimeSpan(16000), 6m },
                { new TimeSpan(6000), -4m },
                { new TimeSpan(10000), 0m },
                { new TimeSpan(5000), 5m },
                { new TimeSpan(8000), -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(3m);
        }

        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_NearestDataPoint()
        {
            // Arrange.
            var xAxisValue = new TimeSpan(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DecimalOverTimeSpanLineSeries(new Dictionary<TimeSpan, Decimal>()
            {
                { new TimeSpan(7000), -3m },
                { new TimeSpan(11000), 1m },
                { new TimeSpan(16000), 6m },
                { new TimeSpan(6000), -4m },
                { new TimeSpan(10000), 0m },
                { new TimeSpan(5000), 5m },
                { new TimeSpan(8000), -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1m);
        }
    }
}