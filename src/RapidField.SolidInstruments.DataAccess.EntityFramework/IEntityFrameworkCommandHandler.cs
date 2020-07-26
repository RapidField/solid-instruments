// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Processes Entity Framework data access commands.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the transaction.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    public interface IEntityFrameworkCommandHandler<TContext, TCommand> : IDataAccessCommandHandler<TCommand>
        where TContext : DbContext
        where TCommand : class, IDataAccessCommand
    {
    }

    /// <summary>
    /// Processes Entity Framework data access commands.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the database session for the transaction.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a data access command.
    /// </typeparam>
    public interface IEntityFrameworkCommandHandler<TContext, TCommand, TResult> : IDataAccessCommandHandler<TCommand, TResult>
        where TContext : DbContext
        where TCommand : class, IDataAccessCommand<TResult>
    {
    }
}