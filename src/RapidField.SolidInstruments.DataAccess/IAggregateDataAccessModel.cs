// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents an object that models a data access entity as a domain aggregate.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface IAggregateDataAccessModel<TIdentifier, TDomainModel> : IAggregateDataAccessModel<TIdentifier>, IValueDataAccessModel<TIdentifier, TDomainModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>
    {
    }

    /// <summary>
    /// Represents an object that models a data access entity as a domain aggregate.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    public interface IAggregateDataAccessModel<TIdentifier> : IAggregateDataAccessModel, IValueDataAccessModel<TIdentifier>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
    }

    /// <summary>
    /// Represents an object that models a data access entity as a domain aggregate.
    /// </summary>
    public interface IAggregateDataAccessModel : IValueDataAccessModel
    {
    }
}