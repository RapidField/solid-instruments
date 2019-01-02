// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Hashing
{
    [TestClass]
    public class HashingStringProcessorTests
    {
        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_Md5() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.Md5);

        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_ShaTwo256() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_ShaTwo384() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void CalculateHash_ShouldBeDeterministic_ForUnsalted_ShaTwo512() => CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_Md5() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.Md5);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_ShaTwo256() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo256);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_ShaTwo384() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo384);

        [TestMethod]
        public void CalculateHash_ShouldNotBeDeterministic_ForSalted_ShaTwo512() => CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification.ShaTwo512);

        private static void CalculateHash_ShouldBeDeterministic(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode) => CalculateHash_ValidateDeterminism(algorithm, saltingMode, true);

        private static void CalculateHash_ShouldBeDeterministic_ForUnsaltedHashing(HashingAlgorithmSpecification algorithm) => CalculateHash_ShouldBeDeterministic(algorithm, SaltingMode.Unsalted);

        private static void CalculateHash_ShouldNotBeDeterministic(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode) => CalculateHash_ValidateDeterminism(algorithm, saltingMode, false);

        private static void CalculateHash_ShouldNotBeDeterministic_ForSaltedHashing(HashingAlgorithmSpecification algorithm) => CalculateHash_ShouldNotBeDeterministic(algorithm, SaltingMode.Salted);

        private static void CalculateHash_ValidateDeterminism(HashingAlgorithmSpecification algorithm, SaltingMode saltingMode, Boolean shouldBeDeterministic)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = new HashingStringProcessor(randomnessProvider);
                var plaintextObject = "䆟`ಮ䷆ʘ‣⦸⏹ⰄͶa✰ṁ亡Zᨖ0༂⽔9㗰";

                // Act.
                var firstHashValue = target.CalculateHash(plaintextObject, algorithm, saltingMode);

                // Assert.
                firstHashValue.Should().NotBeNullOrEmpty();
                firstHashValue.Count(value => value == 0x00).Should().NotBe(firstHashValue.Length);

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
    }
}