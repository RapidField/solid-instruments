// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Domain;
using System;

namespace RapidField.SolidInstruments.DataAccess
{
    /// <summary>
    /// Represents an object that models a data access entity and that is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    /// <typeparam name="TDomainModel">
    /// The type of the domain model to which the data access model is mapped.
    /// </typeparam>
    public interface IGlobalIdentityDataAccessModel<TDomainModel> : IGlobalIdentityDataAccessModel, IDataAccessModel<Guid, TDomainModel>
        where TDomainModel : class, IGlobalIdentityDomainModel, new()
    {
    }

    /// <summary>
    /// Represents an object that models a data access entity and that is identified primarily by a <see cref="Guid" /> value.
    /// </summary>
    public interface IGlobalIdentityDataAccessModel : IDataAccessModel<Guid>, IGlobalIdentityModel
    {
    }
}