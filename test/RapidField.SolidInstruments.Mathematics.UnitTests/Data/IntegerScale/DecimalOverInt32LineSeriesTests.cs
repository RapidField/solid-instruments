// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Data.IntegerScale;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data.IntegerScale
{
    [TestClass]
    public class DecimalOverInt32LineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.Linear;
            var target = new DecimalOverInt32LineSeries(new Dictionary<Int32, Decimal>()
            {
                { 7000, -3m },
                { 11000, 1m },
                { 16000, 6m },
                { 6000, -4m },
                { 10000, 0m },
                { 5000, 5m },
                { 8000, -2m }
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
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DecimalOverInt32LineSeries(new Dictionary<Int32, Decimal>()
            {
                { 7000, -3m },
                { 11000, 1m },
                { 16000, 6m },
                { 6000, -4m },
                { 10000, 0m },
                { 5000, 5m },
                { 8000, -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1m);
        }
    }
}