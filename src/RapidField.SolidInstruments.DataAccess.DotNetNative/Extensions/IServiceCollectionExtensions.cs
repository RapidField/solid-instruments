// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.DataAccess.DotNetNative.Extensions
{
    /// <summary>
    /// Extends the <see cref="IServiceCollection" /> interface with native .NET inversion of control features to support data
    /// access abstractions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a transient command handler for creating and updating instances of the specified data access model type.
        /// </summary>
        /// <typeparam name="TIdentifier">
        /// The type of the unique primary identifier for the data access model type.
        /// </typeparam>
        /// <typeparam name="TDataAccessModel">
        /// The type of the data access model that is associated with the command.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// The type of the data access model repository that is used to process the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddCreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this IServiceCollection target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddDataAccessCommandHandler<CreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel>, CreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

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
            target.TryAddScoped<TRepositoryFactory>();
            target.TryAddScoped<IDataAccessRepositoryFactory>((serviceProvider) => serviceProvider.GetService<TRepositoryFactory>());
            return target;
        }

        /// <summary>
        /// Registers a transient command handler for deleting instances of the specified data access model type.
        /// </summary>
        /// <typeparam name="TIdentifier">
        /// The type of the unique primary identifier for the data access model type.
        /// </typeparam>
        /// <typeparam name="TDataAccessModel">
        /// The type of the data access model that is associated with the command.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// The type of the data access model repository that is used to process the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddDeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this IServiceCollection target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddDataAccessCommandHandler<DeleteDataAccessModelCommand<TIdentifier, TDataAccessModel>, Nix, DeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

        /// <summary>
        /// Registers a transient command handler for finding instances of the specified data access model type by identifier.
        /// </summary>
        /// <typeparam name="TIdentifier">
        /// The type of the unique primary identifier for the data access model type.
        /// </typeparam>
        /// <typeparam name="TDataAccessModel">
        /// The type of the data access model that is associated with the command.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// The type of the data access model repository that is used to process the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddFindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this IServiceCollection target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddDataAccessCommandHandler<FindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel>, TDataAccessModel, FindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

        /// <summary>
        /// Registers the standard transient command handlers for creating, reading, updating and deleting instances of the
        /// specified data access model type.
        /// </summary>
        /// <typeparam name="TIdentifier">
        /// The type of the unique primary identifier for the data access model type.
        /// </typeparam>
        /// <typeparam name="TDataAccessModel">
        /// The type of the data access model that is associated with the command.
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// The type of the data access model repository that is used to process the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddStandardDataAccessModelCommandHandlers<TIdentifier, TDataAccessModel, TRepository>(this IServiceCollection target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel>
        {
            target.AddCreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
            target.AddDeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
            target.AddFindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
            return target;
        }
    }
}