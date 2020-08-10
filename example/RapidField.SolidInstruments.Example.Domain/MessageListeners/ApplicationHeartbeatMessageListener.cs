// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Messaging.Service;
using System;
using System.Diagnostics;

namespace RapidField.SolidInstruments.Example.Domain.MessageListeners
{
    /// <summary>
    /// Listens for and processes <see cref="HeartbeatMessage" /> instances.
    /// </summary>
    public sealed class ApplicationHeartbeatMessageListener : HeartbeatMessageListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationHeartbeatMessageListener" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public ApplicationHeartbeatMessageListener(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="ApplicationHeartbeatMessageListener" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> at a custom
        /// frequency.
        /// </summary>
        /// <param name="heartbeatMessage">
        /// The heartbeat message to process.
        /// </param>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands. Do not process <paramref name="heartbeatMessage" />
        /// using <paramref name="mediator" />, as doing so will generally result in infinite-looping.
        /// </param>
        /// <param name="controlToken">
        /// A token that represents and manages contextual thread safety.
        /// </param>
        protected override void ProcessCustomFrequencyHeartbeat(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            try
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
            finally
            {
                base.ProcessCustomFrequencyHeartbeat(heartbeatMessage, mediator, controlToken);
            }
        }
    }
}