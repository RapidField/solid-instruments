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
    public sealed class LengthTests
    {
        [TestMethod]
        public void AdditionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var augend = new Length(6.0d);
            var addend = new Length(2.5d);

            // Act.
            var result = (augend + addend);

            // Assert.
            result.Should().Be(new Length(8.5m));
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Length(1.0d);
            var targetTwo = new Length(1.0d);
            var targetThree = new Length(1.0000000000001d);
            var targetFour = new Length(1.0000000000002d);

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
            var dividend = new Length(6.0d);
            var divisor = 2.5d;
            var expectedQuotient = new Length(2.4d);

            // Act.
            var result = (dividend / divisor);

            // Assert.
            result.Should().Be(new Length(2.4m));
        }

        [TestMethod]
        public void EqualityComparer_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Length(1.0d);
            var targetTwo = new Length(1.0d);
            var targetThree = new Length(1.0000000000001d);

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
        public void FromCentimeters_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromCentimeters(value);
            var parityValue = target.TotalCentimeters;

            // Assert.
            target.TotalCentimeters.Should().Be(parityValue);
            target.TotalMeters.Should().Be(123.456789m);
        }

        [TestMethod]
        public void FromFeet_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromFeet(value);
            var parityValue = target.TotalFeet;

            // Assert.
            target.TotalFeet.Should().Be(parityValue);
            target.TotalMeters.Should().Be(3762.96292872m);
        }

        [TestMethod]
        public void FromInches_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromInches(value);
            var parityValue = target.TotalInches;

            // Assert.
            target.TotalInches.Should().Be(parityValue);
            target.TotalMeters.Should().Be(313.58024406m);
        }

        [TestMethod]
        public void FromKilometers_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromKilometers(value);
            var parityValue = target.TotalKilometers;

            // Assert.
            target.TotalKilometers.Should().Be(parityValue);
            target.TotalMeters.Should().Be(12345678.9m);
        }

        [TestMethod]
        public void FromLightYears_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromLightYears(value);
            var parityValue = target.TotalLightYears;

            // Assert.
            target.TotalLightYears.Should().Be(parityValue);
            target.TotalMeters.Should().Be(116799140573885319645.5026326m);
        }

        [TestMethod]
        public void FromMeters_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromMeters(value);
            var parityValue = target.TotalMeters;

            // Assert.
            target.TotalMeters.Should().Be(parityValue);
            target.TotalMeters.Should().Be(12345.6789m);
        }

        [TestMethod]
        public void FromMiles_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromMiles(value);
            var parityValue = target.TotalMiles;

            // Assert.
            target.TotalMiles.Should().Be(parityValue);
            target.TotalMeters.Should().Be(19868444.2636415999999999995m);
        }

        [TestMethod]
        public void FromMillimeters_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromMillimeters(value);
            var parityValue = target.TotalMillimeters;

            // Assert.
            target.TotalMillimeters.Should().Be(parityValue);
            target.TotalMeters.Should().Be(12.3456789m);
        }

        [TestMethod]
        public void FromNanometers_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromNanometers(value);
            var parityValue = target.TotalNanometers;

            // Assert.
            target.TotalNanometers.Should().Be(parityValue);
            target.TotalMeters.Should().Be(1.23456789e-5m);
        }

        [TestMethod]
        public void FromYards_ShouldReturnValidResult()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Length.FromYards(value);
            var parityValue = target.TotalYards;

            // Assert.
            target.TotalYards.Should().Be(parityValue);
            target.TotalMeters.Should().Be(11288.88878616m);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new Length(1m),
                new Length(2m),
                new Length(4m),
                new Length(-1m),
                new Length(1024m)
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
            var target = new Length(12345.6789d);
            var input = target.ToString();

            // Act.
            var result = Length.Parse(input);

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void SubtractionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var subtrahend = new Length(6.0d);
            var minuend = new Length(2.5d);
            var expectedDifference = new Length(3.5d);

            // Act.
            var result = (subtrahend - minuend);

            // Assert.
            result.Should().Be(new Length(3.5m));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Length(12345.6789d);

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
            var target = new Length(12345.6789d);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("12345.6789 m");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new Length(12345.6789d);
            var input = target.ToString();

            // Act.
            var resultTwo = Length.TryParse(input, out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void Zero_ShouldRepresentZero()
        {
            // Arrange.
            var target = Length.Zero;

            // Act.
            var result = target.TotalMeters;

            // Assert.
            result.Should().Be(0m);
        }
    }
}