// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Mathematics.Statistics;
using RapidField.SolidInstruments.Mathematics.Statistics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Cryptography.UnitTests
{
    [TestClass]
    public class SecureBufferTests
    {
        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForNegativePlaintextLengthArgument()
        {
            // Arrange.
            var length = -1;

            // Act.
            var action = new Action(() =>
            {
                using (var target = new SecureBuffer(length))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForZeroPlaintextLengthArgument()
        {
            // Arrange.
            var length = 0;

            // Act.
            var action = new Action(() =>
            {
                using (var target = new SecureBuffer(length))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var length = 17;
            var bufferReference = (Byte[])null;
            var bufferSubsegment = new Byte[3];

            using (var target = new SecureBuffer(length))
            {
                // Act.
                target.Access(buffer =>
                {
                    // Assert.
                    buffer.Length.Should().Be(length);
                    buffer.Should().OnlyContain(value => value == 0x00);

                    for (var i = 0; i < length; i++)
                    {
                        buffer[i] = (Byte)i;
                    }

                    // Arrange.
                    bufferReference = buffer;
                    Array.Copy(buffer, 1, bufferSubsegment, 0, 3);
                });

                // Act.
                target.Access(buffer =>
                {
                    // Assert.
                    buffer.Length.Should().Be(length);
                    buffer[0].Should().Be(0x00);
                    buffer[4].Should().Be(0x04);
                    buffer[8].Should().Be(0x08);
                });
            }

            // Assert.
            bufferReference.Length.Should().Be(length);
            bufferReference.Should().OnlyContain(value => value == 0x00);
            bufferSubsegment[0].Should().Be(0x01);
            bufferSubsegment[1].Should().Be(0x02);
            bufferSubsegment[2].Should().Be(0x03);
        }

        [TestMethod]
        public void GenerateHardenedRandomBytes_ShouldYieldHighEntropy()
        {
            // Arrange.
            var valueRange = 256;
            var confidence = 0.223d;
            var curveTolerance = 5m;
            var length = (valueRange * valueRange);
            var target = (RandomByteArrayProfile)null;
            var meanTarget = valueRange;
            var meanLowerBoundary = Convert.ToDecimal(meanTarget - (meanTarget * confidence));
            var meanUpperBoundary = Convert.ToDecimal(meanTarget + (meanTarget * confidence));
            var medianTarget = valueRange;
            var medianLowerBoundary = Convert.ToDecimal(medianTarget - (medianTarget * confidence));
            var medianUpperBoundary = Convert.ToDecimal(medianTarget + (medianTarget * confidence));
            var standardDeviationTarget = Math.Sqrt(valueRange);
            var standardDevisionLowerBoundary = Convert.ToDecimal(standardDeviationTarget - (standardDeviationTarget * confidence));
            var standardDeviationUpperBoundary = Convert.ToDecimal(standardDeviationTarget + (standardDeviationTarget * confidence));
            var valueCountLowerBoundary = Convert.ToInt64(meanTarget - (Convert.ToDecimal(standardDeviationTarget) * curveTolerance));
            var valueCountUpperBoundary = Convert.ToInt64(meanTarget + (Convert.ToDecimal(standardDeviationTarget) * curveTolerance));

            // Act.
            using (var array = SecureBuffer.GenerateHardenedRandomBytes(Convert.ToInt32(length)))
            {
                array.Access(buffer =>
                {
                    target = new RandomByteArrayProfile(buffer);
                });
            }

            // Assert.
            target.ArrayLength.Should().Be(Convert.ToInt64(length));
            target.AverageLengthBetweenStatistics.Mean.Should().BeLessOrEqualTo(meanUpperBoundary);
            target.AverageLengthBetweenStatistics.Mean.Should().BeGreaterOrEqualTo(meanLowerBoundary);
            target.CountStatistics.Mean.Should().BeLessOrEqualTo(meanUpperBoundary);
            target.CountStatistics.Mean.Should().BeGreaterOrEqualTo(meanLowerBoundary);
            target.AverageLengthBetweenStatistics.Median.Should().BeLessOrEqualTo(medianUpperBoundary);
            target.AverageLengthBetweenStatistics.Median.Should().BeGreaterOrEqualTo(medianLowerBoundary);
            target.CountStatistics.Median.Should().BeLessOrEqualTo(medianUpperBoundary);
            target.CountStatistics.Median.Should().BeGreaterOrEqualTo(medianLowerBoundary);
            target.AverageLengthBetweenStatistics.StandardDeviation.Should().BeLessOrEqualTo(standardDeviationUpperBoundary);
            target.AverageLengthBetweenStatistics.StandardDeviation.Should().BeGreaterOrEqualTo(standardDevisionLowerBoundary);
            target.CountStatistics.StandardDeviation.Should().BeLessOrEqualTo(standardDeviationUpperBoundary);
            target.CountStatistics.StandardDeviation.Should().BeGreaterOrEqualTo(standardDevisionLowerBoundary);

            foreach (var record in target.Records)
            {
                // Assert.
                record.Count.Should().BeLessOrEqualTo(valueCountUpperBoundary);
                record.Count.Should().BeGreaterOrEqualTo(valueCountLowerBoundary);
            }
        }

        [TestMethod]
        public void ShouldNotLeakMemory()
        {
            // Arrange.
            var length = 64;
            var iterationCount = 32768;
            var confidence = 0.223d;
            var allocationRange = default(Double);
            var finalAllocation = default(Double);
            var initialAllocation = Convert.ToDouble(GC.GetTotalMemory(true));

            for (var i = 0; i < iterationCount; i++)
            {
                // Act.
                using (var target = new SecureBuffer(length))
                {
                    continue;
                }
            }

            finalAllocation = Convert.ToDouble(GC.GetTotalMemory(true));
            allocationRange = (finalAllocation - initialAllocation);

            // Assert.
            allocationRange.Should().BeLessThan(length * iterationCount * (1d - confidence));
        }

        /// <summary>
        /// Represents analytic information about a random byte array.
        /// </summary>
        internal class RandomByteArrayProfile
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RandomByteArrayProfile" /> class.
            /// </summary>
            /// <param name="array">
            /// The random byte array to evaluate.
            /// </param>
            public RandomByteArrayProfile(Byte[] array)
            {
                ArrayLength = 0;
                Records = new List<ByteRecord>();

                for (var i = 0; i < 256; i++)
                {
                    Records.Add(new ByteRecord((Byte)i));
                }

                foreach (var value in array)
                {
                    RecordValue(value);
                }

                AverageLengthBetweenStatistics = Records.Select(record => record.AverageLengthBetween).ComputeDescriptives();
                CountStatistics = Records.Select(record => record.Count).ComputeDescriptives();
            }

            /// <summary>
            /// Records information about the current value in the associated array.
            /// </summary>
            /// <param name="value">
            /// The current byte value.
            /// </param>
            private void RecordValue(Byte value)
            {
                ArrayLength++;

                foreach (var record in Records)
                {
                    var valueHasBeenRecorded = (record.Count > 0);

                    if (record.Value == value)
                    {
                        record.Count++;

                        if (valueHasBeenRecorded)
                        {
                            record.LengthsBetween.Add(record.LengthFromLastOccurrence);
                            record.LengthFromLastOccurrence = 0;
                        }
                    }
                    else if (valueHasBeenRecorded)
                    {
                        record.LengthFromLastOccurrence++;
                    }
                }
            }

            /// <summary>
            /// Represents descriptive statistics for the average lengths between values in the associated array.
            /// </summary>
            internal readonly DescriptiveStatistics AverageLengthBetweenStatistics;

            /// <summary>
            /// Represents descriptive statistics for the counts of values in the associated array.
            /// </summary>
            internal readonly DescriptiveStatistics CountStatistics;

            /// <summary>
            /// Represents information about the full range of bytes in the associated array.
            /// </summary>
            internal readonly IList<ByteRecord> Records;

            /// <summary>
            /// Represents the total length of the associated array.
            /// </summary>
            internal Int64 ArrayLength;

            /// <summary>
            /// Represents information about a byte value within a random byte array.
            /// </summary>
            internal class ByteRecord
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ByteRecord" /> class.
                /// </summary>
                /// <param name="value">
                /// The byte value for the current statistics.
                /// </param>
                public ByteRecord(Byte value)
                {
                    Count = 0;
                    LengthFromLastOccurrence = 0;
                    LengthsBetween = new List<Int32>();
                    Value = value;
                }

                /// <summary>
                /// Gets the average length between occurrences of <see cref="Value" /> in the associated array.
                /// </summary>
                public Double AverageLengthBetween => (LengthsBetween.Any() ? LengthsBetween.Average() : 0d);

                /// <summary>
                /// Gets or sets the frequency of <see cref="Value" /> in the associated array.
                /// </summary>
                public Int64 Count
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets or sets the length from the last occurrence of <see cref="Value" /> at the current index in the associated
                /// array.
                /// </summary>
                public Int32 LengthFromLastOccurrence
                {
                    get;
                    set;
                }

                /// <summary>
                /// Gets the recorded lengths between occurrences of <see cref="Value" /> in the associated array.
                /// </summary>
                public IList<Int32> LengthsBetween
                {
                    get;
                }

                /// <summary>
                /// Represents the byte value for the current statistics.
                /// </summary>
                public Byte Value;
            }
        }
    }
}