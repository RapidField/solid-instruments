// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Core
{
    /// <summary>
    /// Represents an object that models a general construct.
    /// </summary>
    /// <typeparam name="TIdentifier">
    /// The type of the value that uniquely identifies the model.
    /// </typeparam>
    public interface IModel<TIdentifier> : IComparable<IModel<TIdentifier>>, IModel
        where TIdentifier : IComparable, IComparable<TIdentifier>, IEquatable<TIdentifier>
    {
        /// <summary>
        /// Gets or sets a value that uniquely identifies the current <see cref="IModel{TIdentifier}" />.
        /// </summary>
        TIdentifier Identifier
        {
            get;
        }
    }

    /// <summary>
    /// Represents an object that models a general construct.
    /// </summary>
    public interface IModel : ICloneable, IEquatable<IModel>
    {
    }
}