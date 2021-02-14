// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForIntegralAndFractionalNumber()
        {
            // Arrange.
            var target = 120.0345m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(7);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne()
        {
            // Arrange.
            var target = 1m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneOneThousandth()
        {
            // Arrange.
            var target = 0.001m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThird()
        {
            // Arrange.
            var target = 1m / 3m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(28);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThousand()
        {
            // Arrange.
            var target = 1000m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero()
        {
            // Arrange.
            var target = 0m;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForAwayFromZeroRounding()
        {
            // Arrange.
            var target = 1.25m;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.AwayFromZero;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.3m);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForEvenRounding()
        {
            // Arrange.
            var target = 1.25m;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.ToEven;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.2m);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 3m;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(16);
        }
    }
}