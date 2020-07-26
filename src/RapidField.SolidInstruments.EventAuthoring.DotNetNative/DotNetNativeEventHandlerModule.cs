// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command.DotNetNative;
using System;

namespace RapidField.SolidInstruments.EventAuthoring.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for event handlers.
    /// </summary>
    public abstract class DotNetNativeEventHandlerModule : DotNetNativeCommandHandlerModule, IEventHandlerModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeEventHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeEventHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}