// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to create or update a data access model.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    public interface ICreateOrUpdateDataAccessModelCommand<TIdentifier, TDataAccessModel> : IDataAccessModelCommand<TIdentifier, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
    }
}