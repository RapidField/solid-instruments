// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for a shared aggregate model interface. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Shared models define the
    /// characteristics which are shared between data access models and domain models. The following are guidelines for use of this
    /// declaration.
    /// - DO declare data properties with public getters.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT decorate the interface with attributes.
    /// - DO NOT decorate data properties with attributes.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IAggregateModel
    {
        /// <summary>
        /// Gets the hashed password for the current <see cref="IAggregateModel" />.
        /// </summary>
        public String PasswordHash
        {
            get;
        }
    }
}