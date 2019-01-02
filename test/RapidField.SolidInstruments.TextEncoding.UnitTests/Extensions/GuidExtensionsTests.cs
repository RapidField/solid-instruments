// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.TextEncoding.Extensions;
using System;

namespace RapidField.SolidInstruments.TextEncoding.UnitTests.Extensions
{
    [TestClass]
    public class GuidExtensionsTests
    {
        [TestMethod]
        public void ToEnhancedReadabilityGuid_ShouldReturnValidResult()
        {
            // Arrange.
            var target = Guid.Parse("4aaadf09-0a66-41dc-bfc8-f8520f4aeaf8");

            // Act.
            var result = target.ToEnhancedReadabilityGuid();

            // Assert.
            result.Should().NotBeNull();
            result.ToString().Should().Be("jazikf3ckyzdw9n3azw6yfj7a8");
        }
    }
}