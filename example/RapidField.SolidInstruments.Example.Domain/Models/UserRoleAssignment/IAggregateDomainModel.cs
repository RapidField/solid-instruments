// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    public interface IAggregateDomainModel : IAggregateModel, IBaseDomainModel, IValueDomainModel
    {
        /// <summary>
        /// Gets the user to which a user role is assigned.
        /// </summary>
        public UserModel User
        {
            get;
        }

        /// <summary>
        /// Gets the user role that is a assigned to a user.
        /// </summary>
        public UserRoleModel UserRole
        {
            get;
        }
    }
}