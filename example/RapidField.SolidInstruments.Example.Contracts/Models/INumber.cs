// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Contracts.Models
{
    /// <summary>
    /// Represents an integer number.
    /// </summary>
    public interface INumber
    {
        /// <summary>
        /// Gets a unique identifier for the entity.
        /// </summary>
        Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Gets the value of the number.
        /// </summary>
        Int64 Value
        {
            get;
        }
    }
}