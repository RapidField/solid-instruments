// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Linq;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    /// <remarks>
    /// This is the logic declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare computed properties and domain logic methods.
    /// - DO NOT specify interface implementation(s).
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
        /// <summary>
        /// Assigns the specified role to the current <see cref="DomainModel" /> if the role is not already assigned.
        /// </summary>
        /// <param name="userRole">
        /// The role to assign to the current user.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userRole" /> is <see langword="null" />.
        /// </exception>
        public void AssignRole(UserRoleModel userRole)
        {
            var userRoleIdentifier = userRole.RejectIf().IsNull(nameof(userRole)).TargetArgument.Identifier;

            if (UserRoleAssignments.Any(element => element.UserRoleIdentifier == userRoleIdentifier))
            {
                // The user is already assigned the specified role.
                return;
            }

            UserRoleAssignments.Add(new UserRoleAssignmentModel(Guid.NewGuid())
            {
                User = this,
                UserRole = userRole
            });
        }

        /// <summary>
        /// Removes all roles assigned to the current <see cref="DomainModel" />.
        /// </summary>
        public void ClearRoleAssignments() => UserRoleAssignments.Clear();
    }
}