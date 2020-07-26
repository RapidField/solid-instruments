// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;
using AssociatedRequestMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping.RequestMessage;
using AssociatedResponseMessage = RapidField.SolidInstruments.Messaging.InMemory.UnitTests.Messages.RequestResponse.Ping.ResponseMessage;

namespace RapidField.SolidInstruments.Messaging.InMemory.UnitTests.MessageListeners.RequestResponse.Ping
{
    /// <summary>
    /// Listens for and processes <see cref="AssociatedRequestMessage" /> instances.
    /// </summary>
    internal sealed class RequestListener : RequestListener<AssociatedRequestMessage, AssociatedResponseMessage>
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
        public RequestListener(ICommandMediator mediator)
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
        protected override AssociatedResponseMessage Process(AssociatedRequestMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken) => new AssociatedResponseMessage(command.Identifier, command.CorrelationIdentifier);
    }
}