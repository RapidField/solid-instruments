// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;

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
            where TCommandHandler : class, ICommandHandler<TCommand> => target.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
    }
}