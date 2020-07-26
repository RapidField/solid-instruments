// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Contracts.Models
{
    /// <summary>
    /// Represents an integer number belonging to a specific number series.
    /// </summary>
    public interface INumberSeriesNumber
    {
        /// <summary>
        /// Gets a unique identifier for the entity.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the associated number.
        /// </summary>
        public Guid NumberIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the associated number series.
        /// </summary>
        public Guid NumberSeriesIdentifier
        {
            get;
        }
    }
}