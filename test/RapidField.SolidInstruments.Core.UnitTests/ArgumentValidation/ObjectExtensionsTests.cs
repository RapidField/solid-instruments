﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests.ArgumentValidation
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void RejectIf_ShouldNotRaiseException_ForUnsatisfiedPredicate()
        {
            // Arrange.
            var target = "foo";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf(argument => false);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} doesn't satisfy the predicate");
        }

        [TestMethod]
        public void RejectIf_ShouldRaiseArgumentException_ForSatisfiedPredicate()
        {
            // Arrange.
            var target = "foo";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf(argument => true);
            });

            // Assert.
            action.Should().Throw<ArgumentException>($"because {nameof(target)} satisfies the predicate");
        }

        [TestMethod]
        public void RejectIf_ShouldReturnValidationTarget_ForUnpredicatedSignature()
        {
            // Arrange.
            var target = "foo";
            var result = (ValidationTarget<String>)null;

            // Act.
            result = target.RejectIf();

            // Assert.
            result.Should().NotBeNull();
            result.ArgumentType.Should().Be(target.GetType());
        }

        [TestMethod]
        public void RejectIf_ShouldReturnValidResult_ForPredicatedSignature()
        {
            // Arrange.
            var target = "foo";
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf(argument => false);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} doesn't satisfy the predicate");
            result.Should().BeSameAs(target);
        }
    }
}