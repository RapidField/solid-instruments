// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Data.DateTimeScale;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data.DateTimeScale
{
    [TestClass]
    public class DoubleOverDateTimeLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new DoubleOverDateTimeLineSeries(new Dictionary<DateTime, Double>()
            {
                { new DateTime(7000), -3d },
                { new DateTime(11000), 1d },
                { new DateTime(16000), 6d },
                { new DateTime(6000), -4d },
                { new DateTime(10000), 0d },
                { new DateTime(5000), 5d },
                { new DateTime(8000), -2d }
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
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DoubleOverDateTimeLineSeries(new Dictionary<DateTime, Double>()
            {
                { new DateTime(7000), -3d },
                { new DateTime(11000), 1d },
                { new DateTime(16000), 6d },
                { new DateTime(6000), -4d },
                { new DateTime(10000), 0d },
                { new DateTime(5000), 5d },
                { new DateTime(8000), -2d }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1d);
        }
    }
}