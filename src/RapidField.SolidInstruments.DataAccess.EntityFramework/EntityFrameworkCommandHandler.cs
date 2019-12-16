// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using RapidField.SolidInstruments.Command;
using System;
using System.Data;
using System.Diagnostics;

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
    public abstract class EntityFrameworkCommandHandler<TContext, TCommand> : DataAccessCommandHandler<TCommand>
        where TContext : DbContext
        where TCommand : class, IDataAccessCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory)
            : this(mediator, repositoryFactory, DefaultIsolationLevel)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <param name="isolationLevel">
        /// The isolation level for the transaction, or <see cref="IsolationLevel.Unspecified" /> to use the database default. The
        /// default value is <see cref="IsolationLevel.Unspecified" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory, IsolationLevel isolationLevel)
            : this(mediator, repositoryFactory, new EntityFrameworkTransaction<TContext>(repositoryFactory.Context, isolationLevel))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="transaction" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" /> -or- <paramref name="transaction" /> is <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory, EntityFrameworkTransaction<TContext> transaction)
            : base(mediator, repositoryFactory, transaction)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EntityFrameworkCommandHandler{TContext, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the default isolation level for transactions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const IsolationLevel DefaultIsolationLevel = IsolationLevel.Unspecified;
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
    public abstract class EntityFrameworkCommandHandler<TContext, TCommand, TResult> : DataAccessCommandHandler<TCommand, TResult>
        where TContext : DbContext
        where TCommand : class, IDataAccessCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory)
            : this(mediator, repositoryFactory, DefaultIsolationLevel)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <param name="isolationLevel">
        /// The isolation level for the transaction, or <see cref="IsolationLevel.Unspecified" /> to use the database default. The
        /// default value is <see cref="IsolationLevel.Unspecified" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory, IsolationLevel isolationLevel)
            : this(mediator, repositoryFactory, new EntityFrameworkTransaction<TContext>(repositoryFactory.Context, isolationLevel))
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkCommandHandler{TContext, TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="repositoryFactory">
        /// The factory that produces data access repositories for the handler.
        /// </param>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="transaction" /> is in an invalid state.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="repositoryFactory" /> is
        /// <see langword="null" /> -or- <paramref name="transaction" /> is <see langword="null" />.
        /// </exception>
        protected EntityFrameworkCommandHandler(ICommandMediator mediator, EntityFrameworkRepositoryFactory<TContext> repositoryFactory, EntityFrameworkTransaction<TContext> transaction)
            : base(mediator, repositoryFactory, transaction)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current
        /// <see cref="EntityFrameworkCommandHandler{TContext, TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the default isolation level for transactions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const IsolationLevel DefaultIsolationLevel = IsolationLevel.Unspecified;
    }
}