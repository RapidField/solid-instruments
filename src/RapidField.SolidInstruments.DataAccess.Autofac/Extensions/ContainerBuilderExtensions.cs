// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Autofac;
using RapidField.SolidInstruments.Command.Autofac.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterCreateOrUpdateDataAccessModelCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for creating and updating instances of the specified data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="identifierType">
        /// The type of the unique primary identifier for the data access model type.
        /// </param>
        /// <param name="dataAccessModelType">
        /// The type of the data access model that is associated with the command.
        /// </param>
        /// <param name="repositoryType">
        /// The type of the data access model repository that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="identifierType" /> is <see langword="null" /> -or- <paramref name="dataAccessModelType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="identifierType" /> does not implement <see cref="IComparable{T}" /> and <see cref="IEquatable{T}" />
        /// -or- <paramref name="dataAccessModelType" /> does not implement <see cref="IDataAccessModel{TIdentifier}" /> -or-
        ///  <paramref name="repositoryType" /> does not implement
        ///  <see cref="IDataAccessModelRepository{TIdentifier, TDataAccessModel}" />.
        /// </exception>
        public static void RegisterCreateOrUpdateDataAccessModelCommandHandler(this ContainerBuilder target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = CreateOrUpdateDataAccessModelCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = CreateOrUpdateDataAccessModelCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            target.RegisterDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelCommandHandlerType);
        }

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
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand> => target.RegisterDataAccessCommandHandler(typeof(TCommand), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the data access command for which a handler is registered.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the data access command handler that is registered.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="commandHandlerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="IDataAccessCommand" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="IDataAccessCommandHandler{TCommand}" />.
        /// </exception>
        public static void RegisterDataAccessCommandHandler(this ContainerBuilder target, Type commandType, Type commandHandlerType) => target.RegisterDataAccessCommandHandler(commandType, Nix.Type, commandHandlerType);

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
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand, TResult> => target.RegisterDataAccessCommandHandler(typeof(TCommand), typeof(TResult), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the data access command for which a handler is registered.
        /// </param>
        /// <param name="resultType">
        /// The type of the result that is emitted by the handler when processing a data access command.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the data access command handler that is registered.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="resultType" /> is
        /// <see langword="null" /> -or- <paramref name="commandHandlerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="IDataAccessCommand{TResult}" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="IDataAccessCommandHandler{TCommand, TResult}" />.
        /// </exception>
        public static void RegisterDataAccessCommandHandler(this ContainerBuilder target, Type commandType, Type resultType, Type commandHandlerType)
        {
            var dataAccessCommandInterfaceType = DataAccessCommandInterfaceType.MakeGenericType(resultType.RejectIf().IsNull(nameof(resultType)));
            var dataAccessCommandHandlerInterfaceType = DataAccessCommandHandlerInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(dataAccessCommandInterfaceType, nameof(commandType)), resultType);
            target.RegisterCommandHandler(commandType, commandHandlerType.RejectIf().IsNull(nameof(commandHandlerType)).OrIf().IsNotSupportedType(dataAccessCommandHandlerInterfaceType, nameof(commandHandlerType)));
        }

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
            where TRepositoryFactory : class, IDataAccessRepositoryFactory => target.RegisterDataAccessRepositoryFactory(typeof(TRepositoryFactory));

        /// <summary>
        /// Registers a data access repository factory of the specified type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="repositoryFactoryType">
        /// The type of the data access repository factory that is registered.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repositoryFactoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="repositoryFactoryType" /> does not implement <see cref="IDataAccessRepositoryFactory" />.
        /// </exception>
        public static void RegisterDataAccessRepositoryFactory(this ContainerBuilder target, Type repositoryFactoryType) => target.RegisterType(repositoryFactoryType.RejectIf().IsNull(nameof(repositoryFactoryType)).OrIf().IsNotSupportedType(DataAccessRepositoryFactoryInterfaceType, nameof(repositoryFactoryType))).IfNotRegistered(repositoryFactoryType).As(DataAccessRepositoryFactoryInterfaceType).AsSelf().InstancePerLifetimeScope();

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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterDeleteDataAccessModelCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for deleting instances of the specified data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="identifierType">
        /// The type of the unique primary identifier for the data access model type.
        /// </param>
        /// <param name="dataAccessModelType">
        /// The type of the data access model that is associated with the command.
        /// </param>
        /// <param name="repositoryType">
        /// The type of the data access model repository that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="identifierType" /> is <see langword="null" /> -or- <paramref name="dataAccessModelType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="identifierType" /> does not implement <see cref="IComparable{T}" /> and <see cref="IEquatable{T}" />
        /// -or- <paramref name="dataAccessModelType" /> does not implement <see cref="IDataAccessModel{TIdentifier}" /> -or-
        ///  <paramref name="repositoryType" /> does not implement
        ///  <see cref="IDataAccessModelRepository{TIdentifier, TDataAccessModel}" />.
        /// </exception>
        public static void RegisterDeleteDataAccessModelCommandHandler(this ContainerBuilder target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = DeleteDataAccessModelCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = DeleteDataAccessModelCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            target.RegisterDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelCommandHandlerType);
        }

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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterFindDataAccessModelByIdentifierCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for finding instances of the specified data access model type by identifier.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="identifierType">
        /// The type of the unique primary identifier for the data access model type.
        /// </param>
        /// <param name="dataAccessModelType">
        /// The type of the data access model that is associated with the command.
        /// </param>
        /// <param name="repositoryType">
        /// The type of the data access model repository that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="identifierType" /> is <see langword="null" /> -or- <paramref name="dataAccessModelType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="identifierType" /> does not implement <see cref="IComparable{T}" /> and <see cref="IEquatable{T}" />
        /// -or- <paramref name="dataAccessModelType" /> does not implement <see cref="IDataAccessModel{TIdentifier}" /> -or-
        ///  <paramref name="repositoryType" /> does not implement
        ///  <see cref="IDataAccessModelRepository{TIdentifier, TDataAccessModel}" />.
        /// </exception>
        public static void RegisterFindDataAccessModelByIdentifierCommandHandler(this ContainerBuilder target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = FindDataAccessModelByIdentifierCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = FindDataAccessModelByIdentifierCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            target.RegisterDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelCommandHandlerType);
        }

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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.RegisterStandardDataAccessModelCommandHandlers(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers the standard transient command handlers for creating, reading, updating and deleting instances of the
        /// specified data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="ContainerBuilder" />.
        /// </param>
        /// <param name="identifierType">
        /// The type of the unique primary identifier for the data access model type.
        /// </param>
        /// <param name="dataAccessModelType">
        /// The type of the data access model that is associated with the command.
        /// </param>
        /// <param name="repositoryType">
        /// The type of the data access model repository that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="identifierType" /> is <see langword="null" /> -or- <paramref name="dataAccessModelType" /> is
        /// <see langword="null" /> -or- <paramref name="repositoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="identifierType" /> does not implement <see cref="IComparable{T}" /> and <see cref="IEquatable{T}" />
        /// -or- <paramref name="dataAccessModelType" /> does not implement <see cref="IDataAccessModel{TIdentifier}" /> -or-
        ///  <paramref name="repositoryType" /> does not implement
        ///  <see cref="IDataAccessModelRepository{TIdentifier, TDataAccessModel}" />.
        /// </exception>
        public static void RegisterStandardDataAccessModelCommandHandlers(this ContainerBuilder target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            target.RegisterCreateOrUpdateDataAccessModelCommandHandler(identifierType, dataAccessModelType, repositoryType);
            target.RegisterDeleteDataAccessModelCommandHandler(identifierType, dataAccessModelType, repositoryType);
            target.RegisterFindDataAccessModelByIdentifierCommandHandler(identifierType, dataAccessModelType, repositoryType);
        }

        /// <summary>
        /// Represents the <see cref="IComparable{T}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ComparableInterfaceType = typeof(IComparable<>);

        /// <summary>
        /// Represents the <see cref="CreateOrUpdateDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TRepository}" />
        /// type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CreateOrUpdateDataAccessModelCommandHandlerType = typeof(CreateOrUpdateDataAccessModelCommandHandler<,,>);

        /// <summary>
        /// Represents the <see cref="CreateOrUpdateDataAccessModelCommand{TIdentifier, TDataAccessModel}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CreateOrUpdateDataAccessModelCommandType = typeof(CreateOrUpdateDataAccessModelCommand<,>);

        /// <summary>
        /// Represents the <see cref="IDataAccessCommandHandler{TCommand, TResult}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessCommandHandlerInterfaceType = typeof(IDataAccessCommandHandler<,>);

        /// <summary>
        /// Represents the <see cref="IDataAccessCommand{TResult}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessCommandInterfaceType = typeof(IDataAccessCommand<>);

        /// <summary>
        /// Represents the <see cref="IDataAccessModel{TIdentifier}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessModelInterfaceType = typeof(IDataAccessModel<>);

        /// <summary>
        /// Represents the <see cref="IDataAccessModelRepository{TIdentifier, TDataAccessModel}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessModelRepositoryInterfaceType = typeof(IDataAccessModelRepository<,>);

        /// <summary>
        /// Represents the <see cref="IDataAccessRepositoryFactory" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DataAccessRepositoryFactoryInterfaceType = typeof(IDataAccessRepositoryFactory);

        /// <summary>
        /// Represents the <see cref="DeleteDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TRepository}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DeleteDataAccessModelCommandHandlerType = typeof(DeleteDataAccessModelCommandHandler<,,>);

        /// <summary>
        /// Represents the <see cref="DeleteDataAccessModelCommand{TIdentifier, TDataAccessModel}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DeleteDataAccessModelCommandType = typeof(DeleteDataAccessModelCommand<,>);

        /// <summary>
        /// Represents the <see cref="IEquatable{T}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type EquatableInterfaceType = typeof(IEquatable<>);

        /// <summary>
        /// Represents the <see cref="FindDataAccessModelByIdentifierCommandHandler{TIdentifier, TDataAccessModel, TRepository}" />
        /// type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type FindDataAccessModelByIdentifierCommandHandlerType = typeof(FindDataAccessModelByIdentifierCommandHandler<,,>);

        /// <summary>
        /// Represents the <see cref="FindDataAccessModelByIdentifierCommand{TIdentifier, TDataAccessModel}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type FindDataAccessModelByIdentifierCommandType = typeof(FindDataAccessModelByIdentifierCommand<,>);
    }
}