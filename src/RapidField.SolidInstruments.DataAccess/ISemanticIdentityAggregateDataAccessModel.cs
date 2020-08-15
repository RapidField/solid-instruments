// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents an object that models an aggregate data access entity and that is identified primarily by a <see cref="String" />
    /// value.
    /// </summary>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface ISemanticIdentityAggregateDataAccessModel<TDomainModel> : ISemanticIdentityAggregateDataAccessModel, IAggregateDataAccessModel<String, TDomainModel>
        where TDomainModel : class, ISemanticIdentityAggregateDomainModel, new()
    {
    }

    /// <summary>
    /// Represents an object that models an aggregate data access entity and that is identified primarily by a <see cref="String" />
    /// value.
    /// </summary>
    public interface ISemanticIdentityAggregateDataAccessModel : ISemanticIdentityDataAccessModel, IAggregateDataAccessModel<String>
    {
    }
}