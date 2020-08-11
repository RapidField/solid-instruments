// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Processes a single <see cref="IDataAccessModelCommand{TIdentifier, TDataAccessModel}" />.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TCommand}" /> as a registration target
    /// for inversion of control tools. Use <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model type.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model that is associated with the command.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the data access model command that is processed by the handler.
    /// </typeparam>
    public interface IDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, in TCommand> : IDataAccessCommandHandler<TCommand>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
        where TCommand : class, IDataAccessModelCommand<TIdentifier, TDataAccessModel>
    {
    }

    /// <summary>
    /// Processes a single <see cref="IDataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" />.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IDataAccessModelCommandHandler{TIdentifier, TDataAccessModel, TCommand, TResult}" /> as a registration
    /// target for inversion of control tools. Use <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model type.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model that is associated with the command.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the data access model command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a data access model command.
    /// </typeparam>
    public interface IDataAccessModelCommandHandler<TIdentifier, TDataAccessModel, in TCommand, TResult> : IDataAccessCommandHandler<TCommand, TResult>, IDataAccessModelCommandHandler
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
        where TCommand : class, IDataAccessModelCommand<TIdentifier, TDataAccessModel, TResult>
    {
    }

    /// <summary>
    /// Processes a single <see cref="IDataAccessModelCommand{TIdentifier, TDataAccessModel, TResult}" />.
    /// </summary>
    public interface IDataAccessModelCommandHandler : IDataAccessCommandHandler
    {
    }
}