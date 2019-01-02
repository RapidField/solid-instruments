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
    public class DoubleOverInt32LineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.Linear;
            var target = new DoubleOverInt32LineSeries(new Dictionary<Int32, Double>()
            {
                { 7000, -3d },
                { 11000, 1d },
                { 16000, 6d },
                { 6000, -4d },
                { 10000, 0d },
                { 5000, 5d },
                { 8000, -2d }
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
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DoubleOverInt32LineSeries(new Dictionary<Int32, Double>()
            {
                { 7000, -3d },
                { 11000, 1d },
                { 16000, 6d },
                { 6000, -4d },
                { 10000, 0d },
                { 5000, 5d },
                { 8000, -2d }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1d);
        }
    }
}