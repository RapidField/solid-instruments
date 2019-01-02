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
    public class SingleOverDateTimeLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new SingleOverDateTimeLineSeries(new Dictionary<DateTime, Single>()
            {
                { new DateTime(7000), -3f },
                { new DateTime(11000), 1f },
                { new DateTime(16000), 6f },
                { new DateTime(6000), -4f },
                { new DateTime(10000), 0f },
                { new DateTime(5000), 5f },
                { new DateTime(8000), -2f }
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
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new SingleOverDateTimeLineSeries(new Dictionary<DateTime, Single>()
            {
                { new DateTime(7000), -3f },
                { new DateTime(11000), 1f },
                { new DateTime(16000), 6f },
                { new DateTime(6000), -4f },
                { new DateTime(10000), 0f },
                { new DateTime(5000), 5f },
                { new DateTime(8000), -2f }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1f);
        }
    }
}