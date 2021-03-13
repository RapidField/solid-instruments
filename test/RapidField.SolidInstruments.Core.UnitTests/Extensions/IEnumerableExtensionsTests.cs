// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void Flatten_ShouldProduceDesiredResults()
        {
            // Arrange.
            var collectionOne = new Int32[] { 1, 2, 3 };
            var collectionTwo = new Int32[] { 4, 5, 6 };
            var collectionThree = new Int32[] { 7, 8, 9 };
            var target = new List<Int32[]>() { collectionOne, collectionTwo, collectionThree };

            // Act.
            var result = target.Flatten();

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(target.Select(collection => collection.Length).Sum());
            result.Should().ContainInOrder(collectionOne);
            result.Should().ContainInOrder(collectionTwo);
            result.Should().ContainInOrder(collectionThree);
        }

        [TestMethod]
        public void IsEquivalentTo_ShouldReturnFalse_ForInequivalentCollections()
        {
            // Arrange.
            var targetOne = new List<String>() { "foo", null, "bar", null, "foobar" };
            var targetTwo = new List<String>() { "foo", null, "bar", null, String.Empty };

            // Act.
            var result = targetOne.IsEquivalentTo(targetTwo);

            // Assert.
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEquivalentTo_ShouldReturnTrue_ForEquivalentCollections()
        {
            // Arrange.
            var targetOne = new List<String>() { "foo", null, "bar", null, "foobar" };
            var targetTwo = new List<String>() { "foo", null, "bar", null, "foobar" };

            // Act.
            var result = targetOne.IsEquivalentTo(targetTwo);

            // Assert.
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldReturnValidResult_ForArrayTarget()
        {
            // Arrange.
            var targetOne = (String[])null;
            var targetTwo = Array.Empty<String>();
            var targetThree = new String[] { "foo" };

            // Act.
            var resultOne = targetOne.IsNullOrEmpty();
            var resultTwo = targetTwo.IsNullOrEmpty();
            var resultThree = targetThree.IsNullOrEmpty();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldReturnValidResult_ForIDictionaryTarget()
        {
            // Arrange.
            var targetOne = (IDictionary<Int32, String>)null;
            var targetTwo = new Dictionary<Int32, String>();
            var targetThree = new Dictionary<Int32, String>() { { 1, "foo" } };

            // Act.
            var resultOne = targetOne.IsNullOrEmpty();
            var resultTwo = targetTwo.IsNullOrEmpty();
            var resultThree = targetThree.IsNullOrEmpty();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldReturnValidResult_ForIListTarget()
        {
            // Arrange.
            var targetOne = (IList<String>)null;
            var targetTwo = new List<String>();
            var targetThree = new List<String>() { "foo" };

            // Act.
            var resultOne = targetOne.IsNullOrEmpty();
            var resultTwo = targetTwo.IsNullOrEmpty();
            var resultThree = targetThree.IsNullOrEmpty();

            // Assert.
            resultOne.Should().BeTrue();
            resultTwo.Should().BeTrue();
            resultThree.Should().BeFalse();
        }
    }
}