// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.Messaging;
using System;

namespace RapidField.SolidInstruments.Example.Domain.MessageListeners
{
    /// <summary>
    /// Listens for and processes <see cref="PingRequestMessage" /> instances.
    /// </summary>
    public sealed class PingRequestMessageListener : RequestListener<PingRequestMessage, PingResponseMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingRequestMessageListener" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public PingRequestMessageListener(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="PingRequestMessageListener" />.
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
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected override PingResponseMessage Process(PingRequestMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken) => new PingResponseMessage(command.Identifier, command.CorrelationIdentifier);
    }
}