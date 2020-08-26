// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Concurrency;
using RapidField.SolidInstruments.Core.Extensions;
using System;

namespace RapidField.SolidInstruments.Messaging.Service
{
    /// <summary>
    /// Listens for and processes <see cref="HeartbeatMessage" /> instances.
    /// </summary>
    public abstract class HeartbeatMessageListener : TopicListener<HeartbeatMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatMessageListener" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected HeartbeatMessageListener(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="HeartbeatMessageListener" />.
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
        protected sealed override void Process(HeartbeatMessage command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            if (command.Label.IsNullOrEmpty())
            {
                return;
            }

            switch (command.Label)
            {
                case HeartbeatMessage.FrequencyALabel:

                    ProcessFrequencyAHeartbeatMessage(command, mediator, controlToken);
                    return;

                case HeartbeatMessage.FrequencyBLabel:

                    ProcessFrequencyBHeartbeatMessage(command, mediator, controlToken);
                    return;

                case HeartbeatMessage.FrequencyCLabel:

                    ProcessFrequencyCHeartbeatMessage(command, mediator, controlToken);
                    return;

                case HeartbeatMessage.FrequencyDLabel:

                    ProcessFrequencyDHeartbeatMessage(command, mediator, controlToken);
                    return;

                case HeartbeatMessage.FrequencyELabel:

                    ProcessFrequencyEHeartbeatMessage(command, mediator, controlToken);
                    return;

                default:

                    ProcessCustomFrequencyHeartbeat(command, mediator, controlToken);
                    return;
            }
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="MessagingServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> at a custom
        /// frequency.
        /// </summary>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessCustomFrequencyHeartbeat(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> every thirteen (13)
        /// seconds [0.22 minutes / 6,646 times per calendar day].
        /// </summary>
        /// <remarks>
        /// Invoking the default constructor for
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> causes frequency
        /// "A" heartbeat messages to be suppressed. Publishing of frequency "A" heartbeat messages can be enabled by using one of
        /// several non-default constructors for that class.
        /// </remarks>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessFrequencyAHeartbeatMessage(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> every 89 seconds
        /// [1.48 minutes / 0.02 hours / 970 times per calendar day].
        /// </summary>
        /// <remarks>
        /// Invoking the default constructor for
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> causes frequency
        /// "B" heartbeat messages to be suppressed. Publishing of frequency "B" heartbeat messages can be enabled by using one of
        /// several non-default constructors for that class.
        /// </remarks>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessFrequencyBHeartbeatMessage(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> every 233 seconds
        /// [3.88 minutes / 0.06 hours / 370 times per calendar day].
        /// </summary>
        /// <remarks>
        /// Invoking the default constructor for
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> causes frequency
        /// "C" heartbeat messages to be transmitted. Publishing of frequency "C" heartbeat messages can be disabled by using one of
        /// several non-default constructors for that class.
        /// </remarks>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessFrequencyCHeartbeatMessage(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> every 1,597 seconds
        /// [26.62 minutes / 0.44 hours / 54 times per calendar day].
        /// </summary>
        /// <remarks>
        /// Invoking the default constructor for
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> causes frequency
        /// "D" heartbeat messages to be transmitted. Publishing of frequency "D" heartbeat messages can be disabled by using one of
        /// several non-default constructors for that class.
        /// </remarks>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessFrequencyDHeartbeatMessage(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }

        /// <summary>
        /// Processes a heartbeat message that is transmitted by a
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> every 28,657
        /// seconds [477.62 minutes / 7.96 hours / three (3) times per calendar day].
        /// </summary>
        /// <remarks>
        /// Invoking the default constructor for
        /// <see cref="BeaconServiceExecutor{TDependencyPackage, TDependencyConfigurator, TDependencyEngine}" /> causes frequency
        /// "E" heartbeat messages to be transmitted. Publishing of frequency "E" heartbeat messages can be disabled by using one of
        /// several non-default constructors for that class.
        /// </remarks>
        /// <note type="note">
        /// Do not process <paramref name="heartbeatMessage" /> using <paramref name="mediator" />, as doing so will generally
        /// result in infinite-looping; <paramref name="mediator" /> is exposed to this method to facilitate sub-command processing.
        /// </note>
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
        protected virtual void ProcessFrequencyEHeartbeatMessage(HeartbeatMessage heartbeatMessage, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            return;
        }
    }
}