// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.InversionOfControl;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Serves as a dependency resolver and processing intermediary for commands.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandMediator" /> is the default implementation of <see cref="ICommandMediator" />.
    /// </remarks>
    public sealed class CommandMediator : Instrument, ICommandMediator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMediator" /> class.
        /// </summary>
        /// <param name="scope">
        /// The object initialization and disposal scope for the mediator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="scope" /> is <see langword="null" />.
        /// </exception>
        public CommandMediator(IDependencyScope scope)
            : base(ConcurrencyControlMode.ProcessorCountSemaphore)
        {
            Scope = scope.RejectIf().IsNull(nameof(scope)).TargetArgument;
        }

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
        public TResult Process<TResult>(ICommand<TResult> command)
        {
            var commandType = command.RejectIf().IsNull(nameof(command)).TargetArgument.GetType();

            try
            {
                var registeredHandlerType = CommandHandlerRegisteredType.MakeGenericType(commandType);
                var implementedHandlerType = CommandHandlerImplementedType.MakeGenericType(commandType, typeof(TResult));
                var processMethod = implementedHandlerType.GetMethod(CommandHandlerProcessMethodName);
                var processMethodArguments = new Object[1] { command };

                using (var controlToken = StateControl.Enter())
                {
                    RejectIfDisposed();
                    var commandHandler = Scope.Resolve(registeredHandlerType);
                    var result = processMethod.Invoke(commandHandler, processMethodArguments);
                    return (TResult)result;
                }
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
                throw new CommandHandlingException(commandType, exception);
            }
        }

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
        public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command) => Task.FromResult(Process(command));

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CommandMediator" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Represents the name of the <see cref="ICommandHandler{TCommand, TResult}.Process(TCommand)" /> method.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String CommandHandlerProcessMethodName = "Process";

        /// <summary>
        /// Represents the generic <see cref="ICommandHandler{TCommand, TResult}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandHandlerImplementedType = typeof(ICommandHandler<,>);

        /// <summary>
        /// Represents the generic <see cref="ICommandHandler{TCommand}" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandHandlerRegisteredType = typeof(ICommandHandler<>);

        /// <summary>
        /// Represents the object initialization and disposal scope for the current <see cref="CommandMediator" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDependencyScope Scope;
    }
}