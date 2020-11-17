// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Threading;

namespace RapidField.SolidInstruments.Core.UnitTests.Concurrency
{
    [TestClass]
    public class SingleThreadSpinLockControlTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadSpinLock;

            // Assert.
            ConcurrencyControlTests.FunctionalLifeSpanTest_ShouldProduceDesiredResults(mode);
        }

        [TestMethod]
        public void OperationLatency_ShouldBeLow()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadSpinLock;
            var latencyThresholdInTicks = 6765;

            // Assert.
            ConcurrencyControlTests.OperationLatency_ShouldBeLow(mode, PerformUsingPrimitive, latencyThresholdInTicks);
        }

        private static void PerformUsingPrimitive(Action<Action<Action>> perform)
        {
            var control = new SpinLock(false);
            perform((operation) =>
            {
                var lockTaken = false;
                control.TryEnter(ref lockTaken);

                try
                {
                    operation();
                }
                finally
                {
                    if (lockTaken)
                    {
                        control.Exit();
                    }
                }
            });
        }
    }
}