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
    public class DoubleExtensionsTests
    {
        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForIntegralAndFractionalNumber()
        {
            // Arrange.
            var target = 120.0345d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(7);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne()
        {
            // Arrange.
            var target = 1d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneOneThousandth()
        {
            // Arrange.
            var target = 0.001d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThird()
        {
            // Arrange.
            var target = 1d / 3d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(16);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneThousand()
        {
            // Arrange.
            var target = 1000d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero()
        {
            // Arrange.
            var target = 0d;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForIntegralAndFractionalNumber()
        {
            // Arrange.
            var target = 120.0345d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(1200345d);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForOne()
        {
            // Arrange.
            var target = 1d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForOneOneThousandth()
        {
            // Arrange.
            var target = 0.001d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForOneThird()
        {
            // Arrange.
            var target = 1d / 3d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(3333333333333333d);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForOneThousand()
        {
            // Arrange.
            var target = 1000d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(1d);
        }

        [TestMethod]
        public void GetSignificand_ShouldReturnValidResult_ForZero()
        {
            // Arrange.
            var target = 0d;

            // Act.
            var result = target.GetSignificand();

            // Assert.
            result.Should().Be(0d);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForAwayFromZeroRounding()
        {
            // Arrange.
            var target = 1.25d;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.AwayFromZero;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.3d);
        }

        [TestMethod]
        public void RoundedTo_ShouldReturnValidResult_ForEvenRounding()
        {
            // Arrange.
            var target = 1.25d;
            var digits = 1;
            var midpointRoundingMode = MidpointRounding.ToEven;

            // Act.
            var result = target.RoundedTo(digits, midpointRoundingMode);

            // Assert.
            result.Should().Be(1.2d);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 3d;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
            BitConverter.ToDouble(result).Should().Be(target);
        }
    }
}