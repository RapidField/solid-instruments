// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Physics;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Physics
{
    [TestClass]
    public sealed class MassTests
    {
        [TestMethod]
        public void AdditionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var augend = new Mass(6.0d);
            var addend = new Mass(2.5d);

            // Act.
            var result = augend + addend;

            // Assert.
            result.Should().Be(new Mass(8.5m));
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Mass(1.0d);
            var targetTwo = new Mass(1.0d);
            var targetThree = new Mass(1.0000000000001d);
            var targetFour = new Mass(1.0000000000002d);

            // Act.
            var resultOne = targetOne.CompareTo(targetTwo) == 0;
            var resultTwo = targetTwo.CompareTo(targetThree) == -1;
            var resultThree = targetTwo < targetOne;
            var resultFour = targetThree > targetOne;
            var resultFive = targetFour <= targetThree;
            var resultSix = targetTwo >= targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
            resultSix.Should().BeTrue();
        }

        [TestMethod]
        public void DivisionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dividend = new Mass(6.0d);
            var divisor = 2.5d;

            // Act.
            var result = dividend / divisor;

            // Assert.
            result.Should().Be(new Mass(2.4m));
        }

        [TestMethod]
        public void EqualityComparer_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Mass(1.0d);
            var targetTwo = new Mass(1.0d);
            var targetThree = new Mass(1.0000000000001d);

            // Act.
            var resultOne = targetOne.Equals(targetTwo);
            var resultTwo = targetTwo.Equals(targetThree);
            var resultThree = targetTwo == targetOne;
            var resultFour = targetThree == targetOne;
            var resultFive = targetOne != targetThree;
            var resultSix = targetTwo != targetOne;

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeFalse();
            resultThree.Should().BeTrue();
            resultFour.Should().BeFalse();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
        }

        [TestMethod]
        public void FromCentigrams_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromCentigrams(value);
            var parityValue = target.TotalCentigrams;

            // Assert.
            target.TotalCentigrams.Should().Be(parityValue);
            target.TotalGrams.Should().Be(123.456789m);
        }

        [TestMethod]
        public void FromGrams_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromGrams(value);
            var parityValue = target.TotalGrams;

            // Assert.
            target.TotalGrams.Should().Be(parityValue);
            target.TotalGrams.Should().Be(12345.6789m);
        }

        [TestMethod]
        public void FromKilograms_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromKilograms(value);
            var parityValue = target.TotalKilograms;

            // Assert.
            target.TotalKilograms.Should().Be(parityValue);
            target.TotalGrams.Should().Be(12345678.9m);
        }

        [TestMethod]
        public void FromMicrograms_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromMicrograms(value);
            var parityValue = target.TotalMicrograms;

            // Assert.
            target.TotalMicrograms.Should().Be(parityValue);
            target.TotalGrams.Should().Be(0.0123456789m);
        }

        [TestMethod]
        public void FromMilligrams_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromMilligrams(value);
            var parityValue = target.TotalMilligrams;

            // Assert.
            target.TotalMilligrams.Should().Be(parityValue);
            target.TotalGrams.Should().Be(12.3456789m);
        }

        [TestMethod]
        public void FromNanograms_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Mass.FromNanograms(value);
            var parityValue = target.TotalNanograms;

            // Assert.
            target.TotalNanograms.Should().Be(parityValue);
            target.TotalGrams.Should().Be(1.23456789e-5m);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new Mass(1m),
                new Mass(2m),
                new Mass(4m),
                new Mass(-1m),
                new Mass(1024m)
            };

            // Act.
            var results = targets.Select(target => target.GetHashCode());

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Mass(12345.6789d);
            var input = target.ToString();

            // Act.
            var result = Mass.Parse(input);

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void SubtractionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var subtrahend = new Mass(6.0d);
            var minuend = new Mass(2.5d);

            // Act.
            var result = subtrahend - minuend;

            // Assert.
            result.Should().Be(new Mass(3.5m));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Mass(12345.6789d);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(16);
        }

        [TestMethod]
        public void ToString_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Mass(12345.6789d);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("12345.6789 g");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new Mass(12345.6789d);
            var input = target.ToString();

            // Act.
            var resultTwo = Mass.TryParse(input, out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void Zero_ShouldRepresentZero()
        {
            // Arrange.
            var target = Mass.Zero;

            // Act.
            var result = target.TotalGrams;

            // Assert.
            result.Should().Be(0m);
        }
    }
}