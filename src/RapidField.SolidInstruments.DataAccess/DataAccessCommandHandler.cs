// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.ObjectComposition;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Processes data access commands.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessCommandHandler{TCommand}" /> is the default implementation of
    /// <see cref="IDataAccessCommandHandler{TCommand}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    public abstract class DataAccessCommandHandler<TCommand> : CommandHandler<TCommand>, IDataAccessCommandHandler<TCommand>
        where TCommand : class, IDataAccessCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessCommandHandler{TCommand}" /> class.
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
        protected DataAccessCommandHandler(ICommandMediator mediator, IDataAccessRepositoryFactory repositoryFactory, IDataAccessTransaction transaction)
            : base(mediator)
        {
            Repositories = new FactoryProducedInstanceGroup(repositoryFactory);
            Transaction = transaction.RejectIf().IsNull(nameof(transaction)).OrIf((argument) => argument.State != DataAccessTransactionState.Ready, nameof(transaction)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessCommandHandler{TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Repositories.Dispose();

                    /* IMPORTANT
                     * Don't remove this. Although the transaction object is injected and, therefore, should normally be managed by
                     * a consuming class, the handler manages the complete life cycle of the transaction. Further, derived classes
                     * may initialize transactions in their constructors and thereby rely on this class to manage them.
                    */
                    Transaction.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected sealed override void Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            if (Transaction.State == DataAccessTransactionState.Ready)
            {
                try
                {
                    Transaction.Begin();
                    Process(command, Repositories);
                    CommitTransaction(Transaction, controlToken);
                    return;
                }
                catch
                {
                    AbortTransaction(Transaction, controlToken);
                    throw;
                }
            }

            throw new InvalidOperationException($"The transaction is in the invalid state, {Transaction.State}.");
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="repositories">
        /// An object that provides access to data access repositories.
        /// </param>
        protected abstract void Process(TCommand command, IFactoryProducedInstanceGroup repositories);

        /// <summary>
        /// Conditionally starts an asynchronous task that rejects all changes made within the scope of the specified transaction.
        /// </summary>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        [DebuggerHidden]
        private static void AbortTransaction(IDataAccessTransaction transaction, IConcurrencyControlToken controlToken)
        {
            if (transaction.State == DataAccessTransactionState.InProgress)
            {
                controlToken.AttachTask(transaction.RejectAsync());
            }
        }

        /// <summary>
        /// Conditionally starts an asynchronous task that commits all changes made within the scope of the specified transaction.
        /// </summary>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        [DebuggerHidden]
        private static void CommitTransaction(IDataAccessTransaction transaction, IConcurrencyControlToken controlToken)
        {
            if (transaction.State == DataAccessTransactionState.InProgress)
            {
                controlToken.AttachTask(transaction.CommitAsync());
            }
        }

        /// <summary>
        /// Represents an object that provides access to data access repositories.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IFactoryProducedInstanceGroup Repositories;

        /// <summary>
        /// Represents a transaction that is used to process the command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDataAccessTransaction Transaction;
    }

    /// <summary>
    /// Processes data access commands.
    /// </summary>
    /// <remarks>
    /// <see cref="DataAccessCommandHandler{TCommand, TResult}" /> is the default implementation of
    /// <see cref="IDataAccessCommandHandler{TCommand, TResult}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a data access command.
    /// </typeparam>
    public abstract class DataAccessCommandHandler<TCommand, TResult> : CommandHandler<TCommand, TResult>, IDataAccessCommandHandler<TCommand, TResult>
        where TCommand : class, IDataAccessCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessCommandHandler{TCommand, TResult}" /> class.
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
        protected DataAccessCommandHandler(ICommandMediator mediator, IDataAccessRepositoryFactory repositoryFactory, IDataAccessTransaction transaction)
            : base(mediator)
        {
            Repositories = new FactoryProducedInstanceGroup(repositoryFactory);
            Transaction = transaction.RejectIf().IsNull(nameof(transaction)).OrIf((argument) => argument.State != DataAccessTransactionState.Ready, nameof(transaction)).TargetArgument;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DataAccessCommandHandler{TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Repositories.Dispose();

                    /* IMPORTANT
                     * Don't remove this. Although the transaction object is injected and, therefore, should normally be managed by
                     * a consuming class, the handler manages the complete life cycle of the transaction. Further, derived classes
                     * may initialize transactions in their constructors and thereby rely on this class to manage them.
                    */
                    Transaction.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="command" /> using
        /// <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected sealed override TResult Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            if (Transaction.State == DataAccessTransactionState.Ready)
            {
                try
                {
                    Transaction.Begin();
                    var result = Process(command, Repositories);
                    CommitTransaction(Transaction, controlToken);
                    return result;
                }
                catch
                {
                    AbortTransaction(Transaction, controlToken);
                    throw;
                }
            }

            throw new InvalidOperationException($"The transaction is in the invalid state, {Transaction.State}.");
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <param name="repositories">
        /// An object that provides access to data access repositories.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected abstract TResult Process(TCommand command, IFactoryProducedInstanceGroup repositories);

        /// <summary>
        /// Conditionally starts an asynchronous task that rejects all changes made within the scope of the specified transaction.
        /// </summary>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        [DebuggerHidden]
        private static void AbortTransaction(IDataAccessTransaction transaction, IConcurrencyControlToken controlToken)
        {
            if (transaction.State == DataAccessTransactionState.InProgress)
            {
                controlToken.AttachTask(transaction.RejectAsync());
            }
        }

        /// <summary>
        /// Conditionally starts an asynchronous task that commits all changes made within the scope of the specified transaction.
        /// </summary>
        /// <param name="transaction">
        /// A transaction that is used to process the command.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        [DebuggerHidden]
        private static void CommitTransaction(IDataAccessTransaction transaction, IConcurrencyControlToken controlToken)
        {
            if (transaction.State == DataAccessTransactionState.InProgress)
            {
                controlToken.AttachTask(transaction.CommitAsync());
            }
        }

        /// <summary>
        /// Represents an object that provides access to data access repositories.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IFactoryProducedInstanceGroup Repositories;

        /// <summary>
        /// Represents a transaction that is used to process the command.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDataAccessTransaction Transaction;
    }
}