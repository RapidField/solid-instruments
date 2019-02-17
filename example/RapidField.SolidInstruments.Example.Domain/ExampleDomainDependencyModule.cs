// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.Service;
using RapidField.SolidInstruments.Prototype.Contracts.Messages;
using RapidField.SolidInstruments.Prototype.Domain.MessageSubscribers;
using System;

namespace RapidField.SolidInstruments.Prototype.Domain
{
    /// <summary>
    /// Encapsulates configuration for Prototype domain dependencies.
    /// </summary>
    public sealed class PrototypeDomainDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrototypeDomainDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public PrototypeDomainDependencyModule(IConfiguration applicationConfiguration)
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
            // Register queue subscribers.
            configurator.AddTransient<IMessageSubscriber<ApplicationStartingMessage>, ApplicationStartingMessageSubscriber>();
            configurator.AddTransient<IMessageSubscriber<ApplicationStoppingMessage>, ApplicationStoppingMessageSubscriber>();
            configurator.AddTransient<IMessageSubscriber<ExceptionRaisedMessage>, ExceptionRaisedMessageSubscriber>();

            // Register topic subscribers.
            configurator.AddTransient<IMessageSubscriber<HeartbeatMessage>, HeartbeatMessageSubscriber>();

            // Register request subscribers.
            configurator.AddTransient<IMessageSubscriber<PingRequestMessage, PingResponseMessage>, PingRequestMessageSubscriber>();
        }
    }
}