// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using RapidField.SolidInstruments.Prototype.Contracts.Messages;
using System;

namespace RapidField.SolidInstruments.Prototype.DatabaseModel
{
    /// <summary>
    /// Encapsulates configuration for Prototype contracts dependencies.
    /// </summary>
    public sealed class PrototypeContractsDependencyModule : DotNetNativeDependencyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrototypeContractsDependencyModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public PrototypeContractsDependencyModule(IConfiguration applicationConfiguration)
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
            // Register queue publishers.
            configurator.AddTransient<ICommandHandler<ApplicationStartingMessage>, QueuePublisher<ApplicationStartingMessage>>();
            configurator.AddTransient<ICommandHandler<ApplicationStoppingMessage>, QueuePublisher<ApplicationStoppingMessage>>();

            // Register topic publishers.
            configurator.AddTransient<ICommandHandler<ExceptionRaisedMessage>, TopicPublisher<ExceptionRaisedMessage>>();

            // Register request publishers.
            configurator.AddTransient<ICommandHandler<PingRequestMessage>, RequestPublisher<PingRequestMessage, PingResponseMessage>>();
        }
    }
}