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
    public class SByteExtensionsTests
    {
        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine()
        {
            // Arrange.
            var target = (SByte)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne()
        {
            // Arrange.
            var target = (SByte)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred()
        {
            // Arrange.
            var target = (SByte)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero()
        {
            // Arrange.
            var target = (SByte)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }
    }
}