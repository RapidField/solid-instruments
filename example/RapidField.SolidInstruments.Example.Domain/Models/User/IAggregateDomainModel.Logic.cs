// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the logic declaration for an aggregate domain model interface. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare computed properties and domain logic methods.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public partial interface IAggregateDomainModel
    {
        /// <summary>
        /// Assigns the specified role to the current <see cref="IAggregateDomainModel" /> if the role is not already assigned.
        /// </summary>
        /// <param name="userRole">
        /// The role to assign to the current user.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userRole" /> is <see langword="null" />.
        /// </exception>
        public void AssignRole(UserRoleModel userRole);

        /// <summary>
        /// Removes all roles assigned to the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        public void ClearRoleAssignments();
    }
}