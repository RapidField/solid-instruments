﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes a single <see cref="ICommand" />.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public abstract class CommandHandler<TCommand> : Instrument, ICommandHandler<TCommand, Nix>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler{TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected CommandHandler(ICommandMediator mediator)
            : base()
        {
            Mediator = mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument;
        }

        /// <summary>
        /// Processes the specified <see cref="ICommand" />
        /// </summary>
        /// <returns>
        /// Nothing.
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
        public Nix Process(TCommand command)
        {
            _ = command.RejectIf().IsNull(nameof(command));

            try
            {
                WithStateControl(controlToken => Process(command, Mediator, controlToken));
                return Nix.Instance;
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(CommandType, exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CommandHandler{TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <note type="note">
        /// Do not process <paramref name="command" /> using <paramref name="mediator" />, as doing so will generally result in
        /// infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected abstract void Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the type of the command that is processed by the handler.
        /// </summary>
        public Type CommandType => CommandTypeValue;

        /// <summary>
        /// Gets the type of the result that is emitted by the handler when processing a command, or the <see cref="Nix" /> type if
        /// the handler does not emit a result.
        /// </summary>
        public Type ResultType => Nix.Type;

        /// <summary>
        /// Represents the type of the command that is processed by the handler.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandTypeValue = typeof(TCommand);

        /// <summary>
        /// Represents processing intermediary that is used to process sub-commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICommandMediator Mediator;
    }

    /// <summary>
    /// Processes a single <see cref="ICommand{TResult}" />.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandHandler{TCommand, TResult}" /> is the default implementation of
    /// <see cref="ICommandHandler{TCommand, TResult}" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result that is emitted by the handler when processing a command.
    /// </typeparam>
    public abstract class CommandHandler<TCommand, TResult> : Instrument, ICommandHandler<TCommand, TResult>
        where TCommand : class, ICommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler{TCommand, TResult}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected CommandHandler(ICommandMediator mediator)
            : base()
        {
            Mediator = mediator.RejectIf().IsNull(nameof(mediator)).TargetArgument;
        }

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
        public TResult Process(TCommand command)
        {
            _ = command.RejectIf().IsNull(nameof(command));

            try
            {
                return WithStateControl(controlToken => Process(command, Mediator, controlToken));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(CommandType, exception);
            }
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CommandHandler{TCommand, TResult}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <note type="note">
        /// Do not process <paramref name="command" /> using <paramref name="mediator" />, as doing so will generally result in
        /// infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected abstract TResult Process(TCommand command, ICommandMediator mediator, IConcurrencyControlToken controlToken);

        /// <summary>
        /// Gets the type of the command that is processed by the handler.
        /// </summary>
        public Type CommandType => CommandTypeValue;

        /// <summary>
        /// Gets the type of the result that is emitted by the handler when processing a command, or the <see cref="Nix" /> type if
        /// the handler does not emit a result.
        /// </summary>
        public Type ResultType => ResultTypeValue;

        /// <summary>
        /// Represents the type of the command that is processed by the handler.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type CommandTypeValue = typeof(TCommand);

        /// <summary>
        /// Represents the type of the result that is emitted by the handler when processing a command, or the <see cref="Nix" />
        /// type if the handler does not emit a result.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type ResultTypeValue = typeof(TResult);

        /// <summary>
        /// Represents processing intermediary that is used to process sub-commands.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICommandMediator Mediator;
    }
}