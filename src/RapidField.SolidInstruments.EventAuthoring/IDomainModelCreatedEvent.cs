﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.Domain;

namespace RapidField.SolidInstruments.EventAuthoring
{
    /// <summary>
    /// Represents information about the creation of an object that models a domain construct.
    /// </summary>
    public interface IDomainModelCreatedEvent<TModel> : IDomainModelEvent<TModel>
        where TModel : class, IDomainModel
    {
    }
}