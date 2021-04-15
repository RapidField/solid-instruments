// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes a single <see cref="ICommand" />.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="ICommandHandler{TCommand, TResult}" /> as a registration target for inversion of control tools. Use
    /// <see cref="ICommandHandler{TCommand}" /> instead.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a command.
    /// </typeparam>
    public interface ICommandHandler<in TCommand, TResult> : ICommandHandler<TCommand>
        where TCommand : class, ICommand<TResult>
    {
        /// <summary>
        /// Processes the specified <see cref="ICommand{TResult}" />
        /// </summary>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="command" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public TResult Process(TCommand command);
    }

    /// <summary>
    /// Processes a single <see cref="ICommand" />.
    /// </summary>
    /// <remarks>
    /// Do not implement <see cref="ICommandHandler{TCommand}" /> directly in user code. Use
    /// <see cref="ICommandHandler{TCommand}" /> as a registration target for inversion of control tools.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public interface ICommandHandler<in TCommand> : ICommandHandler
        where TCommand : class, ICommandBase
    {
        /// <summary>
        /// Gets the type of the result that is emitted by the handler when processing a command, or the <see cref="Nix" /> type if
        /// the handler does not emit a result.
        /// </summary>
        public Type ResultType
        {
            get;
        }
    }

    /// <summary>
    /// Processes a single <see cref="ICommand" />.
    /// </summary>
    public interface ICommandHandler : IInstrument
    {
        /// <summary>
        /// Gets the type of the command that is processed by the handler.
        /// </summary>
        public Type CommandType
        {
            get;
        }
    }
}