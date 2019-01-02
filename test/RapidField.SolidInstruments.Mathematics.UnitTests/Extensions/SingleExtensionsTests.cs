// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Extensions;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public sealed class SingleExtensionsTests
    {
        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = -2f;
            var targetTwo = 0f;
            var targetThree = 2f;

            // Act.
            var resultOne = targetOne.AbsoluteValue();
            var resultTwo = targetTwo.AbsoluteValue();
            var resultThree = targetThree.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2f);
            resultTwo.Should().Be(0f);
            resultThree.Should().Be(2f);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult()
        {
            // Arrange.
            var target = 65f;
            var lowerBoundary = 50f;
            var upperBoundary = 150f;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }
    }
}