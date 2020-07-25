// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;

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
            where TCommandHandler : class, ICommandHandler<TCommand> => target.RegisterType<TCommandHandler>().As<ICommandHandler<TCommand>>().InstancePerDependency();
    }
}