// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Core.UnitTests
{
    [TestClass]
    public class ReferenceManagerTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var disposableObjectCount = 30;
            var disposableObjects = new SimulatedInstrument[disposableObjectCount];
            var target = new ReferenceManager();

            for (var i = 0; i < disposableObjectCount; i++)
            {
                // Arrange.
                var disposableObject = new SimulatedInstrument(ConcurrencyControlMode.Unconstrained);
                disposableObject.StoreIntegerValue(i);
                disposableObjects[i] = disposableObject;

                // Act.
                target.AddObject(disposableObject);
                target.AddObject(new Object());
                target.AddObject((Object)null);

                // Assert.
                target.ObjectCount.Should().Be((i + 1) * 3);
            }

            // Act.
            target.Dispose();

            // Assert.
            target.ObjectCount.Should().Be(0);

            foreach (var disposableObject in disposableObjects)
            {
                // Assert.
                disposableObject.NullableIntegerValue.Should().BeNull();
            }
        }
    }
}