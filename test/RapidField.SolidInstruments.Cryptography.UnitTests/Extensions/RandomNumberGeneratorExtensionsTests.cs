// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public sealed class RandomNumberGeneratorExtensionsTests
    {
        [TestMethod]
        public void FillBooleanArray_ShouldProduceDesiredResults()
        {
            // Arrange.
            var arrayLength = 180;
            var array = new Boolean[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillBooleanArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Where(value => value).Count().Should().BeGreaterOrEqualTo(45).And.BeLessOrEqualTo(135);
            }
        }

        [TestMethod]
        public void FillDateTimeArray_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = new DateTime(1000, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var ceiling = new DateTime(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var array = new DateTime[arrayLength];

            // Define expected results.
            var expectedArrayLength = arrayLength;

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDateTimeArray(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Min().Should().BeOnOrAfter(floor);
                array.Max().Should().BeOnOrBefore(ceiling);
            }
        }

        [TestMethod]
        public void FillDateTimeArray_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 270;
            var array = new DateTime[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDateTimeArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillDecimalArray_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -100000m;
            var ceiling = 100000m;
            var array = new Decimal[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDecimalArray(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Average().Should().BeGreaterOrEqualTo(-50000m).And.BeLessOrEqualTo(50000m);
            }
        }

        [TestMethod]
        public void FillDecimalArray_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 270;
            var array = new Decimal[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDecimalArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillDoubleArray_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -100000d;
            var ceiling = 100000d;
            var array = new Double[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDoubleArray(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Average().Should().BeGreaterOrEqualTo(-50000d).And.BeLessOrEqualTo(50000d);
            }
        }

        [TestMethod]
        public void FillDoubleArray_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 270;
            var array = new Double[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillDoubleArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillInt16Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -10000;
            var ceiling = 10000;
            var array = new Int16[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt16Array(array, (Int16)floor, (Int16)ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(-5000).And.BeLessOrEqualTo(5000);
            }
        }

        [TestMethod]
        public void FillInt16Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 30;
            var array = new Int16[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt16Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 4).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillInt32Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -100000;
            var ceiling = 100000;
            var array = new Int32[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt32Array(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(-50000).And.BeLessOrEqualTo(50000);
            }
        }

        [TestMethod]
        public void FillInt32Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 90;
            var array = new Int32[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt32Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 4).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillInt64Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -100000;
            var ceiling = 100000;
            var array = new Int64[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt64Array(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(-50000).And.BeLessOrEqualTo(50000);
            }
        }

        [TestMethod]
        public void FillInt64Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 270;
            var array = new Int64[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillInt64Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillSingleArray_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = -100000f;
            var ceiling = 100000f;
            var array = new Single[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillSingleArray(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Average().Should().BeGreaterOrEqualTo(-50000f).And.BeLessOrEqualTo(50000f);
            }
        }

        [TestMethod]
        public void FillSingleArray_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 90;
            var array = new Single[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillSingleArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 4).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillStringArray_ShouldProduceDesiredResults()
        {
            // Arrange.
            var arrayLength = 45;
            var characterLengthFloor = 100;
            var characterLengthCeiling = 300;
            var permitNonLatin = false;
            var permitLowercaseAlphabetic = true;
            var permitUppercaseAlphabetic = true;
            var permitNumeric = true;
            var permitSymbolic = false;
            var permitWhitespace = false;
            var permitControl = false;
            var array = new String[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillStringArray(array, characterLengthFloor, characterLengthCeiling, permitNonLatin, permitLowercaseAlphabetic, permitUppercaseAlphabetic, permitNumeric, permitSymbolic, permitWhitespace, permitControl);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => value.Length).Average().Should().BeGreaterOrEqualTo(125d).And.BeLessOrEqualTo(275d);

                foreach (var randomString in array)
                {
                    // Assert.
                    randomString.Length.Should().BeGreaterOrEqualTo(characterLengthFloor).And.BeLessOrEqualTo(characterLengthCeiling);
                    randomString.Where(character => character.IsBasicLatin()).Count().Should().Be(randomString.Length);
                    randomString.Where(character => character.IsLowercaseAlphabetic()).Any().Should().Be(permitLowercaseAlphabetic);
                    randomString.Where(character => character.IsUppercaseAlphabetic()).Any().Should().Be(permitUppercaseAlphabetic);
                    randomString.Where(character => character.IsNumeric()).Any().Should().Be(permitNumeric);
                    randomString.Where(character => character.IsSymbolic()).Any().Should().Be(permitSymbolic);
                    randomString.Where(character => character.IsWhiteSpaceCharacter()).Any().Should().Be(permitWhitespace);
                    randomString.Where(character => character.IsControlCharacter()).Any().Should().Be(permitControl);
                }
            }
        }

        [TestMethod]
        public void FillTimeSpanArray_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = new TimeSpan(1000, 0, 0, 0, 0);
            var ceiling = new TimeSpan(3000, 0, 0, 0, 0);
            var array = new TimeSpan[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillTimeSpanArray(array, floor, ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                new TimeSpan(Convert.ToInt64(array.Select(value => value.Ticks).Average())).Should().BeGreaterOrEqualTo(new TimeSpan(1500, 0, 0, 0, 0)).And.BeLessOrEqualTo(new TimeSpan(2500, 0, 0, 0, 0));
            }
        }

        [TestMethod]
        public void FillTimeSpanArray_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var array = new TimeSpan[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillTimeSpanArray(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillUInt16Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = 10000;
            var ceiling = 30000;
            var array = new UInt16[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt16Array(array, (UInt16)floor, (UInt16)ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(15000).And.BeLessOrEqualTo(25000);
            }
        }

        [TestMethod]
        public void FillUInt16Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 30;
            var array = new UInt16[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt16Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillUInt32Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = 100000;
            var ceiling = 300000;
            var array = new UInt32[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt32Array(array, (UInt32)floor, (UInt32)ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(150000).And.BeLessOrEqualTo(250000);
            }
        }

        [TestMethod]
        public void FillUInt32Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 90;
            var array = new UInt32[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt32Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 4).Should().BeFalse();
            }
        }

        [TestMethod]
        public void FillUInt64Array_ShouldProduceDesiredResults_ForBoundedInvocation()
        {
            // Arrange.
            var arrayLength = 180;
            var floor = 100000;
            var ceiling = 300000;
            var array = new UInt64[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt64Array(array, (UInt64)floor, (UInt64)ceiling);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.Select(value => Convert.ToDouble(value)).Average().Should().BeGreaterOrEqualTo(150000).And.BeLessOrEqualTo(250000);
            }
        }

        [TestMethod]
        public void FillUInt64Array_ShouldProduceDesiredResults_ForUnboundedInvocation()
        {
            // Arrange.
            var arrayLength = 270;
            var array = new UInt64[arrayLength];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Act.
                randomNumberGenerator.FillUInt64Array(array);

                // Assert.
                array.Length.Should().Be(arrayLength);
                array.GroupBy(value => value).Any(group => group.Count() > 2).Should().BeFalse();
            }
        }
    }
}