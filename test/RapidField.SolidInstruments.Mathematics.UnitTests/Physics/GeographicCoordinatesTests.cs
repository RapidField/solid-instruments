// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Mathematics.Physics;
using System;

namespace RapidField.SolidInstruments.Mathematics.UnitTests.Physics
{
    [TestClass]
    public sealed class GeographicCoordinatesTests
    {
        [TestMethod]
        public void EqualityComparer_ShouldReturnValidResult()
        {
            // Arrange.
            var latitude = 0.0d;
            var longitudeOne = 0.0d;
            var longitudeTwo = 180.0d;
            var targetOne = new GeographicCoordinates(latitude, longitudeOne);
            var targetTwo = new GeographicCoordinates(latitude, longitudeOne);
            var targetThree = new GeographicCoordinates(latitude, longitudeTwo);

            // Assert.
            targetOne.Equals(targetTwo).Should().BeTrue();
            targetTwo.Equals(targetThree).Should().BeFalse();
            (targetTwo == targetOne).Should().BeTrue();
            (targetThree == targetOne).Should().BeFalse();
            (targetOne != targetThree).Should().BeTrue();
            (targetTwo != targetOne).Should().BeFalse();
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            var latitude = 0.0d;
            var longitudeOne = 0.0d;
            var longitudeTwo = 180.0d;
            var longitudeThree = -180.0d;
            var longitudeFour = 18.0d;
            var longitudeFive = -18.0d;
            var targetOne = new GeographicCoordinates(latitude, longitudeOne);
            var targetTwo = new GeographicCoordinates(latitude, longitudeTwo);
            var targetThree = new GeographicCoordinates(latitude, longitudeThree);
            var targetFour = new GeographicCoordinates(latitude, longitudeFour);
            var targetFive = new GeographicCoordinates(latitude, longitudeFive);

            // Act.
            var results = new Int32[]
            {
                targetOne.GetHashCode(),
                targetTwo.GetHashCode(),
                targetThree.GetHashCode(),
                targetFour.GetHashCode(),
                targetFive.GetHashCode()
            };

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentEmptyException_ForEmptyArgument()
        {
            // Arrange.
            var input = String.Empty;

            // Act.
            var action = new Action(() =>
            {
                var result = GeographicCoordinates.Parse(input);
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>();
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentNullException_ForNullArgument()
        {
            // Arrange.
            var input = (String)null;

            // Act.
            var action = new Action(() =>
            {
                var result = GeographicCoordinates.Parse(input);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentOutOfRangeException_ForOutOfRangeArgument()
        {
            // Arrange.
            var input = "42.42,-320.7";

            // Act.
            var action = new Action(() =>
            {
                var result = GeographicCoordinates.Parse(input);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Parse_ShouldRaiseFormatException_ForInvalidFormatArgument()
        {
            // Arrange.
            var input = "42.42 : -32.7";

            // Act.
            var action = new Action(() =>
            {
                var result = GeographicCoordinates.Parse(input);
            });

            // Assert.
            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult_ForValidArgument_WithoutSpacing()
        {
            // Arrange.
            var input = "42.42,-32.7";

            // Act.
            var result = GeographicCoordinates.Parse(input);

            // Assert.
            result.Should().Be(new GeographicCoordinates(42.42d, -32.7d));
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult_ForValidArgument_WithSpacing()
        {
            // Arrange.
            var input = "42.42, -32.7";

            // Act.
            var result = GeographicCoordinates.Parse(input);

            // Assert.
            result.Should().Be(new GeographicCoordinates(42.42d, -32.7d));
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var latitude = 42.05d;
            var longitude = -42.3d;
            var target = new GeographicCoordinates(latitude, longitude);

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
            var latitude = 42.05d;
            var longitude = -42.3d;
            var target = new GeographicCoordinates(latitude, longitude);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().Be("42.05,-42.3");
        }
    }
}