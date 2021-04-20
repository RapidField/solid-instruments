// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public sealed class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void SelectRandom_ShouldProduceDesiredResults()
        {
            // Arrange.
            var elementCount = 1800;
            var target = new Single[elementCount];
            using var randomnessProvider = RandomNumberGenerator.Create();

            // Arrange.
            randomnessProvider.FillSingleArray(target);

            // Act.
            var results = new Single[]
            {
                target.SelectRandom(randomnessProvider),
                target.SelectRandom(randomnessProvider),
                target.SelectRandom(randomnessProvider)
            };

            // Assert.
            target.Should().Contain(results[0]);
            target.Should().Contain(results[1]);
            target.Should().Contain(results[2]);
        }

        [TestMethod]
        public void SelectRandom_ShouldRaiseArgumentNullException_ForNullRandomnessProviderArgument()
        {
            // Arrange.
            var target = new Int32[] { 0, 1, 2 };

            // Act.
            var action = new Action(() =>
            {
                var result = target.SelectRandom(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }
    }
}