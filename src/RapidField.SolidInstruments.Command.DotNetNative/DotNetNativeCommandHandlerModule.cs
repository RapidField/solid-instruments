// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative;
using RapidField.SolidInstruments.InversionOfControl.DotNetNative.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RapidField.SolidInstruments.Command.DotNetNative
{
    /// <summary>
    /// Encapsulates native .NET container configuration for command handlers.
    /// </summary>
    public class DotNetNativeCommandHandlerModule : DotNetNativeCommandHandlerModule<ICommandHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeCommandHandlerModule" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which command handler types are registered. The default assembly is the assembly in which the derived
        /// module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        public DotNetNativeCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration, targetAssembly)
        {
            return;
        }

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

    /// <summary>
    /// Encapsulates native .NET container configuration for command handlers.
    /// </summary>
    /// <typeparam name="TBaseCommandHandler">
    /// The base class or implemented interface type that is shared by all command handler types that are registered by the module.
    /// </typeparam>
    public abstract class DotNetNativeCommandHandlerModule<TBaseCommandHandler> : DotNetNativeDependencyModule, ICommandHandlerModule<ServiceCollection, TBaseCommandHandler>
        where TBaseCommandHandler : ICommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" /> class.
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
            TargetAssembly = null; // This causes the target assembly to be resolved at runtime.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" /> class.
        /// </summary>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        /// <param name="targetAssembly">
        /// The assembly from which command handler types are registered. The default assembly is the assembly in which the derived
        /// module is defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="applicationConfiguration" /> is <see langword="null" /> -or- <paramref name="targetAssembly" /> is
        /// <see langword="null" />.
        /// </exception>
        protected DotNetNativeCommandHandlerModule(IConfiguration applicationConfiguration, Assembly targetAssembly)
            : base(applicationConfiguration)
        {
            TargetAssembly = targetAssembly.RejectIf().IsNull(nameof(targetAssembly));
        }

        /// <summary>
        /// Configures the module.
        /// </summary>
        /// <param name="configurator">
        /// An object that configures containers.
        /// </param>
        /// <param name="applicationConfiguration">
        /// Configuration information for the application.
        /// </param>
        protected sealed override void Configure(ServiceCollection configurator, IConfiguration applicationConfiguration)
        {
            configurator.AddApplicationConfiguration(applicationConfiguration);
            configurator.AddConfigurationCommandHandlers();

            foreach (var commandHandlerType in MatchedTypes)
            {
                var commandHandlerInterfaceType = commandHandlerType.GetInterfaces().Where(implementedInterface => CommandHandlerInterfaceType.IsAssignableFrom(implementedInterface) && implementedInterface.GenericTypeArguments.Length == 1).FirstOrDefault();
                var commandType = commandHandlerInterfaceType?.GenericTypeArguments.FirstOrDefault();

                if (commandType is null)
                {
                    continue;
                }

                configurator.AddCommandHandler(commandType, commandHandlerType);
            }
        }

        /// <summary>
        /// Gets the base class or implemented interface type that is shared by all command handler types that are registered by the
        /// current <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" />.
        /// </summary>
        public Type BaseCommandHandlerType => BaseCommandHandlerTypeReference;

        /// <summary>
        /// Gets the collection of non-abstract public class types defined by <see cref="TargetAssembly" /> that are registered by
        /// the current <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" />.
        /// </summary>
        public IEnumerable<Type> MatchedTypes => TargetAssembly.GetTypes().Where(type => type.IsPublic && type.IsClass && type.IsAbstract is false && BaseCommandHandlerType.IsAssignableFrom(type));

        /// <summary>
        /// Gets the assembly from which command handler types are registered by the current
        /// <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" />.
        /// </summary>
        public Assembly TargetAssembly
        {
            get
            {
                if (TargetAssemblyReference is null)
                {
                    TargetAssemblyReference = Assembly.GetAssembly(GetType());
                }

                return TargetAssemblyReference;
            }
            private set => TargetAssemblyReference = value;
        }

        /// <summary>
        /// Represents the base class or implemented interface type that is shared by all command handler types that are registered
        /// by the current <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type BaseCommandHandlerTypeReference = typeof(TBaseCommandHandler);

        /// <summary>
        /// Represents the <see cref="ICommandHandler" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandHandlerInterfaceType = typeof(ICommandHandler);

        /// <summary>
        /// Represents the assembly from which command handler types are registered by the current
        /// <see cref="DotNetNativeCommandHandlerModule{TBaseCommandHandler}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Assembly TargetAssemblyReference;
    }
}