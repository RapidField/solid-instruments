// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Contracts.Models
{
    /// <summary>
    /// Represents a sequential series of integer numbers.
    /// </summary>
    public interface INumberSeries
    {
        /// <summary>
        /// Gets a unique identifier for the entity.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the name of the series.
        /// </summary>
        public String Name
        {
            get;
        }
    }
}