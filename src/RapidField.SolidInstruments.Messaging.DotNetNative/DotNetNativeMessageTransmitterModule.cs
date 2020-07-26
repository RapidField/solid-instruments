// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RapidField.SolidInstruments.Messaging.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for message transmitters.
    /// </summary>
    public abstract class DotNetNativeMessageTransmitterModule : DotNetNativeMessageHandlerModule, IMessageTransmitterModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeMessageTransmitterModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeMessageTransmitterModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}