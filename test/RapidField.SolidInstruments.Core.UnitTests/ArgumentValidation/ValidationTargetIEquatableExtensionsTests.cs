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
    public class ValidationTargetIEquatableExtensionsTests
    {
        [TestMethod]
        public void IsEqualTo_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var otherObject = "foo";
            var targetParameterName = "targetParameterName";
            var otherParameterName = "otherParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsEqualTo(otherObject, targetParameterName, otherParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is equal to {nameof(otherObject)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherParameterName).Should().BeTrue($"because {nameof(otherParameterName)} is not null");
        }

        [TestMethod]
        public void IsEqualTo_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var otherObject = "foo";
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsEqualTo(otherObject, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is equal to {nameof(otherObject)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsEqualTo_ShouldNotRaiseException_ForInequalTarget()
        {
            // Arrange.
            var target = "foo";
            var otherObject = "bar";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsEqualTo(otherObject);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not equal to {nameof(otherObject)}");
        }

        [TestMethod]
        public void IsEqualTo_ShouldRaiseArgumentOutOfRangeException_ForEqualTarget()
        {
            // Arrange.
            var target = "foo";
            var otherObject = "foo";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsEqualTo(otherObject);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is equal to {nameof(otherObject)}");
        }

        [TestMethod]
        public void IsEqualTo_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var otherObject = "bar";
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsEqualTo(otherObject);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not equal to {nameof(otherObject)}");
            result.Should().BeSameAs(target);
        }
    }
}