// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.Messaging
{
    /// <summary>
    /// Processes topic messages as a subscriber.
    /// </summary>
    /// <typeparam name="TMessage">
    /// The type of the message that is subscribed to.
    /// </typeparam>
    public abstract class TopicSubscriber<TMessage> : MessageSubscriber<TMessage>
        where TMessage : class, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicSubscriber{TMessage}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected TopicSubscriber(ICommandMediator mediator)
            : base(mediator, MessagingEntityType.Topic)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="TopicSubscriber{TMessage}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}