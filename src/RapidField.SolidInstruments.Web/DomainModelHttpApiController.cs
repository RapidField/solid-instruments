// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.Web
{
    /// <summary>
    /// Processes HTTP requests representing domain model CRUD operations for an API endpoint.
    /// </summary>
    /// <typeparam name="TDomainModelIdentifier">
    /// The type of the unique primary identifier for the associated model type.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model for which the controller processes CRUD operations.
    /// </typeparam>
    public abstract class DomainModelHttpApiController<TDomainModelIdentifier, TDomainModel> : HttpApiController
        where TDomainModelIdentifier : IComparable, IComparable<TDomainModelIdentifier>, IEquatable<TDomainModelIdentifier>
        where TDomainModel : class, IDomainModel<TDomainModelIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainModelHttpApiController{TDomainModelIdentifier, TDomainModel}" />
        /// class.
        /// </summary>
        /// <param name="mediator">
        /// A processing intermediary that is used to process sub-commands.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mediator" /> is <see langword="null" />.
        /// </exception>
        protected DomainModelHttpApiController(ICommandMediator mediator)
            : base(mediator)
        {
            return;
        }
    }
}