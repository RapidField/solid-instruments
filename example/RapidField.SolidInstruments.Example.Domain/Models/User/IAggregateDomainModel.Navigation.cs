// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the navigation declaration for an aggregate data access model interface. Aggregate models expose the full schema for
    /// a model group and are appropriate for use in a system of record or in any context in which detail-level information is
    /// needed. Data access models are in-memory representations of data entities and are used to perform data access operations.
    /// The following are guidelines for use of this declaration.
    /// - DO declare public standard navigation properties as domain model interfaces.
    /// - DO declare public inverse collection navigation properties as <see cref="IEnumerable{T}" /> of domain model interfaces.
    /// - DO expose getters (but not setters) for standard navigation properties.
    /// - DO expose getters (but not setters) for inverse collection navigation properties.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT decorate the interface with attributes.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// </remarks>
    public partial interface IAggregateDomainModel
    {
        /// <summary>
        /// Gets a collection of user roles which are assigned to the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        public ICollection<UserRoleAssignmentModel> UserRoleAssignments
        {
            get;
        }
    }
}