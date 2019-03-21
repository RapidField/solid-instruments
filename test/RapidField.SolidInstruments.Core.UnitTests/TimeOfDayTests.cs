// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        public void BeginningOfThisHour_ShouldReturnValidResult()
        {
            // Arrange.
            var target = TimeOfDay.NowLocal;

            // Act.
            var result = target.BeginningOfThisHour();

            // Assert.
            result.Should().NotBeNull();
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisMinute_ShouldReturnValidResult()
        {
            // Arrange.
            var target = TimeOfDay.NowLocal;

            // Act.
            var result = target.BeginningOfThisMinute();

            // Assert.
            result.Should().NotBeNull();
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisSecond_ShouldReturnValidResult()
        {
            // Arrange.
            var target = TimeOfDay.NowLocal;

            // Act.
            var result = target.BeginningOfThisSecond();

            // Assert.
            result.Should().NotBeNull();
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(target.Second);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new TimeOfDay(TimeZoneInfo.Utc, 4, 30, 0, 0);
            var targetTwo = new TimeOfDay(TimeZoneInfo.Utc, 4, 30);
            var targetThree = new TimeOfDay(TimeZoneInfo.Utc, 4, 30, 0, 1);
            var targetFour = new TimeOfDay(TimeZoneInfo.Utc, 4, 30, 0, 1);

            // Act.
            var resultOne = targetOne.CompareTo(targetTwo) == 0;
            var resultTwo = targetTwo.CompareTo(targetThree) == -1;
            var resultThree = targetTwo < targetOne;
            var resultFour = targetThree > targetOne;
            var resultFive = targetFour != targetThree;
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
        public void EqualityComparer_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new TimeOfDay(TimeZoneInfo.Local, 4, 30, 0, 0);
            var targetTwo = new TimeOfDay(TimeZoneInfo.Local, 4, 30);
            var targetThree = new TimeOfDay(TimeZoneInfo.Local, 4, 30, 0, 1);

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
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var targets = new[]
            {
                new TimeOfDay(TimeZoneInfo.GetSystemTimeZones().First(), 0, 0, 0, 0),
                new TimeOfDay(TimeZoneInfo.GetSystemTimeZones().First(), 12, 0, 0, 0),
                new TimeOfDay(TimeZoneInfo.Utc, 0, 0, 0, 1),
                new TimeOfDay(TimeZoneInfo.Utc, 12, 0, 0, 1),
                new TimeOfDay(TimeZoneInfo.GetSystemTimeZones().Last(), 11, 59, 59, 999)
            };

            // Act.
            var results = targets.Select(target => target.GetHashCode());

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void NowLocal_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.Now;

            // Act.
            var result = TimeOfDay.NowLocal;

            // Assert.
            result.Zone.Should().Be(TimeZoneInfo.Local);

            if (target.Second < 57)
            {
                // Assert.
                result.Hour.Should().Be(target.Hour);
                result.Minute.Should().Be(target.Minute);
            }
        }

        [TestMethod]
        public void NowUtc_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = TimeOfDay.NowUtc;

            // Assert.
            result.Zone.Should().Be(TimeZoneInfo.Utc);

            if (target.Second < 57)
            {
                // Assert.
                result.Hour.Should().Be(target.Hour);
                result.Minute.Should().Be(target.Minute);
            }
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new TimeOfDay(TimeZoneInfo.Utc, 15, 32, 56, 490);

            // Act.
            var result = TimeOfDay.Parse(target.ToString());

            // Assert.
            result.Should().Be(target);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingBinaryFormat()
        {
            // Arrange.
            var format = SerializationFormat.Binary;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.Json;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.Xml;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new TimeOfDay(TimeZoneInfo.Utc, 4, 42, 3, 75);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(24);
        }

        [TestMethod]
        public void ToString_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new TimeOfDay(TimeZoneInfo.Utc, 4, 42, 3, 75);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().Be("4:42:03.075 AM UTC");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults_ForValidTimeOfDayString()
        {
            // Arrange.
            var target = new TimeOfDay(TimeZoneInfo.Utc, 15, 32, 56, 490);
            var resultOne = (TimeOfDay)null;

            // Act.
            var resultTwo = TimeOfDay.TryParse(target.ToString(), out resultOne);

            // Assert.
            resultOne.Should().Be(target);
            resultTwo.Should().BeTrue();
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            // Arrange. Arrange.
            var target = new TimeOfDay(TimeZoneInfo.Utc, 15, 32, 56, 490);
            var serializer = new DynamicSerializer<TimeOfDay>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().Be(target);
        }
    }
}
