// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.DataAccess.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support data access abstractions.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the data access command for which a handler is registered.
        /// </typeparam>
        /// <typeparam name="TCommandHandler">
        /// The type of the data access command handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterDataAccessCommandHandler<TCommand, TCommandHandler>(this ContainerBuilder target)
            where TCommand : class, IDataAccessCommand
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand> => target.RegisterDataAccessCommandHandler<TCommand, Nix, TCommandHandler>();

        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the data access command for which a handler is registered.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result that is emitted by the handler when processing a data access command.
        /// </typeparam>
        /// <typeparam name="TCommandHandler">
        /// The type of the data access command handler that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterDataAccessCommandHandler<TCommand, TResult, TCommandHandler>(this ContainerBuilder target)
            where TCommand : class, IDataAccessCommand<TResult>
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand, TResult> => target.RegisterCommandHandler<TCommand, TCommandHandler>();
    }
}