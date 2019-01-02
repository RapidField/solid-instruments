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
    public class ValidationTargetStructureExtensionsTests
    {
        [TestMethod]
        public void IsEqualToValue_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherArgument = 3;
            var targetParameterName = "targetParameterName";
            var otherValueParameterName = "otherValueParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsEqualToValue(otherArgument, targetParameterName, otherValueParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is equal to {nameof(otherArgument)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherValueParameterName).Should().BeTrue($"because {nameof(otherValueParameterName)} is not null");
        }

        [TestMethod]
        public void IsEqualToValue_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherValue = 3;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsEqualToValue(otherValue, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is equal to {nameof(otherValue)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsEqualToValue_ShouldNotRaiseException_ForInequalTarget()
        {
            // Arrange.
            var target = 3;
            var otherValue = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsEqualToValue(otherValue);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not equal to {nameof(otherValue)}");
        }

        [TestMethod]
        public void IsEqualToValue_ShouldRaiseArgumentOutOfRangeException_ForEqualTarget()
        {
            // Arrange.
            var target = 3;
            var otherValue = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsEqualToValue(otherValue);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is equal to {nameof(otherValue)}");
        }
    }
}