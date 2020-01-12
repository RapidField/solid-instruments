// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core.Domain
{
    /// <summary>
    /// Represents an object that models a domain construct.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the unique primary identifier for the model.
    /// </typeparam>
    public interface IDomainModel<TIdentifier> : IDomainModel, IModel<TIdentifier>
        where TIdentifier : IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
    }

    /// <summary>
    /// Represents an object that models a domain construct.
    /// </summary>
    public interface IDomainModel : IModel
    {
    }
}