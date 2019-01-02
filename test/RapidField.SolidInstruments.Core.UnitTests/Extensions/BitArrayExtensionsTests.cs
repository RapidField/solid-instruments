// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class BitArrayExtensionsTests
    {
        [TestMethod]
        public void PerformCircularShift_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new BitArray(Array.Empty<Boolean>());
            var targetTwo = new BitArray(new Boolean[1] { true });
            var targetThree = new BitArray(new Boolean[5] { false, true, true, false, true });
            var targetFour = new BitArray(new Boolean[11] { true, true, true, false, true, true, true, true, true, true, true });
            var targetFive = new BitArray(new Boolean[17] { true, true, false, true, true, false, true, true, false, true, true, false, true, true, true, true, true });

            // Act.
            var resultOne = targetOne.PerformCircularShift(BitShiftDirection.Left, 3).ToBinaryString();
            var resultTwo = targetTwo.PerformCircularShift(BitShiftDirection.Right, 3).ToBinaryString();
            var resultThree = targetThree.PerformCircularShift(BitShiftDirection.Left, 3).ToBinaryString();
            var resultFour = targetFour.PerformCircularShift(BitShiftDirection.Right, 25).ToBinaryString();
            var resultFive = targetFive.PerformCircularShift(BitShiftDirection.Left, 10).ToBinaryString();

            // Assert.
            resultOne.Should().Be(String.Empty);
            resultTwo.Should().Be("1");
            resultThree.Should().Be("01011");
            resultFour.Should().Be("11111101111");
            resultFive.Should().Be("10111111101101101");
        }

        [TestMethod]
        public void ReverseOrder_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new BitArray(Array.Empty<Boolean>());
            var targetTwo = new BitArray(new Boolean[1] { true });
            var targetThree = new BitArray(new Boolean[2] { true, false });
            var targetFour = new BitArray(new Boolean[4] { true, true, true, false });
            var targetFive = new BitArray(new Boolean[5] { false, true, true, false, true });

            // Act.
            var resultOne = targetOne.ReverseOrder().ToBinaryString();
            var resultTwo = targetTwo.ReverseOrder().ToBinaryString();
            var resultThree = targetThree.ReverseOrder().ToBinaryString();
            var resultFour = targetFour.ReverseOrder().ToBinaryString();
            var resultFive = targetFive.ReverseOrder().ToBinaryString();

            // Assert.
            resultOne.Should().Be(String.Empty);
            resultTwo.Should().Be("1");
            resultThree.Should().Be("01");
            resultFour.Should().Be("0111");
            resultFive.Should().Be("10110");
        }

        [TestMethod]
        public void ToBinaryString_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new BitArray(Array.Empty<Boolean>());
            var targetTwo = new BitArray(new Boolean[1] { true });
            var targetThree = new BitArray(new Boolean[5] { false, true, true, false, true });

            // Act
            var resultOne = targetOne.ToBinaryString();
            var resultTwo = targetTwo.ToBinaryString();
            var resultThree = targetThree.ToBinaryString();

            // Assert.
            resultOne.Should().Be(String.Empty);
            resultTwo.Should().Be("1");
            resultThree.Should().Be("01101");
        }
    }
}