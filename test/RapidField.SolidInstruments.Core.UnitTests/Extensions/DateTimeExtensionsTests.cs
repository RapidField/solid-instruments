// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void BeginningOfThisDay_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisDay();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(0);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisHour_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisHour();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisMillisecond_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisMillisecond();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(target.Second);
            result.Millisecond.Should().Be(target.Millisecond);
        }

        [TestMethod]
        public void BeginningOfThisMinute_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisMinute();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisMonth_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisMonth();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(1);
            result.Hour.Should().Be(0);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisSecond_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisSecond();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(target.Second);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void BeginningOfThisYear_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.BeginningOfThisYear();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(1);
            result.Day.Should().Be(1);
            result.Hour.Should().Be(0);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void CalendarQuarter_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new DateTime(2000, 7, 1);

            // Act.
            var result = target.CalendarQuarter();

            // Assert.
            result.Should().Be(3);
        }

        [TestMethod]
        public void DayOfWeekMonthlyOrdinal_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new DateTime(2014, 7, 7, 12, 0, 0);
            var targetTwo = new DateTime(2014, 7, 8, 12, 0, 0);
            var targetThree = new DateTime(2014, 7, 16, 12, 0, 0);
            var targetFour = new DateTime(2014, 7, 24, 12, 0, 0);
            var targetFive = new DateTime(2014, 7, 25, 12, 0, 0);
            var targetSix = new DateTime(2014, 7, 31, 12, 0, 0);

            // Act.
            var resultOne = targetOne.DayOfWeekOrdinal();
            var resultTwo = targetTwo.DayOfWeekOrdinal();
            var resultThree = targetThree.DayOfWeekOrdinal();
            var resultFour = targetFour.DayOfWeekOrdinal();
            var resultFive = targetFive.DayOfWeekOrdinal();
            var resultSix = targetSix.DayOfWeekOrdinal();

            // Assert.
            resultOne.Should().Be(DayOfWeekMonthlyOrdinal.First);
            resultTwo.Should().Be(DayOfWeekMonthlyOrdinal.Second);
            resultThree.Should().Be(DayOfWeekMonthlyOrdinal.Third);
            resultFour.Should().Be(DayOfWeekMonthlyOrdinal.Fourth);
            resultFive.Should().Be(DayOfWeekMonthlyOrdinal.Last);
            resultSix.Should().Be(DayOfWeekMonthlyOrdinal.Last);
        }

        [TestMethod]
        public void IsLastDayOfMonth_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new DateTime(2000, 1, 1);
            var targetTwo = new DateTime(2000, 1, 31);
            var targetThree = new DateTime(2000, 2, 28);
            var targetFour = new DateTime(2001, 2, 28);
            var targetFive = new DateTime(2000, 2, 29);

            // Act.
            var resultOne = targetOne.IsLastDayOfMonth();
            var resultTwo = targetTwo.IsLastDayOfMonth();
            var resultThree = targetThree.IsLastDayOfMonth();
            var resultFour = targetFour.IsLastDayOfMonth();
            var resultFive = targetFive.IsLastDayOfMonth();

            // Assert.
            resultOne.Should().BeFalse();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
            resultFour.Should().BeTrue();
            resultFive.Should().BeTrue();
        }

        [TestMethod]
        public void MidpointOfThisDay_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.MidpointOfThisDay();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(12);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void MidpointOfThisHour_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.MidpointOfThisHour();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(30);
            result.Second.Should().Be(0);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void MidpointOfThisMillisecond_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.MidpointOfThisMillisecond();

            // Assert.
            result.Should().Be(target.BeginningOfThisMillisecond().AddTicks(5000));
        }

        [TestMethod]
        public void MidpointOfThisMinute_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.MidpointOfThisMinute();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(30);
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        public void MidpointOfThisMonth_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new DateTime(1999, 2, 1);
            var targetTwo = new DateTime(2000, 2, 1);
            var targetThree = new DateTime(2000, 4, 1);
            var targetFour = new DateTime(2000, 1, 1);
            var rangeOne = new DateTimeRange(targetOne, targetOne.AddMonths(1));
            var rangeTwo = new DateTimeRange(targetTwo, targetTwo.AddMonths(1));
            var rangeThree = new DateTimeRange(targetThree, targetThree.AddMonths(1));
            var rangeFour = new DateTimeRange(targetFour, targetFour.AddMonths(1));

            // Act.
            var resultOne = targetOne.MidpointOfThisMonth();
            var resultTwo = targetTwo.MidpointOfThisMonth();
            var resultThree = targetThree.MidpointOfThisMonth();
            var resultFour = targetFour.MidpointOfThisMonth();

            // Assert.
            resultOne.Should().Be(rangeOne.Midpoint);
            resultTwo.Should().Be(rangeTwo.Midpoint);
            resultThree.Should().Be(rangeThree.Midpoint);
            resultFour.Should().Be(rangeFour.Midpoint);
        }

        [TestMethod]
        public void MidpointOfThisSecond_ShouldReturnValidResult()
        {
            // Arrange.
            var target = DateTime.UtcNow;

            // Act.
            var result = target.MidpointOfThisSecond();

            // Assert.
            result.Year.Should().Be(target.Year);
            result.Month.Should().Be(target.Month);
            result.Day.Should().Be(target.Day);
            result.Hour.Should().Be(target.Hour);
            result.Minute.Should().Be(target.Minute);
            result.Second.Should().Be(target.Second);
            result.Millisecond.Should().Be(500);
        }

        [TestMethod]
        public void MidpointOfThisYear_ShouldReturnValidResult()
        {
            // Arrange.
            var targetOne = new DateTime(2000, 1, 1);
            var targetTwo = new DateTime(1999, 1, 1);
            var rangeOne = new DateTimeRange(targetOne, targetOne.AddYears(1));
            var rangeTwo = new DateTimeRange(targetTwo, targetTwo.AddYears(1));

            // Act.
            var resultOne = targetOne.MidpointOfThisYear();
            var resultTwo = targetTwo.MidpointOfThisYear();

            // Assert.
            resultOne.Should().Be(rangeOne.Midpoint);
            resultTwo.Should().Be(rangeTwo.Midpoint);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_WhenKindIsEncoded()
        {
            // Arrange.
            var target = DateTime.UtcNow;
            var derivativeOne = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, target.Millisecond, DateTimeKind.Local);
            var derivativeTwo = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, target.Millisecond, DateTimeKind.Unspecified);
            var encodeKind = true;
            var resultsAreEqual = true;

            // Act.
            var resultOne = derivativeOne.ToByteArray(encodeKind);
            var resultTwo = derivativeTwo.ToByteArray(encodeKind);

            // Assert.
            resultOne.Should().NotBeNull();
            resultTwo.Should().NotBeNull();
            resultOne.Length.Should().Be(8);
            resultTwo.Length.Should().Be(8);

            for (var i = 0; i < 8; i++)
            {
                if (resultOne[i] != resultTwo[i])
                {
                    resultsAreEqual = false;
                    break;
                }
            }

            // Assert.
            resultsAreEqual.Should().BeFalse("because the kind component is encoded");
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult_WhenKindIsNotEncoded()
        {
            // Arrange.
            var target = DateTime.UtcNow;
            var derivativeOne = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, target.Millisecond, DateTimeKind.Local);
            var derivativeTwo = new DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, target.Second, target.Millisecond, DateTimeKind.Unspecified);
            var encodeKind = false;
            var resultsAreEqual = true;

            // Act.
            var resultOne = derivativeOne.ToByteArray(encodeKind);
            var resultTwo = derivativeTwo.ToByteArray(encodeKind);

            // Assert.
            resultOne.Should().NotBeNull();
            resultTwo.Should().NotBeNull();
            resultOne.Length.Should().Be(8);
            resultTwo.Length.Should().Be(8);

            for (var i = 0; i < 8; i++)
            {
                if (resultOne[i] != resultTwo[i])
                {
                    resultsAreEqual = false;
                    break;
                }
            }

            // Assert.
            resultsAreEqual.Should().BeTrue("because the kind component is not encoded");
        }

        [TestMethod]
        public void ToFullDetailString_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new DateTime(2187, 11, 6, 16, 9, 59, 507, DateTimeKind.Unspecified);

            // Act.
            var result = target.ToFullDetailString();

            // Assert.
            result.Should().Be("Tue, 06 Nov 2187 04:09:59.5070000 PM");
        }

        [TestMethod]
        public void ToSerializedString_ShouldReturnValidResult()
        {
            // Arrange.
            var target = new DateTime(2187, 11, 6, 16, 9, 59, 507, DateTimeKind.Utc);

            // Act.
            var result = target.ToSerializedString();

            // Assert.
            result.Should().Be("2187-11-06T16:09:59.5070000Z");
        }

        [TestMethod]
        public void ToTimeOfDay_SHouldReturnValidResult_ForLocalKind()
        {
            // Arrange.
            var year = 2029;
            var month = 3;
            var day = 20;
            var hour = 15;
            var minute = 49;
            var second = 42;
            var millisecond = 867;
            var kind = DateTimeKind.Local;
            var target = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            // Act.
            var result = target.ToTimeOfDay();

            // Assert.
            result.Should().NotBeNull();
            result.Hour.Should().Be(hour);
            result.Minute.Should().Be(minute);
            result.Second.Should().Be(second);
            result.Millisecond.Should().Be(millisecond);
            result.Zone.Should().Be(TimeZoneInfo.Local);
        }

        [TestMethod]
        public void ToTimeOfDay_SHouldReturnValidResult_ForUtcKind()
        {
            // Arrange.
            var year = 2029;
            var month = 3;
            var day = 20;
            var hour = 15;
            var minute = 49;
            var second = 42;
            var millisecond = 867;
            var kind = DateTimeKind.Utc;
            var target = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            // Act.
            var result = target.ToTimeOfDay();

            // Assert.
            result.Should().NotBeNull();
            result.Hour.Should().Be(hour);
            result.Minute.Should().Be(minute);
            result.Second.Should().Be(second);
            result.Millisecond.Should().Be(millisecond);
            result.Zone.Should().Be(TimeZoneInfo.Utc);
        }
    }
}