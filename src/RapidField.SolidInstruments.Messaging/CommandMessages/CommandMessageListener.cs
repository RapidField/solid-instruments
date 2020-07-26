// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Processes command messages as a listener.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    /// <typeparam name="TMessage">
    /// The type of the message that is listened for.
    /// </typeparam>
    public class CommandMessageListener<TCommand, TMessage> : QueueListener<TMessage>
        where TCommand : class, ICommand
        where TMessage : class, ICommandMessage<TCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessageListener{TCommand, TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public CommandMessageListener(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CommandMessageListener{TCommand, TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
        protected override void Process(TMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => mediator.Process(command.Command);
    }
}