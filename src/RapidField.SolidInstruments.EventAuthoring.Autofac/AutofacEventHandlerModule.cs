// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using Microsoft.Extensions.Configuration;
using RapidField.SolidInstruments.Command.Autofac;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.EventAuthoring.Autofac
{
    /// <summary>
    /// Encapsulates Autofac container configuration for event handlers.
    /// </summary>
    public class AutofacEventHandlerModule : AutofacCommandHandlerModule<IEventHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacEventHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which event handler types are registered. The default assembly is the assembly in which the derived
        /// module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        public AutofacEventHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacEventHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacEventHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }
    }

    /// <summary>
    /// Encapsulates Autofac container configuration for event handlers.
    /// </summary>
    /// <typeparam name="TBaseEventHandler">
    /// The base class or implemented interface type that is shared by all event handler types that are registered by the module.
    /// </typeparam>
    public abstract class AutofacEventHandlerModule<TBaseEventHandler> : AutofacCommandHandlerModule<TBaseEventHandler>, IEventHandlerModule<ContainerBuilder, TBaseEventHandler>
        where TBaseEventHandler : IEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacEventHandlerModule{TBaseEventHandler}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" />.
        /// </exception>
        protected AutofacEventHandlerModule(IConfiguration applicationConfiguration)
            : base(applicationConfiguration)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacEventHandlerModule{TBaseEventHandler}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which event handler types are registered. The default assembly is the assembly in which the derived
        /// module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        protected AutofacEventHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }
    }
}