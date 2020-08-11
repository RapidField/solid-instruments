// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support the
    /// command and mediator patterns.
    /// </summary>
    public static class IServiceCollectionExtensions
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(this IServiceCollection target)
            where TCommand : class, ICommandBase
            where TCommandHandler : class, ICommandHandler<TCommand> => target.AddCommandHandler(typeof(TCommand), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the command for which a handler is registered.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the command handler that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="commandHandlerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="ICommandBase" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="ICommandHandler{TCommand}" />.
        /// </exception>
        public static IServiceCollection AddCommandHandler(this IServiceCollection target, Type commandType, Type commandHandlerType)
        {
            var commandHandlerInterfaceType = CommandHandlerInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(CommandBaseInterfaceType, nameof(commandType)));
            target.TryAddTransient(commandHandlerInterfaceType, commandHandlerType.RejectIf().IsNull(nameof(commandHandlerType)).OrIf().IsNotSupportedType(commandHandlerInterfaceType, nameof(commandHandlerType)));
            return target;
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