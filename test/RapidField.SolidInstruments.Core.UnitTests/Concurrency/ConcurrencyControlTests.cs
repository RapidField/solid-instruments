// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.UnitTests.Concurrency
{
    [TestClass]
    public class ConcurrencyControlTests
    {
        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForNegativeBlockTimeoutThresholdArgument()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.DuplexSemaphore;
            var blockTimeoutThreshold = TimeSpan.FromDays(-1);

            // Act.
            var action = new Action(() =>
            {
                using (var target = ConcurrencyControl.New(mode, blockTimeoutThreshold))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void New_ShouldRaiseArgumentOutOfRangeException_ForUnspecifiedModeArgument()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.Unspecified;
            var blockTimeoutThreshold = Timeout.InfiniteTimeSpan;

            // Act.
            var action = new Action(() =>
            {
                using (var target = ConcurrencyControl.New(mode, blockTimeoutThreshold))
                {
                    return;
                }
            });

            // Assert.
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void New_ShouldReturnValidResult_ForDuplexSemaphoreMode()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.DuplexSemaphore;

            // Act.
            using (var target = ConcurrencyControl.New(mode))
            {
                // Assert.
                target.Should().NotBeNull();
                target.Should().BeOfType<DuplexSemaphoreControl>();
            }
        }

        [TestMethod]
        public void New_ShouldReturnValidResult_ForProcessorCountSemaphoreMode()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.ProcessorCountSemaphore;

            // Act.
            using (var target = ConcurrencyControl.New(mode))
            {
                // Assert.
                target.Should().NotBeNull();
                target.Should().BeOfType<ProcessorCountSemaphoreControl>();
            }
        }

        [TestMethod]
        public void New_ShouldReturnValidResult_ForSingleThreadLockMode()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadLock;

            // Act.
            using (var target = ConcurrencyControl.New(mode))
            {
                // Assert.
                target.Should().NotBeNull();
                target.Should().BeOfType<SingleThreadLockControl>();
            }
        }

        [TestMethod]
        public void New_ShouldReturnValidResult_ForSingleThreadSpinLockMode()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadSpinLock;

            // Act.
            using (var target = ConcurrencyControl.New(mode))
            {
                // Assert.
                target.Should().NotBeNull();
                target.Should().BeOfType<SingleThreadSpinLockControl>();
            }
        }

        [TestMethod]
        public void New_ShouldReturnValidResult_ForUnconstrainedMode()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.Unconstrained;

            // Act.
            using (var target = ConcurrencyControl.New(mode))
            {
                // Assert.
                target.Should().NotBeNull();
                target.Should().BeOfType<UnconstrainedControl>();
            }
        }

        internal static void FunctionalLifeSpanTest_ShouldProduceDesiredResults(ConcurrencyControlMode mode)
        {
            // Arrange.
            var operationCount = 30;
            var operations = new Action[operationCount];
            var tasks = new Task[operationCount];
            InitializeOperations(operations);

            // Act.
            var action = new Action(() =>
            {
                using (var target = ConcurrencyControl.New(mode))
                {
                    for (var i = 0; i < operationCount; i++)
                    {
                        var operation = operations[i];
                        tasks[i] = Task.Factory.StartNew(() =>
                        {
                            using (var controlToken = target.Enter())
                            {
                                controlToken.IsActive.Should().BeTrue();
                                operation();
                                ((ConcurrencyControlToken)controlToken).Release();
                                controlToken.IsActive.Should().BeFalse();
                            }
                        });
                    }

                    Task.WaitAll(tasks);
                }
            });

            // Assert.
            action.Should().NotThrow();
        }

        internal static void OperationLatency_ShouldBeLow(ConcurrencyControlMode mode, Action<Action<Action<Action>>> performUsingPrimitive, Int32 latencyThresholdInTicks)
        {
            // Arrange.
            var operationCount = 30;
            var iterationCount = 300;
            var operations = new Action[operationCount];
            var sumOfLatenciesPerOperation = TimeSpan.Zero;
            InitializeOperations(operations);

            for (var i = 0; i < iterationCount; i++)
            {
                // Act.
                var primitiveControlDuration = PerformUsingPrimitive(operations, performUsingPrimitive);
                var solidControlDuration = PerformUsingSolidControl(operations, mode);
                var totalLatency = (solidControlDuration - primitiveControlDuration);
                var latencyPerOperation = TimeSpan.FromTicks(totalLatency.Ticks / operationCount);
                sumOfLatenciesPerOperation += latencyPerOperation;
            }

            // Assert.
            var averageLatencyPerOperationInTicks = (sumOfLatenciesPerOperation.Ticks / iterationCount);
            averageLatencyPerOperationInTicks.Should().BeLessThan(latencyThresholdInTicks);
        }

        private static void InitializeOperations(Action[] operations)
        {
            for (var i = 0; i < operations.Length; i++)
            {
                operations[i] = TestOperation;
            }
        }

        private static TimeSpan PerformUsingPrimitive(Action[] operations, Action<Action<Action<Action>>> performUsingPrimitive)
        {
            var tasks = new Task[operations.Length];
            var perform = new Action<Action<Action>>((action) =>
            {
                for (var i = 0; i < operations.Length; i++)
                {
                    var operation = operations[i];
                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        action(operation);
                    });
                }

                Task.WaitAll(tasks);
            });
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                performUsingPrimitive(perform);
            }
            finally
            {
                stopwatch.Stop();
            }

            return stopwatch.Elapsed;
        }

        private static TimeSpan PerformUsingSolidControl(Action[] operations, ConcurrencyControlMode mode)
        {
            var tasks = new Task[operations.Length];
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (var control = ConcurrencyControl.New(mode))
                {
                    for (var i = 0; i < operations.Length; i++)
                    {
                        var operation = operations[i];
                        tasks[i] = Task.Factory.StartNew(() =>
                        {
                            using (var controlToken = control.Enter())
                            {
                                operation();
                            }
                        });
                    }

                    Task.WaitAll(tasks);
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            return stopwatch.Elapsed;
        }

        private static void TestOperation()
        {
            var numbers = new Int32[] { 3, 5, 8, 13 };

            for (var i = (numbers.Length - 2); i >= 0; i--)
            {
                if ((numbers[i + 1] % numbers[i]) == 2)
                {
                    return;
                }
            }
        }
    }
}