// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command.DotNetNative;
using System;

namespace RapidField.SolidInstruments.Example.Domain.AccessControl
{
    /// <summary>
    /// Encapsulates container configuration for <see cref="AccessControl" /> domain command handlers.
    /// </summary>
    public sealed class CommandHandlerModule : DotNetNativeCommandHandlerModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        public CommandHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}