// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes queue messages as a subscriber.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is subscribed to.
    /// </typeparam>
    public abstract class QueueSubscriber<TMessage> : MessageSubscriber<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueSubscriber{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected QueueSubscriber(ICommandMediator mediator)
            : base(mediator, MessagingEntityType.Queue)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="QueueSubscriber{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}