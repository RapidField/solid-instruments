// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.Cryptography.UnitTests
{
    [TestClass]
    public class SecureMemoryTests
    {
        [TestMethod]
        public void Constructor_ShouldRaiseArgumentOutOfRangeException_ForNegativePlaintextLengthArgument()
        {
            // Arrange.
            var length = -1;

            // Act.
            var action = new Action(() =>
            {
                using (var target = new SecureMemory(length))
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
                using (var target = new SecureMemory(length))
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
            var memoryReference = (Byte[])null;
            var memorySubsegment = new Byte[3];

            using (var target = new SecureMemory(length))
            {
                // Act.
                target.Access(memory =>
                {
                    // Assert.
                    memory.Length.Should().Be(length);
                    memory.Should().OnlyContain(value => value == 0x00);

                    for (var i = 0; i < length; i++)
                    {
                        memory[i] = (Byte)i;
                    }

                    // Arrange.
                    memoryReference = memory;
                    Array.Copy(memory, 1, memorySubsegment, 0, 3);
                });

                // Act.
                target.Access(memory =>
                {
                    // Assert.
                    memory.Length.Should().Be(length);
                    memory[0].Should().Be(0x00);
                    memory[4].Should().Be(0x04);
                    memory[8].Should().Be(0x08);
                });

                // Act.
                target.AccessAsync(memory =>
                {
                    // Assert.
                    memory.Length.Should().Be(length);
                    memory[0].Should().Be(0x00);
                    memory[4].Should().Be(0x04);
                    memory[8].Should().Be(0x08);
                }).Wait();
            }

            // Assert.
            memoryReference.Length.Should().Be(length);
            memoryReference.Should().OnlyContain(value => value == 0x00);
            memorySubsegment[0].Should().Be(0x01);
            memorySubsegment[1].Should().Be(0x02);
            memorySubsegment[2].Should().Be(0x03);
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
            using (var array = SecureMemory.GenerateHardenedRandomBytes(Convert.ToInt32(length)))
            {
                array.Access(memory =>
                {
                    target = new RandomByteArrayProfile(memory);
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
        public void RegeneratePrivateKey_ShouldProduceDesiredResults()
        {
            // Arrange.
            var length = 32;
            var referenceByte = (Byte)0x3f;
            var referenceBytePosition = 21;

            using (var target = new SecureMemory(length))
            {
                // Arrange.
                using (var privateKeyOne = target.DerivePrivateKey())
                {
                    target.Access(memory =>
                    {
                        // Arrange.
                        memory[referenceBytePosition] = referenceByte;
                    });

                    // Act.
                    target.RegeneratePrivateKey();

                    using (var privateKeyTwo = target.DerivePrivateKey())
                    {
                        target.Access(memory =>
                        {
                            // Assert.
                            memory[referenceBytePosition].Should().Be(referenceByte);
                            memory[referenceBytePosition - 1].Should().Be(0x00);
                            memory[referenceBytePosition + 1].Should().Be(0x00);
                        });

                        // Act.
                        target.RegeneratePrivateKey();

                        using (var privateKeyThree = target.DerivePrivateKey())
                        {
                            target.Access(memory =>
                            {
                                // Assert.
                                memory[referenceBytePosition].Should().Be(referenceByte);
                                memory[referenceBytePosition - 1].Should().Be(0x00);
                                memory[referenceBytePosition + 1].Should().Be(0x00);
                            });

                            // Assert.
                            privateKeyOne.Should().NotBeEquivalentTo(privateKeyTwo);
                            privateKeyOne.Should().NotBeEquivalentTo(privateKeyThree);
                            privateKeyTwo.Should().NotBeEquivalentTo(privateKeyThree);
                        }
                    }
                }
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
                using (var target = new SecureMemory(length))
                {
                    continue;
                }
            }

            finalAllocation = Convert.ToDouble(GC.GetTotalMemory(true));
            allocationRange = (finalAllocation - initialAllocation);

            // Assert.
            allocationRange.Should().BeLessThan(length * iterationCount * (1d - confidence));
        }
    }
}