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
    public class IntegerExtensionsTests
    {
        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForInt32Target()
        {
            // Arrange.
            var target = (Int32)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForNinetyNine_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)99;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(2);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForInt32Target()
        {
            // Arrange.
            var target = (Int32)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOne_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)1;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForInt32Target()
        {
            // Arrange.
            var target = (Int32)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForOneHundred_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)100;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(1);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForInt32Target()
        {
            // Arrange.
            var target = (Int32)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void CountSignificantFigures_ShouldReturnValidResult_ForZero_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)0;

            // Act.
            var result = target.CountSignificantFigures();

            // Assert.
            result.Should().Be(0);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt16Target()
        {
            // Arrange.
            var target = (Int16)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt32Target()
        {
            // Arrange.
            var target = 3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(4);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForInt64Target()
        {
            // Arrange.
            var target = (Int64)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt16Target()
        {
            // Arrange.
            var target = (UInt16)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt32Target()
        {
            // Arrange.
            var target = (UInt32)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(4);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_ForUInt64Target()
        {
            // Arrange.
            var target = (UInt64)3;

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }
    }
}