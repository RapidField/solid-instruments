// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Transmits request messages.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is transmitted by the transmitter.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public class RequestTransmitter<TRequestMessage, TResponseMessage> : MessageTransmitter<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransmitter{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="facade">
        /// An appliance that facilitates implementation-specific request message transmission operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="facade" /> is <see langword="null" />.
        /// </exception>
        public RequestTransmitter(ICommandMediator mediator, IMessageRequestingFacade facade)
            : base(mediator, facade)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RequestTransmitter{TRequestMessage, TResponseMessage}" />.
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
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected override TResponseMessage Process(TRequestMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => base.Process(command, mediator, controlToken);
    }
}