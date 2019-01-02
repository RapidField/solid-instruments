// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Cryptography.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RapidField.SolidInstruments.Cryptography.UnitTests.Extensions
{
    [TestClass]
    public sealed class IListExtensionsTests
    {
        [TestMethod]
        public void ReverseStaggerSort_ShouldBeReversible_ForOddLengthTarget()
        {
            // Arrange.
            var target = new Char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            var result = (String)null;

            // Act.
            target.ReverseStaggerSort();
            target.ReverseStaggerSort();
            result = new String(target);

            // Assert.
            result.Should().Be("ABCDEFGHI");
        }

        [TestMethod]
        public void ReverseStaggerSort_ShouldNotBeReversible_ForEvenLengthTarget()
        {
            // Arrange.
            var target = new Char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var result = (String)null;

            // Act.
            target.ReverseStaggerSort();
            target.ReverseStaggerSort();
            result = new String(target);

            // Assert.
            result.Should().Be("GHEFCDAB");
        }

        [TestMethod]
        public void ReverseStaggerSort_ShouldProduceDesiredResults_ForArrayTarget()
        {
            // Arrange.
            var target = new Char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var result = (String)null;

            // Act.
            target.ReverseStaggerSort();
            result = new String(target);

            // Assert.
            result.Should().Be("BGDEFCHA");
        }

        [TestMethod]
        public void ReverseStaggerSort_ShouldProduceDesiredResults_ForListTarget()
        {
            // Arrange.
            var target = new List<Int32> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var stringBuilder = new StringBuilder();
            var result = (String)null;

            // Act.
            target.ReverseStaggerSort();

            foreach (var integer in target)
            {
                stringBuilder.Append(integer.ToString());
            }

            result = stringBuilder.ToString();

            // Assert.
            result.Should().Be("927456381");
        }

        [TestMethod]
        public void Shuffle_ShouldProduceDesiredResult_ForArrayTarget()
        {
            // Arrange.
            var elementCount = 1800;
            var target = new Single[elementCount];
            var initialSumOfRandomValues = 0d;
            var finalSumOfRandomValues = 0d;

            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                randomnessProvider.FillSingleArray(target);
                initialSumOfRandomValues = target.Sum();
                var firstValue = target[0];
                var lastValue = target[elementCount - 1];

                // Act.
                target.Shuffle(randomnessProvider);
                finalSumOfRandomValues = target.Sum();

                // Assert.
                target.Length.Should().Be(elementCount);
                (firstValue == target[0] && lastValue == target[elementCount - 1]).Should().BeFalse();
                finalSumOfRandomValues.Should().Be(initialSumOfRandomValues);
            }
        }

        [TestMethod]
        public void Shuffle_ShouldProduceDesiredResult_ForListTarget()
        {
            // Arrange.
            var elementCount = 1800;
            var array = new Single[elementCount];
            var target = (IList<Single>)null;
            var initialSumOfRandomValues = 0d;
            var finalSumOfRandomValues = 0d;

            using (var randomnessProvider = RandomNumberGenerator.Create())
            {
                // Arrange.
                randomnessProvider.FillSingleArray(array);
                target = new List<Single>(array);
                initialSumOfRandomValues = array.Sum();
                var firstValue = target[0];
                var lastValue = target[elementCount - 1];

                // Act.
                target.Shuffle(randomnessProvider);
                finalSumOfRandomValues = target.Sum();

                // Assert.
                target.Count().Should().Be(elementCount);
                (firstValue == target[0] && lastValue == target[elementCount - 1]).Should().BeFalse();
                finalSumOfRandomValues.Should().Be(initialSumOfRandomValues);
            }
        }

        [TestMethod]
        public void Shuffle_ShouldRaiseArgumentNullException_ForNullRandomnessProviderArgument()
        {
            // Arrange.
            var target = new Int32[] { 0, 1, 2 };

            // Act.
            var action = new Action(() =>
            {
                target.Shuffle(null);
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>();
        }
    }
}