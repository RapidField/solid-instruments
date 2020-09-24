// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Transmits messages to a topic.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is transmitted by the transmitter.
    /// </typeparam>
    public class TopicTransmitter<TMessage> : MessageTransmitter<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicTransmitter{TMessage}" /> class.
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
        public TopicTransmitter(ICommandMediator mediator, IMessageTransmittingFacade facade)
            : base(mediator, facade, MessagingEntityType.Topic)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="TopicTransmitter{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
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