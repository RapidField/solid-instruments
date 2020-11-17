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
    public class ProcessorCountSemaphoreControlTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.ProcessorCountSemaphore;

            // Assert.
            ConcurrencyControlTests.FunctionalLifeSpanTest_ShouldProduceDesiredResults(mode);
        }

        [TestMethod]
        public void OperationLatency_ShouldBeLow()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.ProcessorCountSemaphore;
            var latencyThresholdInTicks = 6765;

            // Assert.
            ConcurrencyControlTests.OperationLatency_ShouldBeLow(mode, PerformUsingPrimitive, latencyThresholdInTicks);
        }

        private static void PerformUsingPrimitive(Action<Action<Action>> perform)
        {
            using (var control = new SemaphoreSlim(Environment.ProcessorCount))
            {
                perform((operation) =>
                {
                    control.Wait();

                    try
                    {
                        operation();
                    }
                    finally
                    {
                        control.Release();
                    }
                });
            }
        }
    }
}