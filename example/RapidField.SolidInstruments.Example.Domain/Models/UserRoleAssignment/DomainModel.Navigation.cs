// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using IUserModel = RapidField.SolidInstruments.Example.Domain.Models.User.IAggregateDomainModel;
using IUserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.IAggregateDomainModel;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    /// <remarks>
    /// This is the navigation declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a
    /// model group and are appropriate for use in any context in which detail-level information is needed. Domain models represent
    /// domain constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare public standard navigation properties as domain model interfaces.
    /// - DO declare public inverse collection navigation properties as <see cref="IEnumerable{T}" /> of domain model interfaces.
    /// - DO declare private standard navigation fields as concrete domain models.
    /// - DO declare internal inverse collection navigation properties as <see cref="ICollection{T}" /> of concrete domain models.
    /// - DO decorate (public and internal) standard navigation properties with <see cref="IgnoreDataMemberAttribute" />.
    /// - DO decorate public inverse collection navigation properties with <see cref="DataMemberAttribute" />.
    /// - DO decorate internal inverse collection navigation properties with <see cref="IgnoreDataMemberAttribute" />.
    /// - DO expose getters and internal setters for public standard navigation properties.
    /// - DO expose getters (but not setters) for (public and internal) inverse collection navigation properties.
    /// - DO lazily initialize inverse navigation collections.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// </remarks>
    public sealed partial class DomainModel
    {
        /// <summary>
        /// Gets the user to which a user role is assigned.
        /// </summary>
        [IgnoreDataMember]
        public IUserModel User
        {
            get => UserValue;
            internal set => UserValue = value as UserModel;
        }

        /// <summary>
        /// Gets the user role that is a assigned to a user.
        /// </summary>
        [IgnoreDataMember]
        public IUserRoleModel UserRole
        {
            get => UserRoleValue;
            internal set => UserRoleValue = value as UserRoleModel;
        }

        /// <summary>
        /// Represents the user role that is a assigned to a user.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private UserRoleModel UserRoleValue;

        /// <summary>
        /// Represents the user to which a user role is assigned.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private UserModel UserValue;
    }
}