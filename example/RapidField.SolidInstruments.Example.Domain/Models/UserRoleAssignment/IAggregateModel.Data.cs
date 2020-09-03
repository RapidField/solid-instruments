// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
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
        /// Gets a value that uniquely identifies the user to which a user role is assigned.
        /// </summary>
        public Guid UserIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets a value that uniquely identifies the user role that is assigned to a user.
        /// </summary>
        public Guid UserRoleIdentifier
        {
            get;
        }
    }
}