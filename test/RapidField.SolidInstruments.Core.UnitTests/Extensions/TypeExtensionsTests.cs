// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void GetDefaultValue_ShouldReturnDefaultValue_ForReferenceTypes()
        {
            // Arrange.
            var target = typeof(String);

            // Act.
            var result = (String)target.GetDefaultValue();

            // Assert.
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetDefaultValue_ShouldReturnDefaultValue_ForValueTypes()
        {
            // Arrange.
            var target = typeof(DateTime);

            // Act.
            var result = (DateTime)target.GetDefaultValue();

            // Assert.
            result.Should().Be(default);
        }

        [TestMethod]
        public void GetUniqueIdentifier_ShouldReturnUniqueValues_ForUniqueTypes()
        {
            // Arrange.
            var targets = new Type[]
            {
                typeof(Object),
                typeof(Decimal),
                typeof(String),
                typeof(Action),
                typeof(Action<Object>),
                typeof(Action<Object, Object>),
                typeof(Action<Object, Object, Object>),
                typeof(IEnumerable<Object>),
                typeof(IEnumerable<Decimal>),
                typeof(IEnumerable<String>),
                typeof(IEnumerable<Action>),
                typeof(IEnumerable<Action<Object>>),
                typeof(IEnumerable<Action<Object, Object>>),
                typeof(IEnumerable<Action<Object, Object, Object>>),
                typeof(IList<Object>),
                typeof(IList<Decimal>),
                typeof(IList<String>),
                typeof(IList<Action>),
                typeof(IList<Action<Object>>),
                typeof(IList<Action<Object, Object>>),
                typeof(IList<Action<Object, Object, Object>>),
                typeof(List<Object>),
                typeof(List<Decimal>),
                typeof(List<String>),
                typeof(List<Action>),
                typeof(List<Action<Object>>),
                typeof(List<Action<Object, Object>>),
                typeof(List<Action<Object, Object, Object>>),
                typeof(IEnumerable<IEnumerable<Object>>),
                typeof(IEnumerable<IEnumerable<Decimal>>),
                typeof(IEnumerable<IEnumerable<String>>),
                typeof(IEnumerable<IEnumerable<Action>>),
                typeof(IEnumerable<IEnumerable<Action<Object>>>),
                typeof(IEnumerable<IEnumerable<Action<Object, Object>>>),
                typeof(IEnumerable<IEnumerable<Action<Object, Object, Object>>>),
                typeof(IList<IEnumerable<Object>>),
                typeof(IList<IEnumerable<Decimal>>),
                typeof(IList<IEnumerable<String>>),
                typeof(IList<IEnumerable<Action>>),
                typeof(IList<IEnumerable<Action<Object>>>),
                typeof(IList<IEnumerable<Action<Object, Object>>>),
                typeof(IList<IEnumerable<Action<Object, Object, Object>>>),
                typeof(List<IEnumerable<Object>>),
                typeof(List<IEnumerable<Decimal>>),
                typeof(List<IEnumerable<String>>),
                typeof(List<IEnumerable<Action>>),
                typeof(List<IEnumerable<Action<Object>>>),
                typeof(List<IEnumerable<Action<Object, Object>>>),
                typeof(List<IEnumerable<Action<Object, Object, Object>>>)
            };

            // Act.
            var result = targets.Select(element => element.GetUniqueIdentifier());

            // Assert.
            result.Should().NotBeNullOrEmpty();
            result.Should().OnlyHaveUniqueItems();
            result.Should().NotContain(Guid.Empty);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnValidResult()
        {
            // Arrange.
            var encoding = Encoding.UTF8;
            var target = typeof(IEnumerable<IEnumerable<Action<Object, Object>>>);
            var expectedResult = "System.Collections.Generic.IEnumerable`1[System.Collections.Generic.IEnumerable`1[System.Action`2[System.Object,System.Object]]]";

            // Act.
            var resultOne = target.ToByteArray();
            var resultTwo = encoding.GetString(resultOne);

            // Assert.
            resultOne.Should().NotBeNull();
            resultOne.Length.Should().Be(expectedResult.Length);
            resultTwo.Should().NotBeNullOrEmpty();
            resultTwo.Should().Be(expectedResult);
        }
    }
}