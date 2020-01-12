// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Serialization;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class DateTimeRangeTests
    {
        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dateTimeOne = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            var dateTimeTwo = new DateTime(2001, 1, 1, 0, 0, 0, 1, DateTimeKind.Local);
            var dateTimeThree = new DateTime(2001, 1, 1, 0, 0, 0, 2, DateTimeKind.Local);
            var dateTimeFour = new DateTime(2001, 1, 1, 0, 0, 2, 3, DateTimeKind.Local);
            var dateTimeFive = new DateTime(2001, 1, 1, 0, 0, 2, 2, DateTimeKind.Local);
            var dateTimeSix = new DateTime(2001, 1, 1, 0, 3, 5, 3, DateTimeKind.Local);
            var dateTimeSeven = new DateTime(2001, 1, 1, 0, 2, 2, 2, DateTimeKind.Local);
            var dateTimeEight = new DateTime(2001, 1, 1, 4, 5, 3, 3, DateTimeKind.Local);
            var dateTimeNine = new DateTime(2001, 1, 1, 2, 2, 2, 2, DateTimeKind.Local);
            var dateTimeTen = new DateTime(2001, 1, 6, 5, 3, 3, 3, DateTimeKind.Local);
            var dateTimeEleven = new DateTime(2001, 1, 2, 2, 2, 2, 2, DateTimeKind.Local);
            var dateTimeTwelve = new DateTime(2001, 7, 5, 3, 3, 3, 3, DateTimeKind.Local);
            var dateTimeThirteen = new DateTime(2001, 2, 2, 2, 2, 2, 2, DateTimeKind.Local);
            var dateTimeFourteen = new DateTime(2008, 5, 3, 3, 3, 3, 3, DateTimeKind.Local);
            var targetOne = new DateTimeRange(dateTimeOne, dateTimeTwo, DateTimeRangeGranularity.AccurateToTheMillisecond);
            var targetTwo = new DateTimeRange(dateTimeThree, dateTimeFour, DateTimeRangeGranularity.AccurateToTheSecond);
            var targetThree = new DateTimeRange(dateTimeFive, dateTimeSix, DateTimeRangeGranularity.AccurateToTheMinute);
            var targetFour = new DateTimeRange(dateTimeSeven, dateTimeEight, DateTimeRangeGranularity.AccurateToTheHour);
            var targetFive = new DateTimeRange(dateTimeNine, dateTimeTen, DateTimeRangeGranularity.AccurateToTheDay);
            var targetSix = new DateTimeRange(dateTimeEleven, dateTimeTwelve, DateTimeRangeGranularity.AccurateToTheMonth);
            var targetSeven = new DateTimeRange(dateTimeThirteen, dateTimeFourteen, DateTimeRangeGranularity.AccurateToTheYear);

            // Act.
            var resultOne = targetOne.LengthInMilliseconds;
            var resultTwo = targetTwo.LengthInMilliseconds;
            var resultThree = targetThree.LengthInSeconds;
            var resultFour = targetFour.LengthInMinutes;
            var resultFive = targetFive.LengthInHours;
            var resultSix = targetSix.LengthInDays;
            var resultSeven = targetSeven.LengthInDays;

            // Assert.
            resultOne.Should().Be(1, "because the first range is 1 millisecond exactly");
            resultTwo.Should().Be(2000, "because there are 2000 milliseconds in two seconds");
            resultThree.Should().Be(180, "because there are 180 seconds in three minutes");
            resultFour.Should().Be(240, "because there are 240 minutes in four days");
            resultFive.Should().Be(120, "because there are 120 hours in five days");
            resultSix.Should().Be(181, "because there are 181 days between 15-Jan-2001 and 15-Jul-2001");
            resultSeven.Should().Be(2556, "because there are 2556 days between 2-Jul-2001 and 1-Jul-2008");
        }

        [TestMethod]
        public void Contains_ShouldReturnValidResult_ForRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var tomorrow = start.AddDays(1);
            var nextWeek = start.AddDays(7);
            var nextYear = start.AddYears(1);
            var nextCentury = start.AddYears(100);
            var targetOne = new DateTimeRange(start, nextYear);
            var targetTwo = new DateTimeRange(start, tomorrow);
            var targetThree = new DateTimeRange(nextWeek, nextYear);
            var targetFour = new DateTimeRange(tomorrow, nextWeek);
            var targetFive = new DateTimeRange(nextWeek, nextCentury);

            // Act.
            var resultOne = targetOne.Contains(targetOne);
            var resultTwo = targetOne.Contains(targetTwo);
            var resultThree = targetOne.Contains(targetThree);
            var resultFour = targetOne.Contains(targetFour);
            var resultFive = targetOne.Contains(targetFive);

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeTrue();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
        }

        [TestMethod]
        public void Contains_ShouldReturnValidResult_ForSinglePoint()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var tomorrow = start.AddDays(1);
            var nextWeek = start.AddDays(7);
            var nextYear = start.AddYears(1);
            var nextCentury = start.AddYears(100);
            var target = new DateTimeRange(tomorrow, nextYear);

            // Act.
            var resultOne = target.Contains(start);
            var resultTwo = target.Contains(tomorrow);
            var resultThree = target.Contains(nextWeek);
            var resultFour = target.Contains(nextYear);
            var resultFive = target.Contains(nextCentury);

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeTrue();
            resultFour.Should().BeTrue();
            resultFive.Should().BeFalse();
        }

        [TestMethod]
        public void EqualityComparer_ShouldReturnValidResult()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var tomorrow = start.AddDays(1);
            var nextWeek = start.AddDays(7);
            var targetOne = new DateTimeRange(start, tomorrow);
            var targetTwo = new DateTimeRange(start, tomorrow);
            var targetThree = new DateTimeRange(start, nextWeek);

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
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForComplexRange()
        {
            // Arrange.
            var start = new DateTime(1997, 11, 6, 14, 7, 46, 482, DateTimeKind.Local);
            var end = new DateTime(2024, 2, 29, 17, 41, 10, 193, DateTimeKind.Local);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(target.Midpoint).Should().BeTrue();
            target.LengthInYears.Should().Be(26);
            target.LengthInMonths.Should().Be(315);
            target.LengthInWeeks.Should().Be(1373);
            target.LengthInDays.Should().Be(9611);
            target.LengthInHours.Should().Be(230667);
            target.LengthInMinutes.Should().Be(13840053);
            target.LengthInSeconds.Should().Be(830403203);
            target.LengthInMilliseconds.Should().Be(830403203711);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForOneDayRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var end = start.AddDays(1);
            var midpoint = start.AddHours(12);
            var outlier = end.AddDays(3);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(midpoint).Should().BeTrue();
            target.Contains(outlier).Should().BeFalse();
            target.LengthInYears.Should().Be(0);
            target.LengthInMonths.Should().Be(0);
            target.LengthInWeeks.Should().Be(0);
            target.LengthInDays.Should().Be(1);
            target.LengthInHours.Should().Be(24);
            target.LengthInMinutes.Should().Be(1440);
            target.LengthInSeconds.Should().Be(86400);
            target.LengthInMilliseconds.Should().Be(86400000);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForOneHourRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var end = start.AddHours(1);
            var midpoint = start.AddMinutes(30);
            var outlier = end.AddHours(3);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(midpoint).Should().BeTrue();
            target.Contains(outlier).Should().BeFalse();
            target.LengthInYears.Should().Be(0);
            target.LengthInMonths.Should().Be(0);
            target.LengthInWeeks.Should().Be(0);
            target.LengthInDays.Should().Be(0);
            target.LengthInHours.Should().Be(1);
            target.LengthInMinutes.Should().Be(60);
            target.LengthInSeconds.Should().Be(3600);
            target.LengthInMilliseconds.Should().Be(3600000);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForOneMinuteRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var end = start.AddMinutes(1);
            var midpoint = start.AddSeconds(30);
            var outlier = end.AddMinutes(3);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(midpoint).Should().BeTrue();
            target.Contains(outlier).Should().BeFalse();
            target.LengthInYears.Should().Be(0);
            target.LengthInMonths.Should().Be(0);
            target.LengthInWeeks.Should().Be(0);
            target.LengthInDays.Should().Be(0);
            target.LengthInHours.Should().Be(0);
            target.LengthInMinutes.Should().Be(1);
            target.LengthInSeconds.Should().Be(60);
            target.LengthInMilliseconds.Should().Be(60000);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForOneWeekRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var end = start.AddDays(7);
            var midpoint = start.AddHours(84);
            var outlier = end.AddDays(30);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(midpoint).Should().BeTrue();
            target.Contains(outlier).Should().BeFalse();
            target.LengthInYears.Should().Be(0);
            target.LengthInMonths.Should().Be(0);
            target.LengthInWeeks.Should().Be(1);
            target.LengthInDays.Should().Be(7);
            target.LengthInHours.Should().Be(168);
            target.LengthInMinutes.Should().Be(10080);
            target.LengthInSeconds.Should().Be(604800);
            target.LengthInMilliseconds.Should().Be(604800000);
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForOneYearRange()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var end = start.AddYears(1);
            var midpoint = start.AddMonths(6);
            var outlier = end.AddYears(3);

            // Act.
            var target = new DateTimeRange(start, end);

            // Assert.
            target.Start.Should().Be(start);
            target.End.Should().Be(end);
            target.Contains(start).Should().BeTrue();
            target.Contains(end).Should().BeTrue();
            target.Contains(midpoint).Should().BeTrue();
            target.Contains(outlier).Should().BeFalse();
            target.LengthInYears.Should().Be(1);
            target.LengthInMonths.Should().Be(12);
            target.LengthInWeeks.Should().Be(52);
            target.LengthInDays.Should().Be(365);
            target.LengthInHours.Should().Be(8760);
            target.LengthInMinutes.Should().Be(525600);
            target.LengthInSeconds.Should().Be(31536000);
            target.LengthInMilliseconds.Should().Be(31536000000);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnUniqueHashCode()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var oneMillisecondLater = start.AddMilliseconds(1);
            var twoMillisecondsLater = start.AddMilliseconds(2);
            var tomorrow = start.AddDays(1);
            var nextWeek = start.AddDays(7);
            var nextYear = start.AddYears(1);
            var minValue = DateTime.MinValue;
            var maxValue = DateTime.MaxValue;
            var oneMillisecondRange = new DateTimeRange(start, oneMillisecondLater);
            var twoMillisecondRange = new DateTimeRange(start, twoMillisecondsLater);
            var oneDayRange = new DateTimeRange(start, tomorrow);
            var oneWeekRange = new DateTimeRange(start, nextWeek);
            var oneYearRange = new DateTimeRange(start, nextYear);
            var maxRange = new DateTimeRange(minValue, maxValue);

            // Act.
            var results = new Int32[]
            {
                oneMillisecondRange.GetHashCode(),
                twoMillisecondRange.GetHashCode(),
                oneDayRange.GetHashCode(),
                oneWeekRange.GetHashCode(),
                oneYearRange.GetHashCode(),
                maxRange.GetHashCode()
            };

            // Assert.
            results.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void Midpoint_ShouldReturnValidResult()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var tomorrow = start.AddHours(24);
            var twoDaysFromNow = start.AddHours(48);
            var target = new DateTimeRange(start, twoDaysFromNow);

            // Act.
            var result = target.Midpoint;

            // Assert.
            result.Should().Be(tomorrow);
        }

        [TestMethod]
        public void Overlaps_ShouldReturnValidResult()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var tomorrow = start.AddDays(1);
            var nextWeek = start.AddDays(7);
            var nextMonth = start.AddMonths(1);
            var nextYear = start.AddYears(1);
            var nextCentury = start.AddYears(100);
            var targetOne = new DateTimeRange(start, nextMonth);
            var targetTwo = new DateTimeRange(start, tomorrow);
            var targetThree = new DateTimeRange(nextWeek, nextYear);
            var targetFour = new DateTimeRange(tomorrow, nextWeek);
            var targetFive = new DateTimeRange(nextWeek, nextCentury);
            var targetSix = new DateTimeRange(nextMonth, nextYear);
            var targetSeven = new DateTimeRange(nextMonth, nextMonth);

            // Act.
            var resultOne = targetOne.Overlaps(targetOne);
            var resultTwo = targetOne.Overlaps(targetTwo);
            var resultThree = targetOne.Overlaps(targetThree);
            var resultFour = targetOne.Overlaps(targetFour);
            var resultFive = targetOne.Overlaps(targetFive);
            var resultSix = targetOne.Overlaps(targetSix);
            var resultSeven = targetSeven.Overlaps(targetSeven);
            var resultEight = targetThree.Overlaps(targetSeven);

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeTrue();
            resultFour.Should().BeTrue();
            resultFive.Should().BeTrue();
            resultSix.Should().BeFalse();
            resultSeven.Should().BeFalse();
            resultEight.Should().BeTrue();
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentEmptyException_ForEmptyDateTimeRangeString()
        {
            // Arrange.
            var target = String.Empty;

            // Act.
            var action = new Action(() =>
            {
                var result = DateTimeRange.Parse(target);
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>($"because {nameof(target)} is empty");
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentNullException_ForNullDateTimeRangeString()
        {
            // Arrange.
            var target = (String)null;

            // Act.
            var action = new Action(() =>
            {
                var result = DateTimeRange.Parse(target);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(target)} is null");
        }

        [TestMethod]
        public void Parse_ShouldRaiseArgumentOutOfRangeException_ForOutOfRangeDateTimeRangeString()
        {
            // Arrange.
            var end = TimeStamp.Current;
            var start = end.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} | {granularity}";

            // Act.
            var action = new Action(() =>
            {
                var result = DateTimeRange.Parse(target);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is out of range");
        }

        [TestMethod]
        public void Parse_ShouldRaiseFormatException_ForInvalidFormatDateTimeRangeString()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} ; {granularity}";

            // Act.
            var action = new Action(() =>
            {
                var result = DateTimeRange.Parse(target);
            });

            // Assert.
            action.Should().Throw<FormatException>($"because {nameof(target)} has an invalid format");
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult_ForValidDateTimeRangeString_WithoutSpacing()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()}|{end.ToFullDetailString()}|{granularity}";

            // Act.
            var result = DateTimeRange.Parse(target);

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(new DateTimeRange(start, end, granularity));
        }

        [TestMethod]
        public void Parse_ShouldReturnValidResult_ForValidDateTimeRangeString_WithSpacing()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} | {granularity}";

            // Act.
            var result = DateTimeRange.Parse(target);

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(new DateTimeRange(start, end, granularity));
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
        public void ShouldBeSerializable_UsingCompressedJsonFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedJson;

            // Assert.
            ShouldBeSerializable(format);
        }

        [TestMethod]
        public void ShouldBeSerializable_UsingCompressedXmlFormat()
        {
            // Arrange.
            var format = SerializationFormat.CompressedXml;

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
            var start = new DateTime(2001, 1, 1, 3, 42, 7, 833, DateTimeKind.Utc);
            var end = start.Add(TimeSpan.FromDays(2));
            var granularity = DateTimeRangeGranularity.Exact;
            var target = new DateTimeRange(start, end, granularity);

            // Act.
            var result = target.ToByteArray();

            // Assert.
            result.Should().NotBeNull();
            result.Length.Should().Be(20);
        }

        [TestMethod]
        public void ToString_ShouldReturnValidResult()
        {
            // Arrange.
            var start = new DateTime(2001, 1, 1, 3, 42, 7, 833, DateTimeKind.Utc);
            var end = start.Add(TimeSpan.FromDays(2));
            var granularity = DateTimeRangeGranularity.Exact;
            var target = new DateTimeRange(start, end, granularity);

            // Act.
            var result = target.ToString();

            // Assert.
            result.Should().Be("Mon, 01 Jan 2001 03:42:07.8330000 AM UTC | Wed, 03 Jan 2001 03:42:07.8330000 AM UTC | Exact");
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults_ForValidDateTimeRangeString_WithoutSpacing()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()}|{end.ToFullDetailString()}|{granularity}";
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().Be(new DateTimeRange(start, end, granularity));
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_ShouldProduceDesiredResults_ForValidDateTimeRangeString_WithSpacing()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} | {granularity}";
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().Be(new DateTimeRange(start, end, granularity));
            resultTwo.Should().BeTrue();
        }

        [TestMethod]
        public void TryParse_ShouldReturnFalse_ForEmptyDateTimeRangeString()
        {
            // Arrange.
            var target = String.Empty;
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().BeNull();
            resultTwo.Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_ShouldReturnFalse_ForInvalidFormatDateTimeRangeString()
        {
            // Arrange.
            var start = TimeStamp.Current;
            var end = start.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} ; {granularity}";
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().BeNull();
            resultTwo.Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_ShouldReturnFalse_ForNullDateTimeRangeString()
        {
            // Arrange.
            var target = (String)null;
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().BeNull();
            resultTwo.Should().BeFalse();
        }

        [TestMethod]
        public void TryParse_ShouldReturnFalse_ForOutOfRangeDateTimeRangeString()
        {
            // Arrange.
            var end = TimeStamp.Current;
            var start = end.AddSeconds(55555);
            var granularity = DateTimeRangeGranularity.Exact;
            var target = $"{start.ToFullDetailString()} | {end.ToFullDetailString()} | {granularity}";
            var resultOne = (DateTimeRange)null;

            // Act.
            var resultTwo = DateTimeRange.TryParse(target, out resultOne);

            // Assert.
            resultOne.Should().BeNull();
            resultTwo.Should().BeFalse();
        }

        private static void ShouldBeSerializable(SerializationFormat format)
        {
            // Arrange.
            var start = new DateTime(1997, 11, 6, 14, 7, 46, 482, DateTimeKind.Local);
            var end = new DateTime(2024, 2, 29, 17, 41, 10, 193, DateTimeKind.Local);
            var target = new DateTimeRange(start, end);
            var serializer = new DynamicSerializer<DateTimeRange>(format);

            // Act.
            var serializedTarget = serializer.Serialize(target);
            var deserializedResult = serializer.Deserialize(serializedTarget);

            // Assert.
            deserializedResult.Should().Be(target);
        }
    }
}