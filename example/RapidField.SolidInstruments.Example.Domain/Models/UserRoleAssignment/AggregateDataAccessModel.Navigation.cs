﻿// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.AggregateDataAccessModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.AggregateDataAccessModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    /// <remarks>
    /// This is the navigation declaration for a concrete aggregate data access model. Aggregate models expose the full schema for a
    /// model group and are appropriate for use in a system of record or in any context in which detail-level information is needed.
    /// Data access models are in-memory representations of data entities and are used to perform data access operations. The
    /// following are guidelines for use of this declaration.
    /// - DO declare public standard navigation properties as concrete domain models.
    /// - DO declare public inverse collection navigation properties as <see cref="ICollection{T}" /> of concrete domain models.
    /// - DO expose getters and setters for standard navigation properties.
    /// - DO expose getters (but not setters) for inverse collection navigation properties.
    /// - DO lazily initialize inverse navigation collections.
    /// - DO decorate standard navigation properties with <see cref="ForeignKeyAttribute" />.
    /// - DO decorate standard navigation properties with <see cref="IgnoreDataMemberAttribute" />.
    /// - DO decorate inverse collection navigation properties with <see cref="DataMemberAttribute" />.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// </remarks>
    public sealed partial class AggregateDataAccessModel
    {
        /// <summary>
        /// Gets or sets the user to which a user role is assigned.
        /// </summary>
        [ForeignKey(nameof(UserIdentifier))]
        [IgnoreDataMember]
        public UserModel User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user role that is a assigned to a user.
        /// </summary>
        [ForeignKey(nameof(UserRoleIdentifier))]
        [IgnoreDataMember]
        public UserRoleModel UserRole
        {
            get;
            set;
        }
    }
}