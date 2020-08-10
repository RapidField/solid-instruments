// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.DataAccess;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.Command.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for data access command handlers.
    /// </summary>
    public class DotNetNativeDataAccessCommandHandlerModule : DotNetNativeDataAccessCommandHandlerModule<IDataAccessCommandHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeDataAccessCommandHandlerModule" /> class.
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
        public DotNetNativeDataAccessCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

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

    /// <summary>
    /// Encapsulates native .NET container configuration for data access command handlers.
    /// </summary>
    /// <typeparam name="TBaseDataAccessCommandHandler">
    /// The base class or implemented interface type that is shared by all data access command handler types that are registered by
    /// the module.
    /// </typeparam>
    public abstract class DotNetNativeDataAccessCommandHandlerModule<TBaseDataAccessCommandHandler> : DotNetNativeCommandHandlerModule<TBaseDataAccessCommandHandler>, IDataAccessCommandHandlerModule<ServiceCollection, TBaseDataAccessCommandHandler>
        where TBaseDataAccessCommandHandler : IDataAccessCommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DotNetNativeDataAccessCommandHandlerModule{TBaseDataAccessCommandHandler}" /> class.
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

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DotNetNativeDataAccessCommandHandlerModule{TBaseDataAccessCommandHandler}" /> class.
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
        protected DotNetNativeDataAccessCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }
    }
}