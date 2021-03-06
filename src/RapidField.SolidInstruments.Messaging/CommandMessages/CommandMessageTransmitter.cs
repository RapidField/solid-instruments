﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Messaging.CommandMessages
{
    /// <summary>
    /// Transmits command messages to a queue.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandMessageTransmitter{TCommand, TMessage}" /> is the default implementation of
    /// <see cref="ICommandMessageTransmitter" />.
    /// </remarks>
    /// <typeparam name="TCommand">
    /// The type of the associated command.
    /// </typeparam>
    /// <typeparam name="TMessage">
    /// The type of the message that is transmitted by the transmitter.
    /// </typeparam>
    public class CommandMessageTransmitter<TCommand, TMessage> : QueueTransmitter<TMessage>, ICommandMessageTransmitter
        where TCommand : class, ICommand
        where TMessage : class, ICommandMessage<TCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessageTransmitter{TCommand, TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="facade">
        /// An appliance that facilitates implementation-specific message transmission operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="facade" /> is <see langword="null" />.
        /// </exception>
        public CommandMessageTransmitter(ICommandMediator mediator, IMessageTransmittingFacade facade)
            : base(mediator, facade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="CommandMessageTransmitter{TCommand, TMessage}" />.
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
        protected override void Process(TMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => base.Process(command, mediator, controlToken);
    }
}