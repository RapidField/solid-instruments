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
    public class ValidationTargetIComparableExtensionsTests
    {
        [TestMethod]
        public void IsGreaterThan_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherArgument = 2;
            var targetParameterName = "targetParameterName";
            var otherParameterName = "otherParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsGreaterThan(otherArgument, targetParameterName, otherParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is greater than {nameof(otherArgument)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherParameterName).Should().BeTrue($"because {nameof(otherParameterName)} is not null");
        }

        [TestMethod]
        public void IsGreaterThan_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var exclusiveUpperBoundary = 2;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsGreaterThan(exclusiveUpperBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is greater than {nameof(exclusiveUpperBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsGreaterThan_ShouldNotRaiseException_ForEqualTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveUpperBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is equal to {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThan_ShouldNotRaiseException_ForLesserTarget()
        {
            // Arrange.
            var target = 2;
            var exclusiveUpperBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is less than {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThan_ShouldRaiseArgumentOutOfRangeException_ForGreaterTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveUpperBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is greater than {nameof(exclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThan_ShouldReturnTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveUpperBoundary = 4;
            var result = default(Int32);

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsGreaterThan(exclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is less than {nameof(exclusiveUpperBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherArgument = 2;
            var targetParameterName = "targetParameterName";
            var otherParameterName = "otherParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsGreaterThanOrEqualTo(otherArgument, targetParameterName, otherParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is greater than {nameof(otherArgument)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherParameterName).Should().BeTrue($"because {nameof(otherParameterName)} is not null");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var inclusiveUpperBoundary = 2;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsGreaterThanOrEqualTo(inclusiveUpperBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is greater than {nameof(inclusiveUpperBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldNotRaiseException_ForLesserTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveUpperBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is less than {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForEqualTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveUpperBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is equal to {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForGreaterTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveUpperBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is greater than {nameof(inclusiveUpperBoundary)}");
        }

        [TestMethod]
        public void IsGreaterThanOrEqualTo_ShouldReturnTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveUpperBoundary = 4;
            var result = default(Int32);

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsGreaterThanOrEqualTo(inclusiveUpperBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is less than {nameof(inclusiveUpperBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void IsLessThan_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherArgument = 4;
            var targetParameterName = "targetParameterName";
            var otherParameterName = "otherParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsLessThan(otherArgument, targetParameterName, otherParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is less than {nameof(otherArgument)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherParameterName).Should().BeTrue($"because {nameof(otherParameterName)} is not null");
        }

        [TestMethod]
        public void IsLessThan_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var exclusiveLowerBoundary = 4;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsLessThan(exclusiveLowerBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is less than {nameof(exclusiveLowerBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsLessThan_ShouldNotRaiseException_ForEqualTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveLowerBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is equal to {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThan_ShouldNotRaiseException_ForGreaterTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveLowerBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is greater than {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThan_ShouldRaiseArgumentOutOfRangeException_ForLesserTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveLowerBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is less than {nameof(exclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThan_ShouldReturnTarget()
        {
            // Arrange.
            var target = 3;
            var exclusiveLowerBoundary = 2;
            var result = default(Int32);

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsLessThan(exclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is greater than {nameof(exclusiveLowerBoundary)}");
            result.Should().Be(target);
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldConveyOtherArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var otherArgument = 4;
            var targetParameterName = "targetParameterName";
            var otherParameterName = "otherParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsLessThanOrEqualTo(otherArgument, targetParameterName, otherParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is less than {nameof(otherArgument)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
            exceptionMessage.Contains(otherParameterName).Should().BeTrue($"because {nameof(otherParameterName)} is not null");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldConveyTargetArgumentParameterName()
        {
            // Arrange.
            var target = 3;
            var inclusiveLowerBoundary = 4;
            var targetParameterName = "targetParameterName";
            var exceptionMessage = (String)null;

            // Act.
            var action = new Action(() =>
            {
                try
                {
                    target.RejectIf().IsLessThanOrEqualTo(inclusiveLowerBoundary, targetParameterName);
                }
                catch (Exception exception)
                {
                    exceptionMessage = exception.Message;
                    throw;
                }
            });

            // Assert.
            action.Should().Throw<Exception>($"because {nameof(target)} is less than {nameof(inclusiveLowerBoundary)}");
            exceptionMessage.Contains(targetParameterName).Should().BeTrue($"because {nameof(targetParameterName)} is not null");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldNotRaiseException_ForGreaterTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveLowerBoundary = 2;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is greater than {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForEqualTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveLowerBoundary = 3;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is equal to {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldRaiseArgumentOutOfRangeException_ForLesserTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveLowerBoundary = 4;

            // Act.
            var action = new Action(() =>
            {
                target.RejectIf().IsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>($"because {nameof(target)} is less than {nameof(inclusiveLowerBoundary)}");
        }

        [TestMethod]
        public void IsLessThanOrEqualTo_ShouldReturnTarget()
        {
            // Arrange.
            var target = 3;
            var inclusiveLowerBoundary = 2;
            var result = default(Int32);

            // Act.
            var action = new Action(() =>
            {
                result = target.RejectIf().IsLessThanOrEqualTo(inclusiveLowerBoundary);
            });

            // Assert.
            action.Should().NotThrow($"because {nameof(target)} is greater than {nameof(inclusiveLowerBoundary)}");
            result.Should().Be(target);
        }
    }
}