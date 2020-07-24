// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using RapidField.SolidInstruments.Messaging.DotNetNative.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests
{
    /// <summary>
    /// Encapsulates container configuration for test dependencies.
    /// </summary>
    public class SimulatedDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public SimulatedDependencyModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected override void Configure(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            // Register the configuration.
            configurator.AddApplicationConfiguration(applicationConfiguration);

            // Register messaging support types.
            configurator.AddSupportingTypesForInMemoryMessaging();

            // Register command message transmitters.
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Customer.CreateDomainModelCommand, Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Customer.DeleteDomainModelCommand, Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Customer.UpdateDomainModelCommand, Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.CustomerOrder.CreateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.CustomerOrder.DeleteDomainModelCommand, Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.CustomerOrder.UpdateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Product.CreateDomainModelCommand, Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Product.DeleteDomainModelCommand, Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.Product.UpdateDomainModelCommand, Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>();

            // Register event message transmitters.
            configurator.AddEventMessageTransmitter<Events.ModelState.Customer.DomainModelCreatedEvent, Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.Customer.DomainModelDeletedEvent, Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.Customer.DomainModelUpdatedEvent, Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelCreatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelDeletedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelUpdatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.Product.DomainModelCreatedEvent, Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.Product.DomainModelDeletedEvent, Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.Product.DomainModelUpdatedEvent, Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>();

            // Register request message transmitters.
            configurator.AddRequestMessageTransmitter<Messages.RequestResponse.Ping.RequestMessage, Messages.RequestResponse.Ping.ResponseMessage>();

            // Register command message listeners and handlers.
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Customer.CreateDomainModelCommand, Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage, CommandHandlers.ModelState.Customer.CreateDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Customer.DeleteDomainModelCommand, Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage, CommandHandlers.ModelState.Customer.DeleteDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Customer.UpdateDomainModelCommand, Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage, CommandHandlers.ModelState.Customer.UpdateDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.CustomerOrder.CreateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage, CommandHandlers.ModelState.CustomerOrder.CreateDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.CustomerOrder.DeleteDomainModelCommand, Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage, CommandHandlers.ModelState.CustomerOrder.DeleteDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.CustomerOrder.UpdateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage, CommandHandlers.ModelState.CustomerOrder.UpdateDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Product.CreateDomainModelCommand, Messages.Command.ModelState.Product.CreateDomainModelCommandMessage, CommandHandlers.ModelState.Product.CreateDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Product.DeleteDomainModelCommand, Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage, CommandHandlers.ModelState.Product.DeleteDomainModelCommandHandler>();
            configurator.AddCommandMessageListenerAndHandler<Commands.ModelState.Product.UpdateDomainModelCommand, Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage, CommandHandlers.ModelState.Product.UpdateDomainModelCommandHandler>();

            // Register event message listeners and handlers.
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Customer.DomainModelCreatedEvent, Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage, EventHandlers.ModelState.Customer.DomainModelCreatedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Customer.DomainModelDeletedEvent, Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage, EventHandlers.ModelState.Customer.DomainModelDeletedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Customer.DomainModelUpdatedEvent, Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage, EventHandlers.ModelState.Customer.DomainModelUpdatedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.CustomerOrder.DomainModelCreatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage, EventHandlers.ModelState.CustomerOrder.DomainModelCreatedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.CustomerOrder.DomainModelDeletedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage, EventHandlers.ModelState.CustomerOrder.DomainModelDeletedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.CustomerOrder.DomainModelUpdatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage, EventHandlers.ModelState.CustomerOrder.DomainModelUpdatedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Product.DomainModelCreatedEvent, Messages.Event.ModelState.Product.DomainModelCreatedEventMessage, EventHandlers.ModelState.Product.DomainModelCreatedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Product.DomainModelDeletedEvent, Messages.Event.ModelState.Product.DomainModelDeletedEventMessage, EventHandlers.ModelState.Product.DomainModelDeletedEventHandler>();
            configurator.AddEventMessageListenerAndHandler<Events.ModelState.Product.DomainModelUpdatedEvent, Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage, EventHandlers.ModelState.Product.DomainModelUpdatedEventHandler>();

            // Register request message listeners.
            configurator.AddRequestMessageListener<Messages.RequestResponse.Ping.RequestMessage, Messages.RequestResponse.Ping.ResponseMessage, MessageListeners.RequestResponse.Ping.RequestListener>();
        }
    }
}