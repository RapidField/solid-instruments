// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.DataAccess.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support data
    /// access abstractions.
    /// </summary>
    public static class IServiceCollectionExtensions
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddDataAccessCommandHandler<TCommand, TCommandHandler>(this IServiceCollection target)
            where TCommand : class, IDataAccessCommand
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand> => target.AddDataAccessCommandHandler<TCommand, Nix, TCommandHandler>();

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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddDataAccessCommandHandler<TCommand, TResult, TCommandHandler>(this IServiceCollection target)
            where TCommand : class, IDataAccessCommand<TResult>
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand, TResult> => target.AddCommandHandler<TCommand, TCommandHandler>();

        /// <summary>
        /// Registers a data access repository factory of the specified type.
        /// </summary>
        /// <typeparam name="TRepositoryFactory">
        /// The type of the data access repository factory that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddDataAccessRepositoryFactory<TRepositoryFactory>(this IServiceCollection target)
            where TRepositoryFactory : class, IDataAccessRepositoryFactory
        {
            target.AddScoped<TRepositoryFactory>();
            target.AddScoped<IDataAccessRepositoryFactory, TRepositoryFactory>((serviceProvider) => serviceProvider.GetService<TRepositoryFactory>());
            return target;
        }
    }
}