// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Sequences;
using System;
using System.Numerics;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Sequences
{
    [TestClass]
    public class FibonacciSequenceTests
    {
        [TestMethod]
        public void Classic_ShouldRepresentClassicFibonacciSequence()
        {
            // Arrange.
            var target = FibonacciSequence.Classic;

            // Assert.
            target[0].Should().Be(0);
            target[1].Should().Be(1);
            target[2].Should().Be(1);
            target[3].Should().Be(2);
            target[4].Should().Be(3);
            target[5].Should().Be(5);
            target[6].Should().Be(8);
            target[7].Should().Be(13);
            target[8].Should().Be(21);
            target[9].Should().Be(34);
            target[10].Should().Be(55);
            target[11].Should().Be(89);
            target[12].Should().Be(144);
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentException_ForSeedsWithSumZero()
        {
            // Arrange.
            var firstTerm = new BigInteger(-3);
            var secondTerm = new BigInteger(3);

            // Act.
            var action = new Action(() =>
            {
                var target = new FibonacciSequence(firstTerm, secondTerm);
            });

            // Assert.
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForInvalidSeedCombination()
        {
            // Arrange.
            var firstTerm = new BigInteger(3);
            var secondTerm = new BigInteger(2);

            // Act.
            var action = new Action(() =>
            {
                var target = new FibonacciSequence(firstTerm, secondTerm);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var firstTerm = new BigInteger(3);
            var secondTerm = new BigInteger(3);

            // Act.
            var target = new FibonacciSequence(firstTerm, secondTerm);

            // Assert.
            target.CalculatedTermCount.Should().Be(2);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(secondTerm);
            target[2].Should().Be(6);
            target[3].Should().Be(9);
            target[4].Should().Be(15);
            target.CalculatedTermCount.Should().Be(5);
            target[5].Should().Be(24);
            target[6].Should().Be(39);
            target[7].Should().Be(63);
            target[8].Should().Be(102);
            target[9].Should().Be(165);
            target.CalculatedTermCount.Should().Be(10);
            target.Reset();
            target.CalculatedTermCount.Should().Be(2);
            target[0].Should().Be(firstTerm);
            target[1].Should().Be(secondTerm);
            target[2].Should().Be(6);
        }
    }
}