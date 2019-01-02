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
    public class Int32OverDoubleLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.Linear;
            var target = new Int32OverDoubleLineSeries(new Dictionary<Double, Int32>()
            {
                { 7000d, -3 },
                { 11000d, 1 },
                { 16000d, 6 },
                { 6000d, -4 },
                { 10000d, 0 },
                { 5000d, 5 },
                { 8000d, -2 }
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
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new Int32OverDoubleLineSeries(new Dictionary<Double, Int32>()
            {
                { 7000d, -3 },
                { 11000d, 1 },
                { 16000d, 6 },
                { 6000d, -4 },
                { 10000d, 0 },
                { 5000d, 5 },
                { 8000d, -2 }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1);
        }
    }
}