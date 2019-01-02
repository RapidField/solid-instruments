// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Extensions;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = -2m;
            var targetTwo = 0m;
            var targetThree = 2m;

            // Act.
            var resultOne = targetOne.AbsoluteValue();
            var resultTwo = targetTwo.AbsoluteValue();
            var resultThree = targetThree.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2m);
            resultTwo.Should().Be(0m);
            resultThree.Should().Be(2m);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 65m;
            var lowerBoundary = 50m;
            var upperBoundary = 150m;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }
    }
}