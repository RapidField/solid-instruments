// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Mathematics.Physics;
using System.Linq;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Physics
{
    [TestClass]
    public sealed class VolumeTests
    {
        [TestMethod]
        public void AdditionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var augend = new Volume(6.0d);
            var addend = new Volume(2.5d);

            // Act.
            var result = augend + addend;

            // Assert.
            result.Should().Be(new Volume(8.5m));
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Volume(1.0d);
            var targetTwo = new Volume(1.0d);
            var targetThree = new Volume(1.0000000000001d);
            var targetFour = new Volume(1.0000000000002d);

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
            var dividend = new Volume(6.0d);
            var divisor = 2.5d;

            // Act.
            var result = dividend / divisor;

            // Assert.
            result.Should().Be(new Volume(2.4m));
        }

        [TestMethod]
        public void EqualityComparer_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Volume(1.0d);
            var targetTwo = new Volume(1.0d);
            var targetThree = new Volume(1.0000000000001d);

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
        public void FromCubicCentimeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicCentimeters(value);
            var parityValue = target.TotalCubicCentimeters;

            // Assert.
            target.TotalCubicCentimeters.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(0.0123456789m);
        }

        [TestMethod]
        public void FromCubicFeet_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicFeet(value);
            var parityValue = target.TotalCubicFeet;

            // Assert.
            target.TotalCubicFeet.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(349.59069548539131127916252208m);
        }

        [TestMethod]
        public void FromCubicInches_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicInches(value);
            var parityValue = target.TotalCubicInches;

            // Assert.
            target.TotalCubicInches.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(0.2023094302577496131044072203m);
        }

        [TestMethod]
        public void FromCubicKilometers_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicKilometers(value);
            var parityValue = target.TotalCubicKilometers;

            // Assert.
            target.TotalCubicKilometers.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(12345678900000m);
        }

        [TestMethod]
        public void FromCubicMeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicMeters(value);
            var parityValue = target.TotalCubicMeters;

            // Assert.
            target.TotalCubicMeters.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(value.ToDecimal());
        }

        [TestMethod]
        public void FromCubicMiles_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicMiles(value);
            var parityValue = target.TotalCubicMiles;

            // Assert.
            target.TotalCubicMiles.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(51458982631203.811381625839367m);
        }

        [TestMethod]
        public void FromCubicMillimeters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicMillimeters(value);
            var parityValue = target.TotalCubicMillimeters;

            // Assert.
            target.TotalCubicMillimeters.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(0.0000123456789m);
        }

        [TestMethod]
        public void FromCubicYards_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromCubicYards(value);
            var parityValue = target.TotalCubicYards;

            // Assert.
            target.TotalCubicYards.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(9438.946030852837759212692219m);
        }

        [TestMethod]
        public void FromGallons_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromGallons(value);
            var parityValue = target.TotalGallons;

            // Assert.
            target.TotalGallons.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(46.73347827599310817333126453m);
        }

        [TestMethod]
        public void FromLiters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromLiters(value);
            var parityValue = target.TotalLiters;

            // Assert.
            target.TotalLiters.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(12.3456789m);
        }

        [TestMethod]
        public void FromMilliliters_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 12345.6789d;

            // Act.
            var target = Volume.FromMilliliters(value);
            var parityValue = target.TotalMilliliters;

            // Assert.
            target.TotalMilliliters.Should().Be(parityValue);
            target.TotalCubicMeters.Should().Be(0.0123456789m);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new Volume(1m),
                new Volume(2m),
                new Volume(4m),
                new Volume(-1m),
                new Volume(1024m)
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
            var target = new Volume(12345.6789d);
            var input = target.ToString();

            // Act.
            var result = Volume.Parse(input);

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void SubtractionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var subtrahend = new Volume(6.0d);
            var minuend = new Volume(2.5d);

            // Act.
            var result = subtrahend - minuend;

            // Assert.
            result.Should().Be(new Volume(3.5m));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Volume(12345.6789d);

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
            var target = new Volume(12345.6789d);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("12345.6789 cbm");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new Volume(12345.6789d);
            var input = target.ToString();

            // Act.
            var resultTwo = Volume.TryParse(input, out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void Zero_ShouldRepresentZero()
        {
            // Arrange.
            var target = Volume.Zero;

            // Act.
            var result = target.TotalCubicMeters;

            // Assert.
            result.Should().Be(0m);
        }
    }
}