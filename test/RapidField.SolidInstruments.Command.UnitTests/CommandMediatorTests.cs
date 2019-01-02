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
    public class CommandMediatorTests
    {
        [TestMethod]
        public void Process_ShouldProcessCommand_WithoutResult()
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

                // Act.
                var result = commandMediator.Process(command);

                // Assert.
                command.IsProcessed.Should().BeTrue();
                result.Should().BeOfType<Nix>();
            }
        }

        [TestMethod]
        public void Process_ShouldProcessCommand_WithResult()
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

                // Act.
                var result = commandMediator.Process(command);

                // Assert.
                command.IsProcessed.Should().BeTrue();
                result.Should().Be(command.Identifier);
            }
        }

        [TestMethod]
        public void ProcessAsync_ShouldProcessCommand_WithoutResult()
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

                // Act.
                var task = commandMediator.ProcessAsync(command);
                task.Wait();
                var result = task.Result;

                // Assert.
                command.IsProcessed.Should().BeTrue();
                result.Should().BeOfType<Nix>();
            }
        }

        [TestMethod]
        public void ProcessAsync_ShouldProcessCommand_WithResult()
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

                // Act.
                var task = commandMediator.ProcessAsync(command);
                task.Wait();
                var result = task.Result;

                // Assert.
                command.IsProcessed.Should().BeTrue();
                result.Should().Be(command.Identifier);
            }
        }
    }
}