// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes commands.
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
        TResult Process(TCommand command);
    }

    /// <summary>
    /// Processes commands.
    /// </summary>
    /// <remarks>
    /// Do not implement <see cref="ICommandHandler{TCommand}" /> directly in user code. Use <see cref="ICommandHandler{TCommand}" />
    /// as a registration target for inversion of control tools.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public interface ICommandHandler<in TCommand> : IDisposable
        where TCommand : class, ICommandBase
    {
    }
}