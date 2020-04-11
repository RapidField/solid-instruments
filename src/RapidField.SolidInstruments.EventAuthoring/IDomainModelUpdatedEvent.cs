// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about an update to an object that models a domain construct.
    /// </summary>
    public interface IDomainModelUpdatedEvent<TModel> : IDomainModelEvent<TModel>
        where TModel : class, IDomainModel
    {
    }
}