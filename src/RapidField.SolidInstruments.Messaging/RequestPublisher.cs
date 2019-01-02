// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Publishes request messages.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is published by the publisher.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is published in response to the request.
    /// </typeparam>
    public class RequestPublisher<TRequestMessage, TResponseMessage> : MessagePublisher<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePublisher{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <param name="client">
        /// A client that facilitates message publishing operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" /> -or- <paramref name="client" /> is <see langword="null" />.
        /// </exception>
        public RequestPublisher(ICommandMediator mediator, IMessagePublishingClient client)
            : base(mediator, client)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RequestPublisher{TRequestMessage, TResponseMessage}" />.
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
        /// A token that ensures thread safety for the operation.
        /// </param>
        /// <returns>
        /// The result that is emitted when processing the command.
        /// </returns>
        protected override TResponseMessage Process(TRequestMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken) => base.Process(command, mediator, controlToken);
    }
}