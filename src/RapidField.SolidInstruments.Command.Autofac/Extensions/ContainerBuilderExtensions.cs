// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Configuration;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support the command and mediator
    /// patterns.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers a transient command handler for the specified command type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command for which a handler is registered.
        /// </typeparam>
        /// <typeparam name="TCommandHandler">
        /// The type of the command handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterCommandHandler<TCommand, TCommandHandler>(this ContainerBuilder target)
            where TCommand : class, ICommandBase
            where TCommandHandler : class, ICommandHandler<TCommand> => target.RegisterCommandHandler(typeof(TCommand), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the command for which a handler is registered.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the command handler that is registered.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="commandHandlerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="ICommandBase" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="ICommandHandler{TCommand}" />.
        /// </exception>
        public static void RegisterCommandHandler(this ContainerBuilder target, Type commandType, Type commandHandlerType)
        {
            var commandHandlerInterfaceType = CommandHandlerInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(CommandBaseInterfaceType, nameof(commandType)));
            target.RegisterType(commandHandlerType.RejectIf().IsNull(nameof(commandHandlerType)).OrIf().IsNotSupportedType(commandHandlerInterfaceType, nameof(commandHandlerType))).IfNotRegistered(commandHandlerType).As(commandHandlerInterfaceType).InstancePerDependency();
        }

        /// <summary>
        /// Registers transient command handlers for retrieving configuration sections and values.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterConfigurationCommandHandlers(this ContainerBuilder target)
        {
            target.RegisterCommandHandler<GetConfigurationSectionCommand, GetConfigurationSectionCommandHandler>();
            target.RegisterCommandHandler<GetConfigurationValueCommand, GetConfigurationValueCommandHandler>();
            target.RegisterCommandHandler<GetConnectionStringCommand, GetConnectionStringCommandHandler>();
        }

        /// <summary>
        /// Represents the <see cref="ICommandBase" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandBaseInterfaceType = typeof(ICommandBase);

        /// <summary>
        /// Represents the <see cref="ICommandHandler{TCommand}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandHandlerInterfaceType = typeof(ICommandHandler<>);
    }
}