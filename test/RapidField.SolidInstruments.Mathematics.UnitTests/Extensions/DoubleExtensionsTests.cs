// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Extensions;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public class DoubleExtensionsTests
    {
        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = -2d;
            var targetTwo = 0d;
            var targetThree = 2d;

            // Act.
            var resultOne = targetOne.AbsoluteValue();
            var resultTwo = targetTwo.AbsoluteValue();
            var resultThree = targetThree.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2d);
            resultTwo.Should().Be(0d);
            resultThree.Should().Be(2d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 65d;
            var lowerBoundary = 50d;
            var upperBoundary = 150d;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }
    }
}