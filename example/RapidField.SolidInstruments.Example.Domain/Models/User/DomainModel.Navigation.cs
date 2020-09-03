// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
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
    /// - DO decorate public inverse collection navigation properties with <see cref="IgnoreDataMemberAttribute" />.
    /// - DO decorate internal inverse collection navigation properties with <see cref="DataMemberAttribute" />.
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
        /// Gets a collection of user roles which are assigned to the current <see cref="DomainModel" />.
        /// </summary>
        [DataMember]
        public ICollection<UserRoleAssignmentModel> UserRoleAssignments
        {
            get
            {
                if (UserRoleAssignmentsList is null)
                {
                    UserRoleAssignmentsList = new List<UserRoleAssignmentModel>();
                }

                return UserRoleAssignmentsList;
            }
        }

        /// <summary>
        /// Represents a lazily-initialized collection of user roles which are assigned to the current <see cref="DomainModel" />.
        /// </summary>
        /// <remarks>
        /// Lazy initialization is used for collections because <see cref="DataContractSerializer" /> cannot initialize collections
        /// without public setters.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [IgnoreDataMember]
        private List<UserRoleAssignmentModel> UserRoleAssignmentsList;
    }
}