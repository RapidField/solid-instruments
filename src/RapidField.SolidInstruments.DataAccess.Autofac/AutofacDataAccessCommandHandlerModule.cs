// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command.Autofac;
using System;

namespace RapidField.SolidInstruments.DataAccess.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for data access command handlers.
    /// </summary>
    public abstract class AutofacDataAccessCommandHandlerModule : AutofacCommandHandlerModule, IDataAccessCommandHandlerModule<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDataAccessCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacDataAccessCommandHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}