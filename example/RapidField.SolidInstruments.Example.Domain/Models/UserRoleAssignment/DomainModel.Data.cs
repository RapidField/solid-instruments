// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Runtime.Serialization;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    /// <remarks>
    /// This is the data declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a model
    /// group and are appropriate for use in any context in which detail-level information is needed. Domain models represent domain
    /// constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare data properties with public getters and setters.
    /// - DO decorate data properties with <see cref="DataMemberAttribute" />.
    /// - DO add validation logic to data property setters to enforce state validity, as needed.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
        /// <summary>
        /// Gets or sets a value that uniquely identifies the user to which a user role is assigned.
        /// </summary>
        [DataMember]
        public Guid UserIdentifier
        {
            get => User?.Identifier ?? Guid.Empty;
            set => User = User is null ? new UserModel(value) : User;
        }

        /// <summary>
        /// Gets or sets a value that uniquely identifies the user role that is assigned to a user.
        /// </summary>
        [DataMember]
        public Guid UserRoleIdentifier
        {
            get => UserRole?.Identifier ?? Guid.Empty;
            set => UserRole = UserRole is null ? new UserRoleModel(value) : UserRole;
        }
    }
}