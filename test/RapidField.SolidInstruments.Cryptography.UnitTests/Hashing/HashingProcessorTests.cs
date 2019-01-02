// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using RapidField.SolidInstruments.Serialization;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Hashing
{
    [TestClass]
    public class HashingProcessorTests
    {
        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_Sha256() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_Sha384() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_Sha512() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_Sha256() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_Sha384() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_Sha512() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForSalted_Sha256() => EvaluateHash_ShouldProduceDesiredResults_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForSalted_Sha384() => EvaluateHash_ShouldProduceDesiredResults_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForSalted_Sha512() => EvaluateHash_ShouldProduceDesiredResults_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForUnsalted_Sha256() => EvaluateHash_ShouldProduceDesiredResults_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForUnsalted_Sha384() => EvaluateHash_ShouldProduceDesiredResults_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void EvaluateHash_ShouldProduceDesiredResults_ForUnsalted_Sha512() => EvaluateHash_ShouldProduceDesiredResults_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        private static void CalculateHash_ShouldBeDeterministic(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode) => CalculateHash_ValidateDeterminism(algorithm, saltingMode, true);

        private static void CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification algorithm) => CalculateHash_ShouldBeDeterministic(algorithm, SaltingMode.Unsalted);

        private static void CalculateHash_ShouldNotBeDeterministic(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode) => CalculateHash_ValidateDeterminism(algorithm, saltingMode, false);

        private static void CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification algorithm) => CalculateHash_ShouldNotBeDeterministic(algorithm, SaltingMode.Salted);

        private static void CalculateHash_ValidateDeterminism(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode, Boolean shouldBeDeterministic)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var binarySerializer = new BinaryPassThroughSerializer();
                var target = new HashingProcessor<Byte[]>(randomnessProvider, binarySerializer);
                var plaintextObject = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

                // Act.
                var firstHashValue = target.CalculateHash(plaintextObject, algorithm, saltingMode);

                // Assert.
                firstHashValue.Should().NotBeNullOrEmpty();
                firstHashValue.Count(value => value == 0x00).Should().NotBe(firstHashValue.Length);
                ValidateDigestLength(firstHashValue, algorithm, saltingMode, target.SaltLengthInBytes);

                // Act.
                var secondHashValue = target.CalculateHash(plaintextObject, algorithm, saltingMode);

                // Assert.
                secondHashValue.Should().NotBeNullOrEmpty();
                secondHashValue.Length.Should().Be(firstHashValue.Length);
                secondHashValue.Count(value => value == 0x00).Should().NotBe(secondHashValue.Length);

                if (shouldBeDeterministic)
                {
                    // Assert.
                    firstHashValue.ComputeThirtyTwoBitHash().Should().Be(secondHashValue.ComputeThirtyTwoBitHash());
                }
                else
                {
                    // Assert.
                    firstHashValue.ComputeThirtyTwoBitHash().Should().NotBe(secondHashValue.ComputeThirtyTwoBitHash());
                }
            }
        }

        private static void EvaluateHash_ShouldProduceDesiredResults(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var binarySerializer = new BinaryPassThroughSerializer();
                var target = new HashingProcessor<Byte[]>(randomnessProvider, binarySerializer);
                var plaintextObject = new Byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
                var matchingHashValue = target.CalculateHash(plaintextObject, algorithm, saltingMode);
                var nonMatchingHashValue = matchingHashValue.PerformCircularBitShift(Core.BitShiftDirection.Left, 16);

                // Act.
                var resultOne = target.EvaluateHash(matchingHashValue, plaintextObject, algorithm, saltingMode);
                var resultTwo = target.EvaluateHash(nonMatchingHashValue, plaintextObject, algorithm, saltingMode);

                // Assert.
                resultOne.Should().BeTrue();
                resultTwo.Should().BeFalse();
                ValidateDigestLength(matchingHashValue, algorithm, saltingMode, target.SaltLengthInBytes);
            }
        }

        private static void EvaluateHash_ShouldProduceDesiredResults_ForSaltedHashing(HashingAlgorithmSpecification algorithm) => EvaluateHash_ShouldProduceDesiredResults(algorithm, SaltingMode.Salted);

        private static void EvaluateHash_ShouldProduceDesiredResults_ForUnsaltedHashing(HashingAlgorithmSpecification algorithm) => EvaluateHash_ShouldProduceDesiredResults(algorithm, SaltingMode.Unsalted);

        private static void ValidateDigestLength(Byte[] hashValue, HashingAlgorithmSpecification algorithm, SaltingMode saltingMode, Int32 saltLengthInBytes)
        {
            switch (algorithm)
            {
                case HashingAlgorithmSpecification.Md5:

                    hashValue.Length.Should().Be(16 + (saltingMode == SaltingMode.Salted ? saltLengthInBytes : 0));
                    break;

                case HashingAlgorithmSpecification.ShaTwo256:

                    hashValue.Length.Should().Be(32 + (saltingMode == SaltingMode.Salted ? saltLengthInBytes : 0));
                    break;

                case HashingAlgorithmSpecification.ShaTwo384:

                    hashValue.Length.Should().Be(48 + (saltingMode == SaltingMode.Salted ? saltLengthInBytes : 0));
                    break;

                case HashingAlgorithmSpecification.ShaTwo512:

                    hashValue.Length.Should().Be(64 + (saltingMode == SaltingMode.Salted ? saltLengthInBytes : 0));
                    break;

                default:

                    Assert.Fail();
                    return;
            }
        }
    }
}