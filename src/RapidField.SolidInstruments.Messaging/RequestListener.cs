// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes request messages as a listener.
    /// </summary>
    /// <typeparam name="TRequestMessage">
    /// The type of the request message that is processed by the listener.
    /// </typeparam>
    /// <typeparam name="TResponseMessage">
    /// The type of the response message that is transmitted in response to the request.
    /// </typeparam>
    public abstract class RequestListener<TRequestMessage, TResponseMessage> : MessageListener<TRequestMessage, TResponseMessage>
        where TRequestMessage : class, IRequestMessage<TResponseMessage>
        where TResponseMessage : class, IResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestListener{TRequestMessage, TResponseMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected RequestListener(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="RequestListener{TRequestMessage, TResponseMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}