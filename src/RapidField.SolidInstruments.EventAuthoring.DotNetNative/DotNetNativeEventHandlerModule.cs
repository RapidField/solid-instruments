// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command.DotNetNative;
using System;
using System.Reflection;

namespace RapidField.SolidInstruments.EventAuthoring.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for event handlers.
    /// </summary>
    public class DotNetNativeEventHandlerModule : DotNetNativeCommandHandlerModule<IEventHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeEventHandlerModule" /> class.
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
        public DotNetNativeEventHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

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

    /// <summary>
    /// Encapsulates native .NET container configuration for event handlers.
    /// </summary>
    /// <typeparam name="TBaseEventHandler">
    /// The base class or implemented interface type that is shared by all event handler types that are registered by the module.
    /// </typeparam>
    public abstract class DotNetNativeEventHandlerModule<TBaseEventHandler> : DotNetNativeCommandHandlerModule<TBaseEventHandler>, IEventHandlerModule<ServiceCollection, TBaseEventHandler>
        where TBaseEventHandler : IEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeEventHandlerModule{TBaseEventHandler}" /> class.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeEventHandlerModule{TBaseEventHandler}" /> class.
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
        protected DotNetNativeEventHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }
    }
}