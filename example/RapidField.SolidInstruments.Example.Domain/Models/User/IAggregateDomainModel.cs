// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;
using System.Collections.Generic;
using IBaseDomainModel = RapidField.SolidInstruments.Core.Domain.IGlobalIdentityAggregateDomainModel;
using UserRoleAssignmentModel = RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.User
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public interface IAggregateDomainModel : IAggregateModel, IBaseDomainModel, IValueDomainModel
    {
        /// <summary>
        /// Gets or sets the email address for the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="EmailAddress" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="EmailAddress" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="EmailAddress" /> is too long.
        /// </exception>
        public new String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="Name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="Name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Name" /> is too long.
        /// </exception>
        public new String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hashed password for the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        /// <exception cref="ArgumentEmptyException">
        /// <see cref="PasswordHash" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <see cref="PasswordHash" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="PasswordHash" /> is too long.
        /// </exception>
        public new String PasswordHash
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a collection of user roles which are assigned to the current <see cref="IAggregateDomainModel" />.
        /// </summary>
        public ICollection<UserRoleAssignmentModel> UserRoleAssignments
        {
            get;
        }
    }
}