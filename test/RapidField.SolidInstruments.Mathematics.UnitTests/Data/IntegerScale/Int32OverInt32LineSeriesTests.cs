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
    public class Int32OverInt32LineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.Linear;
            var target = new Int32OverInt32LineSeries(new Dictionary<Int32, Int32>()
            {
                { 7000, -3 },
                { 11000, 1 },
                { 16000, 6 },
                { 6000, -4 },
                { 10000, 0 },
                { 5000, 5 },
                { 8000, -2 }
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
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new Int32OverInt32LineSeries(new Dictionary<Int32, Int32>()
            {
                { 7000, -3 },
                { 11000, 1 },
                { 16000, 6 },
                { 6000, -4 },
                { 10000, 0 },
                { 5000, 5 },
                { 8000, -2 }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1);
        }
    }
}