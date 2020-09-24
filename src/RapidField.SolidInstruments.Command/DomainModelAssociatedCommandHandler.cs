﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Processes a single <see cref="IDomainModelAssociatedCommand{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the associated domain model.
    /// </typeparam>
    /// <typeparam name="TCommand">
    /// The type of the command that is processed by the handler.
    /// </typeparam>
    public abstract class DomainModelAssociatedCommandHandler<TModel, TCommand> : DomainModelCommandHandler<TModel, TCommand>
        where TModel : class, IDomainModel
        where TCommand : class, IDomainModelAssociatedCommand<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDomainModelCommandHandler{TModel, TCommand}" /> class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelAssociatedCommandHandler(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }

        /// <summary>
        /// Releases all resources consumed by the current <see cref="DomainModelAssociatedCommandHandler{TModel, TCommand}" />.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether or not disposal was invoked by user code.
        /// </param>
        protected override void Dispose(Boolean disposing) => base.Dispose(disposing);
    }
}