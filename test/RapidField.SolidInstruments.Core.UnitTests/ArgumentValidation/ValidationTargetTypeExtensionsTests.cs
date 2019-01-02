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
    public class ValidationTargetTypeExtensionsTests
    {
        [TestMethod]
        public void IsNotSupportedType_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = typeof(ArgumentNullException);
            var supportedTypes = new Type[] { typeof(ArgumentOutOfRangeException) };
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsNotSupportedType(supportedTypes, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is not matched by {nameof(supportedTypes)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsNotSupportedType_ShouldNotRaiseException_ForMatchedDerivedType()
        {
            // Arrange.
            var target = typeof(ArgumentNullException);
            var supportedBaseType = typeof(ArgumentException);

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNotSupportedType(supportedBaseType);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is derived from {nameof(supportedBaseType)}");
        }

        [TestMethod]
        public void IsNotSupportedType_ShouldNotRaiseException_ForMatchedType()
        {
            // Arrange.
            var target = typeof(ArgumentNullException);
            var supportedTypes = new Type[] { typeof(ArgumentNullException) };

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNotSupportedType(supportedTypes);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is matched by {nameof(supportedTypes)}");
        }

        [TestMethod]
        public void IsNotSupportedType_ShouldRaiseUnsupportedTypeArgumentException_ForMismatchedDerivedType()
        {
            // Arrange.
            var target = typeof(ArgumentNullException);
            var supportedBaseType = typeof(ArgumentOutOfRangeException);

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNotSupportedType(supportedBaseType);
            });

            // Assert.
            action.Should().Throw<UnsupportedTypeArgumentException>($"because {nameof(target)} is not derived from {nameof(supportedBaseType)}");
        }

        [TestMethod]
        public void IsNotSupportedType_ShouldRaiseUnsupportedTypeArgumentException_ForMismatchedType()
        {
            // Arrange.
            var target = typeof(ArgumentNullException);
            var supportedTypes = new Type[] { typeof(ArgumentOutOfRangeException) };

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsNotSupportedType(supportedTypes);
            });

            // Assert.
            action.Should().Throw<UnsupportedTypeArgumentException>($"because {nameof(target)} is not matched by {nameof(supportedTypes)}");
        }
    }
}