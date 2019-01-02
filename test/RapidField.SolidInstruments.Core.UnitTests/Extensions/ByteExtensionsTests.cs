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
    public class ByteExtensionsTests
    {
        [TestMethod]
        public void ReverseBitOrder_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = (Byte)0b00000000;
            var targetTwo = (Byte)0b11111111;
            var targetThree = (Byte)0b00001111;
            var targetFour = (Byte)0b11110000;
            var targetFive = (Byte)0b00000001;
            var targetSix = (Byte)0b10000000;
            var targetSeven = (Byte)0b01001001;
            var targetEight = (Byte)0b10010010;

            // Act.
            var results = new Byte[]
            {
                targetOne.ReverseBitOrder(),
                targetTwo.ReverseBitOrder(),
                targetThree.ReverseBitOrder(),
                targetFour.ReverseBitOrder(),
                targetFive.ReverseBitOrder(),
                targetSix.ReverseBitOrder(),
                targetSeven.ReverseBitOrder(),
                targetEight.ReverseBitOrder()
            };

            // Assert.
            results[0].Should().Be(0b00000000);
            results[1].Should().Be(0b11111111);
            results[2].Should().Be(0b11110000);
            results[3].Should().Be(0b00001111);
            results[4].Should().Be(0b10000000);
            results[5].Should().Be(0b00000001);
            results[6].Should().Be(0b10010010);
            results[7].Should().Be(0b01001001);
        }
    }
}