// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using UserModel = RapidField.SolidInstruments.Example.Domain.Models.User.DomainModel;
using UserRoleModel = RapidField.SolidInstruments.Example.Domain.Models.UserRole.DomainModel;

namespace RapidField.SolidInstruments.Example.Domain.Models.UserRoleAssignment
{
    /// <summary>
    /// Represents the assignment of a single user role to a single user.
    /// </summary>
    /// <remarks>
    /// This is the named entity declaration for a concrete aggregate domain model. Aggregate models expose the full schema for a
    /// model group and are appropriate for use in any context in which detail-level information is needed. Domain models represent
    /// domain constructs and define their characteristics and behavior. The following are guidelines for use of this declaration.
    /// - DO declare the public static class <see cref="Named" />.
    /// - DO declare static <see cref="DomainModel" /> properties with public getters which create and return known entities.
    /// - DO declare the public static method <see cref="Named.All" /> which returns all known entities.
    /// - DO NOT specify class inheritance or interface implementation(s).
    /// - DO NOT decorate the class with attributes.
    /// - DO NOT declare constructors.
    /// - DO NOT declare data fields or properties.
    /// - DO NOT declare computed properties or domain logic methods.
    /// - DO NOT declare navigation properties.
    /// </remarks>
    public sealed partial class DomainModel
    {
        /// <summary>
        /// Contains a collection of known <see cref="DomainModel" /> instances.
        /// </summary>
        public static class Named
        {
            /// <summary>
            /// Returns a collection of all known <see cref="DomainModel" /> instances.
            /// </summary>
            /// <returns>
            /// A collection of all known <see cref="DomainModel" /> instances.
            /// </returns>
            public static IEnumerable<DomainModel> All() => new DomainModel[]
            {
                StevenCallahanSystemAdministrator,
                TomSmithEndUser
            };

            /// <summary>
            /// Gets the system administrator role assignment for Steven Callahan.
            /// </summary>
            public static DomainModel StevenCallahanSystemAdministrator => new DomainModel(Guid.Parse("3af59674-efe9-46ad-b8b3-847e1a74c53c"))
            {
                User = UserModel.Named.StevenCallahan,
                UserRole = UserRoleModel.Named.SystemAdministrator
            };

            /// <summary>
            /// Gets the end user role assignment for Tom Smith.
            /// </summary>
            public static DomainModel TomSmithEndUser => new DomainModel(Guid.Parse("472ceca5-e06b-4ae7-8022-ecb160822137"))
            {
                User = UserModel.Named.TomSmith,
                UserRole = UserRoleModel.Named.EndUser
            };
        }
    }
}