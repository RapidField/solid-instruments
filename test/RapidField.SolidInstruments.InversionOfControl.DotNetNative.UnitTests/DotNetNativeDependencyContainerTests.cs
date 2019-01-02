// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Command;

namespace RapidField.SolidInstruments.InversionOfControl.DotNetNative.UnitTests
{
    [TestClass]
    public class DotNetNativeDependencyContainerTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var package = new SimulatedDotNetNativeDependencyPackage();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var scopeOne = (IDependencyScope)null;
            var scopeTwo = (IDependencyScope)null;
            var scopeThree = (IDependencyScope)null;
            var instrumentOne = (SimulatedInstrument)null;
            var instrumentTwo = (SimulatedInstrument)null;
            var instrumentThree = (SimulatedInstrument)null;
            var instrumentFour = (SimulatedInstrument)null;
            var target = (IDependencyContainer)null;

            using (var engine = package.CreateEngine(configuration))
            {
                // Act.
                target = engine.Container;
                scopeOne = target.CreateScope();
                scopeTwo = target.CreateScope();
                scopeThree = scopeOne.CreateChildScope();

                // Assert.
                scopeOne.Should().NotBeNull();
                scopeTwo.Should().NotBeNull();

                // Act.
                instrumentOne = scopeOne.Resolve<SimulatedInstrument>();
                instrumentTwo = scopeOne.Resolve<SimulatedInstrument>();
                instrumentThree = scopeTwo.Resolve<SimulatedInstrument>();
                instrumentFour = scopeThree.Resolve<SimulatedInstrument>();

                // Assert.
                instrumentOne.Should().NotBeNull();
                instrumentTwo.Should().NotBeNull();
                instrumentThree.Should().NotBeNull();
                instrumentFour.Should().NotBeNull();

                // Act.
                instrumentOne.StoreIntegerValue(1);
                instrumentTwo.StoreIntegerValue(2);
                instrumentThree.StoreIntegerValue(3);
                instrumentFour.StoreIntegerValue(4);

                // Assert.
                instrumentOne.NullableIntegerValue.Should().Be(2, $"because {nameof(instrumentOne)} and {nameof(instrumentTwo)} are the same instance");
                instrumentTwo.NullableIntegerValue.Should().Be(2);
                instrumentThree.NullableIntegerValue.Should().Be(3);
                instrumentFour.NullableIntegerValue.Should().Be(4);

                // Act.
                scopeOne.Dispose();

                // Assert.
                instrumentOne.NullableIntegerValue.Should().BeNull();
                instrumentTwo.NullableIntegerValue.Should().BeNull();
                instrumentThree.NullableIntegerValue.Should().Be(3);
                instrumentFour.NullableIntegerValue.Should().BeNull();
            }

            // Assert.
            instrumentOne.NullableIntegerValue.Should().BeNull();
            instrumentTwo.NullableIntegerValue.Should().BeNull();
            instrumentThree.NullableIntegerValue.Should().BeNull();
            instrumentFour.NullableIntegerValue.Should().BeNull();
        }

        [TestMethod]
        public void ShouldProduceInstancePerDependencyCommandMediators()
        {
            // Arrange.
            var package = new SimulatedDotNetNativeDependencyPackage();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var scope = (IDependencyScope)null;
            var commandMediatorOne = (ICommandMediator)null;
            var commandMediatorTwo = (ICommandMediator)null;
            var target = (IDependencyContainer)null;

            using (var engine = package.CreateEngine(configuration))
            {
                // Arrange.
                target = engine.Container;
                scope = target.CreateScope();

                // Act.
                commandMediatorOne = scope.Resolve<ICommandMediator>();
                commandMediatorTwo = scope.Resolve<ICommandMediator>();

                // Assert.
                commandMediatorOne.Should().NotBeNull();
                commandMediatorTwo.Should().NotBeNull();
                commandMediatorOne.Should().NotBeSameAs(commandMediatorTwo);
            }
        }
    }
}