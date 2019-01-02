// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Sequences;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Sequences
{
    [TestClass]
    public class ArithmeticSequenceTests
    {
        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForZeroCommonDifferenceArgument()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonDifference = 0d;

            // Act.
            var action = new Action(() =>
            {
                var target = new ArithmeticSequence(firstTerm, commonDifference);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonDifference = -7d;

            // Act.
            var target = new ArithmeticSequence(firstTerm, commonDifference);

            // Assert.
            target.CalculatedTermCount.Should().Be(1);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(-4);
            target[2].Should().Be(-11);
            target[3].Should().Be(-18);
            target[4].Should().Be(-25);
            target.CalculatedTermCount.Should().Be(5);
            target[5].Should().Be(-32);
            target[6].Should().Be(-39);
            target[7].Should().Be(-46);
            target[8].Should().Be(-53);
            target[9].Should().Be(-60);
            target.CalculatedTermCount.Should().Be(10);
            target.Reset();
            target.CalculatedTermCount.Should().Be(1);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(-4);
            target[2].Should().Be(-11);
        }
    }
}