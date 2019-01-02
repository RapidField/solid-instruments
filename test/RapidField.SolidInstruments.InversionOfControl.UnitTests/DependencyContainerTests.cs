// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.InversionOfControl.UnitTests
{
    [TestClass]
    public class DependencyContainerTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var configuration = new ConfigurationBuilder().Build();
            var parentScope = (IDependencyScope)null;
            var childScope = (IDependencyScope)null;
            var instrument = (SimulatedInstrument)null;
            var isConfigured = false;
            var configureAction = new Action<IConfiguration, SimulatedSourceConfigurator>((applicationConfiguration, configurator) =>
            {
                isConfigured = true;
            });

            using (var target = new SimulatedDependecyContainer(configuration, configureAction))
            {
                // Act.
                parentScope = target.CreateScope();
                childScope = parentScope?.CreateChildScope();
                instrument = childScope?.Resolve<SimulatedInstrument>();

                // Assert.
                parentScope.Should().NotBeNull();
                childScope.Should().NotBeNull();
                instrument.Should().NotBeNull();
                instrument.Should().BeOfType<SimulatedInstrument>();
                instrument.NullableIntegerValue.Should().NotBeNull();
                instrument.NullableIntegerValue.Should().Be(54);
                isConfigured.Should().BeTrue();
            }

            // Assert.
            instrument.NullableIntegerValue.Should().BeNull();
        }
    }
}