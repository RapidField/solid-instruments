// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Command;
using System.Linq;
using System.Threading;
using CreateCustomerCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.CreateDomainModelCommand;
using CreateCustomerCommandMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage;
using CustomerModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using UpdateCustomerCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.UpdateDomainModelCommand;
using UpdateCustomerCommandMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    [TestClass]
    public class SimulatedMessagingServiceExecutorTests
    {
        [TestMethod]
        public void ShouldProduceDesiredResults()
        {
            using (var serviceExecutor = new SimulatedMessagingServiceExecutor(OnStarting, OnStopping))
            {
                serviceExecutor.Execute();
            }
        }

        private void OnStarting(ICommandMediator mediator)
        {
            // Assert.
            SimulatedServiceState.Customers.Should().BeEmpty();
            SimulatedServiceState.CustomerOrders.Should().BeEmpty();
            SimulatedServiceState.Products.Should().BeEmpty();
        }

        private void OnStopping(ICommandMediator mediator)
        {
            Thread.Sleep(2584);

            // Arrange.
            var acmeCoCustomer = CustomerModel.Named.AcmeCo;
            var smithIndustriesCustomer = CustomerModel.Named.SmithIndustries;

            // Act.
            mediator.Process(new CreateCustomerCommandMessage(new CreateCustomerCommand(acmeCoCustomer)));
            mediator.Process(new CreateCustomerCommandMessage(new CreateCustomerCommand(smithIndustriesCustomer)));

            // Assert.
            Thread.Sleep(6765);
            SimulatedServiceState.Customers.Should().HaveCount(2);

            // Act.
            acmeCoCustomer.Name = "New Acme Corporation";
            mediator.Process(new UpdateCustomerCommandMessage(new UpdateCustomerCommand(acmeCoCustomer)));

            // Assert.
            Thread.Sleep(6765);
            SimulatedServiceState.Customers.Where(entity => entity.Identifier == acmeCoCustomer.Identifier).Single().Name.Should().Be(acmeCoCustomer.Name);
        }
    }
}