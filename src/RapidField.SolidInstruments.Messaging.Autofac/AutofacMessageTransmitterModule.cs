// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using System;

namespace RapidField.SolidInstruments.Messaging.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for message transmitters.
    /// </summary>
    public abstract class AutofacMessageTransmitterModule : AutofacMessageHandlerModule, IMessageTransmitterModule<ContainerBuilder>
    {
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
            : base(applicationConfiguration)
        {
            return;
        }
    }
}