// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents a command to find a data access model by its identifier.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the data access model.
    /// </typeparam>
    /// <typeparam name="TDataAccessModel">
    /// The type of the associated data access model.
    /// </typeparam>
    public interface IFindDataAccessModelByIdentifierCommand<TIdentifier, TDataAccessModel> : IDataAccessModelCommand<TIdentifier, TDataAccessModel, TDataAccessModel>
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
        where TDataAccessModel : class, IDataAccessModel<TIdentifier>
    {
    }
}