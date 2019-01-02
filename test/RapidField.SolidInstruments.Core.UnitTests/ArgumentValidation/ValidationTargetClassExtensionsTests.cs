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
    public class ValidationTargetClassExtensionsTests
    {
        [TestMethod]
        public void IsNull_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = (Object)null;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsNull(targetParameterName);
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
        public void IsNull_ShouldNotRaiseException_ForNonNullTarget()
        {
            // Arrange.
            var target = new Object();

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNull();
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not null");
        }

        [TestMethod]
        public void IsNull_ShouldRaiseArgumentNullException_ForNullTarget()
        {
            // Arrange.
            var target = (Object)null;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNull();
            });

            // Assert.
            action.Should().Throw<ArgumentNullException>($"because {nameof(target)} is null");
        }

        [TestMethod]
        public void IsNull_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsNull();
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not null");
            result.Should().BeSameAs(target);
        }
    }
}