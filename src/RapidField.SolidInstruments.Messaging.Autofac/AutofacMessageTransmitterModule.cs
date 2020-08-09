// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.Messaging.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for message transmitters.
    /// </summary>
    public class AutofacMessageTransmitterModule : AutofacMessageHandlerModule, IMessageTransmitterModule<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageTransmitterModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which message types are identified for handler registration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        public AutofacMessageTransmitterModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, false, true, targetAssembly)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacMessageTransmitterModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacMessageTransmitterModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration, false, true)
        {
            return;
        }
    }
}