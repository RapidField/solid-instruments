// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.ObjectComposition.UnitTests
{
    [TestClass]
    public class FactoryProducedInstanceGroupTests
    {
        [TestMethod]
        public void Get_ShouldRaiseArgumentException_ForUnsupportedTypes()
        {
            // Arrange.
            using (var factory = new SimulatedInstrumentFactory())
            {
                using (var target = new FactoryProducedInstanceGroup(factory))
                {
                    // Act.
                    var action = new Action(() =>
                    {
                        target.Get<DateTimeRange>();
                    });

                    // Assert.
                    action.Should().Throw<ArgumentException>();
                }
            }
        }

        [TestMethod]
        public void Get_ShouldReturnValidInstance_ForSupportedTypes()
        {
            // Arrange.
            using (var factory = new SimulatedInstrumentFactory())
            {
                using (var target = new FactoryProducedInstanceGroup(factory))
                {
                    // Act.
                    var result = target.Get<SimulatedInstrument>();

                    // Assert.
                    result.Should().NotBeNull();
                    result.Should().BeOfType<SimulatedInstrument>();
                }
            }
        }

        [TestMethod]
        public void GetLazy_ShouldReturnValidInstance_ForSupportedTypes()
        {
            // Arrange.
            using (var factory = new SimulatedInstrumentFactory())
            {
                using (var target = new FactoryProducedInstanceGroup(factory))
                {
                    // Act.
                    var result = target.GetLazy<SimulatedInstrument>();

                    // Assert.
                    result.Should().NotBeNull();
                    result.Should().BeOfType<Lazy<SimulatedInstrument>>();
                    result.IsValueCreated.Should().BeFalse();
                    result.Value.Should().NotBeNull();
                    result.Value.Should().BeOfType<SimulatedInstrument>();
                }
            }
        }
    }
}