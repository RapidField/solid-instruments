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