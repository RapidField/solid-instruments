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
    public class DecimalOverDateTimeLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.Linear;
            var target = new DecimalOverDateTimeLineSeries(new Dictionary<DateTime, Decimal>()
            {
                { new DateTime(7000), -3m },
                { new DateTime(11000), 1m },
                { new DateTime(16000), 6m },
                { new DateTime(6000), -4m },
                { new DateTime(10000), 0m },
                { new DateTime(5000), 5m },
                { new DateTime(8000), -2m }
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
            var xAxisValue = new DateTime(13000);
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DecimalOverDateTimeLineSeries(new Dictionary<DateTime, Decimal>()
            {
                { new DateTime(7000), -3m },
                { new DateTime(11000), 1m },
                { new DateTime(16000), 6m },
                { new DateTime(6000), -4m },
                { new DateTime(10000), 0m },
                { new DateTime(5000), 5m },
                { new DateTime(8000), -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1m);
        }
    }
}