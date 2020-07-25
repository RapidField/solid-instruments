// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Processes a single <see cref="IDomainEvent" />.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The type of the event that is processed by the handler.
    /// </typeparam>
    public abstract class DomainEventHandler<TEvent> : EventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventHandler{TEvent}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainEventHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="EventHandler{TEvent}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not managed resources should be released.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}