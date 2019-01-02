﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.TextEncoding.UnitTests
{
    [TestClass]
    public class Base32EncodingTests
    {
        [TestMethod]
        public void GetBytes_ShouldReturnValidResult()
        {
            // Arrange.
            var encoding = Base32Encoding.Default;
            var targetOne = "aa";
            var targetTwo = "777b";
            var targetThree = "rhnyj";
            var targetFour = "7ha6paa";
            var targetFive = "aaaaaaaa";
            var targetSix = "77777777";
            var targetSeven = "3zigk4uubgvb";
            var targetEight = "xjceufky6r2c2od";
            var expectedResultOne = new Byte[] { 0x00 };
            var expectedResultTwo = new Byte[] { 0xff, 0xff };
            var expectedResultThree = new Byte[] { 0xf1, 0x34, 0x9c };
            var expectedResultFour = new Byte[] { 0xff, 0x00, 0xff, 0x00 };
            var expectedResultFive = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
            var expectedResultSix = new Byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var expectedResultSeven = new Byte[] { 0x3b, 0x23, 0xa3, 0x38, 0xa5, 0xc1, 0xd4 };
            var expectedResultEight = new Byte[] { 0x37, 0x09, 0x42, 0x8b, 0xc2, 0x3e, 0x6a, 0xa1, 0xdd };

            // Act.
            var resultOne = encoding.GetBytes(targetOne);
            var resultTwo = encoding.GetBytes(targetTwo);
            var resultThree = encoding.GetBytes(targetThree);
            var resultFour = encoding.GetBytes(targetFour);
            var resultFive = encoding.GetBytes(targetFive);
            var resultSix = encoding.GetBytes(targetSix);
            var resultSeven = encoding.GetBytes(targetSeven);
            var resultEight = encoding.GetBytes(targetEight);

            for (var i = 0; i < expectedResultOne.Length; i++)
            {
                // Assert.
                resultOne[i].Should().Be(expectedResultOne[i]);
            }

            for (var i = 0; i < expectedResultTwo.Length; i++)
            {
                // Assert.
                resultTwo[i].Should().Be(expectedResultTwo[i]);
            }

            for (var i = 0; i < expectedResultThree.Length; i++)
            {
                // Assert.
                resultThree[i].Should().Be(expectedResultThree[i]);
            }

            for (var i = 0; i < expectedResultFour.Length; i++)
            {
                // Assert.
                resultFour[i].Should().Be(expectedResultFour[i]);
            }

            for (var i = 0; i < expectedResultFive.Length; i++)
            {
                // Assert.
                resultFive[i].Should().Be(expectedResultFive[i]);
            }

            for (var i = 0; i < expectedResultSix.Length; i++)
            {
                // Assert.
                resultSix[i].Should().Be(expectedResultSix[i]);
            }

            for (var i = 0; i < expectedResultSeven.Length; i++)
            {
                // Assert.
                resultSeven[i].Should().Be(expectedResultSeven[i]);
            }

            for (var i = 0; i < expectedResultEight.Length; i++)
            {
                // Assert.
                resultEight[i].Should().Be(expectedResultEight[i]);
            }
        }

        [TestMethod]
        public void GetString_ShouldReturnValidResult()
        {
            // Arrange.
            var encoding = Base32Encoding.Default;
            var targetOne = new Byte[] { 0x00 };
            var targetTwo = new Byte[] { 0xff, 0xff };
            var targetThree = new Byte[] { 0xf1, 0x34, 0x9c };
            var targetFour = new Byte[] { 0xff, 0x00, 0xff, 0x00 };
            var targetFive = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
            var targetSix = new Byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
            var targetSeven = new Byte[] { 0x3b, 0x23, 0xa3, 0x38, 0xa5, 0xc1, 0xd4 };
            var targetEight = new Byte[] { 0x37, 0x09, 0x42, 0x8b, 0xc2, 0x3e, 0x6a, 0xa1, 0xdd };
            var expectedResultOne = "aa";
            var expectedResultTwo = "777b";
            var expectedResultThree = "rhnyj";
            var expectedResultFour = "7ha6paa";
            var expectedResultFive = "aaaaaaaa";
            var expectedResultSix = "77777777";
            var expectedResultSeven = "3zigk4uubgvb";
            var expectedResultEight = "xjceufky6r2c2od";

            // Act.
            var resultOne = encoding.GetString(targetOne);
            var resultTwo = encoding.GetString(targetTwo);
            var resultThree = encoding.GetString(targetThree);
            var resultFour = encoding.GetString(targetFour);
            var resultFive = encoding.GetString(targetFive);
            var resultSix = encoding.GetString(targetSix);
            var resultSeven = encoding.GetString(targetSeven);
            var resultEight = encoding.GetString(targetEight);

            // Assert.
            resultOne.Should().Be(expectedResultOne);
            resultTwo.Should().Be(expectedResultTwo);
            resultThree.Should().Be(expectedResultThree);
            resultFour.Should().Be(expectedResultFour);
            resultFive.Should().Be(expectedResultFive);
            resultSix.Should().Be(expectedResultSix);
            resultSeven.Should().Be(expectedResultSeven);
            resultEight.Should().Be(expectedResultEight);
        }
    }
}