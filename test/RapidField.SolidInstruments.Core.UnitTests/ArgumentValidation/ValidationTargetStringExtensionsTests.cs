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
    public class ValidationTargetStringExtensionsTests
    {
        [TestMethod]
        public void DoesNotMatchRegularExpression_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var regularExpressionPattern = "^([f-o]){2}$";
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().DoesNotMatchRegularExpression(regularExpressionPattern, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} does not match {nameof(regularExpressionPattern)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void DoesNotMatchRegularExpression_ShouldNotRaiseException_ForMatchedPattern()
        {
            // Arrange.
            var target = "foo";
            var regularExpressionPattern = "^([f-o]){3}$";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().DoesNotMatchRegularExpression(regularExpressionPattern);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} does match {nameof(regularExpressionPattern)}");
        }

        [TestMethod]
        public void DoesNotMatchRegularExpression_ShouldRaiseStringArgumentPatterException_ForMismatchedPattern()
        {
            // Arrange.
            var target = "foo";
            var regularExpressionPattern = "^([f-o]){2}$";

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().DoesNotMatchRegularExpression(regularExpressionPattern);
            });

            // Assert.
            action.Should().Throw<StringArgumentPatternException>($"because {nameof(target)} does not match {nameof(regularExpressionPattern)}");
        }

        [TestMethod]
        public void IsNullOrEmpty_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = (String)null;
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
        public void IsNullOrEmpty_ShouldNotRaiseException_ForNonNullTarget()
        {
            // Arrange.
            var target = "foo";

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
            var target = String.Empty;

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
            var target = (String)null;

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
            var target = "foo";
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsNullOrEmpty();
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is not null");
            result.Should().BeSameAs(target);
        }

        [TestMethod]
        public void LengthIsGreaterThan_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var exclusiveUpperBoundary = 2;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().LengthIsGreaterThan(exclusiveUpperBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} length is greater than {nameof(exclusiveUpperBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void LengthIsGreaterThan_ShouldNotRaiseException_ForEqualLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveUpperBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is equal to {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThan_ShouldNotRaiseException_ForLesserLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveUpperBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is less than {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThan_ShouldRaiseArgumentOutOfRangeException_ForGreaterLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveUpperBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is greater than {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThan_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveUpperBoundary = 4;
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().LengthIsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is less than {nameof(exclusiveUpperBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void LengthIsGreaterThanOrEqualTo_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var inclusiveUpperBoundary = 2;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} length is greater than {nameof(inclusiveUpperBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void LengthIsGreaterThanOrEqualTo_ShouldNotRaiseException_ForLesserLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveUpperBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is less than {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForEqualLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveUpperBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is equal to {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForGreaterLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveUpperBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is greater than {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void LengthIsGreaterThanOrEqualTo_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveUpperBoundary = 4;
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().LengthIsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is less than {nameof(inclusiveUpperBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void LengthIsLessThan_ShouldNotRaiseException_ForEqualLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveLowerBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is equal to {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThan_ShouldNotRaiseException_ForGreaterLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveLowerBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is greater than {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThan_ShouldRaiseArgumentOutOfRangeException_ForLesserLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveLowerBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is less than {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThan_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var exclusiveLowerBoundary = 2;
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().LengthIsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is greater than {nameof(exclusiveLowerBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void LengthIsLessThanOrEqualTo_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = "foo";
            var inclusiveLowerBoundary = 4;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().LengthIsLessThanOrEqualTo(inclusiveLowerBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} length is less than {nameof(inclusiveLowerBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void LengthIsLessThanOrEqualTo_ShouldNotRaiseException_ForGreaterLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveLowerBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is greater than {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForEqualLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveLowerBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is equal to {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForLesserLengthTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveLowerBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().LengthIsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} length is less than {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void LengthIsLessThanOrEqualTo_ShouldReturnTarget()
        {
            // Arrange.
            var target = "foo";
            var inclusiveLowerBoundary = 2;
            var result = (String)null;

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().LengthIsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} length is greater than {nameof(inclusiveLowerBoundary)}");
            result.Should().Be(target);
        }
    }
}