// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Sequences;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Sequences
{
    [TestClass]
    public class PrimeNumberSequenceTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new PrimeNumberSequence();

            // Assert.
            target.CalculatedTermCount.Should().Be(2);
            target[0].Should().Be(2);
            target[1].Should().Be(3);
            target[2].Should().Be(5);
            target[3].Should().Be(7);
            target[4].Should().Be(11);
            target.CalculatedTermCount.Should().Be(5);
            target[5].Should().Be(13);
            target[6].Should().Be(17);
            target[7].Should().Be(19);
            target[8].Should().Be(23);
            target[9].Should().Be(29);
            target.CalculatedTermCount.Should().Be(10);
            target.Reset();
            target.CalculatedTermCount.Should().Be(2);
            target[0].Should().Be(2);
            target[1].Should().Be(3);
            target[2].Should().Be(5);
        }

        [TestMethod]
        public void Instance_ShouldReprepsentPrimeNumberSequence()
        {
            // Arrange.
            var target = PrimeNumberSequence.Instance;

            // Assert.
            target[0].Should().Be(2);
            target[1].Should().Be(3);
            target[2].Should().Be(5);
            target[3].Should().Be(7);
            target[4].Should().Be(11);
            target[5].Should().Be(13);
            target[6].Should().Be(17);
            target[7].Should().Be(19);
            target[8].Should().Be(23);
            target[9].Should().Be(29);
            target[10].Should().Be(31);
            target[11].Should().Be(37);
            target[12].Should().Be(41);
            target[88].Should().Be(461);
            target[232].Should().Be(1471);
            target[1596].Should().Be(13469);
        }
    }
}