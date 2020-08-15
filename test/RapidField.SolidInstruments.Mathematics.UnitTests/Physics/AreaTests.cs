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
    public sealed class AreaTests
    {
        [TestMethod]
        public void AdditionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var augend = new Area(6.0d);
            var addend = new Area(2.5d);

            // Act.
            var result = augend + addend;

            // Assert.
            result.Should().Be(new Area(8.5m));
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Area(1.0d);
            var targetTwo = new Area(1.0d);
            var targetThree = new Area(1.0000000000001d);
            var targetFour = new Area(1.0000000000002d);

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
            var dividend = new Area(6.0d);
            var divisor = 2.5d;

            // Act.
            var result = dividend / divisor;

            // Assert.
            result.Should().Be(new Area(2.4m));
        }

        [TestMethod]
        public void EqualityComparer_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Area(1.0d);
            var targetTwo = new Area(1.0d);
            var targetThree = new Area(1.0000000000001d);

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
        public void FromAcres_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromAcres(value);
            var parityValue = target.TotalAcres;

            // Assert.
            target.TotalAcres.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(49961189.945353175901993408933m);
        }

        [TestMethod]
        public void FromHectares_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromHectares(value);
            var parityValue = target.TotalHectares;

            // Assert.
            target.TotalHectares.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(123456789m);
        }

        [TestMethod]
        public void FromSquareCentimeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareCentimeters(value);
            var parityValue = target.TotalSquareCentimeters;

            // Assert.
            target.TotalSquareCentimeters.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(1.23456789m);
        }

        [TestMethod]
        public void FromSquareFeet_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareFeet(value);
            var parityValue = target.TotalSquareFeet;

            // Assert.
            target.TotalSquareFeet.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(1146.9511006738562459650398808m);
        }

        [TestMethod]
        public void FromSquareInches_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareInches(value);
            var parityValue = target.TotalSquareInches;

            // Assert.
            target.TotalSquareInches.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(7.964938199124000063719505593m);
        }

        [TestMethod]
        public void FromSquareKilometers_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareKilometers(value);
            var parityValue = target.TotalSquareKilometers;

            // Assert.
            target.TotalSquareKilometers.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(12345678900m);
        }

        [TestMethod]
        public void FromSquareMeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareMeters(value);
            var parityValue = target.TotalSquareMeters;

            // Assert.
            target.TotalSquareMeters.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(12345.6789m);
        }

        [TestMethod]
        public void FromSquareMiles_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareMiles(value);
            var parityValue = target.TotalSquareMiles;

            // Assert.
            target.TotalSquareMiles.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(31975161565.026031024489126476m);
        }

        [TestMethod]
        public void FromSquareMillimeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareMillimeters(value);
            var parityValue = target.TotalSquareMillimeters;

            // Assert.
            target.TotalSquareMillimeters.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(0.0123456789m);
        }

        [TestMethod]
        public void FromSquareYards_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Area.FromSquareYards(value);
            var parityValue = target.TotalSquareYards;

            // Assert.
            target.TotalSquareYards.Should().Be(parityValue);
            target.TotalSquareMeters.Should().Be(10322.559906064706213685358927m);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new Area(1m),
                new Area(2m),
                new Area(4m),
                new Area(-1m),
                new Area(1024m)
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
            var target = new Area(12345.6789d);
            var input = target.ToString();

            // Act.
            var result = Area.Parse(input);

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void SubtractionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var subtrahend = new Area(6.0d);
            var minuend = new Area(2.5d);

            // Act.
            var result = subtrahend - minuend;

            // Assert.
            result.Should().Be(new Area(3.5m));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Area(12345.6789d);

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
            var target = new Area(12345.6789d);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("12345.6789 sqm");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new Area(12345.6789d);
            var input = target.ToString();

            // Act.
            var resultTwo = Area.TryParse(input, out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void Zero_ShouldRepresentZero()
        {
            // Arrange.
            var target = Area.Zero;

            // Act.
            var result = target.TotalSquareMeters;

            // Assert.
            result.Should().Be(0m);
        }
    }
}