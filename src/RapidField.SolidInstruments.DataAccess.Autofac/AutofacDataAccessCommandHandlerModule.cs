// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command.Autofac;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.DataAccess.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for data access command handlers.
    /// </summary>
    public class AutofacDataAccessCommandHandlerModule : AutofacDataAccessCommandHandlerModule<IDataAccessCommandHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDataAccessCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which data access command handler types are registered. The default assembly is the assembly in which
        /// the derived module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        public AutofacDataAccessCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

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

    /// <summary>
    /// Encapsulates Autofac container configuration for data access command handlers.
    /// </summary>
    /// <typeparam name="TBaseDataAccessCommandHandler">
    /// The base class or implemented interface type that is shared by all data access command handler types that are registered by
    /// the module.
    /// </typeparam>
    public abstract class AutofacDataAccessCommandHandlerModule<TBaseDataAccessCommandHandler> : AutofacCommandHandlerModule<TBaseDataAccessCommandHandler>, IDataAccessCommandHandlerModule<ContainerBuilder, TBaseDataAccessCommandHandler>
        where TBaseDataAccessCommandHandler : IDataAccessCommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDataAccessCommandHandlerModule{TBaseDataAccessCommandHandler}" />
        /// class.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDataAccessCommandHandlerModule{TBaseDataAccessCommandHandler}" />
        /// class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which data access command handler types are registered. The default assembly is the assembly in which
        /// the derived module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        protected AutofacDataAccessCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }
    }
}