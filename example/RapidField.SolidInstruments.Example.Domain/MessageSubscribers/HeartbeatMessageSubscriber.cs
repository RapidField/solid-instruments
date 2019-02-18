// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Example.Contracts.Messages;
using RapidField.SolidInstruments.Messaging;
using RapidField.SolidInstruments.Messaging.Service;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.MessageSubscribers
{
    /// <summary>
    /// Subscribes to and processes <see cref="HeartbeatMessage" /> instances.
    /// </summary>
    public sealed class HeartbeatMessageSubscriber : TopicSubscriber<HeartbeatMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatMessageSubscriber" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public HeartbeatMessageSubscriber(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="HeartbeatMessageSubscriber" />.
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
        protected override void Process(HeartbeatMessage command, ICommandMediator mediator, ConcurrencyControlToken controlToken)
        {
            var pingRequest = new PingRequestMessage();
            var pingResponse = (PingResponseMessage)null;
            var stopwatch = Stopwatch.StartNew();
            pingResponse = mediator.Process<PingResponseMessage>(pingRequest);
            stopwatch.Stop();

            if (pingResponse is null == false && pingResponse.RequestMessageIdentifier == pingRequest.Identifier)
            {
                Console.WriteLine($"Success! The round trip ping operation completed in {stopwatch.ElapsedMilliseconds} milliseconds.");
                return;
            }

            Console.WriteLine("The round trip ping operation failed.");
        }
    }
}