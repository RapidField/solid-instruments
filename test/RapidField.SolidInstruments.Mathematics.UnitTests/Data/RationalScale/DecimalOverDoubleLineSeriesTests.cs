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
    public class DecimalOverDoubleLineSeriesTests
    {
        [TestMethod]
        public void GetYAxisValue_ShouldReturnValidResult_UsingInterpolationMode_Linear()
        {
            // Arrange.
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.Linear;
            var target = new DecimalOverDoubleLineSeries(new Dictionary<Double, Decimal>()
            {
                { 7000d, -3m },
                { 11000d, 1m },
                { 16000d, 6m },
                { 6000d, -4m },
                { 10000d, 0m },
                { 5000d, 5m },
                { 8000d, -2m }
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
            var xAxisValue = 13000d;
            var interpolationMode = InterpolationMode.NearestDataPoint;
            var target = new DecimalOverDoubleLineSeries(new Dictionary<Double, Decimal>()
            {
                { 7000d, -3m },
                { 11000d, 1m },
                { 16000d, 6m },
                { 6000d, -4m },
                { 10000d, 0m },
                { 5000d, 5m },
                { 8000d, -2m }
            });

            // Act.
            var result = target.GetYAxisValue(xAxisValue, interpolationMode);

            // Assert.
            result.Should().Be(1m);
        }
    }
}