// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void GetImpliedHashCode_ShouldProduceIdenticalHashCodes_ForIdenticalObjects()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = SimulatedModel.Random(randomnessProvider);
                var copy = target.Copy();

                // Act.
                var targetHashCode = target.GetHashCode();
                var copyHashCode = copy.GetHashCode();

                // Assert.
                target.Should().NotBeNull();
                copy.Should().NotBeNull();
                target.Should().BeEquivalentTo(copy);
                targetHashCode.Should().Be(copyHashCode);
            }
        }

        [TestMethod]
        public void GetImpliedHashCode_ShouldProduceUniqueHashCodes_ForUniqueObjects()
        {
            // Arrange.
            var elementCount = 30;
            var target = new List<Int32>();

            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                for (var i = 0; i < elementCount; i++)
                {
                    // Act.
                    var model = SimulatedModel.Random(randomnessProvider);
                    var randomlyModifiedModel = model.Copy();
                    randomlyModifiedModel.RandomlyModify(randomnessProvider);
                    target.Add(model.GetHashCode());
                    target.Add(randomlyModifiedModel.GetHashCode());
                }
            }

            // Assert.
            target.Should().OnlyHaveUniqueItems();
        }
    }
}