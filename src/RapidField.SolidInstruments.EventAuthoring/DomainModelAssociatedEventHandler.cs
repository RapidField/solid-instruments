// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Processes a single <see cref="IDomainModelAssociatedEvent{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TEvent">
    /// The type of the event that is processed by the handler.
    /// </typeparam>
    public abstract class DomainModelAssociatedEventHandler<TModel, TEvent> : DomainModelEventHandler<TModel, TEvent>
        where TModel : class, IDomainModel
        where TEvent : class, IDomainModelAssociatedEvent<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelAssociatedEventHandler{TModel, TEvent}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelAssociatedEventHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainModelAssociatedEventHandler{TModel, TEvent}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}