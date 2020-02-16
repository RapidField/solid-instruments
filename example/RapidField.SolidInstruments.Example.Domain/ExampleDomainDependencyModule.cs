// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.Example.Domain.MessageListeners;
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
            // Register queue listeners.
            configurator.AddTransient<IMessageListener<ApplicationStartedEventMessage>, ApplicationStartedEventMessageListener>();
            configurator.AddTransient<IMessageListener<ApplicationStoppedEventMessage>, ApplicationStoppedEventMessageListener>();
            configurator.AddTransient<IMessageListener<ExceptionRaisedEventMessage>, ExceptionRaisedEventMessageListener>();

            // Register topic listeners.
            configurator.AddTransient<IMessageListener<HeartbeatMessage>, HeartbeatMessageListener>();

            // Register request listeners.
            configurator.AddTransient<IMessageListener<PingRequestMessage, PingResponseMessage>, PingRequestMessageListener>();
        }
    }
}