// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Serves as a dependency resolver and processing intermediary for commands.
    /// </summary>
    public interface ICommandMediator : IInstrument
    {
        /// <summary>
        /// Processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <returns>
        /// The result that is produced by processing the command.
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
        public TResult Process<TResult>(ICommand<TResult> command);

        /// <summary>
        /// Asynchronously processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="command">
        /// The command to process.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result that is produced by processing the command.
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
        public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command);
    }
}