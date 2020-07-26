// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Collections;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.ObjectComposition.UnitTests
{
    [TestClass]
    public class ObjectFactoryTests
    {
        [TestMethod]
        public void Produce_ShouldRaiseArgumentException_ForUnsupportedType()
        {
            // Arrange.
            using (var target = new SimulatedInstrumentFactory())
            {
                // Act.
                var action = new Action(() =>
                {
                    target.Produce<ReferenceManager>();
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void Produce_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType()
        {
            // Arrange.
            using (var target = new SimulatedInstrumentFactory())
            {
                // Act.
                var result = target.Produce<CircularBuffer<Int32>>();

                // Assert.
                result.Should().NotBeNull();
                result.Should().BeOfType<CircularBuffer<Int32>>();
            }
        }

        [TestMethod]
        public void SupportedProductTypes_ShouldReturnConfiguredTypes()
        {
            // Arrange.
            using (var target = new SimulatedInstrumentFactory())
            {
                // Act.
                var supportedProductTypes = target.SupportedProductTypes;

                // Assert.
                supportedProductTypes.Should().Contain(typeof(PinnedMemory<Int16>));
                supportedProductTypes.Should().Contain(typeof(CircularBuffer<Int32>));
            }
        }
    }
}