// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Extensions;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Extensions
{
    [TestClass]
    public sealed class IntegerExtensionsTests
    {
        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult_ForInt16()
        {
            // Arrange.
            Int16 negativeTwo = -2;
            Int16 zero = 0;
            Int16 two = 2;

            // Act.
            var resultOne = negativeTwo.AbsoluteValue();
            var resultTwo = zero.AbsoluteValue();
            var resultThree = two.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2);
            resultTwo.Should().Be(0);
            resultThree.Should().Be(2);
        }

        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult_ForInt32()
        {
            // Arrange.
            var negativeTwo = -2;
            var zero = 0;
            var two = 2;

            // Act.
            var resultOne = negativeTwo.AbsoluteValue();
            var resultTwo = zero.AbsoluteValue();
            var resultThree = two.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2);
            resultTwo.Should().Be(0);
            resultThree.Should().Be(2);
        }

        [TestMethod]
        public void AbsoluteValue_ShouldReturnValidResult_ForInt64()
        {
            // Arrange.
            Int64 negativeTwo = -2;
            Int64 zero = 0;
            Int64 two = 2;

            // Act.
            var resultOne = negativeTwo.AbsoluteValue();
            var resultTwo = zero.AbsoluteValue();
            var resultThree = two.AbsoluteValue();

            // Assert.
            resultOne.Should().Be(2);
            resultTwo.Should().Be(0);
            resultThree.Should().Be(2);
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForInt16()
        {
            // Arrange.
            Int16 negativeTwo = -2;
            Int16 negativeOne = -1;
            Int16 zero = 0;
            Int16 one = 1;
            Int16 two = 2;
            Int16 three = 3;
            Int16 four = 4;

            // Act.
            var resultOne = negativeTwo.IsEven();
            var resultTwo = negativeOne.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForInt32()
        {
            // Arrange.
            var negativeTwo = -2;
            var negativeOne = -1;
            var zero = 0;
            var one = 1;
            var two = 2;
            var three = 3;
            var four = 4;

            // Act.
            var resultOne = negativeTwo.IsEven();
            var resultTwo = negativeOne.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForInt64()
        {
            // Arrange.
            Int64 negativeTwo = -2;
            Int64 negativeOne = -1;
            Int64 zero = 0;
            Int64 one = 1;
            Int64 two = 2;
            Int64 three = 3;
            Int64 four = 4;

            // Act.
            var resultOne = negativeTwo.IsEven();
            var resultTwo = negativeOne.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForUInt16()
        {
            // Arrange.
            UInt16 twoHundredFiftySix = 256;
            UInt16 eleven = 11;
            UInt16 zero = 0;
            UInt16 one = 1;
            UInt16 two = 2;
            UInt16 three = 3;
            UInt16 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsEven();
            var resultTwo = eleven.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForUInt32()
        {
            // Arrange.
            UInt32 twoHundredFiftySix = 256;
            UInt32 eleven = 11;
            UInt32 zero = 0;
            UInt32 one = 1;
            UInt32 two = 2;
            UInt32 three = 3;
            UInt32 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsEven();
            var resultTwo = eleven.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsEven_ShouldReturnValidResult_ForUInt64()
        {
            // Arrange.
            UInt64 twoHundredFiftySix = 256;
            UInt64 eleven = 11;
            UInt64 zero = 0;
            UInt64 one = 1;
            UInt64 two = 2;
            UInt64 three = 3;
            UInt64 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsEven();
            var resultTwo = eleven.IsEven();
            var resultThree = zero.IsEven();
            var resultFour = one.IsEven();
            var resultFive = two.IsEven();
            var resultSix = three.IsEven();
            var resultSeven = four.IsEven();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeTrue();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForInt16()
        {
            // Arrange.
            Int16 negativeTwo = -2;
            Int16 negativeOne = -1;
            Int16 zero = 0;
            Int16 one = 1;
            Int16 two = 2;
            Int16 three = 3;
            Int16 four = 4;

            // Act.
            var resultOne = negativeTwo.IsOdd();
            var resultTwo = negativeOne.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForInt32()
        {
            // Arrange.
            var negativeTwo = -2;
            var negativeOne = -1;
            var zero = 0;
            var one = 1;
            var two = 2;
            var three = 3;
            var four = 4;

            // Act.
            var resultOne = negativeTwo.IsOdd();
            var resultTwo = negativeOne.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForInt64()
        {
            // Arrange.
            Int64 negativeTwo = -2;
            Int64 negativeOne = -1;
            Int64 zero = 0;
            Int64 one = 1;
            Int64 two = 2;
            Int64 three = 3;
            Int64 four = 4;

            // Act.
            var resultOne = negativeTwo.IsOdd();
            var resultTwo = negativeOne.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForUInt16()
        {
            // Arrange.
            UInt16 twoHundredFiftySix = 256;
            UInt16 eleven = 11;
            UInt16 zero = 0;
            UInt16 one = 1;
            UInt16 two = 2;
            UInt16 three = 3;
            UInt16 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsOdd();
            var resultTwo = eleven.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForUInt32()
        {
            // Arrange.
            UInt32 twoHundredFiftySix = 256;
            UInt32 eleven = 11;
            UInt32 zero = 0;
            UInt32 one = 1;
            UInt32 two = 2;
            UInt32 three = 3;
            UInt32 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsOdd();
            var resultTwo = eleven.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void IsOdd_ShouldReturnValidResult_ForUInt64()
        {
            // Arrange.
            UInt64 twoHundredFiftySix = 256;
            UInt64 eleven = 11;
            UInt64 zero = 0;
            UInt64 one = 1;
            UInt64 two = 2;
            UInt64 three = 3;
            UInt64 four = 4;

            // Act.
            var resultOne = twoHundredFiftySix.IsOdd();
            var resultTwo = eleven.IsOdd();
            var resultThree = zero.IsOdd();
            var resultFour = one.IsOdd();
            var resultFive = two.IsOdd();
            var resultSix = three.IsOdd();
            var resultSeven = four.IsOdd();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
            resultSeven.Should().BeFalse();
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForInt16()
        {
            // Arrange.
            Int16 target = 65;
            Int16 lowerBoundary = 50;
            Int16 upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForInt32()
        {
            // Arrange.
            var target = 65;
            var lowerBoundary = 50;
            var upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForInt64()
        {
            // Arrange.
            Int64 target = 65;
            Int64 lowerBoundary = 50;
            Int64 upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForUInt16()
        {
            // Arrange.
            UInt16 target = 65;
            UInt16 lowerBoundary = 50;
            UInt16 upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForUInt32()
        {
            // Arrange.
            UInt32 target = 65;
            UInt32 lowerBoundary = 50;
            UInt32 upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void PositionInRange_ShouldReturnValidResult_ForUInt64()
        {
            // Arrange.
            UInt64 target = 65;
            UInt64 lowerBoundary = 50;
            UInt64 upperBoundary = 150;

            // Act.
            var result = target.PositionInRange(lowerBoundary, upperBoundary);

            // Assert.
            result.Should().Be(0.15d);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForInt16_WithUnspecifiedModeArgument()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForInt32_WithUnspecifiedModeArgument()
        {
            // Arrange.
            var target = 10;
            var factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForInt64_WithUnspecifiedModeArgument()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForUInt16_WithUnspecifiedModeArgument()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForUInt32_WithUnspecifiedModeArgument()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldRaiseArgumentOutOfRangeException_ForUInt64_WithUnspecifiedModeArgument()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 5;
            var mode = FactorRoundingMode.Unspecified;

            // Act.
            var action = new Action(() =>
            {
                var result = target.RoundedToFactor(factor, mode);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt16_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int16 target = 10;
            Int16 factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            var target = 10;
            var factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            var target = 10;
            var factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            var target = 10;
            var factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt32_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            var target = 10;
            var factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForInt64_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            Int64 target = 10;
            Int64 factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt16_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt16 target = 10;
            UInt16 factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt32_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt32 target = 10;
            UInt32 factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingEvenFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 5;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingEvenFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 5;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingEvenFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 5;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(10);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingHighFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 6;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(6);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingHighFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 6;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingHighFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 6;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingLowFactor_InInwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 3;
            var mode = FactorRoundingMode.InwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingLowFactor_InInwardOrOutwardMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 3;
            var mode = FactorRoundingMode.InwardOrOutward;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(9);
        }

        [TestMethod]
        public void RoundedToFactor_ShouldReturnValidResult_ForUInt64_UsingLowFactor_InOutwardOnlyMode()
        {
            // Arrange.
            UInt64 target = 10;
            UInt64 factor = 3;
            var mode = FactorRoundingMode.OutwardOnly;

            // Act.
            var result = target.RoundedToFactor(factor, mode);

            // Assert.
            result.Should().Be(12);
        }
    }
}