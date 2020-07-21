// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.TransportPrimitives;
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
            configurator.AddSingleton(applicationConfiguration);

            // Register messaging types.
            var messageTransport = MessageTransport.Instance;
            var messageTransportConnection = messageTransport.CreateConnection();
            configurator.AddSingleton(messageTransport);
            configurator.AddSingleton<IMessageTransport, MessageTransport>((serviceProvider) => serviceProvider.GetService<MessageTransport>());
            configurator.AddSingleton(messageTransportConnection);
            configurator.AddScoped<InMemoryMessageAdapter>();
            configurator.AddScoped<IMessageAdapter<PrimitiveMessage>, InMemoryMessageAdapter>((serviceProvider) => serviceProvider.GetService<InMemoryMessageAdapter>());
            configurator.AddScoped<InMemoryClientFactory>();
            configurator.AddScoped<IMessagingClientFactory<IMessagingEntitySendClient, IMessagingEntityReceiveClient, PrimitiveMessage>, InMemoryClientFactory>((serviceProvider) => serviceProvider.GetService<InMemoryClientFactory>());
            configurator.AddScoped<InMemoryTransmittingFacade>();
            configurator.AddScoped<IMessageTransmittingFacade, InMemoryTransmittingFacade>((serviceProvider) => serviceProvider.GetService<InMemoryTransmittingFacade>());
            configurator.AddSingleton<InMemoryListeningFacade>();
            configurator.AddSingleton<IMessageListeningFacade, InMemoryListeningFacade>((serviceProvider) => serviceProvider.GetService<InMemoryListeningFacade>());
            configurator.AddSingleton<InMemoryRequestingFacade>();
            configurator.AddSingleton<IMessageRequestingFacade, InMemoryRequestingFacade>((serviceProvider) => serviceProvider.GetService<InMemoryRequestingFacade>());

            // Register queue transmitters.
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Customer.CreateDomainModelCommand, Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Customer.DeleteDomainModelCommand, Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Customer.UpdateDomainModelCommand, Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.CustomerOrder.CreateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.CustomerOrder.DeleteDomainModelCommand, Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.CustomerOrder.UpdateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Product.CreateDomainModelCommand, Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Product.DeleteDomainModelCommand, Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>, CommandMessageTransmitter<Commands.ModelState.Product.UpdateDomainModelCommand, Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>>();

            // Register topic transmitters.
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>, EventMessageTransmitter<Events.ModelState.Customer.DomainModelCreatedEvent, Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>, EventMessageTransmitter<Events.ModelState.Customer.DomainModelDeletedEvent, Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>, EventMessageTransmitter<Events.ModelState.Customer.DomainModelUpdatedEvent, Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>, EventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelCreatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>, EventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelDeletedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>, EventMessageTransmitter<Events.ModelState.CustomerOrder.DomainModelUpdatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>, EventMessageTransmitter<Events.ModelState.Product.DomainModelCreatedEvent, Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>, EventMessageTransmitter<Events.ModelState.Product.DomainModelDeletedEvent, Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<ICommandHandler<Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>, EventMessageTransmitter<Events.ModelState.Product.DomainModelUpdatedEvent, Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>>();

            // Register queue listeners.
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Customer.CreateDomainModelCommand, Messages.Command.ModelState.Customer.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Customer.DeleteDomainModelCommand, Messages.Command.ModelState.Customer.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Customer.UpdateDomainModelCommand, Messages.Command.ModelState.Customer.UpdateDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.CustomerOrder.CreateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.CustomerOrder.DeleteDomainModelCommand, Messages.Command.ModelState.CustomerOrder.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.CustomerOrder.UpdateDomainModelCommand, Messages.Command.ModelState.CustomerOrder.UpdateDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Product.CreateDomainModelCommand, Messages.Command.ModelState.Product.CreateDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Product.DeleteDomainModelCommand, Messages.Command.ModelState.Product.DeleteDomainModelCommandMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>, CommandMessageListener<Commands.ModelState.Product.UpdateDomainModelCommand, Messages.Command.ModelState.Product.UpdateDomainModelCommandMessage>>();

            // Register topic listeners.
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>, EventMessageListener<Events.ModelState.Customer.DomainModelCreatedEvent, Messages.Event.ModelState.Customer.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>, EventMessageListener<Events.ModelState.Customer.DomainModelDeletedEvent, Messages.Event.ModelState.Customer.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>, EventMessageListener<Events.ModelState.Customer.DomainModelUpdatedEvent, Messages.Event.ModelState.Customer.DomainModelUpdatedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>, EventMessageListener<Events.ModelState.CustomerOrder.DomainModelCreatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>, EventMessageListener<Events.ModelState.CustomerOrder.DomainModelDeletedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>, EventMessageListener<Events.ModelState.CustomerOrder.DomainModelUpdatedEvent, Messages.Event.ModelState.CustomerOrder.DomainModelUpdatedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>, EventMessageListener<Events.ModelState.Product.DomainModelCreatedEvent, Messages.Event.ModelState.Product.DomainModelCreatedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>, EventMessageListener<Events.ModelState.Product.DomainModelDeletedEvent, Messages.Event.ModelState.Product.DomainModelDeletedEventMessage>>();
            configurator.AddTransient<IMessageListener<Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>, EventMessageListener<Events.ModelState.Product.DomainModelUpdatedEvent, Messages.Event.ModelState.Product.DomainModelUpdatedEventMessage>>();

            // Register command handlers.
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Customer.CreateDomainModelCommand>, CommandHandlers.ModelState.Customer.CreateDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Customer.DeleteDomainModelCommand>, CommandHandlers.ModelState.Customer.DeleteDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Customer.UpdateDomainModelCommand>, CommandHandlers.ModelState.Customer.UpdateDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.CustomerOrder.CreateDomainModelCommand>, CommandHandlers.ModelState.CustomerOrder.CreateDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.CustomerOrder.DeleteDomainModelCommand>, CommandHandlers.ModelState.CustomerOrder.DeleteDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.CustomerOrder.UpdateDomainModelCommand>, CommandHandlers.ModelState.CustomerOrder.UpdateDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Product.CreateDomainModelCommand>, CommandHandlers.ModelState.Product.CreateDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Product.DeleteDomainModelCommand>, CommandHandlers.ModelState.Product.DeleteDomainModelCommandHandler>();
            configurator.AddTransient<ICommandHandler<Commands.ModelState.Product.UpdateDomainModelCommand>, CommandHandlers.ModelState.Product.UpdateDomainModelCommandHandler>();

            // Register event handlers.
            configurator.AddTransient<ICommandHandler<Events.ModelState.Customer.DomainModelCreatedEvent>, EventHandlers.ModelState.Customer.DomainModelCreatedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.Customer.DomainModelDeletedEvent>, EventHandlers.ModelState.Customer.DomainModelDeletedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.Customer.DomainModelUpdatedEvent>, EventHandlers.ModelState.Customer.DomainModelUpdatedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.CustomerOrder.DomainModelCreatedEvent>, EventHandlers.ModelState.CustomerOrder.DomainModelCreatedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.CustomerOrder.DomainModelDeletedEvent>, EventHandlers.ModelState.CustomerOrder.DomainModelDeletedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.CustomerOrder.DomainModelUpdatedEvent>, EventHandlers.ModelState.CustomerOrder.DomainModelUpdatedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.Product.DomainModelCreatedEvent>, EventHandlers.ModelState.Product.DomainModelCreatedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.Product.DomainModelDeletedEvent>, EventHandlers.ModelState.Product.DomainModelDeletedEventHandler>();
            configurator.AddTransient<ICommandHandler<Events.ModelState.Product.DomainModelUpdatedEvent>, EventHandlers.ModelState.Product.DomainModelUpdatedEventHandler>();
        }
    }
}