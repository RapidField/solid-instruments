// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;

namespace RapidField.SolidInstruments.Prototype.Domain.MessageSubscribers
{
    /// <summary>
    /// Subscribes to and processes <see cref="ExceptionRaisedMessage" /> instances.
    /// </summary>
    public sealed class ExceptionRaisedMessageSubscriber : QueueSubscriber<ExceptionRaisedMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionRaisedMessageSubscriber" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public ExceptionRaisedMessageSubscriber(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ExceptionRaisedMessageSubscriber" />.
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
        protected override void Process(ExceptionRaisedMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken) => Console.WriteLine($"{command.ApplicationEvent.Summary}{Environment.NewLine}");
    }
}