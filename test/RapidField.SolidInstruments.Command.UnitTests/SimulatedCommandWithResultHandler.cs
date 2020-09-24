// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.Command.UnitTests
{
    /// <summary>
    /// Represents a <see cref="CommandHandler{TCommand, TResult}" /> derivative that is used for testing.
    /// </summary>
    internal sealed class SimulatedCommandWithResultHandler : CommandHandler<SimulatedCommandWithResult, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedCommandWithResultHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        public SimulatedCommandWithResultHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="SimulatedCommandWithResultHandler" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected sealed override void Dispose(Boolean disposing) => base.Dispose(disposing);

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
        /// The result that is emitted by processing the command.
        /// </returns>
        protected sealed override Guid Process(SimulatedCommandWithResult command, ICommandMediator mediator, IConcurrencyControlToken controlToken)
        {
            command.IsProcessed = true;
            return command.Identifier;
        }
    }
}