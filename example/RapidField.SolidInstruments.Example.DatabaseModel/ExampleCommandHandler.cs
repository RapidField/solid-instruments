﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.DataAccess;
using RapidField.SolidInstruments.DataAccess.EntityFramework;
using System;
using System.Data;

namespace RapidField.SolidInstruments.Example.DatabaseModel
{
    /// <summary>
    /// Processes Example data access commands.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    public abstract class ExampleCommandHandler<TCommand> : EntityFrameworkCommandHandler<ExampleContext, TCommand>
        where TCommand : class, IDataAccessCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommandHandler{TCommand}" /> class.
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
        protected ExampleCommandHandler(ICommandMediator mediator, ExampleRepositoryFactory repositoryFactory)
            : base(mediator, repositoryFactory)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommandHandler{TCommand}" /> class.
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
        protected ExampleCommandHandler(ICommandMediator mediator, ExampleRepositoryFactory repositoryFactory, IsolationLevel isolationLevel)
            : base(mediator, repositoryFactory, isolationLevel)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ExampleCommandHandler{TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }

    /// <summary>
    /// Processes Example data access commands.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the data access command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a data access command.
    /// </typeparam>
    public abstract class ExampleCommandHandler<TCommand, TResult> : EntityFrameworkCommandHandler<ExampleContext, TCommand, TResult>
        where TCommand : class, IDataAccessCommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommandHandler{TCommand, TResult}" /> class.
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
        protected ExampleCommandHandler(ICommandMediator mediator, ExampleRepositoryFactory repositoryFactory)
            : base(mediator, repositoryFactory)
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommandHandler{TCommand, TResult}" /> class.
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
        protected ExampleCommandHandler(ICommandMediator mediator, ExampleRepositoryFactory repositoryFactory, IsolationLevel isolationLevel)
            : base(mediator, repositoryFactory, isolationLevel)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ExampleCommandHandler{TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}