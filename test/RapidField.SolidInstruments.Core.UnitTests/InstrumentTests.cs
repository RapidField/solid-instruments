// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class InstrumentTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var stateControlMode = ConcurrencyControlMode.SingleThreadLock;
            var target = (SimulatedInstrument)null;
            var integerValue = 3;

            using (target = new SimulatedInstrument(stateControlMode))
            {
                // Act.
                target.StoreIntegerValue(integerValue);

                // Assert.
                target.NullableIntegerValue.Should().Be(integerValue);
            }

            // Assert.
            target.NullableIntegerValue.Should().Be(null);
        }

        [TestMethod]
        public void RejectIfDisposed_ShouldRaiseObjectDisposedException_ForDisposedTarget()
        {
            // Arrange.
            var stateControlMode = ConcurrencyControlMode.SingleThreadLock;
            var target = (SimulatedInstrument)null;
            var integerValue = 3;

            using (target = new SimulatedInstrument(stateControlMode))
            {
                // Act.
                target.StoreIntegerValue(integerValue);
            }

            // Act.
            var action = new Action(() =>
            {
                target.StoreIntegerValue(integerValue);
            });

            // Assert.
            action.Should().Throw<ObjectDisposedException>();
        }

        [TestMethod]
        public void StateControl_ShouldProduceDesiredResults_ForMultipleThreadConcurrencyControl()
        {
            // Arrange.
            var stateControlMode = ConcurrencyControlMode.DuplexSemaphore;
            var taskCount = 7;
            var tasks = new Task[taskCount];
            var target = (SimulatedInstrument)null;

            // Act.
            var action = new Action(() =>
            {
                using (target = new SimulatedInstrument(stateControlMode))
                {
                    for (var i = 0; i < taskCount; i++)
                    {
                        // Act.
                        tasks[i] = Task.Factory.StartNew(target.SimulateThreadSafeOperation);
                    }

                    // Act.
                    Task.WaitAll(tasks);

                    // Assert.
                    target.IsBusy.Should().BeFalse();
                    tasks.Count(task => task.Status == TaskStatus.RanToCompletion).Should().Be(taskCount);
                    target.StateIsVolatile.Should().BeFalse();
                }
            });

            // Assert.
            action.Should().NotThrow();
        }

        [TestMethod]
        public void StateControl_ShouldProduceDesiredResults_ForSingleThreadConcurrencyControl()
        {
            // Arrange.
            var stateControlMode = ConcurrencyControlMode.SingleThreadLock;
            var taskCount = 7;
            var tasks = new Task[taskCount];
            var target = (SimulatedInstrument)null;

            // Act.
            var action = new Action(() =>
            {
                using (target = new SimulatedInstrument(stateControlMode))
                {
                    for (var i = 0; i < taskCount; i++)
                    {
                        // Act.
                        tasks[i] = Task.Factory.StartNew(target.SimulateThreadSafeOperation);
                    }

                    // Act.
                    Task.WaitAll(tasks);

                    // Assert.
                    target.IsBusy.Should().BeFalse();
                    tasks.Count(task => task.Status == TaskStatus.RanToCompletion).Should().Be(taskCount);
                    target.StateIsVolatile.Should().BeFalse();
                }
            });

            // Assert.
            action.Should().NotThrow();
        }
    }
}