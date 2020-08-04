// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Processes a single <see cref="IDataAccessCommand" />.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IDataAccessCommandHandler{TCommand}" /> as a registration target for inversion of control tools. Use
    /// <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    public interface IDataAccessCommandHandler<in TCommand> : IDataAccessCommandHandler<TCommand, Nix>, ICommandHandler<TCommand>
        where TCommand : class, IDataAccessCommand
    {
    }

    /// <summary>
    /// Processes a single <see cref="IDataAccessCommand{TResult}" />.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="IDataAccessCommandHandler{TCommand, TResult}" /> as a registration target for inversion of control
    /// tools. Use <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a data access command.
    /// </typeparam>
    public interface IDataAccessCommandHandler<in TCommand, TResult> : ICommandHandler<TCommand, TResult>, IDataAccessCommandHandler
        where TCommand : class, IDataAccessCommand<TResult>
    {
    }

    /// <summary>
    /// Processes a single <see cref="IDataAccessCommand{TResult}" />.
    /// </summary>
    public interface IDataAccessCommandHandler : ICommandHandler
    {
    }
}