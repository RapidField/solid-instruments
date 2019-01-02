// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Data.RationalScale;
using System;
using System.Collections.Generic;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Data.RationalScale
{
    [TestClass]
    public class DoubleOverDoubleLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.Linear;
            var target = new DoubleOverDoubleLineSeries(new Dictionary<Double, Double>()
            {
                { 7000d, -3d },
                { 11000d, 1d },
                { 16000d, 6d },
                { 6000d, -4d },
                { 10000d, 0d },
                { 5000d, 5d },
                { 8000d, -2d }
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
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DoubleOverDoubleLineSeries(new Dictionary<Double, Double>()
            {
                { 7000d, -3d },
                { 11000d, 1d },
                { 16000d, 6d },
                { 6000d, -4d },
                { 10000d, 0d },
                { 5000d, 5d },
                { 8000d, -2d }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1d);
        }
    }
}