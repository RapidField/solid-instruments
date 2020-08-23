// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RapidField.SolidInstruments.Command.DotNetNative.Extensions;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Diagnostics;

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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddCreateOrUpdateDataAccessModelCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for creating and updating instances of the specified data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
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
        public static IServiceCollection AddCreateOrUpdateDataAccessModelCommandHandler(this IServiceCollection target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = CreateOrUpdateDataAccessModelCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = CreateOrUpdateDataAccessModelCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            return target.AddDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelCommandHandlerType);
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddDataAccessCommandHandler<TCommand, TCommandHandler>(this IServiceCollection target)
            where TCommand : class, IDataAccessCommand
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand> => target.AddDataAccessCommandHandler(typeof(TCommand), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="commandType">
        /// The type of the data access command for which a handler is registered.
        /// </param>
        /// <param name="commandHandlerType">
        /// The type of the data access command handler that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="commandHandlerType" /> is
        /// <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="IDataAccessCommand" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="IDataAccessCommandHandler{TCommand}" />.
        /// </exception>
        public static IServiceCollection AddDataAccessCommandHandler(this IServiceCollection target, Type commandType, Type commandHandlerType) => target.AddDataAccessCommandHandler(commandType, Nix.Type, commandHandlerType);

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
            where TCommandHandler : class, IDataAccessCommandHandler<TCommand, TResult> => target.AddDataAccessCommandHandler(typeof(TCommand), typeof(TResult), typeof(TCommandHandler));

        /// <summary>
        /// Registers a transient data access command handler for the specified command type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandType" /> is <see langword="null" /> -or- <paramref name="resultType" /> is
        /// <see langword="null" /> -or- <paramref name="commandHandlerType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="commandType" /> does not implement <see cref="IDataAccessCommand{TResult}" /> -or-
        /// <paramref name="commandHandlerType" /> does not implement <see cref="IDataAccessCommandHandler{TCommand, TResult}" />.
        /// </exception>
        public static IServiceCollection AddDataAccessCommandHandler(this IServiceCollection target, Type commandType, Type resultType, Type commandHandlerType)
        {
            var dataAccessCommandInterfaceType = DataAccessCommandInterfaceType.MakeGenericType(resultType.RejectIf().IsNull(nameof(resultType)));
            var dataAccessCommandHandlerInterfaceType = DataAccessCommandHandlerInterfaceType.MakeGenericType(commandType.RejectIf().IsNull(nameof(commandType)).OrIf().IsNotSupportedType(dataAccessCommandInterfaceType, nameof(commandType)), resultType);
            return target.AddCommandHandler(commandType, commandHandlerType.RejectIf().IsNull(nameof(commandHandlerType)).OrIf().IsNotSupportedType(dataAccessCommandHandlerInterfaceType, nameof(commandHandlerType)));
        }

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
            where TRepositoryFactory : class, IDataAccessRepositoryFactory => target.AddDataAccessRepositoryFactory(typeof(TRepositoryFactory));

        /// <summary>
        /// Registers a data access repository factory of the specified type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <param name="repositoryFactoryType">
        /// The type of the data access repository factory that is registered.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repositoryFactoryType" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="UnsupportedTypeArgumentException">
        /// <paramref name="repositoryFactoryType" /> does not implement <see cref="IDataAccessRepositoryFactory" />.
        /// </exception>
        public static IServiceCollection AddDataAccessRepositoryFactory(this IServiceCollection target, Type repositoryFactoryType)
        {
            target.TryAddScoped(repositoryFactoryType.RejectIf().IsNull(nameof(repositoryFactoryType)).OrIf().IsNotSupportedType(DataAccessRepositoryFactoryInterfaceType, nameof(repositoryFactoryType)));
            target.TryAddScoped(DataAccessRepositoryFactoryInterfaceType, repositoryFactoryType);
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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddDeleteDataAccessModelCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for deleting instances of the specified data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
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
        public static IServiceCollection AddDeleteDataAccessModelCommandHandler(this IServiceCollection target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = DeleteDataAccessModelCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = DeleteDataAccessModelCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            return target.AddDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelCommandHandlerType);
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
        /// The current <see cref="IServiceCollection" />.
        /// </param>
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddFindDataAccessModelByIdentifierCommandHandler<TIdentifier, TDataAccessModel, TRepository>(this IServiceCollection target)
            where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
            where TDataAccessModel : class, IDataAccessModel<TIdentifier>
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddFindDataAccessModelByIdentifierCommandHandler(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Registers a transient command handler for finding instances of the specified data access model type by identifier.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
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
        public static IServiceCollection AddFindDataAccessModelByIdentifierCommandHandler(this IServiceCollection target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            var comparableInterfaceType = ComparableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNull(nameof(identifierType)));
            var equatableInterfaceType = EquatableInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(comparableInterfaceType, nameof(identifierType)));
            var dataAccessModelInterfaceType = DataAccessModelInterfaceType.MakeGenericType(identifierType.RejectIf().IsNotSupportedType(equatableInterfaceType, nameof(identifierType)));
            var dataAccessModelRepositoryInterfaceType = DataAccessModelRepositoryInterfaceType.MakeGenericType(identifierType, dataAccessModelType.RejectIf().IsNull(nameof(dataAccessModelType)).OrIf().IsNotSupportedType(dataAccessModelInterfaceType, nameof(dataAccessModelType)));
            var dataAccessModelCommandType = FindDataAccessModelByIdentifierCommandType.MakeGenericType(identifierType, dataAccessModelType);
            var dataAccessModelCommandHandlerType = FindDataAccessModelByIdentifierCommandHandlerType.MakeGenericType(identifierType, dataAccessModelType, repositoryType.RejectIf().IsNull(nameof(repositoryType)).OrIf().IsNotSupportedType(dataAccessModelRepositoryInterfaceType, nameof(repositoryType)));
            return target.AddDataAccessCommandHandler(dataAccessModelCommandType, dataAccessModelType, dataAccessModelCommandHandlerType);
        }

        /// <summary>
        /// Adds the standard transient command handlers for creating, reading, updating and deleting instances of the specified
        /// data access model type.
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
            where TRepository : class, IDataAccessModelRepository<TIdentifier, TDataAccessModel> => target.AddStandardDataAccessModelCommandHandlers(typeof(TIdentifier), typeof(TDataAccessModel), typeof(TRepository));

        /// <summary>
        /// Adds the standard transient command handlers for creating, reading, updating and deleting instances of the specified
        /// data access model type.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="IServiceCollection" />.
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
        /// <returns>
        /// The resulting <see cref="IServiceCollection" />.
        /// </returns>
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
        public static IServiceCollection AddStandardDataAccessModelCommandHandlers(this IServiceCollection target, Type identifierType, Type dataAccessModelType, Type repositoryType)
        {
            target.AddCreateOrUpdateDataAccessModelCommandHandler(identifierType, dataAccessModelType, repositoryType);
            target.AddDeleteDataAccessModelCommandHandler(identifierType, dataAccessModelType, repositoryType);
            target.AddFindDataAccessModelByIdentifierCommandHandler(identifierType, dataAccessModelType, repositoryType);
            return target;
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