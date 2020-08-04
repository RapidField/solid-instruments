// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.DataAccess.Autofac.Extensions
{
    /// <summary>
    /// Extends the <see cref="ContainerBuilder" /> class with inversion of control features to support data access abstractions.
    /// </summary>
    public static class ContainerBuilderExtensions
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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterCreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this ContainerBuilder target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterDataAccessCommandHandler<CreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel>, CreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

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

        /// <summary>
        /// Registers a data access repository factory of the specified type.
        /// </summary>
        /// <typeparam name="TRepositoryFactory">
        /// The type of the data access repository factory that is registered.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterDataAccessRepositoryFactory<TRepositoryFactory>(this ContainerBuilder target)
            where TRepositoryFactory : class, IDataAccessRepositoryFactory => target.RegisterType<TRepositoryFactory>().IfNotRegistered(typeof(TRepositoryFactory)).As<IDataAccessRepositoryFactory>().AsSelf().InstancePerLifetimeScope();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterDeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this ContainerBuilder target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterDataAccessCommandHandler<DeleteDataAccessModelCommand<TIdentifier, TDataAccessModel>, Nix, DeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterFindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this ContainerBuilder target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterDataAccessCommandHandler<FindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel>, TDataAccessModel, FindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>>();

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
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        public static void RegisterStandardDataAccessModelCommandHandlers<TIdentifier, TDataAccessModel, TRepository>(this ContainerBuilder target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel>
        {
            target.RegisterCreateOrUpdateDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
            target.RegisterDeleteDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
            target.RegisterFindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>();
        }
    }
}