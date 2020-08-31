// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    public interface IAggregateModel : IValueModel
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