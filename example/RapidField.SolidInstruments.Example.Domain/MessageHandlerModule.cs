// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging.DotNetNative.Extensions;
using System;

namespace RapidField.SolidInstruments.Example.Domain
{
    /// <summary>
    /// Encapsulates container configuration for message handlers.
    /// </summary>
    public sealed class MessageHandlerModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public MessageHandlerModule(IConfiguration applicationConfiguration)
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
            // Register command message listeners.
            configurator.AddCommandMessageListener<Commands.ModelState.User.CreateDomainModelCommand, Messages.Command.ModelState.User.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageListener<Commands.ModelState.User.DeleteDomainModelCommand, Messages.Command.ModelState.User.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageListener<Commands.ModelState.User.UpdateDomainModelCommand, Messages.Command.ModelState.User.UpdateDomainModelCommandMessage>();
            configurator.AddCommandMessageListener<Commands.ModelState.UserRole.CreateDomainModelCommand, Messages.Command.ModelState.UserRole.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageListener<Commands.ModelState.UserRole.DeleteDomainModelCommand, Messages.Command.ModelState.UserRole.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageListener<Commands.ModelState.UserRole.UpdateDomainModelCommand, Messages.Command.ModelState.UserRole.UpdateDomainModelCommandMessage>();

            // Register command message transmitters.
            configurator.AddCommandMessageTransmitter<Commands.ModelState.User.CreateDomainModelCommand, Messages.Command.ModelState.User.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.User.DeleteDomainModelCommand, Messages.Command.ModelState.User.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.User.UpdateDomainModelCommand, Messages.Command.ModelState.User.UpdateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.UserRole.CreateDomainModelCommand, Messages.Command.ModelState.UserRole.CreateDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.UserRole.DeleteDomainModelCommand, Messages.Command.ModelState.UserRole.DeleteDomainModelCommandMessage>();
            configurator.AddCommandMessageTransmitter<Commands.ModelState.UserRole.UpdateDomainModelCommand, Messages.Command.ModelState.UserRole.UpdateDomainModelCommandMessage>();

            // Register event message listeners.
            configurator.AddEventMessageListener<Events.ModelState.User.DomainModelCreatedEvent, Messages.Event.ModelState.User.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageListener<Events.ModelState.User.DomainModelDeletedEvent, Messages.Event.ModelState.User.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageListener<Events.ModelState.User.DomainModelUpdatedEvent, Messages.Event.ModelState.User.DomainModelUpdatedEventMessage>();
            configurator.AddEventMessageListener<Events.ModelState.UserRole.DomainModelCreatedEvent, Messages.Event.ModelState.UserRole.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageListener<Events.ModelState.UserRole.DomainModelDeletedEvent, Messages.Event.ModelState.UserRole.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageListener<Events.ModelState.UserRole.DomainModelUpdatedEvent, Messages.Event.ModelState.UserRole.DomainModelUpdatedEventMessage>();

            // Register event message transmitters.
            configurator.AddEventMessageTransmitter<Events.ModelState.User.DomainModelCreatedEvent, Messages.Event.ModelState.User.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.User.DomainModelDeletedEvent, Messages.Event.ModelState.User.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.User.DomainModelUpdatedEvent, Messages.Event.ModelState.User.DomainModelUpdatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.UserRole.DomainModelCreatedEvent, Messages.Event.ModelState.UserRole.DomainModelCreatedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.UserRole.DomainModelDeletedEvent, Messages.Event.ModelState.UserRole.DomainModelDeletedEventMessage>();
            configurator.AddEventMessageTransmitter<Events.ModelState.UserRole.DomainModelUpdatedEvent, Messages.Event.ModelState.UserRole.DomainModelUpdatedEventMessage>();

            // Register request message listeners.
            configurator.AddPingRequestMessageListener();

            // Register request message transmitters.
            configurator.AddPingRequestMessageTransmitter();
        }
    }
}