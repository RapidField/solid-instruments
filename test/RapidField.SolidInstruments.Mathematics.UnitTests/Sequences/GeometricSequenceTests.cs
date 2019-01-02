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
    public class GeometricSequenceTests
    {
        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForNegativeCommonRatioArgument()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonRatio = -1d;

            // Act.
            var action = new Action(() =>
            {
                var target = new GeometricSequence(firstTerm, commonRatio);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForOneCommonRatioArgument()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonRatio = 1d;

            // Act.
            var action = new Action(() =>
            {
                var target = new GeometricSequence(firstTerm, commonRatio);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForZeroCommonRatioArgument()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonRatio = 0d;

            // Act.
            var action = new Action(() =>
            {
                var target = new GeometricSequence(firstTerm, commonRatio);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var firstTerm = 3d;
            var commonRatio = 2d;

            // Act.
            var target = new GeometricSequence(firstTerm, commonRatio);

            // Assert.
            target.CalculatedTermCount.Should().Be(1);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(6);
            target[2].Should().Be(12);
            target[3].Should().Be(24);
            target[4].Should().Be(48);
            target.CalculatedTermCount.Should().Be(5);
            target[5].Should().Be(96);
            target[6].Should().Be(192);
            target[7].Should().Be(384);
            target[8].Should().Be(768);
            target[9].Should().Be(1536);
            target.CalculatedTermCount.Should().Be(10);
            target.Reset();
            target.CalculatedTermCount.Should().Be(1);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(6);
            target[2].Should().Be(12);
        }
    }
}