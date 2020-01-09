// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.Example.Domain.MessageSubscribers;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Messaging.Service;
using System;

namespace RapidField.SolidInstruments.Example.Domain
{
    /// <summary>
    /// Encapsulates configuration for Example domain dependencies.
    /// </summary>
    public sealed class ExampleDomainDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDomainDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ExampleDomainDependencyModule(IConfiguration applicationConfiguration)
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
            configurator.AddTransient<IMessageSubscriber<ApplicationStartedEventMessage>, ApplicationStartedEventMessageSubscriber>();
            configurator.AddTransient<IMessageSubscriber<ApplicationStoppedEventMessage>, ApplicationStoppedEventMessageSubscriber>();
            configurator.AddTransient<IMessageSubscriber<ExceptionRaisedEventMessage>, ExceptionRaisedEventMessageSubscriber>();

            // Register topic subscribers.
            configurator.AddTransient<IMessageSubscriber<HeartbeatMessage>, HeartbeatMessageSubscriber>();

            // Register request subscribers.
            configurator.AddTransient<IMessageSubscriber<PingRequestMessage, PingResponseMessage>, PingRequestMessageSubscriber>();
        }
    }
}