// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class ByteCollectionExtensionsTests
    {
        [TestMethod]
        public void Compress_ShouldBeReversible()
        {
            // Arrange.
            var targetString = "{ \"Foo\" = 4321, \"Bar\" = \"foobar\", \"FooBar\" = [ 1234, 4321 ] }";
            var target = targetString.ToByteArray(Encoding.UTF8);

            // Act.
            var compressedTarget = target.Compress();
            var result = Encoding.UTF8.GetString(compressedTarget.Decompress());

            // Assert.
            result.Should().Be(targetString);
        }

        [TestMethod]
        public void Compress_ShouldReduceLengthOfCompressibleInputArray()
        {
            // Arrange.
            var targetString = "{ \"Foo\" = 4321, \"Bar\" = \"foobar\", \"FooBar\" = [ 1234, 4321 ] }";
            var target = targetString.ToByteArray(Encoding.UTF8);

            // Act.
            var result = target.Compress();

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeLessThan(target.Length);
        }

        [TestMethod]
        public void ComputeThirtyTwoBitHash_ShouldReturnUniqueHash()
        {
            // Arrange.
            var targetOne = new List<Byte>();
            var targetTwo = new List<Byte>() { 0x00 };
            var targetThree = new List<Byte>() { 0xe1 };
            var targetFour = new List<Byte>() { 0xe1, 0x4b };
            var targetFive = new List<Byte>() { 0xe1, 0x4b, 0xd6 };
            var targetSix = new List<Byte>() { 0x00, 0x00, 0x00, 0x00 };
            var targetSeven = new List<Byte>() { 0xe1, 0x4b, 0xd6, 0xcb, 0x00, 0xa4, 0xae, 0x79, 0x07, 0xb3, 0xf8, 0x99 };
            var targetEight = new List<Byte>() { 0xe1, 0x4b, 0xd6, 0xcb, 0x00, 0xa4, 0xae, 0x79, 0x07, 0xb3, 0xf8, 0x99, 0xe9, 0x1b, 0xd1, 0x3e, 0xee, 0xa8, 0x5b, 0xde, 0x86, 0x46, 0x17, 0x75 };

            // Act.
            var results = new List<Int32>
            {
                targetOne.ComputeThirtyTwoBitHash(),
                targetTwo.ComputeThirtyTwoBitHash(),
                targetThree.ComputeThirtyTwoBitHash(),
                targetFour.ComputeThirtyTwoBitHash(),
                targetFive.ComputeThirtyTwoBitHash(),
                targetSix.ComputeThirtyTwoBitHash(),
                targetSeven.ComputeThirtyTwoBitHash(),
                targetEight.ComputeThirtyTwoBitHash()
            };

            foreach (var result in results)
            {
                // Assert.
                results.Count(value => value == result).Should().Be(1);
            }
        }

        [TestMethod]
        public void GenerateChecksumIdentity_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new List<Byte>();
            var targetTwo = new List<Byte>() { 0x00 };
            var targetThree = new List<Byte>() { 0xe1 };
            var targetFour = new List<Byte>() { 0xe1, 0x4b };
            var targetFive = new List<Byte>() { 0xe1, 0x4b, 0xd6 };
            var targetSix = new List<Byte>() { 0x00, 0x00, 0x00, 0x00 };
            var targetSeven = new List<Byte>() { 0xe1, 0x4b, 0xd6, 0xcb, 0x00, 0xa4, 0xae, 0x79, 0x07, 0xb3, 0xf8, 0x99 };
            var targetEight = new List<Byte>() { 0xe1, 0x4b, 0xd6, 0xcb, 0x00, 0xa4, 0xae, 0x79, 0x07, 0xb3, 0xf8, 0x99, 0xe9, 0x1b, 0xd1, 0x3e, 0xee, 0xa8, 0x5b, 0xde, 0x86, 0x46, 0x17, 0x75 };

            // Act.
            var results = new List<Guid>
            {
                targetOne.GenerateChecksumIdentity(),
                targetTwo.GenerateChecksumIdentity(),
                targetThree.GenerateChecksumIdentity(),
                targetFour.GenerateChecksumIdentity(),
                targetFive.GenerateChecksumIdentity(),
                targetSix.GenerateChecksumIdentity(),
                targetSeven.GenerateChecksumIdentity(),
                targetEight.GenerateChecksumIdentity()
            };

            foreach (var result in results)
            {
                // Assert.
                results.Count(value => value == result).Should().Be(1);
            }
        }

        [TestMethod]
        public void PerformCircularShift_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = Array.Empty<Byte>();
            var targetTwo = new Byte[1] { 0x3f };
            var targetThree = new Byte[2] { 0x7b, 0x80 };
            var targetFour = new Byte[2] { 0xe1, 0x93 };
            var targetFive = new Byte[4] { 0x68, 0xc5, 0x8a, 0x13 };
            var bitShiftCountOne = 3;
            var bitShiftCountTwo = 3;
            var bitShiftCountThree = 3;
            var bitShiftCountFour = 25;
            var bitShiftCountFive = 10;
            var expectedResultOne = Array.Empty<Byte>();
            var expectedResultTwo = new Byte[1] { 0xf9 };
            var expectedResultThree = new Byte[2] { 0x0f, 0x70 };
            var expectedResultFour = new Byte[2] { 0x27, 0xc3 };
            var expectedResultFive = new Byte[4] { 0xb1, 0xe2, 0x04, 0x5a };

            // Act.
            var resultOne = targetOne.PerformCircularBitShift(BitShiftDirection.Left, bitShiftCountOne);
            var resultTwo = targetTwo.PerformCircularBitShift(BitShiftDirection.Right, bitShiftCountTwo);
            var resultThree = targetThree.PerformCircularBitShift(BitShiftDirection.Left, bitShiftCountThree);
            var resultFour = targetFour.PerformCircularBitShift(BitShiftDirection.Right, bitShiftCountFour);
            var resultFive = targetFive.PerformCircularBitShift(BitShiftDirection.Left, bitShiftCountFive);

            // Assert.
            resultOne.Length.Should().Be(expectedResultOne.Length);
            resultTwo.Length.Should().Be(expectedResultTwo.Length);
            resultThree.Length.Should().Be(expectedResultThree.Length);
            resultFour.Length.Should().Be(expectedResultFour.Length);
            resultFive.Length.Should().Be(expectedResultFive.Length);

            for (var i = 0; i < targetOne.Length; i++)
            {
                // Assert.
                resultOne[i].Should().Be(expectedResultOne[i]);
            }

            for (var i = 0; i < targetTwo.Length; i++)
            {
                // Assert.
                resultTwo[i].Should().Be(expectedResultTwo[i]);
            }

            for (var i = 0; i < targetThree.Length; i++)
            {
                // Assert.
                resultThree[i].Should().Be(expectedResultThree[i]);
            }

            for (var i = 0; i < targetFour.Length; i++)
            {
                // Assert.
                resultFour[i].Should().Be(expectedResultFour[i]);
            }

            for (var i = 0; i < targetFive.Length; i++)
            {
                // Assert.
                resultFive[i].Should().Be(expectedResultFive[i]);
            }

            // Act.
            resultOne = resultOne.PerformCircularBitShift(BitShiftDirection.Right, bitShiftCountOne);
            resultTwo = resultTwo.PerformCircularBitShift(BitShiftDirection.Left, bitShiftCountTwo);
            resultThree = resultThree.PerformCircularBitShift(BitShiftDirection.Right, bitShiftCountThree);
            resultFour = resultFour.PerformCircularBitShift(BitShiftDirection.Left, bitShiftCountFour);
            resultFive = resultFive.PerformCircularBitShift(BitShiftDirection.Right, bitShiftCountFive);

            // Assert.
            resultOne.Length.Should().Be(targetOne.Length);
            resultTwo.Length.Should().Be(targetTwo.Length);
            resultThree.Length.Should().Be(targetThree.Length);
            resultFour.Length.Should().Be(targetFour.Length);
            resultFive.Length.Should().Be(targetFive.Length);

            for (var i = 0; i < targetOne.Length; i++)
            {
                // Assert.
                resultOne[i].Should().Be(targetOne[i]);
            }

            for (var i = 0; i < targetTwo.Length; i++)
            {
                // Assert.
                resultTwo[i].Should().Be(targetTwo[i]);
            }

            for (var i = 0; i < targetThree.Length; i++)
            {
                // Assert.
                resultThree[i].Should().Be(targetThree[i]);
            }

            for (var i = 0; i < targetFour.Length; i++)
            {
                // Assert.
                resultFour[i].Should().Be(targetFour[i]);
            }

            for (var i = 0; i < targetFive.Length; i++)
            {
                // Assert.
                resultFive[i].Should().Be(targetFive[i]);
            }
        }

        [TestMethod]
        public void ToBase64String_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = Array.Empty<Byte>();
            var targetTwo = new Byte[1] { 0x3f };
            var targetThree = new Byte[2] { 0x7b, 0x80 };
            var targetFour = new Byte[2] { 0xe1, 0x93 };
            var targetFive = new Byte[4] { 0x68, 0xc5, 0x8a, 0x13 };
            var expectedResultOne = Convert.ToBase64String(targetOne);
            var expectedResultTwo = Convert.ToBase64String(targetTwo);
            var expectedResultThree = Convert.ToBase64String(targetThree);
            var expectedResultFour = Convert.ToBase64String(targetFour);
            var expectedResultFive = Convert.ToBase64String(targetFive);

            // Act.
            var resultOne = targetOne.ToBase64String();
            var resultTwo = targetTwo.ToBase64String();
            var resultThree = targetThree.ToBase64String();
            var resultFour = targetFour.ToBase64String();
            var resultFive = targetFive.ToBase64String();

            // Assert.
            resultOne.Should().NotBeNull();
            resultTwo.Should().NotBeNull();
            resultThree.Should().NotBeNull();
            resultFour.Should().NotBeNull();
            resultFive.Should().NotBeNull();
            resultOne.Should().Be(expectedResultOne);
            resultTwo.Should().Be(expectedResultTwo);
            resultThree.Should().Be(expectedResultThree);
            resultFour.Should().Be(expectedResultFour);
            resultFive.Should().Be(expectedResultFive);
        }
    }
}