// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents an object that models a data access entity as a domain value.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface IValueDataAccessModel<TIdentifier, TDomainModel> : IValueDataAccessModel<TIdentifier>, IDataAccessModel<TIdentifier, TDomainModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDomainModel : class, IDomainModel<TIdentifier>
    {
    }

    /// <summary>
    /// Represents an object that models a data access entity as a domain value.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    public interface IValueDataAccessModel<TIdentifier> : IValueDataAccessModel, IDataAccessModel<TIdentifier>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
    }

    /// <summary>
    /// Represents an object that models a data access entity as a domain value.
    /// </summary>
    public interface IValueDataAccessModel : IDataAccessModel
    {
    }
}