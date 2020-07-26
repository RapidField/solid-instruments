// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.DataAccess;
using System;

namespace RapidField.SolidInstruments.Command.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for data access command handlers.
    /// </summary>
    public abstract class DotNetNativeDataAccessCommandHandlerModule : DotNetNativeCommandHandlerModule, IDataAccessCommandHandlerModule<ServiceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDataAccessCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected DotNetNativeDataAccessCommandHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}