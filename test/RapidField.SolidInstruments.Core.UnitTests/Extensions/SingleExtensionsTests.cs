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
    public class SingleExtensionsTests
    {
        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForIntegralAndFractionalNumber()
        {
            // Arrange.
            var target = 120.0345f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(7);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne()
        {
            // Arrange.
            var target = 1f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneOneThousandth()
        {
            // Arrange.
            var target = 0.001f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThird()
        {
            // Arrange.
            var target = 1f / 3f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(8);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThousand()
        {
            // Arrange.
            var target = 1000f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero()
        {
            // Arrange.
            var target = 0f;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForAwayFromZeroRounding()
        {
            // Arrange.
            var target = 1.25f;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.AwayFromZero;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.3f);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForEvenRounding()
        {
            // Arrange.
            var target = 1.25f;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.ToEven;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.2f);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 3f;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(4);
        }
    }
}