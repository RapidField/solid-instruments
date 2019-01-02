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
    public class SingleOverInt32LineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.Linear;
            var target = new SingleOverInt32LineSeries(new Dictionary<Int32, Single>()
            {
                { 7000, -3f },
                { 11000, 1f },
                { 16000, 6f },
                { 6000, -4f },
                { 10000, 0f },
                { 5000, 5f },
                { 8000, -2f }
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
            var xAxisValue = 13000;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new SingleOverInt32LineSeries(new Dictionary<Int32, Single>()
            {
                { 7000, -3f },
                { 11000, 1f },
                { 16000, 6f },
                { 6000, -4f },
                { 10000, 0f },
                { 5000, 5f },
                { 8000, -2f }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1f);
        }
    }
}