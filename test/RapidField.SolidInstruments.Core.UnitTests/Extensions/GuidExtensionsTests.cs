// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.Extensions
{
    [TestClass]
    public class GuidExtensionsTests
    {
        [TestMethod]
        public void ToSerializedString_ShouldReturnValidResult()
        {
            // Arrange.
            var target = Guid.Parse("A4A592FA-A96B-11E4-A553-982AF186852F");

            // Act.
            var result = target.ToSerializedString();

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be("a4a592faa96b11e4a553982af186852f");
            Guid.Parse(result).Should().Be(target);
        }
    }
}