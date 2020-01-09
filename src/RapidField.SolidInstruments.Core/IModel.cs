// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents a serializable data/domain model.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    public interface IModel<TIdentifier> : IComparable<IModel<TIdentifier>>
        where TIdentifier : IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
        /// <summary>
        /// Gets or sets a value that uniquely identifies the current <see cref="IModel{TIdentifier}" />.
        /// </summary>
        TIdentifier Identifier
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a serializable data/domain model.
    /// </summary>
    public interface IModel : IEquatable<IModel>
    {
    }
}