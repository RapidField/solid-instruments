// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using Microsoft.EntityFrameworkCore;
using System;

namespace RapidField.SolidInstruments.DataAccess.EntityFramework
{
    /// <summary>
    /// Encapsulates entity type configuration for an <see cref="IDataAccessModel{TIdentifier}" />.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model.
    /// </typeparam>
    public interface IDataAccessModelConfiguration<TIdentifier, TDataAccessModel> : IDataAccessModelConfiguration<TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
    }

    /// <summary>
    /// Encapsulates entity type configuration for an <see cref="IDataAccessModel" />.
    /// </summary>
    /// <typeparam name="TDataAccessModel">
    /// The type of the data access model.
    /// </typeparam>
    public interface IDataAccessModelConfiguration<TDataAccessModel> : IEntityTypeConfiguration<TDataAccessModel>
        where TDataAccessModel : class, IDataAccessModel
    {
    }
}