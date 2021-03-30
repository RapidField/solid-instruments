// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void ArithmeticOperations_ShouldScaleSuccessfully()
        {
            // Arrange.
            var maxByteValue = Byte.MaxValue.ToNumber();
            var maxUInt64Value = UInt64.MaxValue.ToNumber();
            var maxSingleValue = Single.MaxValue.ToNumber();
            var maxDoubleValue = Double.MaxValue.ToNumber();
            var maxDecimalValue = Decimal.MaxValue.ToNumber();

            // Act.
            var resultOne = maxByteValue * maxUInt64Value;
            var resultTwo = resultOne + maxSingleValue;
            var resultThree = resultTwo - maxDoubleValue;
            var resultFour = resultThree / maxDecimalValue;

            // Assert.
            resultOne.Should().Be(maxByteValue.ToBigInteger() * maxUInt64Value.ToBigInteger());
            resultTwo.Should().Be(resultOne + maxSingleValue.ToBigRational());
            resultThree.Should().Be(resultTwo - maxDoubleValue.ToBigRational());
            resultFour.Should().Be(resultThree / maxDecimalValue.ToBigRational());
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var cases = NumericTestPairList.Instance;
            var caseCount = cases.Count;

            for (var i = 0; i < caseCount; i++)
            {
                var target = cases[i];

                for (var j = 0; j < caseCount; j++)
                {
                    // Assert.
                    var subject = cases[j];
                    target.VerifyRelativeStateConsistency(subject);
                    target.VerifyArithmeticOperationalCorrectness(subject);
                }
            }
        }

        [TestMethod]
        public void RaisedToPower_ShouldProduceValidResults()
        {
            // Arrange.
            var zero = 0.ToNumber();
            var one = 1.ToNumber();
            var two = 2.ToNumber();
            var three = 3.ToNumber();
            var oneHalf = one / two;
            var oneThird = one / three;

            // Act.
            var resultOne = zero.RaisedToPower(3);
            var resultTwo = one.RaisedToPower(3);
            var resultThree = two.RaisedToPower(0);
            var resultFour = two.RaisedToPower(1);
            var resultFive = three.RaisedToPower(3);
            var resultSix = oneHalf.RaisedToPower(2);
            var resultSeven = oneThird.RaisedToPower(3);

            // Assert.
            resultOne.Should().Be(0);
            resultTwo.Should().Be(1);
            resultThree.Should().Be(1);
            resultFour.Should().Be(2);
            resultFive.Should().Be(27);
            resultSix.Should().Be(0.25f);
            resultSeven.Should().Be(0.037037037037037f);
        }

        [TestMethod]
        public void RoundedTo_ShouldProduceValidResults()
        {
            // Arrange.
            var zero = 0.ToNumber();
            var one = 1.ToNumber();
            var two = 2.ToNumber();
            var three = 3.ToNumber();
            var oneHalf = one / two;
            var oneThird = one / three;
            var twoThirds = two / three;

            // Act.
            var resultOne = oneHalf.RoundedTo(0, MidpointRounding.AwayFromZero);
            var resultTwo = oneHalf.RoundedTo(0, MidpointRounding.ToZero);
            var resultThree = oneThird.RoundedTo(3);
            var resultFour = twoThirds.RoundedTo(2);

            // Assert.
            resultOne.Should().Be(one);
            resultTwo.Should().Be(zero);
            resultThree.Should().Be(0.333f);
            resultFour.Should().Be(0.67f);
        }
    }
}