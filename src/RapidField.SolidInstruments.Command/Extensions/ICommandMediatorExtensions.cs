// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Command.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICommandMediator" /> interface with command handling features.
    /// </summary>
    public static class ICommandMediatorExtensions
    {
        /// <summary>
        /// Processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createCommandFunction">
        /// A function that returns the command to process.
        /// </param>
        /// <returns>
        /// The result that is produced by processing the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createCommandFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static TResult Process<TResult>(this ICommandMediator target, Func<ICommandRegister, ICommand<TResult>> createCommandFunction)
        {
            try
            {
                return target.Process(createCommandFunction.RejectIf().IsNull(nameof(createCommandFunction)).TargetArgument(CommandRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(exception);
            }
        }

        /// <summary>
        /// Processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command to process.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createCommandFunction">
        /// A function that returns the command to process.
        /// </param>
        /// <returns>
        /// The result that is produced by processing the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createCommandFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static TResult Process<TCommand, TResult>(this ICommandMediator target, Func<ICommandRegister, TCommand> createCommandFunction)
            where TCommand : class, ICommand<TResult>
        {
            try
            {
                return target.Process(createCommandFunction.RejectIf().IsNull(nameof(createCommandFunction)).TargetArgument(CommandRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TCommand), exception);
            }
        }

        /// <summary>
        /// Asynchronously processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createCommandFunction">
        /// A function that returns the command to process.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result that is produced by processing the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createCommandFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task<TResult> ProcessAsync<TResult>(this ICommandMediator target, Func<ICommandRegister, ICommand<TResult>> createCommandFunction)
        {
            try
            {
                return target.ProcessAsync(createCommandFunction.RejectIf().IsNull(nameof(createCommandFunction)).TargetArgument(CommandRegister.Instance));
            }
            catch (CommandHandlingException exception)
            {
                return Task.FromException<TResult>(exception);
            }
            catch (ObjectDisposedException exception)
            {
                return Task.FromException<TResult>(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException<TResult>(new CommandHandlingException(exception));
            }
        }

        /// <summary>
        /// Asynchronously processes the specified <see cref="ICommand{TResult}" />.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command to process.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the command.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createCommandFunction">
        /// A function that returns the command to process.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result that is produced by processing the command.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createCommandFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the command.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task<TResult> ProcessAsync<TCommand, TResult>(this ICommandMediator target, Func<ICommandRegister, TCommand> createCommandFunction)
            where TCommand : class, ICommand<TResult>
        {
            try
            {
                return target.ProcessAsync(createCommandFunction.RejectIf().IsNull(nameof(createCommandFunction)).TargetArgument(CommandRegister.Instance));
            }
            catch (CommandHandlingException exception)
            {
                return Task.FromException<TResult>(exception);
            }
            catch (ObjectDisposedException exception)
            {
                return Task.FromException<TResult>(exception);
            }
            catch (Exception exception)
            {
                return Task.FromException<TResult>(new CommandHandlingException(typeof(TCommand), exception));
            }
        }
    }
}