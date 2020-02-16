// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;

namespace RapidField.SolidInstruments.Example.DatabaseModel
{
    /// <summary>
    /// Encapsulates configuration for Example contracts dependencies.
    /// </summary>
    public sealed class ExampleContractsDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleContractsDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public ExampleContractsDependencyModule(IConfiguration applicationConfiguration)
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
            // Register queue transmitters.
            configurator.AddTransient<ICommandHandler<ApplicationStartedEventMessage>, QueueTransmitter<ApplicationStartedEventMessage>>();
            configurator.AddTransient<ICommandHandler<ApplicationStoppedEventMessage>, QueueTransmitter<ApplicationStoppedEventMessage>>();

            // Register topic transmitters.
            configurator.AddTransient<ICommandHandler<ExceptionRaisedEventMessage>, TopicTransmitter<ExceptionRaisedEventMessage>>();

            // Register request transmitters.
            configurator.AddTransient<ICommandHandler<PingRequestMessage>, RequestTransmitter<PingRequestMessage, PingResponseMessage>>();
        }
    }
}