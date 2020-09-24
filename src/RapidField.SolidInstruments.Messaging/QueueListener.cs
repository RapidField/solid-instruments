// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes queue messages as a listener.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is listened for.
    /// </typeparam>
    public abstract class QueueListener<TMessage> : MessageListener<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueListener{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected QueueListener(ICommandMediator mediator)
            : base(mediator, MessagingEntityType.Queue)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="QueueListener{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}