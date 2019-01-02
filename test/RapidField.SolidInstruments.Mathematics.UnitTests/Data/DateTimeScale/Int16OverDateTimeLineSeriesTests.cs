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
    public class Int16OverDateTimeLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new Int16OverDateTimeLineSeries(new Dictionary<DateTime, Int16>()
            {
                { new DateTime(7000), -3 },
                { new DateTime(11000), 1 },
                { new DateTime(16000), 6 },
                { new DateTime(6000), -4 },
                { new DateTime(10000), 0 },
                { new DateTime(5000), 5 },
                { new DateTime(8000), -2 }
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
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new Int16OverDateTimeLineSeries(new Dictionary<DateTime, Int16>()
            {
                { new DateTime(7000), -3 },
                { new DateTime(11000), 1 },
                { new DateTime(16000), 6 },
                { new DateTime(6000), -4 },
                { new DateTime(10000), 0 },
                { new DateTime(5000), 5 },
                { new DateTime(8000), -2 }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1);
        }
    }
}