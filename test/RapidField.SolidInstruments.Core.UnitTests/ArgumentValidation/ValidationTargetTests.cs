// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.ArgumentValidation
{
    [TestClass]
    public class ValidationTargetTests
    {
        [TestMethod]
        public void ArgumentType_ShouldBeTypeOfTargetArgument()
        {
            // Arrange.
            var target = "foo";

            // Act.
            var result = target.RejectIf().ArgumentType;

            // Assert.
            result.Should().NotBeNull();
            result.Should().Be(typeof(String));
        }
    }
}