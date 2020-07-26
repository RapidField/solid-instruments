// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.InversionOfControl.Autofac;
using System;

namespace RapidField.SolidInstruments.Command.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for command handlers.
    /// </summary>
    public abstract class AutofacCommandHandlerModule : AutofacDependencyModule, ICommandHandlerModule<ContainerBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacCommandHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }
}