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
    public sealed class TemperatureTests
    {
        [TestMethod]
        public void AbsoluteZero_ShouldRepresentAbsoluteZero()
        {
            // Arrange.
            var target = Temperature.AbsoluteZero;

            // Act.
            var result = target.TotalKelvins;

            // Assert.
            result.Should().Be(0m);
        }

        [TestMethod]
        public void AdditionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var augend = new Temperature(6.0d);
            var addend = new Temperature(2.5d);

            // Act.
            var result = augend + addend;

            // Assert.
            result.Should().Be(new Temperature(8.5m));
        }

        [TestMethod]
        public void CompareTo_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Temperature(1.0d);
            var targetTwo = new Temperature(1.0d);
            var targetThree = new Temperature(1.0000000000001d);
            var targetFour = new Temperature(1.0000000000002d);

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
            var dividend = new Temperature(6.0d);
            var divisor = 2.5d;

            // Act.
            var result = dividend / divisor;

            // Assert.
            result.Should().Be(new Temperature(2.4m));
        }

        [TestMethod]
        public void EqualityComparer_ShouldProduceDesiredResults()
        {
            // Arrange.
            var targetOne = new Temperature(1.0d);
            var targetTwo = new Temperature(1.0d);
            var targetThree = new Temperature(1.0000000000001d);

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
        public void FromDegreesCelsius_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 123.456d;

            // Act.
            var target = Temperature.FromDegreesCelsius(value);
            var parityValue = target.TotalDegreesCelsius;

            // Assert.
            target.TotalDegreesCelsius.Should().Be(parityValue);
            target.TotalDegreesCelsius.Should().Be(123.456m);
        }

        [TestMethod]
        public void FromDegreesFahrenheit_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 123.456d;

            // Act.
            var target = Temperature.FromDegreesFahrenheit(value);
            var parityValue = target.TotalDegreesFahrenheit;

            // Assert.
            target.TotalDegreesFahrenheit.Should().Be(parityValue);
            target.TotalDegreesCelsius.Should().Be(50.808888888888888888888888893m);
        }

        [TestMethod]
        public void FromKelvins_ShouldProduceDesiredResults()
        {
            // Arrange.
            var value = 123.456d;

            // Act.
            var target = Temperature.FromKelvins(value);
            var parityValue = target.TotalKelvins;

            // Assert.
            target.TotalKelvins.Should().Be(parityValue);
            target.TotalDegreesCelsius.Should().Be(-149.694m);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new Temperature(1m),
                new Temperature(2m),
                new Temperature(4m),
                new Temperature(-1m),
                new Temperature(1024m)
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
            var target = new Temperature(12345.6789d);
            var input = target.ToString();

            // Act.
            var result = Temperature.Parse(input);

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void SubtractionOperator_ShouldProduceDesiredResults()
        {
            // Arrange.
            var subtrahend = new Temperature(6.0d);
            var minuend = new Temperature(2.5d);

            // Act.
            var result = subtrahend - minuend;

            // Assert.
            result.Should().Be(new Temperature(3.5m));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new Temperature(12345.6789d);

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
            var target = new Temperature(12345.6789d);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("12345.6789 °C");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults()
        {
            // Arrange.
            var target = new Temperature(12345.6789d);
            var input = target.ToString();

            // Act.
            var resultTwo = Temperature.TryParse(input, out var resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }
    }
}