// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void CalculateSizeInBytes_ShouldProduceAccurateResult_ForAnonymousReferenceTarget()
        {
            // Arrange.
            var expectedResult = 255;
            var reference = new TimeOfDay(TimeZoneInfo.Utc, 8, 23, 40, 819);
            var target = new
            {
                ReferenceOne = reference,
                ReferenceTwo = reference,
                ValueOne = 42,
                ValueTwo = (Byte)0x01,
                ValueThree = (Byte)0x02,
                Collection = new String[]
                {
                    "foo",
                    "bar",
                    "foobar"
                }
            };

            // Act.
            var result = target.CalculateSizeInBytes();

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void CalculateSizeInBytes_ShouldProduceAccurateResult_ForCollectionTarget()
        {
            // Arrange.
            var expectedResult = 52;
            var target = new String[]
            {
                "foo",
                "bar",
                null,
                "foobar"
            };

            // Act.
            var result = target.CalculateSizeInBytes();

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void CalculateSizeInBytes_ShouldProduceAccurateResult_ForComplexReferenceTarget()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = SimulatedModel.Random(randomnessProvider);
                var clone = target.Clone() as SimulatedModel;

                // Act.
                var resultOne = target.CalculateSizeInBytes();
                var resultTwo = target.CalculateSizeInBytes();

                // Assert.
                resultOne.Should().BeGreaterThan(0);
                resultTwo.Should().BeGreaterThan(0);
                resultOne.Should().Be(resultTwo);
            }
        }

        [TestMethod]
        public void CalculateSizeInBytes_ShouldProduceAccurateResult_ForNullTarget()
        {
            // Arrange.
            var expectedResult = IntPtr.Size;
            var target = (Object)null;

            // Act.
            var result = target.CalculateSizeInBytes();

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void CalculateSizeInBytes_ShouldProduceAccurateResult_ForValueTarget()
        {
            // Arrange.
            var expectedResult = 16;
            var target = Guid.NewGuid();

            // Act.
            var result = target.CalculateSizeInBytes();

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void GetImpliedHashCode_ShouldProduceIdenticalHashCodes_ForIdenticalObjects()
        {
            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                var target = SimulatedModel.Random(randomnessProvider);
                var clone = target.Clone() as SimulatedModel;

                // Act.
                var targetHashCode = target.GetHashCode();
                var copyHashCode = clone.GetHashCode();

                // Assert.
                target.Should().NotBeNull();
                clone.Should().NotBeNull();
                target.Should().BeEquivalentTo(clone);
                targetHashCode.Should().Be(copyHashCode);
            }
        }

        [TestMethod]
        public void GetImpliedHashCode_ShouldProduceUniqueHashCodes_ForUniqueObjects()
        {
            // Arrange.
            var elementCount = 300;
            var target = new List<Int32>();

            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                for (var i = 0; i < elementCount; i++)
                {
                    // Act.
                    var model = SimulatedModel.Random(randomnessProvider);
                    var randomlyModifiedModel = model.Clone() as SimulatedModel;
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