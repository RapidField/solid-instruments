// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.InversionOfControl;

namespace RapidField.SolidInstruments.Command.UnitTests
{
    [TestClass]
    public class CommandHandlerTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForCommandWithoutResult()
        {
            // Arrange.
            var package = new SimulatedAutofacDependencyPackage();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var scope = (IDependencyScope)null;
            var commandMediator = (ICommandMediator)null;
            var command = new SimulatedCommand();

            using (var engine = package.CreateEngine(configuration))
            {
                // Arrange.
                scope = engine.Container.CreateScope();
                commandMediator = scope.Resolve<ICommandMediator>();

                using (var target = new SimulatedCommandHandler(commandMediator))
                {
                    // Act.
                    var result = target.Process(command);

                    // Assert.
                    command.IsProcessed.Should().BeTrue();
                    result.Should().BeOfType<Nix>();
                }
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForCommandWithResult()
        {
            // Arrange.
            var package = new SimulatedAutofacDependencyPackage();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var scope = (IDependencyScope)null;
            var commandMediator = (ICommandMediator)null;
            var command = new SimulatedCommandWithResult();

            using (var engine = package.CreateEngine(configuration))
            {
                // Arrange.
                scope = engine.Container.CreateScope();
                commandMediator = scope.Resolve<ICommandMediator>();

                using (var target = new SimulatedCommandWithResultHandler(commandMediator))
                {
                    // Act.
                    var result = target.Process(command);

                    // Assert.
                    command.IsProcessed.Should().BeTrue();
                    result.Should().Be(command.Identifier);
                }
            }
        }
    }
}