// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using RapidField.SolidInstruments.Cryptography.Extensions;
using RapidField.SolidInstruments.Cryptography.Hashing;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Hashing
{
    [TestClass]
    public class HashTreeTests
    {
        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForMd5()
        {
            // Arrange.
            var algorithm = HashingAlgorithmSpecification.Md5;
            var maxBlockCount = TreeHeightDictionary.Count - 1;

            for (var i = 0; i <= maxBlockCount; i++)
            {
                Constructor_ShouldProduceDesiredResults(i, algorithm);
            }
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForShaTwo256()
        {
            // Arrange.
            var algorithm = HashingAlgorithmSpecification.ShaTwo256;
            var maxBlockCount = TreeHeightDictionary.Count - 1;

            for (var i = 0; i <= maxBlockCount; i++)
            {
                Constructor_ShouldProduceDesiredResults(i, algorithm);
            }
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForShaTwo384()
        {
            // Arrange.
            var algorithm = HashingAlgorithmSpecification.ShaTwo384;
            var maxBlockCount = TreeHeightDictionary.Count - 1;

            for (var i = 0; i <= maxBlockCount; i++)
            {
                Constructor_ShouldProduceDesiredResults(i, algorithm);
            }
        }

        [TestMethod]
        public void Constructor_ShouldProduceDesiredResults_ForShaTwo512()
        {
            // Arrange.
            var algorithm = HashingAlgorithmSpecification.ShaTwo512;
            var maxBlockCount = TreeHeightDictionary.Count - 1;

            for (var i = 0; i <= maxBlockCount; i++)
            {
                Constructor_ShouldProduceDesiredResults(i, algorithm);
            }
        }

        private static void Constructor_ShouldProduceDesiredResults(Int32 blockCount, HashingAlgorithmSpecification algorithm)
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var hashingProcessor = new HashingStringProcessor(randomnessProvider);
                var blocks = new String[blockCount];
                randomnessProvider.FillStringArray(blocks, 1, 8, false, true, true, true, false, false, false);

                // Act.
                var target = new HashTree<String>(hashingProcessor, algorithm, blocks);
                var duplicate = new HashTree<String>(hashingProcessor, algorithm, blocks);

                // Assert.
                target.LeafCount.Should().Be(blockCount);
                target.RootNode.Should().NotBeNull();
                target.RootNode.Value.Should().NotBeNull();
                target.RootNode.Value.ComputeThirtyTwoBitHash().Should().Be(duplicate.RootNode.Value.ComputeThirtyTwoBitHash());
                target.RootNode.ToString().Should().Be(Convert.ToBase64String(target.RootNode.Value));
                ValidateDigestLength(blockCount, target.RootNode.Value, algorithm);
                ValidateTreeHeight(blockCount, target.RootNode.Height);
            }
        }

        private static void ValidateDigestLength(Int32 blockCount, Byte[] rootHashValue, HashingAlgorithmSpecification algorithm)
        {
            if (blockCount == 0)
            {
                rootHashValue.Length.Should().Be(0);
                return;
            }

            switch (algorithm)
            {
                case HashingAlgorithmSpecification.Md5:

                    rootHashValue.Length.Should().Be(16);
                    break;

                case HashingAlgorithmSpecification.ShaTwo256:

                    rootHashValue.Length.Should().Be(32);
                    break;

                case HashingAlgorithmSpecification.ShaTwo384:

                    rootHashValue.Length.Should().Be(48);
                    break;

                case HashingAlgorithmSpecification.ShaTwo512:

                    rootHashValue.Length.Should().Be(64);
                    break;

                default:

                    Assert.Fail();
                    return;
            }
        }

        private static void ValidateTreeHeight(Int32 blockCount, Int32 treeHeight) => treeHeight.Should().Be(TreeHeightDictionary[blockCount]);

        private static readonly IDictionary<Int32, Int32> TreeHeightDictionary = new Dictionary<Int32, Int32>()
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 1 },
            { 3, 2 },
            { 4, 2 },
            { 5, 3 },
            { 6, 3 },
            { 7, 3 },
            { 8, 3 },
            { 9, 4 },
            { 10, 4 },
            { 11, 4 },
            { 12, 4 },
            { 13, 4 },
            { 14, 4 },
            { 15, 4 },
            { 16, 4 },
            { 17, 5 },
            { 18, 5 },
            { 19, 5 },
            { 20, 5 },
            { 21, 5 },
            { 22, 5 },
            { 23, 5 },
            { 24, 5 },
            { 25, 5 },
            { 26, 5 },
            { 27, 5 },
            { 28, 5 },
            { 29, 5 },
            { 30, 5 },
            { 31, 5 },
            { 32, 5 },
            { 33, 6 }
        };
    }
}