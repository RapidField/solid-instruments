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
    public class SingleThreadLockControlTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadLock;

            // Assert.
            ConcurrencyControlTests.FunctionalLifeSpanTest_ShouldProduceDesiredResults(mode);
        }

        [TestMethod]
        public void OperationLatency_ShouldBeLow()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.SingleThreadLock;
            var latencyThresholdInTicks = 987;

            // Assert.
            ConcurrencyControlTests.OperationLatency_ShouldBeLow(mode, PerformUsingPrimitive, latencyThresholdInTicks);
        }

        private static void PerformUsingPrimitive(Action<Action<Action>> perform)
        {
            var syncRoot = new Object();
            perform((operation) =>
            {
                Monitor.Enter(syncRoot);

                try
                {
                    operation();
                }
                finally
                {
                    Monitor.Exit(syncRoot);
                }
            });
        }
    }
}