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
    public class SingleOverDoubleLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.Linear;
            var target = new SingleOverDoubleLineSeries(new Dictionary<Double, Single>()
            {
                { 7000d, -3f },
                { 11000d, 1f },
                { 16000d, 6f },
                { 6000d, -4f },
                { 10000d, 0f },
                { 5000d, 5f },
                { 8000d, -2f }
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
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new SingleOverDoubleLineSeries(new Dictionary<Double, Single>()
            {
                { 7000d, -3f },
                { 11000d, 1f },
                { 16000d, 6f },
                { 6000d, -4f },
                { 10000d, 0f },
                { 5000d, 5f },
                { 8000d, -2f }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1f);
        }
    }
}