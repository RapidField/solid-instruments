// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;
using System;
using System.Linq;
using System.Threading;
using CreateCustomerCommand = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Commands.ModelState.Customer.CreateDomainModelCommand;
using CreateCustomerCommandMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage;
using CustomerModel = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Models.Customer.DomainModel;
using PingRequestMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping.RequestMessage;
using PingResponseMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping.ResponseMessage;
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
            // Arrange.
            var customers = SimulatedServiceState.Customers;
            var acmeCoCustomer = CustomerModel.Named.AcmeCo;
            var smithIndustriesCustomer = CustomerModel.Named.SmithIndustries;
            var newAcmeCoCustomerName = "New Acme Corporation";

            // Act.
            Thread.Sleep(2584);
            mediator.Process(new CreateCustomerCommandMessage(new CreateCustomerCommand(acmeCoCustomer)));
            mediator.Process(new CreateCustomerCommandMessage(new CreateCustomerCommand(smithIndustriesCustomer)));

            // Assert.
            ActionRepeater.BuildAndRun(builder => builder.WithConstantDelay(987).WithTimeoutThreshold(46368).While(() => customers.Count < 2));
            customers.Count.Should().Be(2);

            // Act.
            acmeCoCustomer.Name = newAcmeCoCustomerName;
            mediator.Process(new UpdateCustomerCommandMessage(new UpdateCustomerCommand(acmeCoCustomer)));

            // Assert.
            ActionRepeater.BuildAndRun(builder => builder.WithConstantDelay(987).WithTimeoutThreshold(46368).While(() => customers.Where(entity => entity.Identifier == acmeCoCustomer.Identifier).Single().Name != newAcmeCoCustomerName));
            customers.Where(entity => entity.Identifier == acmeCoCustomer.Identifier).Single().Name.Should().Be(newAcmeCoCustomerName);

            // Act.
            var pingCorrelationIdentifier = Guid.NewGuid();
            var response = mediator.Process<PingResponseMessage>(new PingRequestMessage(pingCorrelationIdentifier));

            // Assert.
            response.CorrelationIdentifier.Should().Be(pingCorrelationIdentifier);
        }
    }
}