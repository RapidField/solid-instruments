// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Core.UnitTests.Concurrency
{
    [TestClass]
    public class ConcurrencyControlTokenTests
    {
        [TestMethod]
        public void AttachTask_ShouldCompleteTasksAsynchronously()
        {
            // Arrange.
            var mode = ConcurrencyControlMode.Unconstrained;
            var completionRecords = new List<Int32>();

            using (var control = ConcurrencyControl.New(mode))
            {
                using (var target = control.Enter())
                {
                    // Act.
                    target.AttachTask(Delay(800, completionRecords));
                    target.AttachTask(Delay(0, completionRecords));
                    target.AttachTask(Delay(400, completionRecords));
                }
            }

            // Assert.
            completionRecords.ElementAt(0).Should().Be(0);
            completionRecords.ElementAt(1).Should().Be(400);
            completionRecords.ElementAt(2).Should().Be(800);
        }

        private static Task Delay(Int32 durationInMilliseconds, IList<Int32> completionRecords) => Task.Delay(durationInMilliseconds).ContinueWith(delayTask =>
        {
            completionRecords.Add(durationInMilliseconds);
        });
    }
}