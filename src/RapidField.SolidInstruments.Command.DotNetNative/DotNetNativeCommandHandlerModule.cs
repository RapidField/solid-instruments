// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Command.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for command handlers.
    /// </summary>
    public abstract class DotNetNativeCommandHandlerModule : DotNetNativeDependencyModule, ICommandHandlerModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeCommandHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}