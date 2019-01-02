// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RapidField.SolidInstruments.Core.UnitTests.ArgumentValidation
{
    [TestClass]
    public class ValidationTargetICollectionExtensionsTests
    {
        [TestMethod]
        public void IsNullOrEmpty_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = (ICollection<Int32>)null;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsNullOrEmpty(targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is null");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldNotRaiseException_ForNonEmptyTarget()
        {
            // Arrange.
            var target = new Collection<Int32>(new Int32[1] { 3 }) as ICollection<Int32>;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNullOrEmpty();
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not null or empty");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldRaiseArgumentEmptyException_ForEmptyTarget()
        {
            // Arrange.
            var target = new Collection<Int32>() as ICollection<Int32>;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNullOrEmpty();
            });

            // Assert.
            action.Should().Throw<ArgumentEmptyException>($"because {nameof(target)} is empty");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldRaiseArgumentNullException_ForNullTarget()
        {
            // Arrange.
            var target = (ICollection<Int32>)null;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNullOrEmpty();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(target)} is null");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldReturnTarget()
        {
            // Arrange.
            var target = new Collection<Int32>(new Int32[1] { 3 }) as ICollection<Int32>;
            var result = (ICollection<Int32>)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsNullOrEmpty().TargetArgument;
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not null or empty");
            result.Should().BeSameAs(target);
        }
    }
}