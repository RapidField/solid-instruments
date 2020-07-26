// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.DatabaseModel;
using RapidField.SolidInstruments.Example.DatabaseModel.CommandHandlers;
using RapidField.SolidInstruments.Example.DatabaseModel.Commands;
using RapidField.SolidInstruments.TextEncoding;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework.UnitTests
{
    [TestClass]
    public class EntityFrameworkCommandHandlerTests
    {
        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults()
        {
            // Arrange.
            var package = new SimulatedAutofacDependencyPackage();
            var databaseName = EnhancedReadabilityGuid.New().ToString();
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var command = new GetFibonacciNumberValuesCommand();
            var fibonacciNumberSeriesValues = new Int64[] { 0, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };

            using (var engine = package.CreateEngine(configuration))
            {
                // Arrange.
                var scope = engine.Container.CreateScope();
                var commandMediator = scope.Resolve<ICommandMediator>();

                using (var context = new ExampleInMemoryContext(configuration, databaseName).WithTestData())
                {
                    using (var repositoryFactory = new ExampleRepositoryFactory(context, configuration))
                    {
                        using (var commandHandler = new GetFibonacciNumberValuesCommandHandler(commandMediator, repositoryFactory))
                        {
                            // Act.
                            var result = commandHandler.Process(command);

                            // Assert.
                            result.Should().BeEquivalentTo(fibonacciNumberSeriesValues);
                        }
                    }
                }
            }
        }
    }
}