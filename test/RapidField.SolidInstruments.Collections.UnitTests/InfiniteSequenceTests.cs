// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Collections.UnitTests
{
    [TestClass]
    public class InfiniteSequenceTests
    {
        [TestMethod]
        public void CalculateNext_ShouldProduceDesiredResults_ForParameterlessInvocation()
        {
            // Arrange.
            var seedTerms = new Int32[] { 0, 1, 2 };
            var target = new SimulatedInfiniteSequence(seedTerms);

            // Act.
            var result = target.CalculateNext();

            // Assert.
            result.Should().Be(3);
            target.CalculatedTermCount.Should().Be(4);
        }

        [TestMethod]
        public void CalculateNext_ShouldProduceDesiredResults_ForValidCountArgument()
        {
            // Arrange.
            var seedTerms = new Int32[] { 0, 1, 2 };
            var target = new SimulatedInfiniteSequence(seedTerms);

            // Act.
            var result = target.CalculateNext(5);

            // Assert.
            result.Should().BeEquivalentTo(3, 4, 5, 6, 7);
            target.CalculatedTermCount.Should().Be(8);
        }

        [TestMethod]
        public void CalculateNext_ShouldRaiseArgumentOutOfRangeException_ForNegativeCountArgument()
        {
            // Arrange.
            var seedTerms = new Int32[] { 0, 1, 2 };
            var target = new SimulatedInfiniteSequence(seedTerms);

            // Act.
            var action = new Action(() =>
            {
                var result = target.CalculateNext(-1);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Constructor_ShouldHydrateCalculatedTerms()
        {
            // Arrange.
            var seedTerms = new Int32[] { 0, 1, 2 };

            // Act.
            var target = new SimulatedInfiniteSequence(seedTerms);

            // Assert.
            target.CalculatedTermCount.Should().Be(3);
            target.ToArray(0, 3).Should().OnlyContain(term => seedTerms.Contains(term));
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentNullException_ForNullSeedTermsArgument()
        {
            // Arrange.
            var seedTerms = (Int32[])null;

            // Act.
            var action = new Action(() =>
            {
                var target = new SimulatedInfiniteSequence(seedTerms);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Indexer_ShouldRaiseIndexOutOfRangeException_ForNegativeIndexArgument()
        {
            // Arrange.
            var seedTerm = 5;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var action = new Action(() =>
            {
                var result = target[-1];
            });

            // Assert.
            action.Should().Throw<IndexOutOfRangeException>();
        }

        [TestMethod]
        public void Indexer_ShouldReturnValidResult_ForValidIndexArgument()
        {
            // Arrange.
            var seedTerm = 5;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var result = target[3];

            // Assert.
            result.Should().Be(8);
            target.CalculatedTermCount.Should().Be(4);
        }

        [TestMethod]
        public void Reset_ShouldProduceDesiredResults()
        {
            // Arrange.
            var seedTerm = 5;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var result = target[8];
            target.Reset();

            // Assert.
            target.CalculatedTermCount.Should().Be(1);
            target[8].Should().Be(result);
        }

        [TestMethod]
        public void ToArray_ShouldRaiseArgumentOutOfRangeException_ForNegativeCountArgument()
        {
            // Arrange.
            var seedTerm = 5;
            var startIndex = 0;
            var count = -1;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var action = new Action(() =>
            {
                var result = target.ToArray(startIndex, count);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToArray_ShouldRaiseArgumentOutOfRangeException_ForNegativeStartIndexArgument()
        {
            // Arrange.
            var seedTerm = 5;
            var startIndex = -1;
            var count = 3;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var action = new Action(() =>
            {
                var result = target.ToArray(startIndex, count);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ToArray_ShouldReturnValidResult_ForValidRangeArguments()
        {
            // Arrange.
            var seedTerm = 5;
            var startIndex = 3;
            var count = 5;
            var target = new SimulatedInfiniteSequence(seedTerm);

            // Act.
            var result = target.ToArray(startIndex, count);

            // Assert.
            result.Should().BeEquivalentTo(8, 9, 10, 11, 12);
            target.CalculatedTermCount.Should().Be(8);
        }
    }
}